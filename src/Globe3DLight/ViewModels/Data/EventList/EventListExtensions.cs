using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public static class EventListExtensions
    {
        public static IEventList<T> ToEventList<T>(this IEnumerable<T> intervals) where T : IEventInterval
        {
            var list = new EventList<T>();

            list.AddRange(intervals);

            return list;
        }

        public static void AddRange<T>(this IEventList<T> list, IEnumerable<T> items) where T : IEventInterval
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
