using System;
using System.Collections;
using System.Collections.Generic;
using CategoriesDataComponentTableAdapters;


namespace AspNet.StarterKits.Classifieds.BusinessLogicLayer
{
    public class CategoriesDB : IDisposable
    {

        /// <summary>
        /// InsertCategory
        /// </summary>
        /// <param name="parentCategoryId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int InsertCategory(int parentCategoryId, string name)
        {
            int? dbParentCategoryId = parentCategoryId;
            if (parentCategoryId == DefaultValues.CategoryIdMinValue)
                dbParentCategoryId = null;
            
            using (CategoriesDataAdapter db = new CategoriesDataAdapter())
            {
                return (int) db.InsertCategory(dbParentCategoryId, name);
            }
        }

        public static bool RemoveCategory(int categoryId)
        {
            int result = 0;

            if (categoryId != DefaultValues.CategoryIdMinValue)
            {
                using (CategoriesDataAdapter db = new CategoriesDataAdapter())
                {
                    result = Convert.ToInt32(db.RemoveCategory(categoryId));
                }
            }

            return (result > 0);
        }

        public static CategoriesDataComponent.CategoriesDataTable GetCategoryById(int id)
        {
            using (CategoriesDataAdapter db = new CategoriesDataAdapter())
            {
                return db.GetCategoryById(id);
            }
        }

        public static CategoriesDataComponent.CategoriesDataTable GetParentCategoriesById(int categoryId)
        {
            using (CategoriesDataAdapter db = new CategoriesDataAdapter())
            {
                return db.GetParentCategoriesById(categoryId);
            }
        }

        public static bool MoveCategory(int categoryId, int newParentCategoryId)
        {
            int returnValue = 0;

            if (categoryId != newParentCategoryId)
            {
                using (CategoriesDataAdapter db = new CategoriesDataAdapter())
                {
                    // if newParentCategoryId is 0, we move the category to the top level (ParentCategoryId becomes NULL)
                    if (newParentCategoryId == DefaultValues.IdNullValue)
                        returnValue = Convert.ToInt32(db.MoveCategory(categoryId, null));
                    else
                        returnValue = Convert.ToInt32(db.MoveCategory(categoryId, newParentCategoryId));
                }
            }

            // only a returnValue of 1 indicates success
            return (returnValue == 1);

        }

        public static void UpdateCategoryName(int categoryId, string newName)
        {
            using (CategoriesDataAdapter db = new CategoriesDataAdapter())
            {
                db.UpdateCategoryName(categoryId, newName);
            }
        }

        public CategoriesDB()
        {
            _categories = new CategoriesDataAdapter();
        }

        private CategoriesDataAdapter _categories;
        public CategoriesDataComponent.CategoriesDataTable GetCategoriesByParentId(int parentCategroyId)
        {
            return _categories.GetCategoriesByParentId(parentCategroyId);
        }

        #region IDisposable Members

        private bool disposedValue; // To detect redundant calls
        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposedValue != true)
            {
                if (disposing)
                {
                    // Free unmanaged resources when explicitly called.
                }
                if (_categories != null)
                {
                    _categories.Dispose();
                }
            }
            this.disposedValue = true;
        }

        #endregion

        #region IDisposable Support
        // This code added to implement the disposable pattern.
        public void Dispose()
        {
            // Put cleanup code in Dispose(ByVal disposing As Boolean).
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
