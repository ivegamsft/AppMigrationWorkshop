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

public partial class Statistics_ascx : UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblCompanies.Text = Company.GetCompanyCount().ToString();
        lblJobs.Text = JobPosting.GetJobPostingCount().ToString();
        lblResumes.Text = Resume.GetResumeCount().ToString();
    }

}
