using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class Locations_aspx : System.Web.UI.Page
{
    protected void RemoveLocationButton_Click(object sender, EventArgs e)
    {
        if (this.LocationsListBox.SelectedIndex != -1)
        {
            int locationId = Convert.ToInt32(LocationsListBox.SelectedValue);
            LocationsDB.RemoveLocation(locationId);
            LocationCache.ClearAll(Context);
            LocationsListBox.DataBind();
        }
    }
    protected void AddLocationButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string location = Server.HtmlEncode(this.NewLocationTextBox.Text);
            LocationsDB.InsertLocation(location);
            LocationCache.ClearAll(Context);
            LocationsListBox.DataBind();
            NewLocationTextBox.Text = String.Empty;
        }
    }




}
