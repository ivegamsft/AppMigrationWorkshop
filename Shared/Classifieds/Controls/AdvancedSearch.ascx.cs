using System;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class AdvancedSearch_ascx : System.Web.UI.UserControl
{
    #region Properties exposed by UI
    public int CategoryId
    {
        get
        {
            return CategoryDropDown.CurrentCategoryId;
        }
        set
        {
            CategoryDropDown.CurrentCategoryId = value;
        }
    }

    public string SearchTerm
    {
        get
        {
            return Server.HtmlEncode(SearchTermTextBox.Text);
        }
        set
        {
            SearchTermTextBox.Text = value;
        }
    }

    public decimal MaximumPrice
    {
        get
        {
            if (MaximumPriceTextBox.Text.Equals(String.Empty))
                return -1;
            else
            {
                decimal p = -1;
                if (decimal.TryParse(MaximumPriceTextBox.Text, out p))
                    return p;
                else
                    return -1;
            }
        }
        set
        {
            MaximumPriceTextBox.Text = value.ToString();
        }
    }

    public string Location
    {
        get
        {
            return LocationDropDown.CurrentLocation;
        }
        set
        {
            LocationDropDown.CurrentLocation = value;
        }
    }

    public int DayRange
    {
        get
        {
            if (ViewState["DayRange"] != null)
                return (int)ViewState["DayRange"];
            else
                return -1;
        }
        set
        {
            ViewState["DayRange"] = value;
        }
    }

    public int AdType
    {
        get
        {
            if (ViewState["AdType"] != null)
                return (int)ViewState["AdType"];
            else
                return (int)AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdType.Unspecified;
        }
        set
        {
            ViewState["AdType"] = value;
        }
    }

    public bool MustHavePhotos
    {
        get
        {
            return PhotoCheckBox.Checked;
        }
        set
        {
            PhotoCheckBox.Checked = value;
        }
    }
    #endregion

    #region Properties not exposed by UI (can be accessed programmatically)
    public int RecordLimit
    {
        get
        {
            if (ViewState["RecordLimit"] != null)
                return (int)ViewState["RecordLimit"];
            else
                return 50;
        }
        set
        {
            ViewState["RecordLimit"] = value;
        }
    }

    /// <summary>
    /// Retrieves the MemberId associated with the given member (via Profile) and sets the control's MemberId property
    /// </summary>
    /// <seealso cref="MemberId"/>
    /// <param name="memberName"></param>
    public void SetMemberName(string memberName)
    {
        this.MemberName = memberName;
        ProfileCommon memberProfile = Profile.GetProfile(memberName);
        if (memberProfile != null && memberProfile.MemberId != 0)
        {
            this.MemberId = memberProfile.MemberId;
        }
    }
    public string MemberName
    {
        get
        {
            if (ViewState["MemberName"] != null)
                return (string)ViewState["MemberName"];
            else
                return String.Empty;
        }
        set
        {
            ViewState["MemberName"] = value;
        }
    }
    public int MemberId
    {
        get
        {
            if (ViewState["MemberId"] != null)
                return (int)ViewState["MemberId"];
            else
                return DefaultValues.IdNullValue;
        }
        set
        {
            ViewState["MemberId"] = value;
        }
    }
    #endregion

    protected void DayRangeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DayRangeList.SelectedIndex > -1)
            this.DayRange = Convert.ToInt32(DayRangeList.SelectedValue);
    }
    protected void AdTypeList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (AdTypeList.SelectedIndex > -1)
            this.AdType = Convert.ToInt32(AdTypeList.SelectedValue);
    }

    public void ResetProperties(bool resetSearchTerm, bool resetCategory)
    {
        if (resetSearchTerm)
            SearchTermTextBox.Text = String.Empty;
        if (resetCategory)
            CategoryDropDown.CurrentCategoryId = DefaultValues.CategoryIdMinValue;

        SearchTermTextBox.Text =
            MaximumPriceTextBox.Text = String.Empty;
        LocationDropDown.CurrentLocation = String.Empty;

        this.MemberId = DefaultValues.IdNullValue;

        this.DayRange = -1;

        this.AdType = (int)AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdType.Unspecified;

        PhotoCheckBox.Checked = false;

    }

    public void ImportProperties(AdvancedSearch_ascx other)
    {
        this.CategoryId = other.CategoryId;
        this.SearchTerm = other.SearchTerm;
        this.MaximumPrice = other.MaximumPrice;
        this.Location = other.Location;
        this.DayRange = other.DayRange;
        this.AdType = other.AdType;
        this.MustHavePhotos = other.MustHavePhotos;
        this.MemberId = other.MemberId;
        this.RecordLimit = other.RecordLimit;
    }



    public bool SearchingByMember
    {
        get
        {
            return MemberId != DefaultValues.IdNullValue;
        }
    }

    public bool SearchingByDayRange
    {
        get
        {
            return DayRange != -1;
        }
    }

    /// <summary>
    /// returns true if search parameters have been specified for this control
    /// ("AdvancedCriteria" does not include MemberId or DayRange -- see SearchingByMember and SearchingByDayRange)
    /// </summary>
    /// <value></value>
    public bool SearchingByAdvancedCriteria
    {
        get
        {
            if (!MaximumPriceTextBox.Text.Equals(String.Empty))
                return true;
            if (!LocationDropDown.CurrentLocation.Equals(String.Empty))
                return true;
            if (AdType != (int)AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdType.Unspecified)
                return true;
            if (MustHavePhotos)
                return true;

            return false;

        }
    }

}
