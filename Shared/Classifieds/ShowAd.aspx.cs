using System;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class ShowAd_aspx : System.Web.UI.Page
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

    public int CurrentPhotoIndex
    {
        get
        {
            if (ViewState["CurrentPhotoIndex"] != null)
                return (int)ViewState["CurrentPhotoIndex"];
            else
                return -1;
        }
        set
        {
            ViewState["CurrentPhotoIndex"] = value;
        }
    }

    public int NumPostBacks
    {
        get
        {
            if (ViewState["NumPostBacks"] != null)
                return (int)ViewState["NumPostBacks"];
            else
            {
                ViewState["NumPostBacks"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["NumPostBacks"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check if the URL querystring contains a valid ad.
        int adId = DefaultValues.IdNullValue;
        string adIdQs = Request.QueryString["id"];
        if (adIdQs != null && !Int32.TryParse(adIdQs, out adId))
        {
            Response.Redirect("~/Search.aspx");
        }

        if (!Page.IsPostBack)
        {
           if (User.Identity.IsAuthenticated)
            {
                
                this.ResponseContactEmailTextBox.Text =
                    this.EmailSenderAddressTextBox.Text =
                    Membership.GetUser().Email;

                this.ResponseContactNameTextBox.Text =
                    this.EmailSenderNameTextBox.Text = 
                    Profile.FirstName + " " + Profile.LastName;
            }

        }
    }

    protected void AdsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        AdsDataComponent.AdsRow ad = e.ReturnValue as AdsDataComponent.AdsRow;
        if (ad == null)
        {
            Response.Redirect("~/Search.aspx", true);
        }
        else
        {
            // Save the AdId to ViewState for future reference
            this.AdId = ad.Id;

            // fill in Ad Details for "Email Ad" form

            StringBuilder msg = new StringBuilder();
            msg.Append("Ad: ");
            msg.AppendLine(ad.Title);
            
            msg.Append("Category: ");
            msg.AppendLine(ad.CategoryName);

            msg.Append("Price: ");
            msg.AppendFormat("{0:c}", ad.Price);
            msg.AppendLine();
            
            msg.AppendLine();

            msg.Append("URL: ");
            msg.Append(ClassifiedsHttpApplication.SiteUrl);
            if (!ClassifiedsHttpApplication.SiteUrl.EndsWith("/"))
                msg.Append("/");
            msg.Append("ShowAd.aspx?id=");
            msg.AppendLine(ad.Id.ToString());
            
            msg.AppendLine();

            SiteSettings settings = SiteSettings.GetSharedSettings();
            msg.AppendLine(settings.SiteName);
            msg.AppendLine(ClassifiedsHttpApplication.SiteUrl);

            EmailMessageTextBox.Text = msg.ToString();

            EmailSubjectTextBox.Text = "Take a look at: " + ad.Title;

            CategoryListingsLink.Text = ad.CategoryName;
            CategoryListingsLink.NavigateUrl = "~/Search.aspx?c=" + ad.CategoryId.ToString();

            MemberListingsLink.Text = "Other Listings by " + ad.MemberName;
            MemberListingsLink.NavigateUrl = "~/Search.aspx?member=" + ad.MemberName;

            CommonCategoryDropDown.CurrentCategoryId = ad.CategoryId;
            CategoryPath.CurrentCategoryId = ad.CategoryId;

        }
    }

    protected void RespondButton_Click(object sender, EventArgs e)
    {
        SetActivePanel(ResponsePanel);
    }

    protected void EmailAdButton_Click(object sender, EventArgs e)
    {
        SetActivePanel(EmailPanel);
    }

    protected void SaveAdButton_Click(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
        {
            AdsDB.InsertSavedAd(AdId, Profile.MemberId);
            SetActivePanel(AdSavedPanel);
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage();
        }
    }

    protected void ResponseSubmitButton_Click(object sender, EventArgs e)
    {
        ResponseEmailRequired1.Validate();
        if (Page.IsValid)
        {
            bool EmailSentBool = 
                AdsDB.SendResponse(AdId, Server.HtmlEncode(ResponseContactNameTextBox.Text), Server.HtmlEncode(ResponseContactEmailTextBox.Text), Server.HtmlEncode(ResponseCommentsTextBox.Text));
            if (EmailSentBool)
                SetActivePanel(EmailSentPanel);
            else
                SetActivePanel(EmailNotSentPanel);
        }
    }

    protected void PhotoList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("ShowFullSize"))
        {
            int photoId = Convert.ToInt32(e.CommandArgument);
            ShowFullSizePhoto(photoId);
            CurrentPhotoIndex = e.Item.ItemIndex;
        }
    }

    protected void ShowFullSizePhoto(int photoId)
    {
        PhotoPanel.Visible = true;
        FullSizePhoto.ImageUrl = "~/PhotoDisplay.ashx?size=full&photoid=" + photoId.ToString();
        SetActivePanel(PhotoPanel);
    }

    protected void ShowNextPhoto()
    {
        int currPhotoIndex = CurrentPhotoIndex;
        if (currPhotoIndex != -1)
        {
            int nextIndex = (currPhotoIndex + 1) % PhotoList.DataKeys.Count;
            
            int nextPhotoId = Convert.ToInt32(PhotoList.DataKeys[nextIndex]);
            ShowFullSizePhoto(nextPhotoId);

            CurrentPhotoIndex = nextIndex;
        }
    }

    protected void FullSizePhoto_Click(object sender, ImageClickEventArgs e)
    {
        ShowNextPhoto();
    }

    protected void SetActivePanel(Panel panel)
    {
        if (panel == null)
            return;

        ResponsePanel.Visible = false;
        EmailPanel.Visible = false;
        PhotoPanel.Visible = false;
        EmailSentPanel.Visible = false;
        EmailNotSentPanel.Visible = false;
        AdSavedPanel.Visible = false;
        
        panel.Visible = true;
        Page.SetFocus(AdDetailsPanel);
    }
    protected void HidePanel(Panel panel)
    {
        panel.Visible = false;
        Page.SetFocus(AdDetailsPanel);
    }

    protected void EmailSubmitButton_Click(object sender, EventArgs e)
    {
        EmailSenderAddressTextBoxValidator1.Validate();
        EmailRecipientAddressTextBoxValidator1.Validate();
        if (Page.IsValid)
        {
            bool EmailSentBool =
                AdsDB.SendAdInEmail(AdId, Server.HtmlEncode(EmailSenderNameTextBox.Text), Server.HtmlEncode(EmailSenderAddressTextBox.Text), Server.HtmlEncode(EmailRecipientAddressTextBox.Text), Server.HtmlEncode(EmailSubjectTextBox.Text), Server.HtmlEncode(EmailMessageTextBox.Text));
            if (EmailSentBool)
                SetActivePanel(EmailSentPanel);
            else
                SetActivePanel(EmailNotSentPanel);
        }
    }

    protected void HidePhotoPanel_Click(object sender, EventArgs e)
    {
        HidePanel(PhotoPanel);
    }
    protected void CancelResponseButton_Click(object sender, EventArgs e)
    {
        HidePanel(ResponsePanel);
    }
    protected void CancelEmailButton_Click(object sender, EventArgs e)
    {
        HidePanel(EmailPanel);
    }


    protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        Response.Redirect("~/Search.aspx?c=" + e.CategoryId.ToString());
    }
    protected void AdDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem item = e.Item;
        AdsDataComponent.AdsRow ad = item.DataItem as AdsDataComponent.AdsRow;
        if (ad != null)
        {            
            AdTitleLabel.Text = ad.Title;
            AdPriceLabel.Text = String.Format("{0:c}", ad.Price);

            Label ExpirationLabel = item.FindControl("ExpirationLabel") as Label;
            HyperLink URLLink = item.FindControl("URLLink") as HyperLink;

            AdTypeLabel.Text = OutputFormatting.AdTypeToString(ad.AdType);

            if (ad.URL.Equals(String.Empty))
            {
                URLLink.Text = "N/A";
            }
            else
            {
                URLLink.Text =
                   URLLink.NavigateUrl = ad.URL;
                URLLink.Target = "_blank";
            }

            if (ad.IsExpirationDateNull())
            {
                ExpirationLabel.Text = "N/A";
            }
            else
            {
                ExpirationLabel.Text = String.Format("{0:D}", ad.ExpirationDate);
            }

            if ((AdStatus)ad.AdStatus == AdStatus.Activated)
            {
                if (Profile.MemberId != ad.MemberId)
                {
                    // check if the user has recently viewed this page
                    // by looking in a special cookie we set:

                    string adCookieKey = ad.Id.ToString();
                    if (Request.Cookies["ShowAd_ViewCountCookie"] == null || Request.Cookies["ShowAd_ViewCountCookie"][adCookieKey] == null)
                    {
                        Response.Cookies["ShowAd_ViewCountCookie"][adCookieKey] = String.Empty; // String.Empty is not treated as 'null' but saves a byte
                        Response.Cookies["ShowAd_ViewCountCookie"].Expires = DateTime.Now.AddDays(1);
                        AdsDB.IncrementViewCount(ad.Id);
                    }
                }
            }
            else
            {
                AdNotActivePanel.Visible = true;
                AdActions.Visible = false;
            }
        }
    }
}
