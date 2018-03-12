#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using JobSiteStarterKit.DAL;

#endregion

namespace JobSiteStarterKit.BOL
{
    public class ExperienceLevel
    {

        private int intExpLevelID;
        private string strExpLevelName;

        public int ExperienceLevelID
        {
            get
            {
                return intExpLevelID;
            }
            set
            {
                intExpLevelID = value;
            }
        }

        public string ExperienceLevelName
        {
            get
            {
                return strExpLevelName;
            }
            set
            {
                strExpLevelName = value;
            }
        }

        public static DataSet GetExperienceLevels()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_ExperienceLevels_SelectAll");
        }

        public static int Insert(ExperienceLevel l)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sExperienceLevelName", l.ExperienceLevelName);
            SqlParameter p = new SqlParameter("@iExperienceLevelID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            db.AddParameter(p);

            int retval = db.ExecuteNonQuery("JobsDb_ExperienceLevels_Insert");
            if (retval == 1)
            {
                return int.Parse(p.Value.ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int Update(ExperienceLevel l)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iExperienceLevelID", l.ExperienceLevelID);
            db.AddParameter("@sExperienceLevelName", l.ExperienceLevelName);
            return db.ExecuteNonQuery("JobsDb_ExperienceLevels_Update");
        }

        public static int Delete(ExperienceLevel l)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iExperienceLevelID",l.ExperienceLevelID);
            return db.ExecuteNonQuery("JobsDb_ExperienceLevels_Delete");

        }

        public static string GetExperienceLevelName(int id)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iExperienceLevelID", id);
            return db.ExecuteScalar("JobsDb_ExperienceLevels_GetLevelName").ToString();
        }
    }
}
