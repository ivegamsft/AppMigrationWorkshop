using System;
using System.Web.UI.WebControls;
using System.Web.Security;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class ManagePhotos_aspx : System.Web.UI.Page
{
    public int AdId
    {
        get
        {
            if (ViewState["AdId"] != null)
                return (int)ViewState["AdId"];
            else
                return 0;
        }
        set
        {
            ViewState["AdId"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SiteSettings s = SiteSettings.GetSharedSettings();

            int adId = 0;
            string adIdQs = Request.QueryString["id"];

            AdsDataComponent.AdsRow adRow = null;
            bool isAdmin = Roles.IsUserInRole(DefaultValues.AdministratorRole);

            if (adIdQs != null && Int32.TryParse(adIdQs, out adId))
            {
                AdId = adId;
                adRow = AdsDB.GetAdById(adId);
            }

            // only allow the actual member and administrators to edit photos
            if (adRow != null && (adRow.MemberId == Profile.MemberId || isAdmin))
            {
                AdTitleLabel.Text = adRow.Title;
                string idQs = "id=" + Server.UrlEncode(adIdQs);

                ShowAdLink.NavigateUrl = "~/ShowAd.aspx?" + idQs;
                BackToEditAdLink.NavigateUrl = "~/EditAd.aspx?" + idQs;
                BackToEditAdLink.Visible = (isAdmin || s.AllowUsersToEditAds);
                if (isAdmin && !s.AllowUsersToEditAds)
                    BackToEditAdLink.Text += " (Admin)";
            }
            else
            {
                // if the above test fails, re-direct to MyAds
                Response.Redirect("~/MyAds.aspx", true);
            }

            MainUploadsPanel.Visible = s.AllowImageUploads;
            NoUploadsPanel.Visible = !s.AllowImageUploads;
            if (s.MaxPhotosPerAd == 1)
                NumTotalUploadLabel.Text = "1 photo";
            else
                NumTotalUploadLabel.Text = s.MaxPhotosPerAd.ToString() + " photos";

        }
    }

    protected void UploadPhotoDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        byte[] uploadedOriginal = e.Values["UploadBytes"] as byte[];
        if (uploadedOriginal == null || uploadedOriginal.Length == 0)
        {
            e.Cancel = true;
            UploadErrorMessage.Visible = false;
        }
        else
        {
            try
            {
                // adding resized photo bytes to arguments

                byte[] fullSize = PhotosDB.ResizeImageFile(uploadedOriginal, PhotoSize.Full);
                byte[] mediumSize = PhotosDB.ResizeImageFile(uploadedOriginal, PhotoSize.Medium);
                byte[] smallSize = PhotosDB.ResizeImageFile(uploadedOriginal, PhotoSize.Small);

                e.Values.Add("bytesFull", fullSize);
                e.Values.Add("bytesMedium", mediumSize);
                e.Values.Add("bytesSmall", smallSize);

                // removing original upload bytes from arguments
                e.Values.Remove("UploadBytes");

                // if this is the first ad photo uploaded, use it as the preview
                bool useAsPreview = (PhotoGridView.Rows.Count == 0);
                e.Values.Add("useAsPreview", useAsPreview);
                UploadErrorMessage.Visible = false;
            }
            catch
            {
                // image format was not acceptable
                e.Cancel = true;
                UploadErrorMessage.Visible = true;
            }
        }
    }
    protected void PhotoDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        AdsDataComponent.PhotosDataTable photos = e.ReturnValue as AdsDataComponent.PhotosDataTable;
        SiteSettings settings = SiteSettings.GetSharedSettings();
        if (photos != null)
        {
            PreviewTipPanel.Visible = photos.Rows.Count > 1;
            this.UploadPhotoDetailsView.Visible = (photos.Rows.Count < settings.MaxPhotosPerAd);
        }
        else
        {
            this.UploadPhotoDetailsView.Visible = (settings.MaxPhotosPerAd > 0);
        }
    }
    protected void PhotoGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("SelectAsPreview"))
        {
            int photoId = Convert.ToInt32(e.CommandArgument);
            PhotosDB.SetAdPreviewPhoto(this.AdId, photoId);
            PhotoGridView.DataBind();
        }
    }

}
