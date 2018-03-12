#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using JobSiteStarterKit.DAL;
using System.Data;
using System.Data.SqlClient;
#endregion

namespace JobSiteStarterKit.BOL
{
    public class MyJob
    {

        private int intMyJobID;
        private int intPostingID;
        private string strUserName;

        public int MyJobID
        {
            get
            {
                return intMyJobID;
            }
            set
            {
                intMyJobID = value;
            }
        }

        public int PostingID
        {
            get
            {
                return intPostingID;
            }
            set
            {
                intPostingID = value;
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


        public static int Insert(MyJob j)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iPostingID", j.PostingID);
            db.AddParameter("@sUserName", j.UserName);
            SqlParameter p = new SqlParameter("@iMyJobID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            db.AddParameter(p);

            int retval = db.ExecuteNonQuery("JobsDb_MyJobs_Insert");
            if (retval == 1)
            {
                return int.Parse(p.Value.ToString());
            }
            else
            {
                return -1;
            }
        }


        public static int Delete(MyJob j)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iMyJobID",j.MyJobID);
            return db.ExecuteNonQuery("JobsDb_MyJobs_Delete");
        }

        public static DataSet GetMyJobs(string username)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sUserName", username);
            return db.ExecuteDataSet("JobsDb_MyJobs_SelectForUser");
        }
    }
}
