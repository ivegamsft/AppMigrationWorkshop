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

public partial class User_List_aspx : System.Web.UI.Page {
  public User_List_aspx() {
  }

  void Page_Load(object sender, EventArgs e) {
  }
  protected void Button_Click(Object sender, EventArgs args) {
    Response.Redirect("User_Create.aspx");
  }
}
