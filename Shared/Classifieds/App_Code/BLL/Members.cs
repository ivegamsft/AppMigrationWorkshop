using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MembersDataComponentTableAdapters;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
	/// <summary>
	/// Contains the implementation for the creation of users.
	/// </summary>
	public sealed class MembersDB
    {
        private MembersDB()
        {
        }

        public static int CreateMember(string username, string aspApplicationName)
        {
            DateTime dateCreated = DateTime.Now;

            // All users are added to Guests role upon registration.
            Roles.AddUserToRole(username, "Guests");

            using (MembersDataAdapter db = new MembersDataAdapter())
            {
                return (int)db.InsertMember(username, aspApplicationName, dateCreated);
            }
        }
    }
}
