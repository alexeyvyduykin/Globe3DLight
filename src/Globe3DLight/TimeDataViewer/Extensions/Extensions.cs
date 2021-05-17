using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeDataViewer.Spatial;
using System.Collections.ObjectModel;
using Avalonia;

namespace TimeDataViewer
{
    public static class Extensions
    {
        public static bool IsEmpty(this RectD rect)
        {
            return rect.Left == 0.0 && rect.Right == 0.0 && rect.Width == 0.0 && rect.Height == 0.0;
        }

        public static bool IsEmpty(this Point2D point)
        {
            return point.X == 0.0 && point.Y == 0.0;
        }

        public static Point2D GetCenter(this RectD rect)
        {
            return new(rect.X + rect.Width / 2, rect.Y - rect.Height / 2);
        }

        public static void AddRange<T>(this ObservableCollection<T> arr, IEnumerable<T> values)
        {
            foreach (var item in values)
            {
                arr.Add(item);
            }
        }

        public static Rect ToAvaloniaRect(this RectD rect)
        {
            return new Rect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
