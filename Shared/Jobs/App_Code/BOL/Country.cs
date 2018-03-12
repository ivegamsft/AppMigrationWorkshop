#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using JobSiteStarterKit.DAL;
using System.Data.SqlClient;

#endregion

namespace JobSiteStarterKit.BOL
{
    public class Country
    {
        public Country()
        {
        }

        private string strCountryName;
        private int intCountryID;

        public string CountryName
        {
            get
            {
                return strCountryName;
            }
            set
            {
                strCountryName = value;
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


        public int InsertCountry(Country c)
        {
            DBAccess db = new DBAccess();
            SqlParameter p = new SqlParameter("@iCountryID", 0);
            p.Direction = ParameterDirection.Output;
            db.AddParameter("@sCountryName", c.CountryName);
            db.AddParameter(p);
            return db.ExecuteNonQuery("JobsDb_Countries_Insert");

        }

        public int UpdateCountry(Country c)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iCountryID", c.CountryID);
            db.AddParameter("@sCountryName", c.CountryName);
            return db.ExecuteNonQuery("JobsDb_Countries_Update");
        }

        public DataSet SelectCountries()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_Countries_SelectAll");
        }



        public static DataSet GetCountries()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_Countries_SelectAll");
        }


        public static string GetCountryName(int id)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iCountryID", id);
            return db.ExecuteScalar("JobsDb_Countries_GetCountryName").ToString();
        }

    }
}
