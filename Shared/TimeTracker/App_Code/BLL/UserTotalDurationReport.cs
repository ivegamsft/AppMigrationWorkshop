using System;
using System.Collections.Generic;
using ASPNET.StarterKit.DataAccessLayer;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public class UserTotalDurationReport {
    /*** FIELD PRIVATE ***/
    private decimal _TotalDuration;
    private string _UserName;

    /*** CONSTRUCTOR ***/
    public UserTotalDurationReport(decimal totalDuration, string userName) {

      if (totalDuration < DefaultValues.GetDurationMinValue())
        throw (new ArgumentOutOfRangeException("totalDuration"));

      if (String.IsNullOrEmpty(userName))
        throw (new NullReferenceException("userName"));

      _TotalDuration = totalDuration;
      _UserName = userName;
    }

    /*** PROPERTIES ***/
    public decimal TotalDuration {
      get { return _TotalDuration; }
    }

    public string UserName {
      get {
        if (String.IsNullOrEmpty(_UserName))
          return string.Empty;
        else
          return _UserName;
      }
    }

    public static List<UserTotalDurationReport> GetUserReportsByUserName(string userName) {
      if (String.IsNullOrEmpty(userName))
        return (new List<UserTotalDurationReport>());

      DataAccess DALLayer = DataAccessHelper.GetDataAccess();
      return (DALLayer.GetUserReportsByUserName(userName));
    }

    public static List<UserTotalDurationReport> GetUserReportsByUserNames(string userNames) {
      if (String.IsNullOrEmpty(userNames))
        return (new List<UserTotalDurationReport>());

      char[] separator = new char[] { ',' };
      string[] substrings = userNames.Split(separator);
      List<UserTotalDurationReport> list = new List<UserTotalDurationReport>();

      foreach (string str in substrings) {
        if (!string.IsNullOrEmpty(str)) {
          List<UserTotalDurationReport> tempList = UserTotalDurationReport.GetUserReportsByUserName(str);
          foreach (UserTotalDurationReport userReport in tempList) {
            list.Add(userReport);
          }
        }
      }
      return list;
    }
  }
}
