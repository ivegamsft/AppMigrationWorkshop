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

public partial class Login_aspx : System.Web.UI.Page {
  public Login_aspx() {
  }

  void Page_Load(object sender, EventArgs e) {
      string isAllowUserCreationForAnonymousUsers = ConfigurationManager.AppSettings["AllowUserCreationForAnonymousUsers"];
      if (String.IsNullOrEmpty(isAllowUserCreationForAnonymousUsers))
          return ;

      if (Page.User.Identity.IsAuthenticated || (!Page.User.Identity.IsAuthenticated && String.Compare(isAllowUserCreationForAnonymousUsers, "1")==0))
      {
          Login1.CreateUserText = "Create new user";
          Login1.CreateUserUrl="~/TimeTracker/User_Create.aspx";
      }

  }
}
