using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using ASPNET.StarterKit.BusinessLogicLayer;

namespace ASPNET.StarterKit.DataAccessLayer {
  public abstract class DataAccess {
    /*** PROPERTIES ***/
    protected string ConnectionString {
      get {
        if (ConfigurationManager.ConnectionStrings["aspnet_staterKits_TimeTracker"] == null)
          throw (new NullReferenceException("ConnectionString configuration is missing from you web.config. It should contain  <connectionStrings> <add key=\"aspnet_staterKits_TimeTracker\" value=\"Server=(local);Integrated Security=True;Database=Issue_Tracker\" </connectionStrings>"));

        string connectionString = ConfigurationManager.ConnectionStrings["aspnet_staterKits_TimeTracker"].ConnectionString;

        if (String.IsNullOrEmpty(connectionString))
          throw (new NullReferenceException("ConnectionString configuration is missing from you web.config. It should contain  <connectionStrings> <add key=\"aspnet_staterKits_TimeTracker\" value=\"Server=(local);Integrated Security=True;Database=Issue_Tracker\" </connectionStrings>"));
        else
          return (connectionString);
      }
    }

    /*** METHODS  ***/

    //Category
    public abstract int CreateNewCategory(Category newCategory);
    public abstract bool DeleteCategory(int categoryId);
    public abstract List<Category> GetAllCategories();
    public abstract Category GetCategoryByCategoryId(int Id);
    public abstract List<Category> GetCategoriesByProjectId(int projectId);
    public abstract Category GetCategoryByCategoryNameandProjectId(string categoryName, int projectId);
    public abstract bool UpdateCategory(Category newCategory);

    //TimeEntry
    public abstract int CreateNewTimeEntry(TimeEntry newTimeEntry);
    public abstract bool DeleteTimeEntry(int timeEntryId);
    public abstract List<TimeEntry> GetAllTimeEntries();
    public abstract List<TimeEntry> GetTimeEntries(int projectId, string userName);
    public abstract TimeEntry GetTimeEntryById(int timeEntryId);
    public abstract List<TimeEntry> GetTimeEntriesByUserNameAndDates(string userName,
                                                                      DateTime startingDate, DateTime endDate);
    public abstract bool UpdateTimeEntry(TimeEntry timeEntry);

    // Project
    public abstract bool AddUserToProject(int projectId, string userName);
    public abstract int CreateNewProject(Project newProject);
    public abstract bool DeleteProject(int projectID);
    public abstract List<Project> GetAllProjects();
    public abstract Project GetProjectById(int projectId);
    public abstract List<Project> GetProjectsByManagerUserName(string userName);
    public abstract List<string> GetProjectMembers(int Id);
    public abstract List<Project> GetProjectsByUserName(string userName);
    public abstract bool RemoveUserFromProject(int projectId, string userName);
    public abstract bool UpdateProject(Project projectToUpdate);

    //User report
    public abstract List<UserReport> GetUserReportsByProjectId(int projectId);
    public abstract List<UserReport> GetUserReportsByCategoryId(int categoryId);

    // UserTotalDurationReport
    public abstract List<UserTotalDurationReport> GetUserReportsByUserName(string userName);
  }
}

