using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using ASPNET.StarterKit.BusinessLogicLayer;

public partial class Project_Details_aspx : System.Web.UI.Page
{
    public Project_Details_aspx()
    {
        LoadComplete += new EventHandler(Page_LoadComplete);
    }

    private Project CurrentProject = null;

    

    void GetCurrentProject()
    {
        int projectIdFromQueryString;
        if (Request.QueryString["ProjectId"] != null && Int32.TryParse((string)Request.QueryString["ProjectId"], out projectIdFromQueryString)) {
            CurrentProject = Project.GetProjectById(projectIdFromQueryString);
        }
    }

    void Page_Load(object sender, EventArgs e) {
        GetCurrentProject();
        if (!Page.IsPostBack)
        {
            ManagerData.SelectParameters.Add(new Parameter("roleName", TypeCode.String, "ProjectManager"));
            ProjectConsultantData.SelectParameters.Add(new Parameter("roleName", TypeCode.String, "Consultant"));
            
            if (CurrentProject != null)
            {
                ProjectName.Text = CurrentProject.Name;
                DateTime dt = Convert.ToDateTime(CurrentProject.CompletionDate);
                CompletionDate.Text = dt.ToString("d");
                Duration.Text = Convert.ToString(CurrentProject.EstimateDuration);
                Description.Text = CurrentProject.Description;
                Managers.SelectedValue = CurrentProject.ManagerUserName;

                Consultants.DataBind();
                ProjectCategoryColumn.Visible = true;
            }
            else
            {
                ProjectCategoryColumn.Visible = false;
            }


            if ((Page.User.IsInRole("ProjectAdministrator")))
                ProjectData.SelectMethod = "GetAllProjects";
            else
            {

                ProjectData.SelectParameters.Add(new Parameter("userName", TypeCode.String, Page.User.Identity.Name));
                ProjectData.SelectMethod = "GetProjectsByManagerUserName";
                ProjectData.SortParameterName = "sortParameter";
            }
        }
        DeleteButton2.Attributes.Add("onclick", "return confirm('Deleting a project will also delete all the time entries and categories associated with the project. This deletion cannot be undone. Are you sure you want to delete this project?')");
    }

    void Page_LoadComplete(object sender, EventArgs e)
    {

        if (CurrentProject != null)
        {
            SelectProjectMembers(CurrentProject.Id);
        }

    }

    void Page_PreRender(object sender, EventArgs e)
    {
        ViewState["ActiveConsultants"] = BuildValueList(Consultants.Items, true);
    }

    protected void AddButton_Click(object obj, EventArgs args)
    {
        if (Page.IsValid == false)
            return;

        if (CurrentProject != null)
        {
            bool wasCategorySave = false;

            Category newCategory = new Category(Abbrev.Text, Convert.ToDecimal(CatDuration.Text), CategoryName.Text, CurrentProject.Id);
            wasCategorySave = newCategory.Save();
            ListAllCategories.DataBind();
        }
    }



    protected string BuildValueList(ListItemCollection items, bool itemMustBeSelected)
    {
        StringBuilder idList = new StringBuilder();
        foreach (ListItem item in items)
        {
            if (itemMustBeSelected && !item.Selected)
                continue;

            else
            {
                idList.Append(item.Value.ToString());
                idList.Append(",");
            }
        }
        return idList.ToString();
    }

    protected void CancelButton_Click(object obj, EventArgs args)
    {
        Response.Redirect("Project_List.aspx");
    }

    protected void CopyButton_Click(object obj, EventArgs args)
    {
        if (CurrentProject != null)
        {
            int selectedProjectId = Convert.ToInt32(ProjectList.SelectedValue);

            List<Category> newCategories = Category.GetCategoriesByProjectId(selectedProjectId);

            foreach (Category cat in newCategories)
            {
                Category newCat = new Category(cat.Abbreviation, cat.EstimateDuration, cat.Name, CurrentProject.Id);
                newCat.Save();
            }
            CategoryData.DataBind();
            ListAllCategories.DataBind();
        }
    }

    protected void DeleteButton_Click(object obj, EventArgs args)
    {
        if (CurrentProject != null)
        {
            Project newProject = Project.GetProjectById(CurrentProject.Id);
            newProject.Delete();
            Response.Redirect("Project_List.aspx");
        }
    }
    protected void SaveButton_Click(object obj, EventArgs args)
    {
        if (Page.IsValid == false)
            return;
        Project newProject;

        if (CurrentProject != null)
        {
            newProject = Project.GetProjectById(CurrentProject.Id);

        }
        else
            newProject = new Project(Page.User.Identity.Name, Page.User.Identity.Name, ProjectName.Text);

        newProject.Name = ProjectName.Text;
        newProject.CompletionDate = Convert.ToDateTime(CompletionDate.Text);
        newProject.Description = Description.Text;
        newProject.EstimateDuration = Convert.ToDecimal(Duration.Text);
        newProject.ManagerUserName = Managers.SelectedItem.Value;

        if (!newProject.Save())
        {
            ErrorMessage.Text = "There was an error.  Please fix it and try it again.";
        }
        UpdateProjectMembers(newProject.Id);
        string strUrl = "Project_Details.aspx?ProjectId=" + newProject.Id.ToString();
        Response.Redirect(strUrl);
    }

    protected void SelectProjectMembers(int projectId)
    {
        Consultants.DataBind();
        List<string> userList = Project.GetProjectMembers(projectId);

        foreach (string user in userList)
        {
            ListItem item = Consultants.Items.FindByValue(user);
            item.Selected = true;
        }
    }

    protected void UpdateProjectMembers(int projectId)
    {
        string activeConsultants = string.Empty;

        if (ViewState["ActiveConsultants"] != null)
        {
            activeConsultants = ViewState["ActiveConsultants"].ToString();

        }
        foreach (ListItem item in Consultants.Items)
        {
            if (item.Selected)
            {
                if (!activeConsultants.Contains(item.Value + ","))
                {
                    Project.AddUserToProject(projectId, item.Text);
                }
            }
            else
            {
                if (activeConsultants.Contains(item.Value + ","))
                {
                    Project.RemoveUserFromProject(projectId, item.Text);
                }
            }
        }
    }

    protected void ProjectList_PreRender(object sender, EventArgs e)
    {
        ListItem item = ProjectList.Items.FindByText(ProjectName.Text);
        if (item != null)
        {
            ProjectList.Items.Remove(item);
        }

        if (ProjectList.Items.Count == 0)
        {
            ProjectList.Enabled = false;
        }
    }
    protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args) {

        if (Description.Text.Length > 200)
            args.IsValid = false;
        else
            args.IsValid = true;
    }
}