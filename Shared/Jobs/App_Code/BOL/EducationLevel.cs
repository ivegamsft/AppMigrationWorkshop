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
    public class EducationLevel
    {
        private int intEduLevelID;
        private string strEduLevelName;

        public int EducationLevelID
        {
            get
            {
                return intEduLevelID;
            }
            set
            {
                intEduLevelID=value;
            }
        }

        public string EducationLevelName
        {
            get
            {
                return strEduLevelName;
            }
            set
            {
                strEduLevelName=value;
            }
        }


        public static DataSet GetEducationLevels()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_EducationLevels_SelectAll");
        }

        public static string GetEducationLevelName(int id)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iEducationLevelID", id);
            return db.ExecuteScalar("JobsDb_EducationLevels_GetLevelName").ToString();
        }

        public static int Insert(EducationLevel l)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sEducationLevelName", l.EducationLevelName);
            SqlParameter p = new SqlParameter("@iEducationLevelID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            db.AddParameter(p);

            int retval = db.ExecuteNonQuery("JobsDb_EducationLevels_Insert");
            if (retval == 1)
            {
                return int.Parse(p.Value.ToString());
            }
            else
            {
                return -1;
            }
        }

        public static int Update(EducationLevel l)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iEducationLevelID", l.EducationLevelID);
            db.AddParameter("@sEducationLevelName", l.EducationLevelName);
            return db.ExecuteNonQuery("JobsDb_EducationLevels_Update");
        }

        public static int Delete(EducationLevel e)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iEducationLevelID",e.EducationLevelID);
            return db.ExecuteNonQuery("JobsDb_EducationLevels_Delete");
        }


    }
}
