using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using ASPNET.StarterKit.BusinessLogicLayer;

public partial class Report_Project_Result_aspx : System.Web.UI.Page {
    public Report_Project_Result_aspx() {
    }

    void Page_Load(object sender, EventArgs e) {

    }

    protected void EntryListDataBinding(object sender, EventArgs args) {
        DataList temp = (DataList)sender;
        if (temp.Items.Count <= 0) {
            temp.ShowHeader = false;
            temp.ShowFooter = false;
        }
        else {
            temp.ShowHeader = true;
            temp.ShowFooter = true;
        }
    }

    protected void OnCategoryListItemCreated(object sender, DataListItemEventArgs e) {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
            ObjectDataSource ds = e.Item.FindControl("UserReportData") as ObjectDataSource;
            if (ds != null && (DataBinder.Eval(e.Item.DataItem, "Id") != null)) {
                ds.SelectParameters["CategoryId"].DefaultValue = DataBinder.Eval(e.Item.DataItem, "Id").ToString();
            }
        }
    }

    protected void OnProjectListItemCreated(object sender, DataListItemEventArgs e) {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
            ObjectDataSource ds = e.Item.FindControl("CategorReportData") as ObjectDataSource;
            if (ds != null && (DataBinder.Eval(e.Item.DataItem, "Id") != null)) {
                ds.SelectParameters["ProjectId"].DefaultValue = DataBinder.Eval(e.Item.DataItem, "Id").ToString();
            }

        }
    }
}
