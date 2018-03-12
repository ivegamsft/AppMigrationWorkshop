using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class Register_aspx : System.Web.UI.Page
{
    protected void CreateUserWizardControl_CreatedUser(object sender, EventArgs e)
    {
        string username = CreateUserWizardControl.UserName;

        ProfileCommon userProfile = Profile.GetProfile(CreateUserWizardControl.UserName);
        if (userProfile != null)
        {
            userProfile.MemberId = MembersDB.CreateMember(username, Membership.ApplicationName);
            
            userProfile.FirstName = (Util.FindControlRecursively("FirstName", CreateUserWizardControl.Controls) as TextBox).Text;
            userProfile.LastName = (Util.FindControlRecursively("LastName", CreateUserWizardControl.Controls) as TextBox).Text;

            userProfile.Save();            
        }
    }
    protected void CreateUserWizardControl_CreateUserError(object sender, CreateUserErrorEventArgs e)
    {
        InfoLabel.Visible = true;
        System.Text.StringBuilder mailLink = new System.Text.StringBuilder(this.GetMailLink());

        // MembershipCreateStatus.InvalidAnswer not used.
        // MembershipCreateStatus.InvalidQuestion not used.
        switch (e.CreateUserError)
        {
            case MembershipCreateStatus.DuplicateUserName:
                InfoLabel.Text = "User name already exists. " +
                    "Please enter a different user name.";
                break;

            case MembershipCreateStatus.DuplicateEmail:
                InfoLabel.Text = "A user name for that e-mail address already exists. " + 
                    "Please enter a different e-mail address.";
                break;

            case MembershipCreateStatus.InvalidPassword:
                InfoLabel.Text = "The password provided is invalid. " +
                    "Please enter a valid password . " +
                    "Acceptable passwords are at least 7 characters long and contain at least 1 non-alphanumeric character.";
                break;

            case MembershipCreateStatus.InvalidEmail:
                InfoLabel.Text = "The e-mail address provided is invalid. Please check the value and try again.";
                break;

            case MembershipCreateStatus.InvalidUserName:
                InfoLabel.Text = "The user name provided is invalid. " +
                    "Please check the value and try again.";
                break;

            case MembershipCreateStatus.ProviderError:
                InfoLabel.Text = "The authentication provider returned an error. " +
                    "Please verify your entry and try again. " +
                    "If the problem persists, please contact the " + 
                    mailLink.ToString() + ".";
                break;

            case MembershipCreateStatus.UserRejected:
                InfoLabel.Text = "The user creation request has been canceled. " +
                    "Please verify your entry and try again. " +
                    "If the problem persists, please contact the " +
                    mailLink.ToString() + ".";
                break;

            default:
                InfoLabel.Text = "An unknown error occurred. " +
                    "Please verify your entry and try again. " +
                    "If the problem persists, please contact the " +
                    mailLink.ToString() + ".";
                break;

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        InfoLabel.Visible = false;
    }
    protected void FirstNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        TextBox FirstNameTextBox = CreateUserWizardStep1.Controls[0].FindControl("FirstName") as TextBox;
        if (Regex.IsMatch(FirstNameTextBox.Text, @"^[0-9\-\p{L}\p{Zs}\p{Lu}\p{Ll}\']{1,40}$"))
        {
            args.IsValid = true;
        }
    }
    protected void LastNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        TextBox LastNameTextBox = CreateUserWizardStep1.Controls[0].FindControl("LastName") as TextBox;
        if (Regex.IsMatch(LastNameTextBox.Text, @"^[0-9\-\p{L}\p{Zs}\p{Lu}\p{Ll}\']{1,40}$"))
        {
            args.IsValid = true;
        }
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        Response.Redirect("~/Error.aspx");
    }

    private String GetMailLink()
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
        return mailLink.ToString();
    }

}
