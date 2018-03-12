using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class changeregistrationinfo_aspx : Page
{
    protected void ChangePassword1_ContinueClick(object sender, EventArgs e)
    {
        Response.Redirect("~/default.aspx");
    }

}
