using System.Collections;
using System.Linq;

namespace TimeDataViewer.Core
{
    public abstract class ItemsSeries : Series
    {
        public IEnumerable? ItemsSource { get; set; }

        protected internal override void UpdateValidData()
        {
        }

        protected static object? GetItem(IEnumerable? itemsSource, int index)
        {
            if (itemsSource == null || index < 0)
            {
                return null;
            }

            var list = itemsSource as IList;
            if (list != null)
            {
                if (index < list.Count && index >= 0)
                {
                    return list[index];
                }

                return null;
            }

            var i = 0;
            return itemsSource.Cast<object>().FirstOrDefault(item => i++ == index);
        }

        protected virtual object? GetItem(int i)
        {
            return GetItem(ItemsSource, i);
        }
    }
}
