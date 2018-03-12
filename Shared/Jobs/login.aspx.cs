using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class login_aspx : Page
{

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        if (Membership.ValidateUser(Login1.UserName, Login1.Password))
        {
            FormsAuthentication.SetAuthCookie(Login1.UserName, Login1.RememberMeSet);
            Response.Redirect(FormsAuthentication.GetRedirectUrl(Login1.UserName, Login1.RememberMeSet));
        }
    }

}
