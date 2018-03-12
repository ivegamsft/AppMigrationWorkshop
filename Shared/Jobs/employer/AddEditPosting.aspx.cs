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

public partial class AddEditPosting_aspx : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["employerrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        if (!Page.IsPostBack)
        {
            if (Company.GetCompany(User.Identity.Name) == null)
            {
                Response.Redirect("~/customerrorpages/profilenotfound.aspx");
            }

            if (Request.QueryString["id"] == null)
            {
                DetailsView1.DefaultMode = DetailsViewMode.Insert;
            }
            else
            {
                DetailsView1.DefaultMode = DetailsViewMode.ReadOnly;
            }
        }
    }
    protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        
        DropDownList ddl;
        ddl = (DropDownList)DetailsView1.FindControl("ddlStateInsert");
        e.Values["StateID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlCountryInsert");
        e.Values["CountryID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlEduLevelInsert");
        e.Values["EducationLevelID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlJobTypeInsert");
        e.Values["JobTypeID"] = ddl.SelectedValue;

        e.Values["PostedBy"] = Profile.UserName;
        e.Values["CompanyID"]=Profile.Employer.CompanyID.ToString();
        e.Values["PostingDate"]= DateTime.Today.ToString("MM/dd/yyyy");

    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        DropDownList ddl;
        ddl = (DropDownList)DetailsView1.FindControl("ddlStateUpdate");
        e.NewValues["StateID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlCountryUpdate");
        e.NewValues["CountryID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlEduLevelUpdate");
        e.NewValues["EducationLevelID"] = ddl.SelectedValue;

        ddl = (DropDownList)DetailsView1.FindControl("ddlJobTypeUpdate");
        e.NewValues["JobTypeID"] = ddl.SelectedValue;

        e.NewValues["PostedBy"] = Profile.UserName;
        e.NewValues["CompanyID"] = Profile.Employer.CompanyID.ToString();
        e.NewValues["PostingDate"] = DateTime.Today.ToString("MM/dd/yyyy");

    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        Response.Redirect("~/" + ConfigurationManager.AppSettings["employerfolder"] + "/jobpostings.aspx");
    }

    protected void ddlCountryUpdate_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl;
        ddl = (DropDownList)sender;
        ObjectDataSource2.SelectParameters["countryid"].DefaultValue = ddl.SelectedValue;
        ObjectDataSource2.Select();
    }

    protected void ddlCountryInsert_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl;
        ddl = (DropDownList)sender;
        ObjectDataSource2.SelectParameters["countryid"].DefaultValue = ddl.SelectedValue;
        ObjectDataSource2.Select();

    }

    protected void DetailsView1_DataBound(object sender, EventArgs e)
    {
        DropDownList ddl;
        ddl = (DropDownList)DetailsView1.FindControl("ddlCountryUpdate");
        if (ddl != null)
        {
            ObjectDataSource2.SelectParameters["countryid"].DefaultValue = ddl.SelectedValue;
            ObjectDataSource2.Select();

        }

    }

}
