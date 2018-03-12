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

public partial class postresume_aspx : Page
{
    private Resume r;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["jobseekerrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        if (!Page.IsPostBack)
        {
            if (Profile.JobSeeker.ResumeID != -1)
            {
                r = Resume.GetResume(Profile.UserName);
                txtJobTitle.Text = r.JobTitle;
                txtCity.Text = r.City;
                txtResume.Text = r.ResumeText;
                txtCoveringLetter.Text = r.CoveringLetterText;
            }
            FillCountries();
            FillEduLevels();
            FillExpLevels();
            FillJobTypes();
            FillStates();
        }
    }

    private void FillCountries()
    {
        ddlCountry.DataSource = Country.GetCountries();
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();

        ddlRelocationCountry.DataSource = Country.GetCountries();
        ddlRelocationCountry.DataTextField = "CountryName";
        ddlRelocationCountry.DataValueField = "CountryID";
        ddlRelocationCountry.DataBind();

        if (Profile.JobSeeker.ResumeID != -1)
        {
            ListItem li;
            li = ddlCountry.Items.FindByValue(r.CountryID.ToString());
            if (li != null)
            {
                ddlCountry.ClearSelection();
                li.Selected = true;
            }

            li = ddlRelocationCountry.Items.FindByValue(r.RelocationCountryID.ToString());
            if (li != null)
            {
                ddlRelocationCountry.ClearSelection();
                li.Selected = true;
            }
        }
    }

    private void FillStates()
    {
        r = Resume.GetResume(Profile.UserName);

        ddlState.DataSource = State.GetStates(int.Parse(ddlCountry.SelectedValue));
        ddlState.DataTextField = "StateName";
        ddlState.DataValueField = "StateID";
        ddlState.DataBind();

        if (Profile.JobSeeker.ResumeID != -1)
        {
            ListItem li;
            li = ddlState.Items.FindByValue(r.StateID.ToString());
            if (li != null)
            {
                ddlState.ClearSelection();
                li.Selected = true;
            }
        }

    }

    private void FillJobTypes()
    {
        ddlJobType.DataSource = JobType.GetJobTypes();
        ddlJobType.DataTextField = "JobTypeName";
        ddlJobType.DataValueField = "JobTypeID";
        ddlJobType.DataBind();

        if (Profile.JobSeeker.ResumeID != -1)
        {
            ListItem li;
            li = ddlJobType.Items.FindByValue(r.JobTypeID.ToString());
            if (li != null)
            {
                ddlJobType.ClearSelection();
                li.Selected = true;
            }
        }
    }

    private void FillEduLevels()
    {
        ddlEduLevel.DataSource = EducationLevel.GetEducationLevels();
        ddlEduLevel.DataTextField = "EducationLevelName";
        ddlEduLevel.DataValueField = "EducationLevelID";
        ddlEduLevel.DataBind();

        if (Profile.JobSeeker.ResumeID != -1)
        {
            ListItem li;
            li = ddlEduLevel.Items.FindByValue(r.EducationLevelID.ToString());
            if (li != null)
            {
                ddlEduLevel.ClearSelection();
                li.Selected = true;
            }
        }
    }

    private void FillExpLevels()
    {
        ddlExpLevel.DataSource = ExperienceLevel.GetExperienceLevels();
        ddlExpLevel.DataTextField = "ExperienceLevelName";
        ddlExpLevel.DataValueField = "ExperienceLevelID";
        ddlExpLevel.DataBind();

        if (Profile.JobSeeker.ResumeID != -1)
        {
            ListItem li;
            li = ddlExpLevel.Items.FindByValue(r.ExperienceLevelID.ToString());
            if (li != null)
            {
                ddlExpLevel.ClearSelection();
                li.Selected = true;
            }
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        Resume r = new Resume();
        r.City = txtCity.Text;
        r.CountryID = int.Parse(ddlCountry.SelectedValue);
        r.CoveringLetterText = txtCoveringLetter.Text;
        r.EducationLevelID = int.Parse(ddlEduLevel.SelectedValue);
        r.ExperienceLevelID = int.Parse(ddlExpLevel.SelectedValue);
        r.JobTitle = txtJobTitle.Text;
        r.JobTypeID = int.Parse(ddlJobType.SelectedValue);
        r.RelocationCountryID = int.Parse(ddlRelocationCountry.SelectedValue);
        r.ResumeText = txtResume.Text;
        r.StateID = int.Parse(ddlState.SelectedValue);
        r.UserName = Profile.UserName;
        r.IsSearchable = "Y";
        r.PostedDate = DateTime.Now;
        if (Profile.JobSeeker.ResumeID != -1)
        {
            r.ResumeID = Profile.JobSeeker.ResumeID;
            Resume.Update(r);
        }
        else
        {
            int retval=Resume.Insert(r);
            Profile.JobSeeker.ResumeID = retval;
        }
        lblMsg.Text = "Your resume is successfully updated!";

    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStates();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/default.aspx");
    }

}
