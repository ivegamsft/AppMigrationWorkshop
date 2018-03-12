using System;
using System.Data.SqlTypes;

namespace ASPNET.StarterKit.BusinessLogicLayer {
  public static class DefaultValues {

    public static int GetCategoryIdMinValue() {
      return (0);
    }

    public static DateTime GetDateTimeMinValue() {
      DateTime MinValue = (DateTime)SqlDateTime.MinValue;
      MinValue.AddYears(30);
      return (MinValue);
    }

    public static int GetDurationMinValue() {
      return (0);
    }

    public static int GetCustomFieldIdMinValue() {
      return (0);
    }

    public static int GetIssueIdMinValue() {
      return (0);
    }

    public static int GetIssueCommentIdMinValue() {
      return (0);
    }


    public static int GetIssueNotificationIdMinValue() {
      return (0);
    }

    public static int GetMilestoneIdMinValue() {
      return (0);
    }

    public static int GetPriorityIdMinValue() {
      return (0);
    }

    public static int GetProjectIdMinValue() {
      return (0);
    }

    public static int GetProjectDurationMinValue() {
      return (0);
    }

    public static int GetStatusIdMinValue() {
      return (0);
    }

    public static int GetTimeEntryIdMinValue() {
      return (0);
    }

    public static int GetUserIdMinValue() {
      return (0);
    }
  }
}
