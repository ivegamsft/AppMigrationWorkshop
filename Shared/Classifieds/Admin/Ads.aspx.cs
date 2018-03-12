using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class Ads_aspx : System.Web.UI.Page
{
    protected List<int> GetSelectedAds()
    {
        List<int> adList = new List<int>(AdsGrid.PageSize);
        foreach (GridViewRow r in AdsGrid.Rows)
        {
            CheckBox box = r.FindControl("AdCheckBox") as CheckBox;
            if (box != null && box.Checked)
            {
                DataKey idKey = AdsGrid.DataKeys[r.RowIndex];
                int adId = Convert.ToInt32(idKey.Value);
                adList.Add(adId);
            }
        }
        return adList;
    }

    protected void SelectedActionButton_Click(object sender, EventArgs e)
    {
        List<int> ads = GetSelectedAds();
        if (ads.Count > 0)
        {
            switch (this.SelectedActionList.SelectedValue)
            {
                case "FeaturedLevel":
                    AdsDB.UpdateAdLevelList(ads, AdLevel.Featured);
                    break;
                case "NormalLevel":
                    AdsDB.UpdateAdLevelList(ads, AdLevel.Normal);
                    break;
                case "Activate":
                    AdsDB.UpdateAdStatusList(ads, AdStatus.Activated);
                    break;
                case "Pending":
                    AdsDB.UpdateAdStatusList(ads, AdStatus.ActivationPending);
                    break;
                case "Unlist":
                    AdsDB.UpdateAdStatusList(ads, AdStatus.Inactive);
                    break;
                case "MarkDeleted":
                    AdsDB.UpdateAdStatusList(ads, AdStatus.Deleted);
                    break;
                case "Remove":
                    AdsDB.RemoveListFromDatabase(ads);
                    break;
            }
            AdsGrid.DataBind();

        }

    }
    protected void UserNameTextBox_TextChanged(object sender, EventArgs e)
    {
        ProfileCommon userProfile = Profile.GetProfile(Server.HtmlEncode(UserNameTextBox.Text));
        if (userProfile != null && userProfile.MemberId != DefaultValues.IdNullValue)
            AdsDataSource.SelectParameters["memberId"].DefaultValue = userProfile.MemberId.ToString();
        else
            AdsDataSource.SelectParameters["memberId"].DefaultValue = "-1"; // the user was not found, use -1 to return zero records
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Page.PreviousPage != null)
            {
                TextBox CommonSearchTextBox = Util.FindControlRecursively("CommonSearchTextBox", Page.PreviousPage.Controls) as TextBox;
                DropDownList CommonWhatsNewRangeList = Util.FindControlRecursively("CommonWhatsNewRangeList", Page.PreviousPage.Controls) as DropDownList;
                if (CommonSearchTextBox != null && !CommonSearchTextBox.Text.Equals(String.Empty))
                    QueryTextBox.Text = Server.HtmlEncode(CommonSearchTextBox.Text);
                else if (CommonWhatsNewRangeList != null)
                    DateRangeDropDown.SelectedIndex = CommonWhatsNewRangeList.SelectedIndex;
            }
        }
    }
    protected void QueryButton_Click(object sender, EventArgs e)
    {
        WelcomePanel.Visible = false;
        ResultsPanel.Visible = true;
    }
    protected void AdsGrid_DataBound(object sender, EventArgs e)
    {
        AdActionsPanel.Visible = AdsGrid.Rows.Count > 0;
    }

}
