namespace TimeDataViewer.Core
{
    public abstract class CategorizedItem
    {
        protected CategorizedItem()
        {
            CategoryIndex = -1;
        }

        // Gets or sets the index of the category.
        public int CategoryIndex { get; set; }

        // Gets the index of the category.
        public int GetCategoryIndex(int defaultIndex)
        {
            if (CategoryIndex < 0)
            {
                return defaultIndex;
            }

            return CategoryIndex;
        }
    }
}
