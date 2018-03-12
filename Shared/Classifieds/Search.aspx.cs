using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class Search_aspx : System.Web.UI.Page
{
    // To customize the search experience so that the page 
    // will display a Category Browse view as seen on the homepage
    // instead of search results, set this to false. Setting to false
    // will only display Category Browse for top level category.
    // Note: how search behaves for an empty search string is
    // related to the behavior set here. See the GetAdsByQuery method
    // in the App_Code/BLL/Ads.cs file.
    private bool _showCategoryBrowseForTopLevel = true;
    private bool _advancedSearchInEffect;

    protected void Page_Load(object sender, EventArgs e)
    {
        _advancedSearchInEffect = Convert.ToBoolean(ViewState["advancedSearchInEffect"]);
        if (!Page.IsPostBack && Page.PreviousPage == null)
        {
            // check if the URL contains a query for ads by a given member
            string memberQs = Request.QueryString["member"];
            if (memberQs != null)
            {
                _showCategoryBrowseForTopLevel = true;
                // pass it on to the hidden AdvSearch control (its stores most search parameters)
                AdvancedSearch.SetMemberName(memberQs); 
                _advancedSearchInEffect = true;
                ViewState["advancedSearchInEffect"] = true;
            }


            // check if the URL contains a query for ads in a given category
            string categoryQs = Request.QueryString["c"];
            int categoryIdQs = DefaultValues.CategoryIdMinValue;
            if (categoryQs != null)
            {
                if (Int32.TryParse(categoryQs, out categoryIdQs))
                {
                    _showCategoryBrowseForTopLevel = true;
                }
            }

            // if no category was specified in the QueryString, we set it the "All Categories" id (0)
            SetCurrentCategory(categoryIdQs);
            if (_advancedSearchInEffect)
                SetSearchMessage(2);
            else
                SetSearchMessage(1);

        }

        if (Page.IsPostBack)
        {
            if (_advancedSearchInEffect)
                SetSearchMessage(2);
            else
                SetSearchMessage(1);
            UpdateUI();
        }
        if (Page.PreviousPage != null)
        {
            // we check if the request was cross-posted by AdvancedSearch.aspx (in which case we expect to find a control "AdvancedSearch")
            AdvancedSearch_ascx advParameters = Util.FindControlRecursively("AdvancedSearch", Page.PreviousPage.Controls) as AdvancedSearch_ascx;
            if (advParameters != null && advParameters.SearchingByAdvancedCriteria)
            {
                // we import its properties into the (hidden) AdvancedSearch control on this page
                AdvancedSearch.ImportProperties(advParameters);

                // copy the values which the AdSearchDataSource needs on this page (SearchTerm comes from SearchTermTextBox, Category comes from the CategoryDropDown control)
                SetCurrentCategory(AdvancedSearch.CategoryId);
                SearchTermTextBox.Text = AdvancedSearch.SearchTerm;

                SetSearchMessage(2);
                ViewState["advancedSearchInEffect"] = true;

            }
            else if (advParameters != null && !advParameters.SearchTerm.Equals(String.Empty))
            {
                // carry over just search term as regular search in category
                SearchTermTextBox.Text = advParameters.SearchTerm;
                SetCurrentCategory(AdvancedSearch.CategoryId);
                SetSearchMessage(1);
                ViewState["advancedSearchInEffect"] = false;
            }
            else
            {
                // a number of pages contain a generic search text box, which posts back to this page.
                // if found, import the search term text
                TextBox CommonSearchTextBox = Util.FindControlRecursively("CommonSearchTextBox", Page.PreviousPage.Controls) as TextBox;
                if (CommonSearchTextBox != null)
                {
                    // query came from Master Page Search Box
                    SearchTermTextBox.Text = Server.HtmlEncode(CommonSearchTextBox.Text);
                }

                // if a category was also specified, import it as well
                CategoryDropDown_ascx CommonCategoryDropDown = Util.FindControlRecursively("CommonCategoryDropDown", Page.PreviousPage.Controls) as CategoryDropDown_ascx;
                if (CommonCategoryDropDown != null)
                {
                    SetCurrentCategory(CommonCategoryDropDown.CurrentCategoryId);
                }

                // if a "What's new within the last x days" was specified, import it
                DropDownList CommonWhatsNewRangeList = Util.FindControlRecursively("CommonWhatsNewRangeList", Page.PreviousPage.Controls) as DropDownList;
                Button CommonWhatsNewButton = Util.FindControlRecursively("CommonWhatsNewButton", Page.PreviousPage.Controls) as Button;
                if (CommonWhatsNewRangeList != null && !CommonWhatsNewButton.CommandArgument.Equals(String.Empty))
                {
                    int numDays = Convert.ToInt32(CommonWhatsNewRangeList.SelectedValue);
                    AdvancedSearch.DayRange = numDays;
                    SetSearchMessage(2);
                }
                else
                {
                    SetSearchMessage(1);
                }
            }
        }
    }

    protected void SetCurrentCategory(int categoryId)
    {
        CategoryDropDown.CurrentCategoryId = categoryId;
        CategoryPath.CurrentCategoryId = categoryId;
        AdvancedSearch.CategoryId = categoryId;
        SubcategoriesList.DataBind();
        UpdateUI();
    }

    protected void SetSearchMessage(int flag)
    {
        switch (flag)
        {
            case 1:
                NormalSearch.Visible = true;
                SearchingByAdvancedPanel.Visible = false;
                if (SearchTermTextBox.Text.Length > 0)
                {
                    NormalSearchLabel.Text = "Searching with ";
                    NormalSearchCriteria.Text = "search term = \"" + Server.HtmlEncode(SearchTermTextBox.Text) + "\"";
                    NormalSearchCriteria.Visible = true;
                    ClearSearch.Visible = true;
                }
                else
                {
                    NormalSearchLabel.Text = "No search terms <br/> specified.";
                    NormalSearchCriteria.Visible = false;
                    ClearSearch.Visible = false;
                }
                break;
            case 2:
                NormalSearch.Visible = false;
                SearchingByAdvancedPanel.Visible = true;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (AdvancedSearch.MaximumPrice != -1)
                    sb.Append("max price = \"" + AdvancedSearch.MaximumPrice.ToString() + "\" <br/>");
                if (AdvancedSearch.Location.Length > 0)
                    sb.Append("location = \"" + Server.HtmlEncode(AdvancedSearch.Location) + "\" <br/>");
                if (AdvancedSearch.AdType != 0)
                    sb.Append("ad type = \"" + (AdType)AdvancedSearch.AdType + "\" <br/>");
                if (AdvancedSearch.MustHavePhotos)
                    sb.Append("photo = \"" + AdvancedSearch.MustHavePhotos + "\" <br/>");
                if (AdvancedSearch.MemberName != String.Empty)
                    sb.Append("Ads posted by <br/>\"" + AdvancedSearch.MemberName + "\"");
                if (AdvancedSearch.DayRange != -1)
                {
                    int numDays = Convert.ToInt32(AdvancedSearch.DayRange);
                    if (numDays == 1)
                        sb.Append("Ads for the last day");
                    else
                        sb.Append(String.Format("Ads for the last {0} days", numDays));

                }
                AdvancedSearchCriteria.Text = sb.ToString();
                break;
            default:
                NormalSearch.Visible = false;
                SearchingByAdvancedPanel.Visible = false;
                break;
        }

    }
    protected void UpdateUI()
    {
        bool showSearchResults = false;
        if (_showCategoryBrowseForTopLevel)
            showSearchResults = true;
        else if (CategoryDropDown.CurrentCategoryId != DefaultValues.CategoryIdMinValue)
            showSearchResults = true;
        else if (!String.IsNullOrEmpty(SearchTermTextBox.Text))
            showSearchResults = true;


        if (showSearchResults)
        {
            // if search criteria were specified, show GridView with search results
            BrowsePanel.Visible = false;
            ResultsPanel.Visible = true;
            ResultsPanel.DataBind();
        }
        else
        {
            // if no search critieria were specified, show the regular browse category view (as on Default.aspx)
            BrowsePanel.Visible = true;
            ResultsPanel.Visible = false;
            BrowsePanel.DataBind();
        }
    }

    protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        ResetSearchCriteria(true, false, false);
        SetCurrentCategory(e.CategoryId);
    }

    protected void CategoryDropDown_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        SetCurrentCategory(e.CategoryId);
    }

    protected void CategoryBrowse_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        int categoryId = Convert.ToInt32(e.CategoryId);
        SetCurrentCategory(categoryId);
    }

    protected void SubcategoriesList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (SubcategoriesList.SelectedIndex != -1)
        {
            int categoryId = Convert.ToInt32(SubcategoriesList.SelectedValue);
            if (categoryId == -1)
            {
                ResetSearchCriteria(false, false, false);
                SetCurrentCategory(CategoryPath.CurrentParentCategoryId);
            }
            else if (categoryId > -1)
            {
                ResetSearchCriteria(false, false, false);
                SetCurrentCategory(categoryId);
            }
        }
    }

    protected void SubcategoriesList_DataBound(object sender, EventArgs e)
    {

        if (SubcategoriesList.Items.Count == 0)
        {
            SubcategoriesList.Items.Add(new ListItem("--No sub-categories. Go up one level.", "-1"));
        }
        else if (CategoryPath.CurrentCategoryId != CategoryPath.CurrentParentCategoryId)
        {
            SubcategoriesList.Items.Insert(0, new ListItem("--Go up one level.", "-1"));
        }

    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        // when the Search button is clicked,
        // all search parameters except the current category (from dropdown)
        // and the search term are cleared
        ResetSearchCriteria(false, false, false);
        if (_advancedSearchInEffect)
            SetSearchMessage(2);
        else
            SetSearchMessage(1);

        UpdateUI();
    }

    protected void RemoveAdvancedSearch_Click(object sender, EventArgs e)
    {
        ResetSearchCriteria(false, false, true);
        SetSearchMessage(1);
    }

    protected void ClearSearch_Click(object sender, EventArgs e)
    {
        ResetSearchCriteria(true, false, true);
        SetSearchMessage(1);
    }

    protected void ResetSearchCriteria(bool resetSearchTerm, bool resetCategory, bool resetAdvancedSearch)
    {
        if (resetSearchTerm)
            SearchTermTextBox.Text = String.Empty;
        if (resetCategory)
            SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        if (resetAdvancedSearch)
        {
            ViewState["advancedSearchInEffect"] = false;
            AdvancedSearch.ResetProperties(resetSearchTerm, resetCategory);
        }
    }
}
