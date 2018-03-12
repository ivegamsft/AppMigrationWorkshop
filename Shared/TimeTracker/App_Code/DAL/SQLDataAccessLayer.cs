using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using ASPNET.StarterKit.BusinessLogicLayer;

namespace ASPNET.StarterKit.DataAccessLayer {
  public class SQLDataAccess : DataAccess {
    /*** DELEGATE ***/

    private delegate void TGenerateListFromReader<T>(SqlDataReader returnData, ref List<T> tempList);

    /*****************************  BASE CLASS IMPLEMENTATION *****************************/

    /***  CATEGORY ***/
    private const string SP_CATEGORY_CREATE = "aspnet_starterkits_CreateNewCategory";
    private const string SP_CATEGORY_DELETE = "aspnet_starterkits_DeleteCategory";
    private const string SP_CATEGORY_GETALLCATEGORIES = "aspnet_starterkits_GetAllCategories";
    private const string SP_CATEGORY_GETCATEGORYBYPROJECTID = "aspnet_starterkits_GetCategoriesByProjectId";
    private const string SP_CATEGORY_GETCATEGORYBYID = "aspnet_starterkits_GetCategoryById";
    private const string SP_CATEGORY_GETCATEGORYBYNAMEANDPROJECT = "aspnet_starterkits_GetCategoryByNameAndProjectId";
    private const string SP_CATEGORY_UPDATE = "aspnet_starterkits_UpdateCategories";



    public override int CreateNewCategory(Category newCategory) {
      if (newCategory == null)
        throw (new ArgumentNullException("newCategory"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@CategoryAbbreviation", SqlDbType.NText, 255, ParameterDirection.Input, newCategory.Abbreviation);
      AddParamToSQLCmd(sqlCmd, "@CategoryEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, newCategory.EstimateDuration);
      AddParamToSQLCmd(sqlCmd, "@CategoryName", SqlDbType.NText, 255, ParameterDirection.Input, newCategory.Name);
      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newCategory.ProjectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_CREATE);
      ExecuteScalarCmd(sqlCmd);

      return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
    }

    public override bool DeleteCategory(int categoryId) {
      if (categoryId <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("categoryId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@CategoryIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, categoryId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_DELETE);
      ExecuteScalarCmd(sqlCmd);

      int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
      return (returnValue == 0 ? true : false);
    }

    public override List<Category> GetAllCategories() {
      SqlCommand sqlCmd = new SqlCommand();

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETALLCATEGORIES);

      List<Category> categoryList = new List<Category>();

      TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

      return categoryList;
    }

    public override Category GetCategoryByCategoryId(int Id) {
      if (Id <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("Id"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, Id);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCATEGORYBYID);

      List<Category> categoryList = new List<Category>();

      TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

      if (categoryList.Count > 0)
        return categoryList[0];
      else
        return null;

    }

    public override Category GetCategoryByCategoryNameandProjectId(string categoryName, int projectId) {
      if (projectId <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("Id"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
      AddParamToSQLCmd(sqlCmd, "@CategoryName", SqlDbType.NText, 255, ParameterDirection.Input, categoryName);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCATEGORYBYNAMEANDPROJECT);

      List<Category> categoryList = new List<Category>();

      TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);

      if (categoryList.Count > 0)
        return categoryList[0];
      else
        return null;

    }



    public override List<Category> GetCategoriesByProjectId(int projectId) {
      if (projectId <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("projectId"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_GETCATEGORYBYPROJECTID);

      List<Category> categoryList = new List<Category>();

      TExecuteReaderCmd<Category>(sqlCmd, TGenerateCategoryListFromReader<Category>, ref categoryList);


      return categoryList;
    }

    public override bool UpdateCategory(Category newCategory) {
      if (newCategory == null)
        throw (new ArgumentNullException("newCategory"));

      if (newCategory.Id <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("newCategory.Id"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, newCategory.Id);
      AddParamToSQLCmd(sqlCmd, "@CategoryAbbreviation", SqlDbType.NText, 255, ParameterDirection.Input, newCategory.Abbreviation);
      AddParamToSQLCmd(sqlCmd, "@CategoryEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, newCategory.EstimateDuration);
      AddParamToSQLCmd(sqlCmd, "@CategoryName", SqlDbType.NText, 255, ParameterDirection.Input, newCategory.Name);
      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, newCategory.ProjectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_CATEGORY_UPDATE);
      ExecuteScalarCmd(sqlCmd);

      int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
      return (returnValue == 0 ? true : false);
    }


    /***  PROJECT ***/
    private const string SP_PROJECT_ADDUSERTOPROJECT = "aspnet_starterkits_AddUserToProject";
    private const string SP_PROJECT_CREATE = "aspnet_starterkits_CreateNewProject";
    private const string SP_PROJECT_DELETE = "aspnet_starterkits_DeleteProject";
    private const string SP_PROJECT_GETALLPROJECTS = "aspnet_starterkits_GetAllProjects";
    private const string SP_PROJECT_GETAPROJECTBYID = "aspnet_starterkits_GetProjectById";
    private const string SP_PROJECT_GETAPROJECTSBYMANAGERUSERNAME = "aspnet_starterkits_GetProjectByManagerUserName";
    private const string SP_PROJECT_GETPROJECTSBYYSERNAME = "aspnet_starterkits_GetProjectByUserName";
    private const string SP_PROJECT_GETPROJECTMEMBERS = "aspnet_starterkits_GetProjectMember";
    private const string SP_PROJECT_REMOVEUSERFROMPROJECT = "aspnet_starterkits_RemoveUserFromProject";
    private const string SP_PROJECT_UPDATE = "aspnet_starterkits_UpdateProject";

    public override bool AddUserToProject(int projectId, string userName) {
      if (userName == null || userName.Length == 0)
        throw (new ArgumentOutOfRangeException("userName"));

      if (projectId <= 0)
        throw (new ArgumentOutOfRangeException("projectId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ResultValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@MemberUserName", SqlDbType.NText, 255, ParameterDirection.Input, userName);
      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_ADDUSERTOPROJECT);
      ExecuteScalarCmd(sqlCmd);

      int resultValue = (int)sqlCmd.Parameters["@ResultValue"].Value;

      return (resultValue == 0 ? true : false);
    }


    public override int CreateNewProject(Project newProject) {
      if (newProject == null)
        throw (new ArgumentNullException("newProject"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@ProjectCreatorUserName", SqlDbType.NText, 255, ParameterDirection.Input, newProject.CreatorUserName);
      AddParamToSQLCmd(sqlCmd, "@ProjectCompletionDate", SqlDbType.DateTime, 0, ParameterDirection.Input, newProject.CompletionDate);
      AddParamToSQLCmd(sqlCmd, "@ProjectDescription", SqlDbType.NText, 1000, ParameterDirection.Input, newProject.Description);
      AddParamToSQLCmd(sqlCmd, "@ProjectEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, newProject.EstimateDuration);
      AddParamToSQLCmd(sqlCmd, "@ProjectManagerUserName", SqlDbType.NText, 255, ParameterDirection.Input, newProject.ManagerUserName);
      AddParamToSQLCmd(sqlCmd, "@ProjectName", SqlDbType.NText, 255, ParameterDirection.Input, newProject.Name);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_CREATE);
      ExecuteScalarCmd(sqlCmd);

      return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
    }

    public override bool DeleteProject(int projectID) {
      if (projectID <= 0)
        throw (new ArgumentOutOfRangeException("projectID"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@ProjectIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, projectID);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_DELETE);
      ExecuteScalarCmd(sqlCmd);

      int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;

      return (returnValue == 0 ? true : false);
    }

    public override List<Project> GetAllProjects() {
      SqlCommand sqlCmd = new SqlCommand();

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETALLPROJECTS);

      List<Project> prjList = new List<Project>();

      TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref prjList);

      return prjList;
    }

    public override Project GetProjectById(int projectId) {
      if (projectId <= 0)
        throw (new ArgumentOutOfRangeException("projectId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETAPROJECTBYID);

      List<Project> prjList = new List<Project>();

      TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref prjList);

      if (prjList.Count > 0)
        return prjList[0];
      else
        return null;
    }

    public override List<Project> GetProjectsByManagerUserName(string userName) {
      if (userName == null || userName.Length == 0)
        throw (new ArgumentOutOfRangeException("userName"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectManagerUserName", SqlDbType.NText, 256, ParameterDirection.Input, userName);
      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETAPROJECTSBYMANAGERUSERNAME);

      List<Project> prjList = new List<Project>();

      TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref prjList);

      return prjList;


    }

    public override List<string> GetProjectMembers(int Id) {
      if (Id <= 0)
        throw (new ArgumentOutOfRangeException("Id"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, Id);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTMEMBERS);

      List<string> userList = new List<string>();

      TExecuteReaderCmd<string>(sqlCmd, TGenerateUsertListFromReader<string>, ref userList);

      return userList;

    }

    public override List<Project> GetProjectsByUserName(string userName) {
      if (userName == null || userName.Length == 0)
        throw (new ArgumentOutOfRangeException("userName"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NText, 256, ParameterDirection.Input, userName);
      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_GETPROJECTSBYYSERNAME);

      List<Project> prjList = new List<Project>();

      TExecuteReaderCmd<Project>(sqlCmd, TGenerateProjectListFromReader<Project>, ref prjList);

      return prjList;
    
      
//      return (new List<Project>());

    }


    public override bool RemoveUserFromProject(int projectId, string userName) {
      if (String.IsNullOrEmpty(userName))
        throw (new ArgumentOutOfRangeException("userName"));
      if (projectId <= 0)
        throw (new ArgumentOutOfRangeException("projectId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ResultValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@userName", SqlDbType.NVarChar, 0, ParameterDirection.Input, userName);
      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_REMOVEUSERFROMPROJECT);
      ExecuteScalarCmd(sqlCmd);

      int resultValue = (int)sqlCmd.Parameters["@ResultValue"].Value;

      return (resultValue == 0 ? true : false);
    }

    public override bool UpdateProject(Project projectToUpdate) {
      // validate input
      if (projectToUpdate == null)
        throw (new ArgumentNullException("projectToUpdate"));
      // validate input
      if (projectToUpdate.Id <= 0)
        throw (new ArgumentOutOfRangeException("projectToUpdate"));

      SqlCommand sqlCmd = new SqlCommand();
      // set the type of parameter to add a new project
      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectToUpdate.Id);
      AddParamToSQLCmd(sqlCmd, "@ProjectCompletionDate", SqlDbType.DateTime, 0, ParameterDirection.Input, projectToUpdate.CompletionDate);
      AddParamToSQLCmd(sqlCmd, "@ProjectDescription", SqlDbType.NText, 1000, ParameterDirection.Input, projectToUpdate.Description);
      AddParamToSQLCmd(sqlCmd, "@ProjectEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, projectToUpdate.EstimateDuration);
      AddParamToSQLCmd(sqlCmd, "@ProjectManagerUserName", SqlDbType.NText, 256, ParameterDirection.Input, projectToUpdate.ManagerUserName);
      AddParamToSQLCmd(sqlCmd, "@ProjectName", SqlDbType.NText, 256, ParameterDirection.Input, projectToUpdate.Name);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_PROJECT_UPDATE);
      ExecuteScalarCmd(sqlCmd);

      int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;

      return (returnValue == 0 ? true : false);
    }


    /*** TIME ENTRY ***/
    private string SP_TIMEENTRY_CREATE = "aspnet_starterkits_CreateNewTimeEntry";
    private string SP_TIMEENTRY_DELETE = "aspnet_starterkits_DeleteTimeEntry";
    private string SP_TIMEENTRY_GETALLTIMEENTRIES = "aspnet_starterkits_GetAllTimeEntries";
    private string SP_TIMEENTRY_GETALLTIMEENTRIESBYPROJECTID_USER = "aspnet_starterkits_GetAllTimeEntriesByProjectIdandUser";
    private string SP_TIMEENTRY_GETALLTIMEENTRIESBYUSERNAMEANDDATE = "aspnet_starterkits_GetAllTimeEntriesByProjectIdandUserAndDate";
    private string SP_TIMEENTRY_UPDATE = "aspnet_starterkits_UpdateTimeEntry";
    private string SP_TIMEENTRY_GETTIMEENTRYBYID = "aspnet_starterkits_GetTimeEntryById";



    public override int CreateNewTimeEntry(TimeEntry newTimeEntry) {

      if (newTimeEntry == null)
        throw (new ArgumentNullException("newTimeEntry"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);

      AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, newTimeEntry.CategoryId);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryCreatorUserName", SqlDbType.NText, 255, ParameterDirection.Input, newTimeEntry.CreatorUserName);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryDescription", SqlDbType.NText, 1000, ParameterDirection.Input, newTimeEntry.Description);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, newTimeEntry.Duration);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryEnteredDate", SqlDbType.DateTime, 0, ParameterDirection.Input, newTimeEntry.ReportedDate);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryUserName", SqlDbType.NText, 255, ParameterDirection.Input, newTimeEntry.UserName);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_CREATE);

      ExecuteScalarCmd(sqlCmd);

      return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
    }

    public override bool DeleteTimeEntry(int timeEntryId) {
      if (timeEntryId <= DefaultValues.GetTimeEntryIdMinValue())
        throw (new ArgumentOutOfRangeException("timeEntryId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, timeEntryId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_DELETE);
      ExecuteScalarCmd(sqlCmd);

      int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;

      return (returnValue == 0 ? true : false);

    }

    public override List<TimeEntry> GetAllTimeEntries() {

      SqlCommand sqlCmd = new SqlCommand();

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETALLTIMEENTRIES);

      List<TimeEntry> timeEntryList = new List<TimeEntry>();

      TExecuteReaderCmd<TimeEntry>(sqlCmd, TGenerateTimeEntryListFromReader<TimeEntry>, ref timeEntryList);

      return timeEntryList;
    }

    public override List<TimeEntry> GetTimeEntries(int projectId, string userName) {
      if (projectId <= DefaultValues.GetTimeEntryIdMinValue())
        throw (new ArgumentOutOfRangeException("projectId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryUserName", SqlDbType.NText, 255, ParameterDirection.Input, userName);


      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETALLTIMEENTRIESBYPROJECTID_USER);

      List<TimeEntry> timeEntryList = new List<TimeEntry>();

      TExecuteReaderCmd<TimeEntry>(sqlCmd, TGenerateTimeEntryListFromReader<TimeEntry>, ref timeEntryList);

      return timeEntryList;
    }

    public override TimeEntry GetTimeEntryById(int timeEntryId) {
      if (timeEntryId <= 0)
        throw (new ArgumentOutOfRangeException("timeEntryId"));

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@TimeEntryId", SqlDbType.Int, 0, ParameterDirection.Input, timeEntryId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETTIMEENTRYBYID);

      List<TimeEntry> timeEntryList = new List<TimeEntry>();


      TExecuteReaderCmd<TimeEntry>(sqlCmd, TGenerateTimeEntryListFromReader<TimeEntry>, ref timeEntryList);

      if (timeEntryList.Count > 0)
        return timeEntryList[0];
      else
        return null;

    }

    public override List<TimeEntry> GetTimeEntriesByUserNameAndDates(string userName,
                                                                      DateTime startingDate, DateTime endDate) {
      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
      AddParamToSQLCmd(sqlCmd, "@EndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, endDate);
      AddParamToSQLCmd(sqlCmd, "@StartingDate", SqlDbType.DateTime, 0, ParameterDirection.Input, startingDate);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryUserName", SqlDbType.NText, 255, ParameterDirection.Input, userName);


      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETALLTIMEENTRIESBYUSERNAMEANDDATE);

      List<TimeEntry> timeEntryList = new List<TimeEntry>();

      TExecuteReaderCmd<TimeEntry>(sqlCmd, TGenerateTimeEntryListFromReader<TimeEntry>, ref timeEntryList);

      return timeEntryList;

    }

    public override bool UpdateTimeEntry(TimeEntry timeEntry) {
      if (timeEntry == null)
        throw (new ArgumentNullException("timeEntry"));


      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);


      AddParamToSQLCmd(sqlCmd, "@TimeEntryId", SqlDbType.Int, 0, ParameterDirection.Input, timeEntry.Id);
      AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, timeEntry.CategoryId);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryDescription", SqlDbType.NText, 1000, ParameterDirection.Input, timeEntry.Description);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryEstimateDuration", SqlDbType.Decimal, 0, ParameterDirection.Input, timeEntry.Duration);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryEnteredDate", SqlDbType.DateTime, 0, ParameterDirection.Input, timeEntry.ReportedDate);
      AddParamToSQLCmd(sqlCmd, "@TimeEntryUserName", SqlDbType.NText, 1000, ParameterDirection.Input, timeEntry.UserName);


      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_UPDATE);
      ExecuteScalarCmd(sqlCmd);
      int resultValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;

      return (resultValue == 0 ? true : false);
    }

    /***  USER REPORT ***/
    private string SP_TIMEENTRY_GETUSERREPORT = "aspnet_starterkits_GetTimeEntryUserReport";
    private string SP_TIMEENTRY_GETUSERREPORTBYCATEGORY = "aspnet_starterkits_GetTimeEntryUserReportByCategoryId";

    public override List<UserReport> GetUserReportsByProjectId(int projectId) {

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@ProjectId", SqlDbType.Int, 0, ParameterDirection.Input, projectId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETUSERREPORT);

      List<UserReport> userReport = new List<UserReport>();

      TExecuteReaderCmd<UserReport>(sqlCmd, TGenerateUserReportListFromReader<UserReport>, ref userReport);

      return userReport;
    }

    public override List<UserReport> GetUserReportsByCategoryId(int categoryId) {

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@CategoryId", SqlDbType.Int, 0, ParameterDirection.Input, categoryId);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETUSERREPORTBYCATEGORY);

      List<UserReport> userReport = new List<UserReport>();

      TExecuteReaderCmd<UserReport>(sqlCmd, TGenerateUserReportListFromReader<UserReport>, ref userReport);

      return userReport;
    }

    /***  USER TOTAL DURATION REPORT ***/
    private string SP_TIMEENTRY_GETUSERREPORTBYUSER = "aspnet_starterkits_GetTimeEntryUserReportByUser";

    public override List<UserTotalDurationReport> GetUserReportsByUserName(string userName) {

      SqlCommand sqlCmd = new SqlCommand();

      AddParamToSQLCmd(sqlCmd, "@UserName", SqlDbType.NText, 256, ParameterDirection.Input, userName);

      SetCommandType(sqlCmd, CommandType.StoredProcedure, SP_TIMEENTRY_GETUSERREPORTBYUSER);

      List<UserTotalDurationReport> userReport = new List<UserTotalDurationReport>();

      TExecuteReaderCmd<UserTotalDurationReport>(sqlCmd, TGenerateUserReportListFromReader<UserTotalDurationReport>, ref userReport);
      return userReport;
    }


    /*****************************  SQL HELPER METHODS *****************************/
    private void AddParamToSQLCmd(SqlCommand sqlCmd,
                                  string paramId,
                                  SqlDbType sqlType,
                                  int paramSize,
                                  ParameterDirection paramDirection,
                                  object paramvalue) {

      if (sqlCmd == null)
        throw (new ArgumentNullException("sqlCmd"));
      if (paramId == string.Empty)
        throw (new ArgumentOutOfRangeException("paramId"));

      SqlParameter newSqlParam = new SqlParameter();
      newSqlParam.ParameterName = paramId;
      newSqlParam.SqlDbType = sqlType;
      newSqlParam.Direction = paramDirection;

      if (paramSize > 0)
        newSqlParam.Size = paramSize;

      if (paramvalue != null)
        newSqlParam.Value = paramvalue;

      sqlCmd.Parameters.Add(newSqlParam);
    }

    private void ExecuteScalarCmd(SqlCommand sqlCmd) {
      if (ConnectionString == string.Empty)
        throw (new ArgumentOutOfRangeException("ConnectionString"));

      if (sqlCmd == null)
        throw (new ArgumentNullException("sqlCmd"));

      using (SqlConnection cn = new SqlConnection(this.ConnectionString)) {
        sqlCmd.Connection = cn;
        cn.Open();
        sqlCmd.ExecuteScalar();
      }
    }

    private void SetCommandType(SqlCommand sqlCmd, CommandType cmdType, string cmdText) {
      sqlCmd.CommandType = cmdType;
      sqlCmd.CommandText = cmdText;
    }

    private void TExecuteReaderCmd<T>(SqlCommand sqlCmd, TGenerateListFromReader<T> gcfr, ref List<T> List) {
      if (ConnectionString == string.Empty)
        throw (new ArgumentOutOfRangeException("ConnectionString"));

      if (sqlCmd == null)
        throw (new ArgumentNullException("sqlCmd"));

      using (SqlConnection cn = new SqlConnection(this.ConnectionString)) {
        sqlCmd.Connection = cn;

        cn.Open();

        gcfr(sqlCmd.ExecuteReader(), ref List);
      }
    }


    /*****************************  GENARATE List HELPER METHODS  *****************************/

    private void TGenerateProjectListFromReader<T>(SqlDataReader returnData, ref List<Project> prjList) {
      while (returnData.Read()) {
        decimal actualDuration = 0;
        if (returnData["ProjectActualDuration"] != DBNull.Value)
          actualDuration = Convert.ToDecimal(returnData["ProjectActualDuration"]);

        Project project = new Project(actualDuration, (string)returnData["ProjectCreatorDisplayName"], (DateTime)returnData["ProjectCompletionDate"], (DateTime)returnData["ProjectCreationDate"], (string)returnData["ProjectDescription"],
                                 (Decimal)returnData["ProjectEstimateDuration"], (int)returnData["ProjectId"], (string)returnData["ProjectManagerDisplayName"], (string)returnData["ProjectName"]);
        prjList.Add(project);
      }
    }

    private void TGenerateCategoryListFromReader<T>(SqlDataReader returnData, ref List<Category> categoryList) {
      while (returnData.Read()) {
        decimal actualDuration = 0;
        if (returnData["CategoryActualDuration"] != DBNull.Value)
          actualDuration = Convert.ToDecimal(returnData["CategoryActualDuration"]);

        Category category = new Category((string)returnData["CategoryAbbreviation"], actualDuration, (int)returnData["CategoryId"], (decimal)returnData["CategoryEstimateDuration"], (string)returnData["CategoryName"], (int)returnData["ProjectId"]);
        categoryList.Add(category);
      }
    }

    private void TGenerateTimeEntryListFromReader<T>(SqlDataReader returnData, ref List<TimeEntry> timeEntryList) {
      while (returnData.Read()) {
        TimeEntry timeEntry = new TimeEntry((string)returnData["TimeEntryCreatorDisplayName"], (int)returnData["CategoryId"], (DateTime)returnData["TimeEntryCreated"], (string)returnData["TimeEntryDescription"],
                                        (Decimal)returnData["TimeEntryDuration"], (int)returnData["TimeEntryId"], (DateTime)returnData["TimeEntryDate"], (string)returnData["TimeEntryUserName"]);
        timeEntryList.Add(timeEntry);
      }
    }

    private void TGenerateUsertListFromReader<T>(SqlDataReader returnData, ref List<string> userList) {
      while (returnData.Read()) {
        string userName = (string)returnData["UserName"];
        userList.Add(userName);
      }
    }

    private void TGenerateUserReportListFromReader<T>(SqlDataReader returnData, ref List<UserReport> userReportList) {
      while (returnData.Read()) {
        UserReport userReport = new UserReport((decimal)returnData["duration"], (int)returnData["CategoryId"], (string)returnData["UserName"]);
        userReportList.Add(userReport);
      }
    }

    private void TGenerateUserReportListFromReader<T>(SqlDataReader returnData, ref List<UserTotalDurationReport> userReportList) {
      while (returnData.Read()) {
        decimal totalDuration = 0;
        if (returnData["TotalDuration"] != DBNull.Value)
          totalDuration = (decimal)returnData["TotalDuration"];
        UserTotalDurationReport userReport = new UserTotalDurationReport(totalDuration, (string)returnData["UserName"]);
        userReportList.Add(userReport);
      }
    }
  }
}
