using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class MyProfile_aspx : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            FirstNameTextBox.Text = Server.HtmlDecode(Profile.FirstName);
            LastNameTextBox.Text = Server.HtmlDecode(Profile.LastName);
            EmailTextBox.Text = Membership.GetUser().Email;
        }
    }
    protected void SaveButton_Click(object sender, EventArgs e)
    {
        EmailNotValid.Visible = false;
        Page.Validate("ChangeProfile");
        if (Page.IsValid)
        {
            Profile.FirstName = Server.HtmlEncode(FirstNameTextBox.Text);
            Profile.LastName = Server.HtmlEncode(LastNameTextBox.Text);
            
            MembershipUser u = Membership.GetUser();
            u.Email = Server.HtmlEncode(EmailTextBox.Text);
            try
            {
                Membership.UpdateUser(u);
                BackToMyAds();
            }
            catch (Exception ex)
            {
                EmailNotValid.Text = "This Email is in use or not allowed.";
                EmailNotValid.Visible = true;
            }

        }
    }

    protected void CancelButton_Click(object sender, EventArgs e)
    {
        BackToMyAds();
    }

    protected void BackToMyAds()
    {
        Response.Redirect("~/MyAds.aspx");
    }
    protected void ChangePasswordControl_ChangedPassword(object sender, EventArgs e)
    {
        Page.SetFocus(ChangePasswordControl);
    }
    protected void FirstNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        if (Regex.IsMatch(FirstNameTextBox.Text, @"^[0-9\-\p{L}\p{Zs}\p{Lu}\p{Ll}\']{1,40}$"))
        {
            args.IsValid = true;
        }
    }
    protected void LastNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;
        if (Regex.IsMatch(LastNameTextBox.Text, @"^[0-9\-\p{L}\p{Zs}\p{Lu}\p{Ll}\']{1,40}$"))
        {
            args.IsValid = true;
        }
    }


}
