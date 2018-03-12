using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class LayoutController : System.Web.UI.UserControl
{
  WebPartManager manager;

  void Page_Init(object sender, EventArgs e)
  {
    Page.InitComplete += new EventHandler(InitComplete);
  }  

  void InitComplete(object sender, System.EventArgs e)
  {
    manager = WebPartManager.GetCurrentWebPartManager(Page);
    String browseModeName = WebPartManager.BrowseDisplayMode.Name;
    foreach (WebPartDisplayMode mode in manager.SupportedDisplayModes)
    {
      String modeName = mode.Name;
      if (mode.IsEnabled(manager))
      {
        ListItem item = new ListItem(modeName, modeName);
        DisplayModeDropdown.Items.Add(item);
      }
    }
  }
 
     protected void DisplayModeDropdown_SelectedIndexChanged1(object sender, EventArgs e)
    {
        String selectedMode = DisplayModeDropdown.SelectedValue;
        WebPartDisplayMode mode = manager.SupportedDisplayModes[selectedMode];
        if (mode != null)
            manager.DisplayMode = mode;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
