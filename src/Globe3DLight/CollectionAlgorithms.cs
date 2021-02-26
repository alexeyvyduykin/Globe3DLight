using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight
{
    public static class CollectionAlgorithms
    {
        public static int EnumerableCount<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            IList<T> list = enumerable as IList<T>;

            if (list != null)
            {
                return list.Count;
            }

            int count = 0;
#pragma warning disable 219
            foreach (T t in enumerable)
            {
                ++count;
            }
#pragma warning restore 219

            return count;
        }

        public static bool EnumerableCountGreaterThanOrEqual<T>(IEnumerable<T> enumerable, int minimumCount)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            if (minimumCount < 0)
            {
                throw new ArgumentOutOfRangeException("minimumCount");
            }

            IList<T> list = enumerable as IList<T>;

            if (list != null)
            {
                return list.Count >= minimumCount;
            }

            int count = 0;
#pragma warning disable 219
            foreach (T t in enumerable)
            {
                if (++count >= minimumCount)
                {
                    return true;
                }
            }
#pragma warning restore 219

            return false;
        }

        public static IList<T> EnumerableToList<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            IList<T> list = enumerable as IList<T>;

            if (list != null)
            {
                return list;
            }
            else
            {
                return new List<T>(enumerable);
            }
        }

        public static IList<T> CopyEnumerableToList<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable");
            }

            return new List<T>(enumerable);
        }
    }

}
