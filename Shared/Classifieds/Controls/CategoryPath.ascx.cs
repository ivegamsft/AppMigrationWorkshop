using System;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class CategoryPath_ascx : System.Web.UI.UserControl
{

    private const string AllCategoriesText = "All Categories";

    private const string vskeyCurrentCategoryId = "CurrentCategoryId";
    private const string vskeyCurrentCategoryName = "CurrentCategoryName";
    private const string vskeyCurrentParentCategoryId = "CurrentParentCategoryId";
    private const string vskeyFullCategoryPath = "FullCategoryPath";

    private StringBuilder _categoryPath;

    public int CurrentCategoryId
    {
        get
        {
            if (ViewState[vskeyCurrentCategoryId] == null)
            {
                ViewState[vskeyCurrentCategoryId] = 0;
                return 0;
            }
            else
                return (int)ViewState[vskeyCurrentCategoryId];
        }
        set
        {
            SetCurrentCategory(value);
        }
    }
    public string CurrentCategoryName
    {
        get
        {
            if (ViewState[vskeyCurrentCategoryName] != null)
                return (string)ViewState[vskeyCurrentCategoryName];
            else
                return String.Empty;
        }
    }
    public int CurrentParentCategoryId
    {
        get
        {
            if (ViewState[vskeyCurrentParentCategoryId] == null)
            {
                ViewState[vskeyCurrentParentCategoryId] = 0;
                return 0;
            }
            else
                return (int)ViewState[vskeyCurrentParentCategoryId];
        }
    }

    public string FullCategoryPath
    {
        get
        {
            if (ViewState[vskeyFullCategoryPath] as string == null)
                return String.Empty;
            else
                return ViewState[vskeyFullCategoryPath] as string;
        }
    }

    public event CategorySelectionChangedEventHandler CategorySelectionChanged;


    protected void CategoryPath_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("CategoryClick"))
        {
            int categoryId = Convert.ToInt32(e.CommandArgument);
            SetCurrentCategory(categoryId);
            OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
        }
    }

    protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
    {
        if (CategorySelectionChanged != null)
            CategorySelectionChanged(this, e);
    }

    private void SetCurrentCategory(int categoryId)
    {
        ViewState[vskeyCurrentCategoryId] = categoryId;

        ParentCategoryDataSource.SelectParameters[0].DefaultValue =
            categoryId.ToString();


        // during the databinding event of the Repeater control,
        // we will build the category path ("All Categories > Parent > Parent > Current")...
        // we initialize the string builder here:
        InitCategoryPath();

        CategoryPath.DataBind();

        // at the end, we set the path and save it to ViewState
        SetFullCategoryPath(GetCategoryPath());

    }

    private void SetCurrentCategoryName(string name)
    {
        ViewState[vskeyCurrentCategoryName] = name;
    }
    private void SetCurrentParentCategoryId(int id)
    {
        ViewState[vskeyCurrentParentCategoryId] = id;
    }
    private void SetFullCategoryPath(string path)
    {
        ViewState[vskeyFullCategoryPath] = path;
    }

    private void InitCategoryPath()
    {
        _categoryPath = new StringBuilder();
        _categoryPath.Append(AllCategoriesText);
    }

    private void AppendCategoryToPath(string categoryName)
    {
        if (_categoryPath == null)
            InitCategoryPath();

        // we append a separator first
        _categoryPath.Append(" > ");
        _categoryPath.Append(categoryName);
    }

    private string GetCategoryPath()
    {
        if (_categoryPath == null)
            InitCategoryPath();
        return _categoryPath.ToString();
    }

    protected void AllCategoriesButton_Click(object sender, EventArgs e)
    {
        int categoryId = DefaultValues.CategoryIdMinValue;
        SetCurrentCategory(categoryId);
        OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
    }

    protected void ParentCategoryDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        string currCategoryName = null;
        int currCategoryParentId = 0;

        List<CachedCategory> parentCategories = e.ReturnValue as List<CachedCategory>;
        if (parentCategories != null && parentCategories.Count > 0)
        {
            // the "current" category is the last item/row returned by the GetParentCategories query.
            // we can fetch information like its name and its parent id at this point.
            CachedCategory last = parentCategories[parentCategories.Count - 1];
            currCategoryName = last.Name;
            if (last.Id != DefaultValues.CategoryIdMinValue)
                currCategoryParentId = last.ParentCategoryId;
            else
                currCategoryParentId = DefaultValues.CategoryIdMinValue;

        }
        else
        {
            currCategoryName = AllCategoriesText;
            currCategoryParentId = DefaultValues.CategoryIdMinValue;
        }

        SetCurrentCategoryName(currCategoryName);
        SetCurrentParentCategoryId(currCategoryParentId);

    }

    protected void CategoryPath_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        // we use this event to build up the category path sequentially
        // we start with the first row return (the top-most parent); the process
        // iteratively appends categories until we reach the 'current' category intially request
        // (ex. ([first call] Parent > [second call] Parent > [final call] Current)

        CachedCategory c = e.Item.DataItem as CachedCategory;
        if (c != null)
        {
            AppendCategoryToPath(c.Name);
        }
    }
}

