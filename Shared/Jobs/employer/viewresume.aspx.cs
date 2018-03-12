using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JobSiteStarterKit.BOL;
using ASP;

public partial class viewresume_aspx : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["employerrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        Resume r = Resume.GetResume(int.Parse(Request.QueryString["id"]));

        ProfileCommon p = Profile.GetProfile(r.UserName);
        lblName.Text = "Full Name : " + p.FirstName + " " + p.LastName;
        lblEducation.Text = "Education Level : " + EducationLevel.GetEducationLevelName(r.EducationLevelID);
        lblExperience.Text = "Experience Level : " + ExperienceLevel.GetExperienceLevelName(r.ExperienceLevelID);
        lblCoveringLetter.Text = r.CoveringLetterText.Replace("\n", "<br>");
        lblResume.Text = r.ResumeText.Replace("\n","<br>");

    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/" + ConfigurationManager.AppSettings["employerfolder"] + "/resumesearch.aspx");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        MyResume r = new MyResume();
        r.ResumeID = int.Parse(Request.QueryString["id"]);
        r.UserName = Profile.UserName;
        MyResume.Insert(r);
    }

}
