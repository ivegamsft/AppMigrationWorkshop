
using System;
using System.Collections.Generic;
using ASPNET.StarterKit.DataAccessLayer;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public class UserReport {
    /*** FIELD PRIVATE ***/
    private decimal _ActualDuration;
    private int _CategoryId;
    private string _UserName;

    /*** CONSTRUCTOR ***/
    public UserReport(decimal actualDuration, int categoryId, string userName) {

      if (categoryId <= DefaultValues.GetCategoryIdMinValue())
        throw (new ArgumentOutOfRangeException("categoryId"));

      if (String.IsNullOrEmpty(userName))
        throw (new NullReferenceException("userName"));


      _ActualDuration = actualDuration;
      _CategoryId = categoryId;
      _UserName = userName;
    }

    /*** PROPERTIES ***/
    public decimal ActualDuration {
      get { return _ActualDuration; }
    }

    public int CategoryId {
      get { return _CategoryId; }
    }

    public string UserName {
      get {
        if (String.IsNullOrEmpty(_UserName))
          return string.Empty;
        else
          return _UserName;
      }
    }

    /*** METHOD STATIC ***/
    public static List<UserReport> GetUserReportsByCategoryId(int CategoryId) {
      if (CategoryId <= DefaultValues.GetCategoryIdMinValue())
        return (new List<UserReport>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetUserReportsByCategoryId(CategoryId));
    }

    public static List<UserReport> GetUserReportsByProjectId(int projectId) {
      if (projectId <= DefaultValues.GetProjectIdMinValue())
        return (new List<UserReport>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetUserReportsByProjectId(projectId));
    }

    public static List<UserReport> GetUserReportsByProjectIds(string ProjectIds) {
      if (String.IsNullOrEmpty(ProjectIds))
        return (new List<UserReport>());

      char[] separator = new char[] { ',' };
      string[] substrings = ProjectIds.Split(separator);
      List<UserReport> list = new List<UserReport>();

      foreach (string str in substrings) {
        if (!string.IsNullOrEmpty(str)) {
          int id = Convert.ToInt32(str);
          List<UserReport> tempList = UserReport.GetUserReportsByProjectId(id);
          foreach (UserReport userReport in tempList) {
            list.Add(userReport);
          }
        }
      }
      return list;
    }
  }
}
