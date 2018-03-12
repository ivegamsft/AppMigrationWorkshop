using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using StatsDataComponentTableAdapters;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    /// <summary>
    /// Summary description for Statistics
    /// </summary>
    public class Stats
    {

        private const int RecentMembersDays = 7;
        
        private DateTime _lastGenerated;

        private int _adsPending;
        private int _adsActive;
        private int _adsDeleted;
        private int _adsTotal;

        private int _responsesActiveAds;
        private int _responsesTotal;

        private int _usersNew;
        private int _usersTotal;

        private int _categoriesTop;
        private int _locations;

        public DateTime LastGenerated { get { return _lastGenerated; } }

        public int PendingAds { get { return _adsPending; } }
        public int ActiveAds { get { return _adsActive; } }
        public int DeletedAds { get { return _adsDeleted; } }
        public int TotalAds { get { return _adsTotal; } }

        public int ActiveAdResponses { get { return _responsesActiveAds; } }
        public int TotalResponses { get { return _responsesTotal; } }

        public int NewUsers { get { return _usersNew; } }
        public int TotalUsers { get { return _usersTotal; } }

        public int TopCategories { get { return _categoriesTop; } }
        public int Locations { get { return _locations; } }

        public static Stats GetStatistics()
        {
            Stats stats = new Stats();
            using (StatsDataAdapter db = new StatsDataAdapter())
            {
                stats._adsPending = Convert.ToInt32(db.CountAdsByStatus((int)AdStatus.ActivationPending, null));
                stats._adsActive = Convert.ToInt32(db.CountAdsByStatus((int)AdStatus.Activated, null));
                stats._adsDeleted = Convert.ToInt32(db.CountAdsByStatus((int)AdStatus.Deleted, null));
                stats._adsTotal = Convert.ToInt32(db.CountAdsByStatus(null, null));

                stats._responsesActiveAds = Convert.ToInt32(db.CountAdResponsesByStatus((int)AdStatus.Activated));
                stats._responsesTotal = Convert.ToInt32(db.CountAdResponsesByStatus(null));

                stats._usersNew = (int)db.CountMembersByDateRange(DateTime.Today.AddDays(0 - RecentMembersDays), DateTime.Now);
                stats._usersTotal = (int)db.CountMembersByDateRange(null, null);

                stats._categoriesTop = (int)db.CountTopCategories();
                stats._locations = (int)db.CountLocations();

                stats._lastGenerated = DateTime.Now;
            }

            return stats;

        }

        public static int CountAdsByStatus(AdStatus adStatus)
        {
            using (StatsDataAdapter db = new StatsDataAdapter())
            {
                return Convert.ToInt32(db.CountAdsByStatus((int)adStatus, null));
            }
        }
    }
}