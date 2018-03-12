#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using JobSiteStarterKit.DAL;
using System.Data;

#endregion

namespace JobSiteStarterKit.BOL
{
    public class State
    {

        public static DataSet GetStates()
        {
            DBAccess db = new DBAccess();
            return db.ExecuteDataSet("JobsDb_States_SelectAll");
        }

        public static DataSet GetStates(int countryid)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iCountryID", countryid);
            return db.ExecuteDataSet("JobsDb_States_SelectForCountry");
        }

        public static string GetStateName(int id)
        {
            DBAccess db = new DBAccess();
            db.AddParameter("@iStateID", id);
            return db.ExecuteScalar("JobsDb_States_GetStateName").ToString();
        }
    }
}
