using System;
using System.Collections;
using System.Collections.Generic;
using LocationsDataComponentTableAdapters;

namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    public sealed class LocationsDB
    {
        private LocationsDB()
        {

        }

        public static LocationsDataComponent.LocationsDataTable GetAllLocations()
        {
            using (LocationsDataAdapter db = new LocationsDataAdapter())
            {
                return db.GetAllLocations();
            }
        }

        public static void RemoveLocation(int locationId)
        {
            using (LocationsDataAdapter db = new LocationsDataAdapter())
            {
                db.RemoveLocation(locationId);
            }
        }

        public static void InsertLocation(string name)
        {
            using (LocationsDataAdapter db = new LocationsDataAdapter())
            {
                db.InsertLocation(name);
            }
        }
    }
}
