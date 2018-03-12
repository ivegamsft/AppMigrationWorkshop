using System;
using System.Collections;
using ASPNET.StarterKit.DataAccessLayer;
using System.Collections.Generic;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public class Project {
    /*** FIELD PRIVATE ***/
    private decimal _ActualDuration;
    private string _CreatorUserName;
    private DateTime _CompletionDate;
    private DateTime _DateCreated;
    private string _Description;
    private decimal _EstimateDuration;
    private int _Id;
    private string _ManagerUserName;
    private string _Name;

    /*** CONSTRUCTOR ***/
    public Project(string creatorUsername, string managerUserName, string name)
      : this(DefaultValues.GetDurationMinValue(), creatorUsername, DefaultValues.GetDateTimeMinValue(), DefaultValues.GetDateTimeMinValue(), string.Empty, DefaultValues.GetProjectDurationMinValue(), DefaultValues.GetProjectIdMinValue(), managerUserName, name) {
    }

    public Project(string creatorUsername, string description, int id, string managerUserName, string name)
      : this(DefaultValues.GetDurationMinValue(), creatorUsername, DefaultValues.GetDateTimeMinValue(), DefaultValues.GetDateTimeMinValue(), string.Empty, DefaultValues.GetProjectDurationMinValue(), DefaultValues.GetProjectIdMinValue(), managerUserName, name) {
    }

    public Project(decimal actualDuration, string creatorUserName, DateTime completionDate, DateTime dateCreated, string description, decimal estimateDuration, int id, string managerUserName, string name) {
      // Validate Mandatory Fields//
      if (String.IsNullOrEmpty(creatorUserName))
        throw (new NullReferenceException("creatorUsername"));

      if (String.IsNullOrEmpty(managerUserName))
        throw (new NullReferenceException("managerUserName"));

      if (String.IsNullOrEmpty(name))
        throw (new NullReferenceException("name"));


      _ActualDuration = actualDuration;
      _CreatorUserName = creatorUserName;
      _CompletionDate = completionDate;
      _DateCreated = dateCreated;
      _Description = description;
      _EstimateDuration = estimateDuration;
      _Id = id;
      _ManagerUserName = managerUserName;
      _Name = name;
    }

    /*** PROPERTIES ***/
    public decimal ActualDuration {
      get { return _ActualDuration; }
    }

    public string CreatorUserName {
      get {
        if (String.IsNullOrEmpty(_CreatorUserName))
          return string.Empty;
        else
          return _CreatorUserName;
      }
    }

    public DateTime CompletionDate {
      get { return _CompletionDate; }
      set { _CompletionDate = value; }
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

    public decimal EstimateDuration {
      get { return _EstimateDuration; }
      set { _EstimateDuration = value; }
    }

    public int Id {
      get { return _Id; }
    }

    public string ManagerUserName {
      get {
        if (String.IsNullOrEmpty(_ManagerUserName))
          return string.Empty;
        else
          return _ManagerUserName;
      }
      set { _ManagerUserName = value; }
    }

    public string Name {
      get {
        if (String.IsNullOrEmpty(_Name))
          return string.Empty;
        else
          return _Name;
      }
      set { _Name = value; }
    }

    /*** METHODS  ***/
    public bool Delete() {
      if (this.Id > DefaultValues.GetProjectIdMinValue()) {
        DataAccess DALLayer = DataAccessHelper.GetDataAccess();
        return DALLayer.DeleteProject(this.Id);
      }
      else
        return false;
    }

    public bool Save() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      if (Id <= DefaultValues.GetProjectIdMinValue()) {
        int TempId = DALLayer.CreateNewProject(this);
        if (TempId > DefaultValues.GetProjectIdMinValue()) {
          _Id = TempId;
          return true;
        }
        else
          return false;
      }
      else
        return (DALLayer.UpdateProject(this));
    }

    /*** METHOD STATIC ***/
    public static bool AddUserToProject(int projectId, string userName) {

      if (projectId <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("projectId"));

      if (String.IsNullOrEmpty(userName))
        throw (new NullReferenceException("userName"));


      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.AddUserToProject(projectId, userName));
    }

    public static bool DeleteProject(int Id) {
      if (Id <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("Id"));

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.DeleteProject(Id));
    }

    public static List<Project> GetAllProjects() {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetAllProjects());
    }

    public static List<Project> GetAllProjects(string sortParameter) {
      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      List<Project> projectList = DALLayer.GetAllProjects();

      if (String.IsNullOrEmpty(sortParameter))
        projectList.Sort(new ProjectComparer(sortParameter));

      return (projectList);
    }

    public static Project GetProjectById(int Id) {
      if (Id <= DefaultValues.GetProjectIdMinValue())
        return (null);

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetProjectById(Id));
    }


    public static List<Project> GetProjectByIds(string ProjectIds) {

      if (String.IsNullOrEmpty(ProjectIds))
        return (new List<Project>());

      char[] separator = new char[] { ',' };
      string[] substrings = ProjectIds.Split(separator);
      List<Project> list = new List<Project>();

      foreach (string str in substrings) {
        if (!string.IsNullOrEmpty(str)) {
          int id = Convert.ToInt32(str);
          list.Add(Project.GetProjectById(id));
        }
      }
      return list;
    }


    public static List<Project> GetProjectsByManagerUserName(string sortParameter, string userName) {
      if (String.IsNullOrEmpty(userName))
        return (new List<Project>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      List<Project> prjColl = DALLayer.GetProjectsByManagerUserName(userName);
      if (String.IsNullOrEmpty(sortParameter))
        prjColl.Sort(new ProjectComparer(sortParameter));

      return prjColl;
    }

    public static List<string> GetProjectMembers(int Id) {
      if (Id <= DefaultValues.GetProjectIdMinValue())
        return (new List<string>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetProjectMembers(Id));
    }

    public static List<string> GetProjectMembers(string userNames) {
      if (String.IsNullOrEmpty(userNames))
        return (new List<string>());

      char[] separator = new char[] { ',' };
      string[] substrings = userNames.Split(separator);
      List<string> list = new List<string>();

      foreach (string str in substrings) {
        if (!string.IsNullOrEmpty(str)) {
          int Id = Convert.ToInt32(str);
          List<string> tempList = Project.GetProjectMembers(Id);
          foreach (string userName in tempList) {
            if (!list.Contains(userName)) {
              list.Add(userName);
            }
          }
        }
      }
      return list;
    }

    public static List<Project> GetProjectsByUserName(string userName) {
      if (String.IsNullOrEmpty(userName))
        return (new List<Project>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetProjectsByUserName(userName));

    }

    public static bool RemoveUserFromProject(int projectId, string userName) {

      if (projectId <= DefaultValues.GetProjectIdMinValue())
        throw (new ArgumentOutOfRangeException("projectId"));

      if (String.IsNullOrEmpty(userName))
        throw (new NullReferenceException("userName"));


      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.RemoveUserFromProject(projectId, userName));
    }
  }
}
