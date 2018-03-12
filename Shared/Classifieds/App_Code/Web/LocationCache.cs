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
    /// <summary>
    /// Summary description for CachedLocation
    /// </summary>
    public class CachedLocation
    {
        private int _id;
        private string _name;

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        
        public CachedLocation(int id, string name)
        {
            this._id = id;
            this._name = name;
        }
    }

    /// <summary>
    /// Summary description for LocationCache
    /// </summary>
    public sealed class LocationCache
    {
        internal const string AllLocationsKey = "LocationCache_AllLocationsKey";

        private LocationCache()
        {
        }

        public static void ClearAll(HttpContext context)
        {
            context.Cache.Remove(AllLocationsKey);
        }

        public static List<CachedLocation> GetAllLocations()
        {
            return GetAllCategories(HttpContext.Current);
        }

        public static List<CachedLocation> GetAllCategories(HttpContext context)
        {
            if (context == null)
                return null;

            DateTime expiration = DateTime.Now.AddDays(1);
            List<CachedLocation> allLocations = context.Cache[AllLocationsKey] as List<CachedLocation>;
            if (allLocations == null)
            {
                allLocations = FetchAllLocations();
                context.Cache.Add(AllLocationsKey, allLocations, null, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }
            return allLocations;
        }

        private static List<CachedLocation> FetchAllLocations()
        {
            List<CachedLocation> list = new List<CachedLocation>();
            LocationsDataComponent.LocationsDataTable locationsResult = LocationsDB.GetAllLocations();
            foreach (LocationsDataComponent.LocationsRow location in locationsResult)
            {
                list.Add(new CachedLocation(location.Id, location.Name));
            }
            return list;
        }
    }
}