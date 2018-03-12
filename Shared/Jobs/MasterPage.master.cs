using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage_master : MasterPage
{
    public MasterPage_master()
    {
        Load += new EventHandler(Page_Load);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = ConfigurationManager.AppSettings["pagetitle"];
        HyperLink1.ImageUrl = "~/" + ConfigurationManager.AppSettings["imagefolder"] + "/" + ConfigurationManager.AppSettings["sitelogo"];
        lnkAds.NavigateUrl = "mailto:" + ConfigurationManager.AppSettings["advertiseemail"];
        lnkWebmaster.NavigateUrl = "mailto:" + ConfigurationManager.AppSettings["webmasteremail"];
    }

}
