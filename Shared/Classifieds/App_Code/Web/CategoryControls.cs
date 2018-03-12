using System;

namespace AspNet.StarterKits.Classifieds.Web
{
    public delegate void CategorySelectionChangedEventHandler(object sender, CategorySelectionChangedEventArgs e);
    public class CategorySelectionChangedEventArgs : EventArgs
    {
        private int _categoryId;
        public int CategoryId { get { return _categoryId; } }

        public CategorySelectionChangedEventArgs(int categoryId)
        {
            this._categoryId = categoryId;
        }

    }
}