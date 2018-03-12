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
    public class MyResume
    {
        private int intMyResumeID;
        private int intResumeID;
        private string strUserName;

        public int MyResumeID
        {
            get
            {
                return intMyResumeID;
            }
            set
            {
                intMyResumeID = value;
            }
        }

        public int ResumeID
        {
            get
            {
                return intResumeID;
            }
            set
            {
                intResumeID = value;
            }
        }


        public string UserName
        {
            get
            {
                return strUserName;
            }
            set
            {
                strUserName = value;
            }
        }


        public static int Insert(MyResume r)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iResumeID", r.ResumeID);
            db.AddParameter("@sUserName", r.UserName);
            SqlParameter p = new SqlParameter("@iMyResumeID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            db.AddParameter(p);

            int retval = db.ExecuteNonQuery("JobsDb_MyResumes_Insert");
            if (retval == 1)
            {
                return int.Parse(p.Value.ToString());
            }
            else
            {
                return -1;
            }
        }


        public static int Delete(MyResume r)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iMyResumeID",r.MyResumeID);
            return db.ExecuteNonQuery("JobsDb_MyResumes_Delete");
        }

        public static DataSet GetMyResumes(string username)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sUserName", username);
            return db.ExecuteDataSet("JobsDb_MyResumes_SelectForUser");
        }

    }
}
