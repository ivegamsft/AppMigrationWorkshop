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
    /// Summary description for CachedFeatureAd
    /// </summary>
    public class CachedFeaturedAd
    {
		private int _adId;
		private int _previewImageId;
		private string _title;
		
		public int AdId { get { return _adId; } }
        public int PreviewImageId { get { return _previewImageId; } }
		public string Title { get { return _title; } }
        
		public CachedFeaturedAd(int adId, int previewImageId, string title)
		{
			this._adId = adId;
			this._previewImageId = previewImageId;
			this._title = title;
		}
    }

    /// <summary>
    /// Summary description for FeaturedAdCache
    /// </summary>
    public sealed class FeaturedAdCache
    {
        private const int NumCachedFeatures = 10;
        private const int CacheLifeTime = 10; // in minutes

		private static Random random = new Random();
        private const string CacheKeyPrefix = "FeaturedAdCache_";

        private FeaturedAdCache()
        {
        }

        public static void ClearAll(HttpContext context)
        {
            for (int i = 0; i < NumCachedFeatures; i++)
                context.Cache.Remove(GetCacheKey(i));
        }

        private static string GetCacheKey(int featureCacheNumber)
        {
            return CacheKeyPrefix + featureCacheNumber.ToString();
        }


        public static CachedFeaturedAd GetFeaturedAd()
        {
            return GetFeaturedAd(HttpContext.Current);
        }

        public static CachedFeaturedAd GetFeaturedAd(HttpContext context)
        {
            if (context == null)
                return null;

            int featureCacheNumber = random.Next(0, NumCachedFeatures - 1);
            string cacheKey = GetCacheKey(featureCacheNumber);
			
            CachedFeaturedAd feature = context.Cache[cacheKey] as CachedFeaturedAd;
            if (feature == null)
            {
                feature = FetchFeaturedAd();
				if (feature != null) 
				{
					DateTime expiration = DateTime.Now.AddMinutes(CacheLifeTime);
					context.Cache.Add(cacheKey, feature, null, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
				}
            }

			return feature;
            
        }

		private static CachedFeaturedAd FetchFeaturedAd() 
		{
			CachedFeaturedAd feature = null;

			AdsDataComponent.AdsDataTable table = AdsDB.GetFeaturedAdsSelection(1);
			if (table != null && table.Rows.Count > 0) 
			{
				AdsDataComponent.AdsRow featuredAd = table.Rows[0] as AdsDataComponent.AdsRow;
                feature = new CachedFeaturedAd(featuredAd.Id, featuredAd.PreviewImageId, featuredAd.Title);
            }

			return feature;
		}
    }
}