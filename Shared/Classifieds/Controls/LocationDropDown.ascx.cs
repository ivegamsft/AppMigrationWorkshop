using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.Web;

public partial class LocationDropDown_ascx : System.Web.UI.UserControl
{
    private bool _isDatabound;

    private const string OtherLocationText = "Other Location...";
    private const string AnyLocationText = "Any Location";

    private bool _showAnyLocationChoice = false;

    private string _currentLocation = null;

    public bool ShowAnyLocationChoice
    {
        get
        {
            return _showAnyLocationChoice;
        }
        set
        {
            if (value != _showAnyLocationChoice)
            {
                _showAnyLocationChoice = value;
                Refresh();
            }
        }
    }

    public string CurrentLocation
    {
        get
        {
            if (!OtherLocationTextBox.Text.Equals(String.Empty) || LocationList.SelectedIndex == OtherLocationItemIndex)
            {
                return OtherLocationTextBox.Text;
            }
            else
            {
                return LocationList.SelectedValue;
            }
        }
        set
        {
            SetCurrentLocation(value);
        }
    }

    private int OtherLocationItemIndex
    {
        get
        {
            // the "Other Location..." item is always last
            return LocationList.Items.Count - 1;
        }
    }

    protected void LocationList_DataBound(object sender, EventArgs e)
    {
        if (_showAnyLocationChoice)
        {
            ListItem anyLocation = new ListItem(AnyLocationText, String.Empty);
            LocationList.Items.Insert(0, anyLocation);
        }

        ListItem otherLocation = new ListItem(OtherLocationText, OtherLocationText);
        LocationList.Items.Insert(LocationList.Items.Count, otherLocation);

        _isDatabound = true;
        SetCurrentLocation(_currentLocation);

        
    }

    public void Refresh()
    {
        _isDatabound = false;
        DataBind();
    }

    public override void DataBind()
    {
        if (_isDatabound)
            return;
        else
        {
           base.DataBind();
        }
    }

    private void SetCurrentLocation(string location)
    {
        if (location == null || location.Equals(String.Empty))
            return;
        else
            _currentLocation = location;

        if (!_isDatabound)
            return;

        ListItem currentSelection = LocationList.SelectedItem;
        ListItem predefinedLocation = LocationList.Items.FindByValue(location);
        if (predefinedLocation != null)
        {
            if (currentSelection != null)
                currentSelection.Selected = false;
            
            predefinedLocation.Selected = true;

            OtherLocationPanel.Visible = false;
        }
        else
        {
            OtherLocationPanel.Visible = true;
            OtherLocationTextBox.Text = location;
            LocationList.SelectedIndex = OtherLocationItemIndex;
        }
    }
    protected void LocationsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        string _previousLocation = CurrentLocation;
        List<CachedLocation> locations = e.ReturnValue as List<CachedLocation>;
        if (locations != null && locations.Count > 0)
        {
            LocationList.Visible = true;
            OtherLocationPanel.Visible = false;
        }
        else
        {
            LocationList.Visible = false;
            OtherLocationPanel.Visible = true;
            OtherLabel.Visible = false;
        }
        CurrentLocation = _previousLocation;
    }
    protected void LocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        OtherLocationTextBox.Text = String.Empty;
        if (LocationList.SelectedIndex == OtherLocationItemIndex)
        {
            OtherLocationPanel.Visible = true;
        }
        else
        {
            OtherLocationPanel.Visible = false;
        }
    }

}
