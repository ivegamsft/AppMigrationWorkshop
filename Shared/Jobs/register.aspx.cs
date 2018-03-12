using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class register_aspx : Page
{

    protected void CreateUserWizard1_ContinueButtonClick(object sender, EventArgs e)
    {
        Response.Redirect("~/default.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void CreateUserWizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (CreateUserWizard1.ActiveStep.ID == "WizardStep2")
        {
            TextBox t = ((TextBox)CreateUserWizard1.ActiveStep.FindControl("TextBox1"));
            ViewState["firstname"]=t.Text;
            t = ((TextBox)CreateUserWizard1.ActiveStep.FindControl("TextBox2"));
            ViewState["lastname"]=t.Text;
        }

        if (CreateUserWizard1.ActiveStep.ID == "WizardStep1")
        {
            MembershipUser objUser = Membership.GetUser();
            DropDownList ddl = ((DropDownList)CreateUserWizard1.ActiveStep.FindControl("DropDownList1"));
            if (ddl != null)
            {
                Roles.AddUserToRole(objUser.UserName, ddl.SelectedValue);
            }
            Profile.UserName = objUser.UserName;
            Profile.Email = objUser.Email;
            Profile.FirstName=ViewState["firstname"].ToString();
            Profile.LastName=ViewState["lastname"].ToString();
            Profile.JobSeeker.ResumeID = -1;
            Profile.Employer.CompanyID = -1;
        }
    }

    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {

    }



    protected void CreateUserWizard1_ActiveStepChanged(object sender, EventArgs e)
    {
        if (CreateUserWizard1.ActiveStep.ID == "WizardStep1")
        {
            DropDownList ddl = ((DropDownList)CreateUserWizard1.ActiveStep.FindControl("DropDownList1"));
            if (ddl != null)
            {
                ListItem li1 = new ListItem("Job Seeker", ConfigurationManager.AppSettings["jobseekerrolename"]);
                ListItem li2 = new ListItem("Employer", ConfigurationManager.AppSettings["employerrolename"]);
                ddl.Items.Add(li1);
                ddl.Items.Add(li2);
            }

        }
    }
}
