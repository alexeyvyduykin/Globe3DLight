using System.Collections.Generic;

namespace TimeDataViewer.Core
{
    public abstract class CategorizedSeries : XYAxisSeries
    {
        internal abstract double GetBarWidth();

        protected internal abstract IList<CategorizedItem> GetItems();

        public abstract double GetActualBarWidth();

        public abstract CategoryAxis GetCategoryAxis();
    }
}
