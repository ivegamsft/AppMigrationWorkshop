using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class Activations_aspx : System.Web.UI.Page
{
    protected void ApprovalsGrid_DataBound(object sender, EventArgs e)
    {
        UpdateDeletedCount();
    }

    protected void UpdateDeletedCount()
    {
        int numDeleted = Stats.CountAdsByStatus(AdStatus.Deleted);
        NumDeletedLabel.Text = numDeleted.ToString();
        RemoveDeletedAdsButton.Visible = (numDeleted > 0);
    }

    protected void RemoveDeletedAdsButton_Click(object sender, EventArgs e)
    {
        AdsDB.RemoveFromDatabaseByStatus(AdStatus.Deleted);
        UpdateDeletedCount();
    }


    protected void ActivationsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        AdsDataComponent.AdsDataTable activations = e.ReturnValue as AdsDataComponent.AdsDataTable;
        OkButton.Enabled =
            ActivationActionsList.Enabled = (activations != null && activations.Rows.Count > 0);
    }

    protected void OkButton_Click(object sender, EventArgs e)
    {
        if (ActivationActionsList.SelectedValue != null)
        {
            switch (ActivationActionsList.SelectedValue)
            {
                case "Approve":
                    ApproveSelected();
                    break;
                case "MarkDeleted":
                    MarkSelectedDeleted();
                    break;
                case "RemoveFromDB":
                    RemoveSelectedFromDB();
                    break;
                case "ApproveAll":
                    ApproveAll();
                    break;
            }
        }
    }

    protected void ApprovalsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Approve":
                int adId = Convert.ToInt32(e.CommandArgument);
                AdsDB.UpdateAdStatus(adId, AdStatus.Activated);
                ApprovalsGrid.DataBind();
                break;
            case "MarkDeleted":
                adId = Convert.ToInt32(e.CommandArgument);
                AdsDB.UpdateAdStatus(adId, AdStatus.Deleted);
                ApprovalsGrid.DataBind();
                break;
            case "RemoveFromDB":
                adId = Convert.ToInt32(e.CommandArgument);
                AdsDB.RemoveFromDatabase(adId);
                ApprovalsGrid.DataBind();
                break;
        }
    }

    protected void ApproveSelected()
    {
        List<int> adList = GetSelectedAds();
        if (adList.Count > 0)
        {
            AdsDB.UpdateAdStatusList(adList, AdStatus.Activated);
            ApprovalsGrid.DataBind();
        }
    }
    protected void MarkSelectedDeleted()
    {
        List<int> adList = GetSelectedAds();
        if (adList.Count > 0)
        {
            AdsDB.UpdateAdStatusList(adList, AdStatus.Deleted);
            ApprovalsGrid.DataBind();
        }

    }
    protected void RemoveSelectedFromDB()
    {
        List<int> adList = GetSelectedAds();
        if (adList.Count > 0)
        {
            AdsDB.RemoveListFromDatabase(adList);
            ApprovalsGrid.DataBind();
        }
    }

    protected void ApproveAll()
    {
        if (ApprovalsGrid.Rows.Count > 0)
        {
            List<int> adList = new List<int>(ApprovalsGrid.PageSize);
            foreach (GridViewRow r in ApprovalsGrid.Rows)
            {
                DataKey idKey = ApprovalsGrid.DataKeys[r.RowIndex];
                int adId = Convert.ToInt32(idKey.Value);
                adList.Add(adId);
            }
            AdsDB.UpdateAdStatusList(adList, AdStatus.Activated);
            ApprovalsGrid.DataBind();
        }
    }

    protected List<int> GetSelectedAds()
    {
        List<int> adList = new List<int>(ApprovalsGrid.PageSize);
        foreach (GridViewRow r in ApprovalsGrid.Rows)
        {
            CheckBox box = r.FindControl("AdCheckBox") as CheckBox;
            if (box != null && box.Checked)
            {
                DataKey idKey = ApprovalsGrid.DataKeys[r.RowIndex];
                int adId = Convert.ToInt32(idKey.Value);
                adList.Add(adId);
            }
        }
        return adList;
    }

    

}  