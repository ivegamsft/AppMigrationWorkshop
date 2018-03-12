using System;
using System.Collections.Generic;


namespace
ASPNET.StarterKit.BusinessLogicLayer {

  public class ProjectComparer : IComparer<Project> {
    private string _sortColumn;
    private bool _reverse;

    public ProjectComparer(string sortEx) {
      if (!String.IsNullOrEmpty(sortEx))
      {
        _reverse = sortEx.ToLowerInvariant().EndsWith(" desc");
        if (_reverse)
          _sortColumn = sortEx.Substring(0, sortEx.Length - 5);
        else
          _sortColumn = sortEx;
      }
    }

    public bool Equals(Project x, Project y) {
      if (x.Id == y.Id) {
        return true;
      }
      else {
        return false;
      }
    }

    public int Compare(Project x, Project y) {
      int retVal = 0;
      switch (_sortColumn) {

        case "CompletionDate":
          retVal = DateTime.Compare(x.CompletionDate, y.CompletionDate);
          break;
        case "Description":
          retVal = String.Compare(x.Description, y.Description, StringComparison.InvariantCultureIgnoreCase);
          break;
        case "EstimateDuration":
          retVal = (int)(x.EstimateDuration - y.EstimateDuration);
          break;
        case "Id":
          retVal = (x.Id - y.Id);
          break;
        case "ManagerUserName":
          retVal = String.Compare(x.ManagerUserName, y.ManagerUserName, StringComparison.InvariantCultureIgnoreCase);
          break;
        case "Name":
          retVal = String.Compare(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
          break;
      }
      return (retVal * (_reverse ? -1 : 1));
    }

    public int GetHashCode(Project obj) {
      // TODO: Implement this, but it's not necessary for sorting
      return 0;
    }
  }
}
