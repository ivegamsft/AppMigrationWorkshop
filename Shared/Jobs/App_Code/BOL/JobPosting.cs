#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using JobSiteStarterKit.DAL;
using System.Data.SqlClient;
using System.Web;

#endregion

namespace JobSiteStarterKit.BOL
{
    public class JobPosting
    {

        #region Member Variables

        private int intJobPostingID;
        private int intCompanyID;
        private string strContactPerson;
        private string strTitle;
        private string strDepartment;
        private string strJobCode;
        private string strCity;
        private int intStateID;
        private int intCountryID;
        private int intEducationLevelID;
        private int intJobTypeID;
        private decimal dblMinSalary;
        private decimal dblMaxSalary;
        private string strDescription;
        private DateTime dtPostingDate=DateTime.Now;
        private string strPostedBy=HttpContext.Current.Profile.UserName;

        #endregion

        #region Public Properties

        public int JobPostingID
        {
            get
            {
                return intJobPostingID;
            }
            set
            {
                intJobPostingID=value;
            }
        }

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

        public string ContactPerson
        {
            get
            {
                return strContactPerson;
            }
            set
            {
                strContactPerson = value;
            }
        }

        public string Title
        {
            get
            {
                return strTitle;
            }
            set
            {
                strTitle = value;
            }
        }

        public string Department
        {
            get
            {
                return strDepartment;
            }
            set
            {
                strDepartment = value;
            }
        }

        public string JobCode
        {
            get
            {
                return strJobCode;
            }
            set
            {
                strJobCode = value;
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

        public int EducationLevelID
        {
            get
            {
                return intEducationLevelID;
            }
            set
            {
                intEducationLevelID = value;
            }
        }

        public int JobTypeID
        {
            get
            {
                return intJobTypeID;
            }
            set
            {
                intJobTypeID = value;
            }
        }

        public decimal MinSalary
        {
            get
            {
                return dblMinSalary;
            }
            set
            {
                dblMinSalary = value;
            }
        }

        public decimal MaxSalary
        {
            get
            {
                return dblMaxSalary;
            }
            set
            {
                dblMaxSalary = value;
            }
        }

        public string Description
        {
            get
            {
                return strDescription;
            }
            set
            {
                strDescription = value;
            }
        }

        public DateTime PostingDate
        {
            get
            {
                return dtPostingDate;
            }
            set
            {
                dtPostingDate = value;
            }
        }

        public string PostedBy
        {
            get
            {
                return strPostedBy;
            }
            set
            {
                strPostedBy = value;
            }

        }
#endregion

        #region Public Static Methods


        public static int Insert(JobPosting p)
        {
            DBAccess db = new DBAccess();

            SqlParameter objParam = new SqlParameter("@iPostingID", 0);
            objParam.Direction = ParameterDirection.Output;

            db.Parameters.Add(new SqlParameter("@iCompanyID", p.CompanyID));
            db.Parameters.Add(new SqlParameter("@sContactPerson", p.ContactPerson));
            db.Parameters.Add(new SqlParameter("@sTitle", p.Title));
            db.Parameters.Add(new SqlParameter("@sDepartment", p.Department));
            db.Parameters.Add(new SqlParameter("@sJobCode", p.JobCode));
            db.Parameters.Add(new SqlParameter("@sCity", p.City));
            db.Parameters.Add(new SqlParameter("@iStateID", p.StateID));
            db.Parameters.Add(new SqlParameter("@iCountryID", p.CountryID));
            db.Parameters.Add(new SqlParameter("@iEducationLevelID", p.EducationLevelID));
            db.Parameters.Add(new SqlParameter("@iJobTypeID", p.JobTypeID));
            db.Parameters.Add(new SqlParameter("@curMinSalary", p.MinSalary));
            db.Parameters.Add(new SqlParameter("@curMaxSalary", p.MaxSalary));
            db.Parameters.Add(new SqlParameter("@sJobDescription", p.Description));
            db.Parameters.Add(new SqlParameter("@daPostingDate", p.PostingDate));
            db.Parameters.Add(new SqlParameter("@sPostedBy", p.PostedBy));
            db.Parameters.Add(objParam);

            int retval = db.ExecuteNonQuery("JobsDb_JobPostings_Insert");
            if (retval == 1)
            {
                return int.Parse(objParam.Value.ToString());
            }
            else
            {
                return -1;
            }


        }

        public static int Update(JobPosting p)
        {
            DBAccess db = new DBAccess();

            db.Parameters.Add(new SqlParameter("@iPostingID", p.JobPostingID));
            db.Parameters.Add(new SqlParameter("@iCompanyID", p.CompanyID));
            db.Parameters.Add(new SqlParameter("@sContactPerson", p.ContactPerson));
            db.Parameters.Add(new SqlParameter("@sTitle", p.Title));
            db.Parameters.Add(new SqlParameter("@sDepartment", p.Department));
            db.Parameters.Add(new SqlParameter("@sJobCode", p.JobCode));
            db.Parameters.Add(new SqlParameter("@sCity", p.City));
            db.Parameters.Add(new SqlParameter("@iStateID", p.StateID));
            db.Parameters.Add(new SqlParameter("@iCountryID", p.CountryID));
            db.Parameters.Add(new SqlParameter("@iEducationLevelID", p.EducationLevelID));
            db.Parameters.Add(new SqlParameter("@iJobTypeID", p.JobTypeID));
            db.Parameters.Add(new SqlParameter("@curMinSalary", p.MinSalary));
            db.Parameters.Add(new SqlParameter("@curMaxSalary", p.MaxSalary));
            db.Parameters.Add(new SqlParameter("@sJobDescription", p.Description));
            db.Parameters.Add(new SqlParameter("@daPostingDate", p.PostingDate));
            db.Parameters.Add(new SqlParameter("@sPostedBy", p.PostedBy));

            int retval = db.ExecuteNonQuery("JobsDb_JobPostings_Update");
            return retval;

        }

        public static int Delete(JobPosting p)
        {
            DBAccess db = new DBAccess();
            db.Parameters.Add(new SqlParameter("@iPostingID", p.JobPostingID));
            int retval = db.ExecuteNonQuery("JobsDb_JobPostings_Delete");
            return retval;
        }

        public static JobPosting GetPosting(int id)
        {
            DBAccess db = new DBAccess();
            db.Parameters.Add(new SqlParameter("@iPostingID", id));
            SqlDataReader dr = (SqlDataReader)db.ExecuteReader("JobsDb_JobPostings_SelectOne");
            if (dr.HasRows)
            {
                JobPosting objJobPosting = new JobPosting();
                while (dr.Read())
                {
                    objJobPosting.JobPostingID = dr.GetInt32(dr.GetOrdinal("PostingID"));
                    objJobPosting.CompanyID = dr.GetInt32(dr.GetOrdinal("CompanyID"));
                    objJobPosting.Title = dr.GetString(dr.GetOrdinal("Title"));
                    objJobPosting.ContactPerson = dr.GetString(dr.GetOrdinal("ContactPerson"));
                    objJobPosting.Department = dr.GetString(dr.GetOrdinal("Department"));
                    objJobPosting.Description = dr.GetString(dr.GetOrdinal("JobDescription"));
                    objJobPosting.City = dr.GetString(dr.GetOrdinal("City"));
                    objJobPosting.StateID = dr.GetInt32(dr.GetOrdinal("StateID"));
                    objJobPosting.CountryID = dr.GetInt32(dr.GetOrdinal("CountryID"));
                    objJobPosting.EducationLevelID = dr.GetInt32(dr.GetOrdinal("EducationLevelID"));
                    objJobPosting.JobTypeID = dr.GetInt32(dr.GetOrdinal("JobTypeID"));
                    objJobPosting.JobCode = dr.GetString(dr.GetOrdinal("JobCode"));
                    objJobPosting.MinSalary = dr.GetDecimal(dr.GetOrdinal("MinSalary"));
                    objJobPosting.MaxSalary = dr.GetDecimal(dr.GetOrdinal("MaxSalary"));
                    objJobPosting.PostingDate = dr.GetDateTime(dr.GetOrdinal("PostingDate"));
                    objJobPosting.PostedBy = dr.GetString(dr.GetOrdinal("PostedBy"));
                }
                dr.Close();
                return objJobPosting;
            }
            else
            {
                dr.Close();
                return new JobPosting();
            }

        }

        public static DataSet GetPostings(string username)
        {
            DBAccess db = new DBAccess();
            db.Parameters.Add(new SqlParameter("@sUserName", username));
            DataSet ds = db.ExecuteDataSet("JobsDb_JobPostings_SelectByUser");
            return ds;
        }


        public static DataSet SearchJobs(string skills,int countryid,int stateid)
        {
            string[] arr = skills.Split(' ');
            DataSet ds = new DataSet();
            DataSet dsTemp = new DataSet();
            bool flag = false;

            DBAccess db = new DBAccess();

            foreach (string s in arr)
            {
                db.AddParameter("@sSkill", s);
                if (countryid == -1)
                {
                    db.AddParameter("@iCountryID", DBNull.Value);
                }
                else
                {
                    db.AddParameter("@iCountryID", countryid);
                }
                if (stateid == -1)
                {
                    db.AddParameter("@iStateID", DBNull.Value);
                }
                else
                {
                    db.AddParameter("@iStateID", stateid);
                }
                dsTemp = db.ExecuteDataSet("JobsDb_JobPostings_SelecForMatchingSkills");
                db.Parameters.Clear();
                ds.Merge(dsTemp);
                if (flag == false)
                {
                    DataColumn[] pk = new DataColumn[1];
                    pk[0] = ds.Tables[0].Columns["postingid"];
                    ds.Tables[0].PrimaryKey = pk;
                    flag = true;
                }
            }
            return ds;

        }


        public static int GetJobPostingCount()
        {
            DBAccess db = new DBAccess();
            return (int)db.ExecuteScalar("JobsDb_JobPostings_GetCount");
        }


        public static DataSet GetLatest()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_JobPostings_GetLatest");
        }

        #endregion
    }

}
