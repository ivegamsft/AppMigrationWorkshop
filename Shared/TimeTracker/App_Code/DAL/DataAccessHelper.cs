using System;
using System.Configuration;

namespace ASPNET.StarterKit.DataAccessLayer {
  public class DataAccessHelper {
    public static DataAccess GetDataAccess() {
      string dataAccessStringType = ConfigurationManager.AppSettings["aspnet_staterKits_TimeTracker_DataAccessLayerType"];
      if (String.IsNullOrEmpty(dataAccessStringType)) {
        throw (new NullReferenceException("ConnectionString configuration is missing from you web.config. It should contain  <connectionStrings> <add key=\"aspnet_staterKits_TimeTracker\" value=\"Server=(local);Integrated Security=True;Database=Issue_Tracker\" </connectionStrings>"));
      }
      else {
        Type dataAccessType = Type.GetType(dataAccessStringType);
        if (dataAccessType == null) {
          throw (new NullReferenceException("DataAccessType can not be found"));
        }
        Type tp = Type.GetType("ASPNET.StarterKit.DataAccessLayer.DataAccess");
        if (!tp.IsAssignableFrom(dataAccessType)) {
          throw (new ArgumentException("DataAccessType does not inherits from ASPNET.StarterKit.DataAccessLayer.DataAccess "));

        }
        DataAccess dc = (DataAccess)Activator.CreateInstance(dataAccessType);
        return (dc);
      }
    }
  }
}
