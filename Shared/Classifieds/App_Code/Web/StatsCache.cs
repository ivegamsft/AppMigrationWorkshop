using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using AspNet.StarterKits.Classifieds.BusinessLogicLayer;

namespace AspNet.StarterKits.Classifieds.Web
{
    public sealed class StatsCache
    {
        internal const string StatsCacheKey = "StatsCache";
        internal const int StatsCacheDuration = 10; // in minutes

        private StatsCache()
        {
        }

        public static void Clear(HttpContext context)
        {
            context.Cache.Remove(StatsCacheKey);
        }

        public static Stats GetStatistics()
        {
            return GetStatistics(HttpContext.Current);
        }

        public static Stats GetStatistics(HttpContext context)
        {
            if (context == null)
                return null;

            Stats s = context.Cache[StatsCacheKey] as Stats;
            if (s == null)
            {
                s = Stats.GetStatistics();
                DateTime expiration = DateTime.Now.AddMinutes(StatsCacheDuration);
                context.Cache.Add(StatsCacheKey, s, null, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return s;
        }
    }
}