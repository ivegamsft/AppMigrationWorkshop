using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class MyAds_aspx : System.Web.UI.Page
{
    protected void CurrentAdsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int adId = Convert.ToInt32(e.CommandArgument);
        AdsDB.ExpireAd(adId, Profile.MemberId);
        InactiveAdsGrid.DataBind();
        CurrentAdsGrid.DataBind();
    }

    protected void PendingAds_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        AdsDataComponent.AdsDataTable inactiveAds = e.ReturnValue as AdsDataComponent.AdsDataTable;
        if (inactiveAds != null && inactiveAds.Rows.Count > 0)
            ActivationAdsPanel.Visible = true;
        else
            ActivationAdsPanel.Visible = false;
    }


    protected void CurrentAdsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        LinkButton UnlistButton = e.Row.FindControl("UnlistButton") as LinkButton;
        if (UnlistButton != null)
        {
            UnlistButton.Attributes.Add("onclick", "return confirm('Please confirm that you are unlisting this ad. It will no longer appear among the active listings.');");
        }
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
           // Showing / Hiding "Action" columns for Ads based on site settings;

            SiteSettings settings = SiteSettings.GetSharedSettings();

            // Allow users to edit ads:
            ActivationAdsGrid.Columns[0].Visible = settings.AllowUsersToEditAds; 
            // if user can edit, a link to EditAd is shown
            ActivationAdsGrid.Columns[1].Visible = !settings.AllowUsersToEditAds; 
            // otherwise, the second column (ShowAd link) appears
            CurrentAdsGrid.Columns[CurrentAdsGrid.Columns.Count - 3].Visible = settings.AllowUsersToEditAds;

            // Allow users to upload photos:
            ActivationAdsGrid.Columns[ActivationAdsGrid.Columns.Count - 1].Visible = settings.AllowImageUploads;
            CurrentAdsGrid.Columns[CurrentAdsGrid.Columns.Count - 2].Visible = settings.AllowImageUploads;

           ActivationAdsButtonPanel.Visible = settings.AdActivationRequired;

            if (Request.QueryString["saved"] != null)
                MyAdsMultiView.SetActiveView(SavedAdsView);
        }
    }
    protected void CurrentAdsButton_Click(object sender, EventArgs e)
    {
        MyAdsMultiView.SetActiveView(UserAdsView);
    }
    protected void InactiveAdsButton_Click(object sender, EventArgs e)
    {
        MyAdsMultiView.SetActiveView(UserInactiveAdsView);
    }
    protected void SavedAdsButton_Click(object sender, EventArgs e)
    {
        MyAdsMultiView.SetActiveView(SavedAdsView);
    }
    protected void ActivationAdsButton_Click(object sender, EventArgs e)
    {
        MyAdsMultiView.SetActiveView(UserAdsView);
        Page.SetFocus(ActivationAdsGrid);
    }
}
