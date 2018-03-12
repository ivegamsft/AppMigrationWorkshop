using System;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;
using AspNet.StarterKits.Classifieds.Web;

public partial class Categories_aspx : System.Web.UI.Page
{
    protected void AddCategoryButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            int parentCategoryId = CategoryPath.CurrentCategoryId;
            int newCategoryId = CategoriesDB.InsertCategory(parentCategoryId, Server.HtmlEncode(NewCategoryTextBox.Text));

            CategoryDropDown.Refresh();

            NewCategoryTextBox.Text = String.Empty;
            SubcategoriesList.DataBind();
        }
    }

    protected void RemoveCategoryButton_Click(object sender, EventArgs e)
    {
        int categoryId = CategoryPath.CurrentCategoryId;
        if (CategoriesDB.RemoveCategory(categoryId))
        {
            CategoryDropDown.Refresh();
            SetCurrentCategory(CategoryPath.CurrentParentCategoryId);
            RemoveSuccessLabel.Visible = true;
            RemoveFailedLabel.Visible = false;
        }
        else
        {
            RemoveSuccessLabel.Visible = false;
            RemoveFailedLabel.Visible = true;
        }
    }

    protected void MoveButton_Click(object sender, EventArgs e)
    {
        int currentCategoryId = CategoryPath.CurrentCategoryId;
        int newParentCategoryId = CategoryDropDown.SelectedCategoryId;

        if (MoveAction.SelectedValue.Equals("Category"))
        {
            CategoriesDB.MoveCategory(currentCategoryId, newParentCategoryId);
            CategoryDropDown.Refresh();
            SetCurrentCategory(CategoryPath.CurrentParentCategoryId);
        }
        else if (MoveAction.SelectedValue.Equals("Ads"))
        {
            // newParentCategoryId is the new CategoryId
            // for all Ads entered under 'currentCategoryId'
            AdsDB.MoveAdsToCategory(currentCategoryId, newParentCategoryId);
            CategoryDropDown.SelectedIndex = 0;
        }
    }

    protected void RenameCategoryButton_Click(object sender, EventArgs e)
    {
        if (RequiredCategoryNameValidator.IsValid)
        {
            int categoryId = CategoryPath.CurrentCategoryId;
            CategoriesDB.UpdateCategoryName(categoryId, Server.HtmlEncode(RenameCategoryTextBox.Text));
            CategoryDropDown.Refresh();
            SetCurrentCategory(categoryId);
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            SetCurrentCategory(DefaultValues.CategoryIdMinValue);
        }
    }

    protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
    {
        UpdateCategoryDisplay();
    }

    protected void UpdateCategoryDisplay()
    {
        if (CategoryPath.CurrentCategoryId == DefaultValues.CategoryIdMinValue)
        {
            CurrentCategoryLabel.Text = "[All Categories]";
            CurrentCategoryActionLabel.Text = "All Categories";

            RenameCategoryTextBox.Text = String.Empty;

            RenameCategoryTextBox.Enabled =
                RenameCategoryButton.Enabled = false;


            MoveButton.Enabled = false;
            MoveAction.Enabled = false;
            CategoryDropDown.Enabled = false;

            RemoveCategoryButton.Enabled = false;

        }
        else
        {
            CurrentCategoryLabel.Text =
                CurrentCategoryActionLabel.Text =
                RenameCategoryTextBox.Text = CategoryPath.CurrentCategoryName;

            RenameCategoryTextBox.Enabled =
                RenameCategoryButton.Enabled = true;

            MoveButton.Enabled = true;
            MoveAction.Enabled = true;
            CategoryDropDown.Enabled = true;

            RemoveCategoryButton.Enabled = true;
        }        
    }


    protected void SetCurrentCategory(int categoryId)
    {
        CategoryPath.CurrentCategoryId = categoryId;
        UpdateCategoryDisplay();
    }

    protected void SubcategoriesList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int categoryId = Convert.ToInt32(SubcategoriesList.SelectedValue);
        if (categoryId > DefaultValues.CategoryIdMinValue)
        {
            SetCurrentCategory(categoryId);
        }
    }
    protected void SubcategoriesList_DataBound(object sender, EventArgs e)
    {
        if (SubcategoriesList.Items.Count > 0)
        {
            SubcategoriesList.Enabled = true;
        }
        else
        {
            SubcategoriesList.Items.Add(new ListItem("(no sub-categories)", DefaultValues.CategoryIdMinValueString));
            SubcategoriesList.Enabled = false;
        }
    }
}
