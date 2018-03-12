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

public partial class jobsearch_aspx : Page
{
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton b = (LinkButton)e.Row.Cells[4].Controls[0];
            b.CommandName = "viewdetails";
            b.CommandArgument = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "viewdetails")
        {
            Response.Redirect("~/" + ConfigurationManager.AppSettings["jobseekerfolder"] + "/viewjobposting.aspx?id=" + e.CommandArgument);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["jobseekerrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        if (!Page.IsPostBack)
        {
            FillCountries();
            FillStates();
            lblJobCount.Text = "(Currently we have " + JobPosting.GetJobPostingCount() + " jobs!!!)";

            if (Request.QueryString["mysearchid"] != null)
            {
                MySearch s=MySearch.GetMySearch(int.Parse(Request.QueryString["mysearchid"]));
                txtSkills.Text = s.Criteria;
                txtCity.Text = s.City;
                ddlCountry.SelectedIndex = s.CountryID;
                FillStates();
                ListItem li= ddlState.Items.FindByValue(s.StateID.ToString());
                if (li != null)
                {
                    ddlState.ClearSelection();
                    li.Selected = true;
                }
                
            }
        }
    }

    private void FillCountries()
    {
        ddlCountry.DataSource = Country.GetCountries();
        ddlCountry.DataTextField = "CountryName";
        ddlCountry.DataValueField = "CountryID";
        ddlCountry.DataBind();
    }

    private void FillStates()
    {
        ddlState.DataSource = State.GetStates(int.Parse(ddlCountry.SelectedValue));
        ddlState.DataTextField = "StateName";
        ddlState.DataValueField = "StateID";
        ddlState.DataBind();
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStates();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void BindGrid()
    {
        int countryid = -1, stateid = -1;
        if (ddlCountry.SelectedItem != null)
            countryid = int.Parse(ddlCountry.SelectedValue);
        if (ddlState.SelectedItem != null)
            stateid = int.Parse(ddlState.SelectedValue);

        DataSet ds = JobPosting.SearchJobs(txtSkills.Text, countryid, stateid);
        GridView1.DataSource = ds;
        GridView1.DataBind();

        if (GridView1.Rows.Count <= 0)
        {
            lblMsg.Text = "No records found!";
        }
        else
        {
            lblMsg.Text = "";
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/default.aspx");
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void btnMySearches_Click(object sender, EventArgs e)
    {
        MySearch s = new MySearch();
        s.Criteria = txtSkills.Text;
        s.CountryID = int.Parse(ddlCountry.SelectedValue);
        s.StateID = int.Parse(ddlState.SelectedValue);
        s.City = txtCity.Text;
        s.UserName = Profile.UserName;
        MySearch.Insert(s);
    }

}
