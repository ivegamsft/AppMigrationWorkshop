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


public partial class viewcompanyprofile_aspx : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Roles.IsUserInRole(ConfigurationManager.AppSettings["jobseekerrolename"]))
        {
            Response.Redirect("~/customerrorpages/NotAuthorized.aspx");
        }

        if (!Page.IsPostBack)
        {
            Company objCompany = Company.GetCompany(int.Parse(Request.QueryString["id"]));
            if (objCompany != null)
            {
                lblName.Text = objCompany.CompanyName;
                lblAddress1.Text = objCompany.Address1;
                lblAddress2.Text = objCompany.Address2;
                lblCity.Text = objCompany.City;
                lblState.Text=State.GetStateName(objCompany.StateID);
                lblCountry.Text = Country.GetCountryName(objCompany.CountryID);
                lblZIP.Text = objCompany.ZIP;
                lblPhone.Text = objCompany.Phone;
                lblFax.Text = objCompany.Fax;
                lblEmail.Text = objCompany.Email;
                lblWebSite.Text = objCompany.WebSiteUrl;
                lblProfile.Text = objCompany.BriefProfile;
            }
        }
    }

}
