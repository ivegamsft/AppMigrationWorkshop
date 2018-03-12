using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.Web;
using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

public partial class CategoryDropDown_ascx : System.Web.UI.UserControl
{
    private string _selectOptionText = "Select...";
    private bool _selectOptionVisible = false;

    private bool _allCategoriesVisible = true;
    private string _allCategoriesText = "--- All Categories ----------------------------------------";

    private int _savedSelectedIndex = -1;
    private int _currentCategoryId = -100;

    private bool _isDataBound = false;

    public event CategorySelectionChangedEventHandler CategorySelectionChanged;

    public int CurrentCategoryId
    {
        get
        {
            return SelectedCategoryId;
        }
        set
        {
            SetCurrentCategory(value);
        }
    }

    public bool SelectOptionVisible
    {
        get
        {
            return _selectOptionVisible;
        }
        set
        {
            _selectOptionVisible = value;
        }
    }

    public string SelectOptionText
    {
        get
        {
            return _selectOptionText;
        }
        set
        {
            _selectOptionText = value;
        }
    }

    public string AllCategoriesOptionText
    {
        get
        {
            return _allCategoriesText;
        }
        set
        {
            _allCategoriesText = value;
        }
    }

    public bool AllCategoriesOptionVisible
    {
        get
        {
            return _allCategoriesVisible;
        }
        set
        {
            _allCategoriesVisible = value;
        }
    }



    public int SelectedCategoryId
    {
        get
        {
            if (CategoryList.SelectedIndex >= 0)
                return Convert.ToInt32(CategoryList.SelectedValue);
            else
                return DefaultValues.CategoryIdMinValue;
        }
        set
        {
            string v = value.ToString();
            ListItem selected = CategoryList.Items.FindByValue(v);
            if (selected != null)
            {
                selected.Selected = true;
            }
        }
    }

    public int SelectedIndex
    {
        get
        {
            return CategoryList.SelectedIndex;
        }
        set
        {
            if (CategoryList.Items.Count > 0)
                CategoryList.SelectedIndex = value;
        }
    }

    public bool Enabled
    {
        get
        {
            return CategoryList.Enabled;
        }
        set
        {
            CategoryList.Enabled = value;
        }
    }

    public void Refresh()
    {
        CategoryCache.ClearAll(Context);
        CategoryList.DataBind();
    }

    private void SetCurrentCategory(int categoryId)
    {
        if (!_isDataBound)
            CategoryList.DataBind();

        string categoryIdString = categoryId.ToString();
        ListItem li = CategoryList.Items.FindByValue(categoryIdString);
        if (li != null)
        {
            CategoryList.SelectedItem.Selected = false;
            li.Selected = true;
        }
    }

    protected void CategoryList_DataBound(object sender, EventArgs e)
    {
        _isDataBound = true;

        if (_allCategoriesVisible)
            CategoryList.Items.Insert(0, new ListItem(_allCategoriesText, DefaultValues.CategoryIdMinValueString));

        if (_selectOptionVisible && CategoryList.Items.Count > 0)
            CategoryList.Items.Insert(0, new ListItem(_selectOptionText, "-1"));

        if (_savedSelectedIndex != -1)
            CategoryList.SelectedIndex = _savedSelectedIndex;

    }
    protected void CategoryList_DataBinding(object sender, EventArgs e)
    {
        if (CategoryList.SelectedItem != null)
            _savedSelectedIndex = CategoryList.SelectedIndex;
    }

    protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
    {
        if (CategorySelectionChanged != null)
            CategorySelectionChanged(this, e);
    }
    protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int categoryId = Convert.ToInt32(CategoryList.SelectedValue);
        if (categoryId != _currentCategoryId)
        {
            if (categoryId >= DefaultValues.CategoryIdMinValue)
            {
                OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
            }
        }
        _currentCategoryId = categoryId;
    }
}
