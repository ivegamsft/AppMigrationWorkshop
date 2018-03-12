using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class Settings_aspx : System.Web.UI.Page
{
    protected void Cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/Default.aspx");
    }

    protected void SettingsDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
    {
        // the UpdateSettings methods returns a boolean success value

        bool updateSuccess = false;
        if (e.ReturnValue != null && e.ReturnValue is bool)
            updateSuccess = (bool)e.ReturnValue;

        if (updateSuccess)
        {
            Response.Redirect("~/Admin/Default.aspx");
        }
        else
        {
            // we failed to save the settings to the settings file
            // but we can output its Xml equivalent for manual saving:
            SiteSettings s = SiteSettings.GetSharedSettings();
            SettingsXmlTextBox.Text = s.GetXml();

            UpdateErrorPanel.Visible = true;
        }
    }
}
