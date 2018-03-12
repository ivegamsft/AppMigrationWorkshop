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
    public class MySearch
    {
        private int intMySearchID;
        private string strCriteria;
        private int intCountryID;
        private int intStateID;
        private string strCity;
        private string strUserName;

        public int MySearchID
        {
            get
            {
                return intMySearchID;
            }
            set
            {
                intMySearchID = value;
            }
        }

        public string Criteria
        {
            get
            {
                return strCriteria;
            }
            set
            {
                strCriteria = value;
            }
        }

        public int CountryID
        {
            get
            {
                return intCountryID;
            }
            set
            {
                intCountryID = value;
            }
        }

        public int StateID
        {
            get
            {
                return intStateID;
            }
            set
            {
                intStateID = value;
            }
        }

        public string City
        {
            get
            {
                return strCity;
            }
            set
            {
                strCity = value;
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
                strUserName=value;
            }
        }


        public static int Insert(MySearch s)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sSearchCriteria", s.Criteria);
            db.AddParameter("@iCountryID", s.CountryID);
            db.AddParameter("@iStateID", s.StateID);
            db.AddParameter("@iCity", s.City);
            db.AddParameter("@sUserName", s.UserName);
            SqlParameter p = new SqlParameter("@iMySearchID", SqlDbType.Int);
            p.Direction = ParameterDirection.Output;
            db.AddParameter(p);

            int retval=db.ExecuteNonQuery("JobsDb_MySearches_Insert");
            if (retval == 1)
            {
                return int.Parse(p.Value.ToString());
            }
            else
            {
                return -1;
            }
        }

        public static DataSet GetMySearches(string username)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@sUserName", username);
            return db.ExecuteDataSet("JobsDb_MySearches_SelectForUser");
        }

        public static MySearch GetMySearch(int mysearchid)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iMySearchID", mysearchid);
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader("JobsDb_MySearches_SelectOne");

            if (dr.HasRows)
            {
                MySearch s = new MySearch();
                while (dr.Read())
                {
                    s.MySearchID = dr.GetInt32(dr.GetOrdinal("mysearchid"));
                    s.Criteria = dr.GetString(dr.GetOrdinal("SearchCriteria"));
                    s.CountryID = dr.GetInt32(dr.GetOrdinal("CountryID"));
                    s.StateID = dr.GetInt32(dr.GetOrdinal("StateID"));
                    s.City = dr.GetString(dr.GetOrdinal("City"));
                    s.UserName = dr.GetString(dr.GetOrdinal("UserName"));

                }
                dr.Close();
                return s;
            }
            else
            {
                return null;
            }

        }


        public static int Delete(MySearch s)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iMySearchID", s.MySearchID);
            return db.ExecuteNonQuery("JobsDb_MySearches_Delete");
        }

    }
}
