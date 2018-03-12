using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using ASPNET.StarterKit.BusinessLogicLayer;

public partial class TimeEntry_aspx : System.Web.UI.Page {
  public TimeEntry_aspx() {
  }

  void Page_Load(object sender, EventArgs e) {
    if (!Page.IsPostBack) {
      if ((Page.User.IsInRole("ProjectAdministrator") || Page.User.IsInRole("ProjectManager"))) {
        UserList.DataSourceID = "ProjectMembers";

        if (Page.User.IsInRole("ProjectAdministrator")) {
          ProjectData.SortParameterName = "sortParameter";
          ProjectData.SelectMethod = "GetAllProjects";
        }
        else if (Page.User.IsInRole("ProjectManager")) {

          ProjectData.SelectParameters.Add(new Parameter("userName", TypeCode.String, Page.User.Identity.Name));
          ProjectData.SortParameterName = "sortParameter";
          ProjectData.SelectMethod = "GetProjectsByManagerUserName";
        }

      }
      else {
        ProjectData.SelectParameters.Add(new Parameter("userName", TypeCode.String, Page.User.Identity.Name));
        ProjectData.SelectMethod = "GetProjectsByUserName";

        UserList.Items.Add(Page.User.Identity.Name);
      }
      ProjectList.DataBind();
      UserList.DataBind();
      ProjectListGridView.DataBind();

      if (ProjectList.Items.Count >= 1)
      {
          TimeEntryView.Visible = true;
          MessageView.Visible = false;
      }
      else
      {
          TimeEntryView.Visible = false;
          MessageView.Visible = true;
      }
    }
    if (UserList.Items.Count == 0) {
        AddEntry.Enabled = false;
    }
  }

  protected void AddEntry_Click(object sender, System.EventArgs e) {
                
    TimeEntry timeEntry = new TimeEntry(Page.User.Identity.Name, Convert.ToInt32(CategoryList.SelectedValue), Convert.ToDecimal(Hours.Text), DateTime.Now, UserList.SelectedValue);
    timeEntry.Description = Description.Text;
    timeEntry.Save();

    Description.Text = string.Empty;
    Hours.Text = string.Empty;
    ProjectListGridView.DataBind();

  }

  protected void Cancel_Click(object sender, EventArgs args) {
    Description.Text = string.Empty;
    Hours.Text = string.Empty;
  }

    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args) {

        if (Description.Text.Length > 200)
            args.IsValid = false;
        else
            args.IsValid = true;
    }
    protected void ProjectListGridView_RowDeleting(object sender, GridViewDeleteEventArgs e) {
     }
    protected void ProjectListGridView_RowUpdated(object sender, GridViewUpdatedEventArgs e) {
     
    }
}
