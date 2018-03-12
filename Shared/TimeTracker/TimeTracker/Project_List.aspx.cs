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

public partial class Project_List_aspx : System.Web.UI.Page {
  public Project_List_aspx() {
  }
  void Page_Load(object sender, EventArgs e) {
      if (Page.User.IsInRole("ProjectAdministrator")) {
        ProjectData.SortParameterName = "sortParameter";
        ProjectData.SelectMethod = "GetAllProjects";
      }
      else {

          bool wasFound = false;
          foreach (Parameter parameter in ProjectData.SelectParameters)
          {
              if (parameter.Name == "userName")
                  wasFound = true;
          }
          if (!wasFound) {
              Parameter param = new Parameter("userName", TypeCode.String, Page.User.Identity.Name);
              ProjectData.SelectParameters.Add(param);
          }
        ProjectData.SortParameterName = "sortParameter";
        ProjectData.SelectMethod = "GetProjectsByManagerUserName";
      }
  }
  protected void Button_Click(Object sender, EventArgs args) {
    Server.Transfer("Project_Details.aspx");
  }
    protected void ListAllProjects_RowDeleting(object sender, GridViewDeleteEventArgs e) {
        
        e.Keys.Add("Id", ListAllProjects.Rows[e.RowIndex].Cells[0].Text);
    }
}
