using System;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            MailLink.NavigateUrl = "mailto:" +
                SiteSettings.GetSharedSettings().SiteEmailAddress;
        }
        catch
        {
            MailLink.NavigateUrl = "#";
        }
    }
}
