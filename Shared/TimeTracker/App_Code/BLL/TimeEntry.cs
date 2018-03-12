using System;
using System.Collections.Generic;
using ASPNET.StarterKit.DataAccessLayer;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public class TimeEntry {
    /*** FIELD PRIVATE ***/
    private string _CreatorUserName;
    private Category _Category;
    private int _CategoryId;
    private DateTime _DateCreated;
    private string _Description;
    private decimal _Duration;
    private int _Id;
    private Project _Project;
    private DateTime _ReportedDate;
    private string _UserName;

    /*** CONSTRUCTOR ***/
    public TimeEntry(string creatorUserName, int categoryId, decimal duration, DateTime reportedDate, string userName)
      : this(creatorUserName, categoryId, DefaultValues.GetDateTimeMinValue(), string.Empty, duration, DefaultValues.GetTimeEntryIdMinValue(), reportedDate, userName) {
    }

    public TimeEntry(string creatorUserName, int categoryId, DateTime dateCreated, string description, decimal duration, int id, DateTime reportedDate, string userName) {
      if (String.IsNullOrEmpty(creatorUserName))
        throw (new NullReferenceException("creatorUserName"));

      if (categoryId <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("categoryId"));

      if (duration <= DefaultValues.GetDurationMinValue())
        throw (new ArgumentOutOfRangeException("duration"));

      if (reportedDate <= DefaultValues.GetDateTimeMinValue())
        throw (new ArgumentOutOfRangeException("reportedDate"));

      if (String.IsNullOrEmpty(userName))
        throw (new NullReferenceException("userName"));


      _CreatorUserName = creatorUserName;
      _CategoryId = categoryId;
      _DateCreated = dateCreated;
      _Description = description;
      _Duration = duration;
      _Id = id;
      _ReportedDate = reportedDate;
      _UserName = userName;
    }


    /*** PROPERTIES ***/
    public string CreatorUserName {
      get {
        if (String.IsNullOrEmpty(_CreatorUserName))
          return string.Empty;
        else
          return _CreatorUserName;
      }
    }

    public int CategoryId {
      get { return _CategoryId; }
      set {
        Project currentProject;
        if (_Category == null) {
          _Category = Category.GetCategoryByCategoryId(_CategoryId);
          if (_Category == null)
            throw (new NullReferenceException("Can not find existing category"));
        }

        currentProject = Project.GetProjectById(_Category.ProjectId);
        if (currentProject == null)
          throw (new NullReferenceException("NewProject is not a valid one"));

        Project newProject;
        Category newCategory = Category.GetCategoryByCategoryId(value);

        if (newCategory == null)
          throw (new NullReferenceException("NewCategory is not a valid one"));

        newProject = Project.GetProjectById(newCategory.ProjectId);
        if (newProject == null)
          throw (new NullReferenceException("NewProject is not a valid one"));


        //this ensure data consistency
        if (newProject.Id == currentProject.Id) {
          _CategoryId = value;
          _Category = null;
        }
      }
    }

    public string CategoryName {
      get {
        if (_Category == null) {
          _Category = Category.GetCategoryByCategoryId(CategoryId);
          if (_Category == null)
            return (string.Empty);
        }
        return _Category.Name;
      }
    }

    public DateTime DateCreated {
      get { return _DateCreated; }
    }

    public string Description {
      get {
        if (String.IsNullOrEmpty(_Description))
          return string.Empty;
        else
          return _Description;
      }
      set { _Description = value; }
    }

    public decimal Duration {
      get { return _Duration; }
      set { _Duration = value; }
    }

    public int Id {
      get { return _Id; }
    }

    public DateTime ReportedDate {
      get { return _ReportedDate; }
      set { _ReportedDate = value; }
    }

    public string ProjectName {
      get {
        if (_Category == null) {
          _Category = Category.GetCategoryByCategoryId(CategoryId);
          if (_Category == null)
            return (string.Empty);
        }

        _Project = Project.GetProjectById(_Category.ProjectId);
        if (_Project == null)
          return (string.Empty);

        return _Project.Name;
      }
    }

    public string UserName {
      get {
        if (String.IsNullOrEmpty(_UserName))
          return string.Empty;
        else
          return _UserName;
      }
      set { _UserName = value; }

    }

    /*** METHODS  ***/
    public bool Delete() {
      if (this.Id > DefaultValues.GetProjectIdMinValue()) {
        DataAccess DALLayer = DataAccessHelper.GetDataAccess();
        return DALLayer.DeleteTimeEntry(this.Id);
      }
      else
        return false;
    }

    public bool Save() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      if (Id <= DefaultValues.GetProjectIdMinValue()) {
        int TempId = DALLayer.CreateNewTimeEntry(this);
        if (TempId > DefaultValues.GetTimeEntryIdMinValue()) {
          _Id = TempId;
          return true;
        }
        else
          return false;
      }
      else
        return (DALLayer.UpdateTimeEntry(this));
    }

    /*** METHOD STATIC ***/
    public static bool DeleteTimeEntry(int Id) {
      if (Id <= DefaultValues.GetCategoryIdMinValue())
                    throw (new ArgumentOutOfRangeException("Id"));


      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.DeleteTimeEntry(Id));
    }

    public static List<TimeEntry> GetAllTimeEntriess() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetAllTimeEntries());
    }

    public static List<TimeEntry> GetTimeEntries(int projectId, string userName) {
      if (projectId <= DefaultValues.GetProjectIdMinValue())
        return (new List<TimeEntry>());

      if (String.IsNullOrEmpty(userName))
        return (new List<TimeEntry>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetTimeEntries(projectId, userName));
    }

    public static TimeEntry GetTimeEntryById(int timeEntryId) {
      if (timeEntryId <= DefaultValues.GetTimeEntryIdMinValue())
        return (null);

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetTimeEntryById(timeEntryId));
    }

    public static List<TimeEntry> GetTimeEntriesByUserNameAndDates(string userName, DateTime startingDate, DateTime endDate) {
      if (String.IsNullOrEmpty(userName))
        return (new List<TimeEntry>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetTimeEntriesByUserNameAndDates(userName, startingDate, endDate));
    }

    public static bool UpdateTimeEntry(int Id, string CategoryName, string Description, decimal Duration, DateTime ReportedDate, string UserName) {
      if (String.IsNullOrEmpty(CategoryName))
        throw (new NullReferenceException("categoryName"));

      TimeEntry timeEntry = TimeEntry.GetTimeEntryById(Id);
      if (timeEntry == null)
        return (false);

      Category currentCategory = Category.GetCategoryByCategoryId(timeEntry.CategoryId);
      if (currentCategory == null)
        return (false);

      Project currentProject = Project.GetProjectById(currentCategory.ProjectId);
      if (currentProject == null)
        return (false);

      Category newCategory = Category.GetCategoryByCategoryNameandProjectId(CategoryName, currentProject.Id);
      if (newCategory == null)
        return (false);


      if (timeEntry != null) {
        timeEntry.Description = Description;
        timeEntry.Duration = Duration;
        timeEntry.ReportedDate = ReportedDate;
        timeEntry.CategoryId = newCategory.Id;
        timeEntry.UserName = UserName;
        return (timeEntry.Save());
      }
      else
        return false;

    }
  }
}

