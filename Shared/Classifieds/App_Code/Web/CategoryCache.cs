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
    /// Summary description for CategoryCache
    /// </summary>
    public class CachedCategory
    {

        private string _name;
        private int _id;
        private int _parentId;
        private int _numActiveAds;

        private string _indentedName;
        private string _nameWithActiveCount;

        private string _idString;

        public string Name { get { return _name; } }
        public string NameWithActiveCount { get { return _nameWithActiveCount; } }
        public string LevelIndentedName { get { return _indentedName; } }

        public int Id { get { return _id; } }
        public string IdString { get { return _idString; } }
        public int ParentCategoryId { get { return _parentId; } }

        public int NumActiveAds { get { return _numActiveAds; } }

        private CachedCategory()
        {
        }
        
        public CachedCategory(string name, int id, int parentCategoryId, int numActiveAds, string levelPrefix)
        {
            this._name = name;
            this._id = id;
            this._parentId = parentCategoryId;
            this._numActiveAds = numActiveAds;
            this._nameWithActiveCount = String.Format("{0} ({1})", name, numActiveAds);

            this._indentedName = levelPrefix + " " + name;
            this._idString = id.ToString();
        }
    }

    public sealed class CategoryCache
    {
        internal const string AllCategoriesKey = "CategoryCache_AllCategoriesKey";
        private static readonly string[] AllCategoriesKeyArrayWrap = new string[] { AllCategoriesKey };

        private CategoryCache()
        {
        }

        public static void ClearAll(HttpContext context)
        {
            context.Cache.Remove(AllCategoriesKey);
        }

        public static List<CachedCategory> GetAllCategories()
        {
            return GetAllCategories(HttpContext.Current);
        }

        public static List<CachedCategory> GetParentCategoriesById(int categoryId)
        {
            return GetParentCategoriesById(HttpContext.Current, categoryId);
        }

        public static List<CachedCategory> GetCategoriesByParentId(int parentCategoryId)
        {
            return GetCategoriesByParentId(HttpContext.Current, parentCategoryId, false);
        }

        public static List<CachedCategory> GetBrowseCategoriesByParentId(int parentCategoryId)
        {
            return GetCategoriesByParentId(HttpContext.Current, parentCategoryId, true);
        }

        public static List<CachedCategory> GetAllCategories(HttpContext context)
        {
            if (context == null)
                return null;

            DateTime expiration = DateTime.Now.AddDays(1);
            List<CachedCategory> allCategories = context.Cache[AllCategoriesKey] as List<CachedCategory>;
            if (allCategories == null)
            {
                allCategories = FetchAllCategories();
                context.Cache.Add(AllCategoriesKey, allCategories, null, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            }
            return allCategories;
        }

        public static List<CachedCategory> GetParentCategoriesById(HttpContext context, int categoryId)
        {
            string cacheKey = "PC" + categoryId.ToString();
            DateTime expiration = DateTime.Now.AddDays(1);
            List<CachedCategory> parentCategories = context.Cache[cacheKey] as List<CachedCategory>;
            if (parentCategories == null)
            {
                parentCategories = FetchParentCategoriesById(categoryId);
                CacheDependency watchAllCategoriesKey = new System.Web.Caching.CacheDependency(null, AllCategoriesKeyArrayWrap);
                context.Cache.Add(cacheKey, parentCategories, watchAllCategoriesKey, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return parentCategories;
        }

        public static List<CachedCategory> GetCategoriesByParentId(HttpContext context, int parentCategoryId, bool updateFrequently)
        {
            string cacheKey = (updateFrequently ? "BR" : "SC") + parentCategoryId.ToString();
            DateTime expiration = updateFrequently ? DateTime.Now.AddSeconds(1) : DateTime.Now.AddDays(1);

            List<CachedCategory> subCategories = context.Cache[cacheKey] as List<CachedCategory>;
            if (subCategories == null)
            {
                subCategories = FetchCategoriesByParentId(parentCategoryId);
                CacheDependency watchAllCategoriesKey = new System.Web.Caching.CacheDependency(null, AllCategoriesKeyArrayWrap);
                context.Cache.Add(cacheKey, subCategories, watchAllCategoriesKey, expiration, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return subCategories;
        }

        private static List<CachedCategory> FetchParentCategoriesById(int categoryId)
        {
            List<CachedCategory> parentCategories = new List<CachedCategory>();
            CategoriesDataComponent.CategoriesDataTable parentsResult = CategoriesDB.GetParentCategoriesById(categoryId);
            foreach (CategoriesDataComponent.CategoriesRow parentCategory in parentsResult)
            {
                int parentCategoryId = DefaultValues.CategoryIdMinValue;
                // must ensure that we don't access a NULL field
                if (!parentCategory.IsParentCategoryIdNull())
                    parentCategoryId = parentCategory.ParentCategoryId;

                parentCategories.Add(new CachedCategory(parentCategory.Name, parentCategory.Id, parentCategoryId, parentCategory.NumActiveAds, String.Empty));
            }
            return parentCategories;
        }

        private static List<CachedCategory> FetchCategoriesByParentId(int parentCategoryId)
        {
            List<CachedCategory> subCategories = new List<CachedCategory>();
            using (CategoriesDB db = new CategoriesDB())
            {
                FetchCategoriesRecursively(false, subCategories, db, parentCategoryId, String.Empty);
            }
            return subCategories;
        }

        private static List<CachedCategory> FetchAllCategories()
        {
            List<CachedCategory> list = new List<CachedCategory>();
            using (CategoriesDB db = new CategoriesDB())
            {
                FetchCategoriesRecursively(true, list, db, DefaultValues.CategoryIdMinValue, String.Empty);
            }
            return list;
        }

        private static void FetchCategoriesRecursively(bool recursing, List<CachedCategory> list, CategoriesDB db, int categoryId, string levelPrefix)
        {
            CategoriesDataComponent.CategoriesDataTable subCategories = db.GetCategoriesByParentId(categoryId);

            if (subCategories != null)
            {
                string categoryIdString = categoryId.ToString();
                foreach (CategoriesDataComponent.CategoriesRow category in subCategories)
                {
                    list.Add(new CachedCategory(category.Name, category.Id, categoryId, category.NumActiveAds, levelPrefix));

                    if (recursing)
                        FetchCategoriesRecursively(recursing, list, db, category.Id, levelPrefix + "--");
                }
            }
        }
    }
}