using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class EducationLevelsManager_aspx : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["adminrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        if (DetailsView1.Rows.Count < 1)
        {
            DetailsView1.DefaultMode = DetailsViewMode.Insert;
        }
        else
        {
            DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
        }

    }

}
