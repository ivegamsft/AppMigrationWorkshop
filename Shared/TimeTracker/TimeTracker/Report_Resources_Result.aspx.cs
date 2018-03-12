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

public partial class Report_Resources_Result_aspx : System.Web.UI.Page
{
    public Report_Resources_Result_aspx()
    {
    }

    void Page_Load(object sender, EventArgs e)
    {

    }

    protected void OnListUserTimeEntriesItemCreated(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            ObjectDataSource ds = e.Item.FindControl("TimeEntryData") as ObjectDataSource;
            if (ds != null && (DataBinder.Eval(e.Item.DataItem, "UserName") != null))  {
                ds.SelectParameters["userName"].DefaultValue = DataBinder.Eval(e.Item.DataItem, "UserName").ToString();
            }
        }
    }
}