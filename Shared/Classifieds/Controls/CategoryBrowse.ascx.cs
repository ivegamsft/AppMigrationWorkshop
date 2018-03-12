using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AspNet.StarterKits.Classifieds.Web;

public partial class CategoryBrowse_ascx : System.Web.UI.UserControl
{
    public event CategorySelectionChangedEventHandler CategorySelectionChanged;

    // see AutoNavigate property
    private bool _autoNavigate = false;
    
    // when AutoNavigate is true, the control automatically redirects to Search.aspx 
    // to display ads in the selected category;
    // if false, CategorySelectionChanged will be raised
    public bool AutoNavigate
    {
        get
        {
            return _autoNavigate;
        }
        set
        {
            _autoNavigate = value;
        }
    }

    protected void TopCategoryList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ObjectDataSource nestedDS = e.Item.FindControl("NestedCategoryDS") as ObjectDataSource;
        CachedCategory parentCategory = e.Item.DataItem as CachedCategory;
        nestedDS.SelectParameters[0].DefaultValue = parentCategory.IdString;
    }
    protected void TopCategoryList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CommandArgument);
        OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
    }
    protected void NestedSubCategoryRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CommandArgument);
        OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
    }


    protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
    {
        if (_autoNavigate)
            Response.Redirect("~/Search.aspx?c=" + e.CategoryId.ToString(), true);
        else if (CategorySelectionChanged != null)
            CategorySelectionChanged(this, e);
    }

}
