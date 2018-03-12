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
    public class Company
    {
        private int intCompanyID;
        private string strUserName;

        private string strCompanyName;
        private string strBriefProfile;

        private string strAddress1;
        private string strAddress2;
        private string strCity;
        private int intStateID;
        private int intCountryID;
        private string strZIP;

        private string strPhone;
        private string strFax;
        private string strEmail;
        private string strWebSiteUrl;



        public int CompanyID
        {
            get
            {
                return intCompanyID;
            }
            set
            {
                intCompanyID = value;
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

        public string CompanyName
        {
            get
            {
                return strCompanyName;
            }
            set
            {
                strCompanyName = value;
            }
        }

        public string BriefProfile
        {
            get
            {
                return strBriefProfile;
            }
            set
            {
                strBriefProfile = value;
            }
        }

        public string Address1
        {
            get
            {
                return strAddress1;
            }
            set
            {
                strAddress1 = value;
            }
        }

        public string Address2
        {
            get
            {
                return strAddress2;
            }
            set
            {
                strAddress2 = value;
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

        public string ZIP
        {
            get
            {
                return strZIP;
            }
            set
            {
                strZIP = value;
            }
        }

        public string Phone
        {
            get
            {
                return strPhone;
            }
            set
            {
                strPhone = value;
            }
        }

        public string Fax
        {
            get
            {
                return strFax;
            }
            set
            {
                strFax = value;
            }
        }
        public string Email
        {
            get
            {
                return strEmail;
            }
            set
            {
                strEmail = value;
            }
        }
        public string WebSiteUrl
        {
            get
            {
                return strWebSiteUrl;
            }
            set
            {
                strWebSiteUrl = value;
            }
        }

        public static int Insert(Company c)
        {
            DBAccess db = new DBAccess();

            SqlParameter objParam = new SqlParameter("@iCompanyID",0);
            objParam.Direction = ParameterDirection.Output;

            db.Parameters.Add(new SqlParameter("@sUserName", c.UserName));
            db.Parameters.Add(new SqlParameter("@sCompanyName", c.CompanyName));
            db.Parameters.Add(new SqlParameter("@sAddress1", c.Address1));
            db.Parameters.Add(new SqlParameter("@sAddress2", c.Address2));
            db.Parameters.Add(new SqlParameter("@sCity", c.City));
            db.Parameters.Add(new SqlParameter("@iStateID", c.StateID));
            db.Parameters.Add(new SqlParameter("@iCountryID", c.CountryID));
            db.Parameters.Add(new SqlParameter("@sZip", c.ZIP));
            db.Parameters.Add(new SqlParameter("@sPhone", c.Phone));
            db.Parameters.Add(new SqlParameter("@sFax", c.Fax));
            db.Parameters.Add(new SqlParameter("@sCompanyEmail", c.Email));
            db.Parameters.Add(new SqlParameter("@sWebSiteUrl", c.WebSiteUrl));
            db.Parameters.Add(new SqlParameter("@sCompanyProfile", c.BriefProfile));
            db.Parameters.Add(objParam);

            int retval = db.ExecuteNonQuery("JobsDb_Companies_Insert");
            if (retval == 1)
            {
                return int.Parse(objParam.Value.ToString());
            }
            else
            {
                return -1;
            }

        }

        public static int Update(Company c)
        {
            DBAccess db = new DBAccess();

            db.Parameters.Add(new SqlParameter("@iCompanyID", c.CompanyID));
            db.Parameters.Add(new SqlParameter("@sUserName", c.UserName));
            db.Parameters.Add(new SqlParameter("@sCompanyName", c.CompanyName));
            db.Parameters.Add(new SqlParameter("@sAddress1", c.Address1));
            db.Parameters.Add(new SqlParameter("@sAddress2", c.Address2));
            db.Parameters.Add(new SqlParameter("@sCity", c.City));
            db.Parameters.Add(new SqlParameter("@iStateID", c.StateID));
            db.Parameters.Add(new SqlParameter("@iCountryID", c.CountryID));
            db.Parameters.Add(new SqlParameter("@sZip", c.ZIP));
            db.Parameters.Add(new SqlParameter("@sPhone", c.Phone));
            db.Parameters.Add(new SqlParameter("@sFax", c.Fax));
            db.Parameters.Add(new SqlParameter("@sCompanyEmail", c.Email));
            db.Parameters.Add(new SqlParameter("@sWebSiteUrl", c.WebSiteUrl));
            db.Parameters.Add(new SqlParameter("@sCompanyProfile", c.BriefProfile));

            int retval = db.ExecuteNonQuery("JobsDb_Companies_Update");

            return retval;
        }

        public static Company GetCompany(string username)
        {
            DBAccess db = new DBAccess();
            db.Parameters.Add(new SqlParameter("@sUserName", username));
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader("JobsDb_Companies_SelectByUserName");
            if (dr.HasRows)
            {
                Company c = new Company();
                while (dr.Read())
                {
                    c.CompanyID = dr.GetInt32(dr.GetOrdinal("companyid"));
                    c.CompanyName = dr.GetString(dr.GetOrdinal("CompanyName"));
                    c.BriefProfile = dr.GetString(dr.GetOrdinal("CompanyProfile"));
                    c.WebSiteUrl = dr.GetString(dr.GetOrdinal("WebSiteUrl"));
                    c.Address1 = dr.GetString(dr.GetOrdinal("Address1"));
                    c.Address2 = dr.GetString(dr.GetOrdinal("Address2"));
                    c.City = dr.GetString(dr.GetOrdinal("City"));
                    c.StateID = dr.GetInt32(dr.GetOrdinal("StateID"));
                    c.CountryID = dr.GetInt32(dr.GetOrdinal("CountryID"));
                    c.ZIP = dr.GetString(dr.GetOrdinal("ZIP"));
                    c.Phone = dr.GetString(dr.GetOrdinal("Phone"));
                    c.Fax = dr.GetString(dr.GetOrdinal("Fax"));
                    c.Email = dr.GetString(dr.GetOrdinal("CompanyEmail"));
                    c.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                }
                dr.Close();
                return c;
            }
            else
            {
                dr.Close();
                return null;
            }
        }

        public static Company GetCompany(int companyid)
        {
            DBAccess db = new DBAccess();
            db.Parameters.Add(new SqlParameter("@iCompanyID", companyid));
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader("JobsDb_Companies_SelectOne");
            if (dr.HasRows)
            {
                Company c = new Company();
                while (dr.Read())
                {
                    c.CompanyID = dr.GetInt32(dr.GetOrdinal("companyid"));
                    c.CompanyName = dr.GetString(dr.GetOrdinal("CompanyName"));
                    c.BriefProfile = dr.GetString(dr.GetOrdinal("CompanyProfile"));
                    c.WebSiteUrl = dr.GetString(dr.GetOrdinal("WebSiteUrl"));
                    c.Address1 = dr.GetString(dr.GetOrdinal("Address1"));
                    c.Address2 = dr.GetString(dr.GetOrdinal("Address2"));
                    c.City = dr.GetString(dr.GetOrdinal("City"));
                    c.StateID = dr.GetInt32(dr.GetOrdinal("StateID"));
                    c.CountryID = dr.GetInt32(dr.GetOrdinal("CountryID"));
                    c.ZIP = dr.GetString(dr.GetOrdinal("ZIP"));
                    c.Phone = dr.GetString(dr.GetOrdinal("Phone"));
                    c.Fax = dr.GetString(dr.GetOrdinal("Fax"));
                    c.Email = dr.GetString(dr.GetOrdinal("CompanyEmail"));
                    c.UserName = dr.GetString(dr.GetOrdinal("UserName"));
                }
                dr.Close();
                return c;
            }
            else
            {
                dr.Close();
                return null;
            }
        }


        public static string GetCompanyName(int companyid)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iCompanyID", companyid);
            return (string)db.ExecuteScalar("JobsDb_Companies_SelectName");
        }

        public static int GetCompanyCount()
        {
            DBAccess db = new DBAccess();
            return (int)db.ExecuteScalar("JobsDb_Companies_GetCount");
        }
    }
}
