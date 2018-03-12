using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class Login_aspx : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            AccessNoticePanel.Visible = (Request.QueryString["ReturnUrl"] != null);
        }
        else
            AccessNoticePanel.Visible = false;

    }
    protected void ForgotPasswordButton_Click(object sender, EventArgs e)
    {
        this.PasswordRecovery.Visible = true;
    }

    protected void PasswordRecovery_Init(object sender, EventArgs e)
    {
        SiteSettings s = SiteSettings.GetSharedSettings();
        PasswordRecovery.MailDefinition.From = s.SiteEmailFromField;

    }
    protected void PasswordRecovery_SendMailError(object sender, SendMailErrorEventArgs e)
    {
        System.Text.StringBuilder mailLink = new System.Text.StringBuilder("<a href=\"mailto:");
        try
        {
            mailLink.Append(SiteSettings.GetSharedSettings().SiteEmailAddress);
        }
        catch
        {
            mailLink.Append("#");
        }
        mailLink.Append("\">system administrator</a>");
        PasswordRecovery.SuccessText = "A problem occurred sending the email. " +
            "Please contact the " + mailLink.ToString() + ".";
        e.Handled = true;
    }
}
