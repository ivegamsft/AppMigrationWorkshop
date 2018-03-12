using System;
using System.Text;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using AspNet.StarterKits.Classifieds.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;


public partial class PostAd_aspx : System.Web.UI.Page
{
    public int PreviousAdId
    {
        get
        {
            if (ViewState["PreviousAdId"] != null)
                return (int)ViewState["PreviousAdId"];
            else
                return 0;
        }
        set
        {
            ViewState["PreviousAdId"] = value;
        }
    }
    public bool IsPreviousAd
    {
        get
        {
            return (ViewState["PreviousAdId"] != null);
        }
    }

    private const string OtherLocationText = "Other...";

    protected void PostAdWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (Page.IsValid)
        {
            int memberId = Profile.MemberId;
            int categoryId = CategoryPath.CurrentCategoryId;
            string title = Server.HtmlEncode(TitleTextBox.Text);
            string description = Server.HtmlEncode(DescriptionTextBox.Text);
            string url = Server.UrlEncode(UrlTextBox.Text);
            Decimal price = Decimal.Parse(PriceTextBox.Text);

            string location = Server.HtmlEncode(LocationDropDown.CurrentLocation);

            int numDays = Convert.ToInt32(NumDaysList.SelectedValue);

            AdType adType = AdType.ForSale;

            if (Enum.IsDefined(typeof(AdType), Convert.ToInt32(AdTypeSelection.SelectedValue)))
                adType = (AdType)Enum.Parse(typeof(AdType), AdTypeSelection.SelectedValue);

            if (IsPreviousAd)
            {
                AdsDB.RelistAd(PreviousAdId, 
                        categoryId,
                        title,
                        description,
                        url,
                        price,
                        location,
                        numDays,
                        AdLevel.Unspecified,
                        AdStatus.Unspecified,
                        adType);

                Response.Redirect("~/MyAds.aspx", true);

            }
            else
            {
                int adId = AdsDB.InsertAd(memberId,
                        categoryId,
                        title,
                        description,
                        url,
                        price,
                        location,
                        numDays,
                        AdLevel.Unspecified,
                        AdStatus.Unspecified,
                        adType);

                SiteSettings s = SiteSettings.GetSharedSettings();
                UploadImagesLink.Visible = s.AllowImageUploads;
                UploadImagesLink.NavigateUrl = "~/ManagePhotos.aspx?id=" + adId.ToString();
            }

        }
        else
            e.Cancel = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Limit Description text.
        DescriptionTextBox.Attributes.Add("onkeydown", "textCounter(this,500);");
        DescriptionTextBox.Attributes.Add("onkeyup", "textCounter(this,500);");

        if (!Page.IsPostBack)
        {
            
            PostAdWizard.MoveTo(PostAdWizard.WizardSteps[0]);

            string qsRelistId = Request.QueryString["relist"];
            if (qsRelistId != null)
            {
                int adId;
                if (Int32.TryParse(qsRelistId, out adId))
                {
                    LoadPreviousAd(adId);
                }
            }
        }
    }

    protected void LoadPreviousAd(int adId)
    {
        AdsDataComponent.AdsRow ad = AdsDB.GetAdById(adId);
        if (ad != null)
        {
            if (ad.MemberId == Profile.MemberId)
            {
                PreviousAdId = adId;

                SetCurrentCategory(ad.CategoryId);

                if ((AdType)ad.AdType == AdType.Wanted)
                    AdTypeSelection.SelectedIndex = 1;
                else
                    AdTypeSelection.SelectedIndex = 0;

                TitleTextBox.Text = ad.Title;
                DescriptionTextBox.Text = ad.Description;
                UrlTextBox.Text = ad.URL;
                LocationDropDown.CurrentLocation = ad.Location;
            }
        }
    }

    protected void UpdateCategoryDisplay()
    {
        this.CategoryPathLabel.Text = CategoryPath.FullCategoryPath;
    }

    protected void SubcategoriesDS_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        List<CachedCategory> subCategories = e.ReturnValue as List<CachedCategory>;
        if (subCategories == null || subCategories.Count == 0)
        {
            PostAdWizard.MoveTo(PostAdWizard.WizardSteps[1]);
        }
    }

    protected void SetCurrentCategory(int categoryId)
    {
        CategoryPath.CurrentCategoryId = categoryId;
        UpdateCategoryDisplay();
    }

    protected void SubcategoriesList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CommandArgument);
        SetCurrentCategory(categoryId);
    }

    protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        UpdateCategoryDisplay();
    }

    protected void ChangeCategoryButton_Click(object sender, EventArgs e)
    {
        SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        PostAdWizard.MoveTo(PostAdWizard.WizardSteps[0]);
    }
    protected void PostAdWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (e.NextStepIndex == 0)
        {
            SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        }
    }
    protected void ValidLocationRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !LocationDropDown.CurrentLocation.Equals(String.Empty);
    }
    protected void PriceValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        decimal p = -1;
        if (decimal.TryParse(PriceTextBox.Text, out p))
        {
            args.IsValid = (p >= 0);
        }
        else
            args.IsValid = false;
    }
    protected void URLValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        TextBox URLTextBox = AdDetailsStep.FindControl("UrlTextBox") as TextBox;
        if (URLTextBox != null && !URLTextBox.Text.Equals(String.Empty))
        {
            try
            {
                Uri uri = new Uri(URLTextBox.Text);
                if (uri.IsWellFormedOriginalString() &
                    (uri.Scheme == "http" | uri.Scheme == "https"))
                {
                    args.IsValid = true;
                }
            }
            catch
            {
            }
        }
        else
        {
            // Empty URL is okay.
            args.IsValid = true;
        }
    }
}
