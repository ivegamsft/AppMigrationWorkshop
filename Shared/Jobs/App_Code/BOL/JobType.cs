#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using JobSiteStarterKit.DAL;
#endregion

namespace JobSiteStarterKit.BOL
{
    public class JobType
    {
        public JobType()
        {

        }

        public static DataSet GetJobTypes()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_JobTypes_SelectAll");

        }

        public static string GetJobTypeName(int id)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iJobTypeID", id);
            return db.ExecuteScalar("JobsDb_JobTypes_GetTypeName").ToString();
        }
    }
}
