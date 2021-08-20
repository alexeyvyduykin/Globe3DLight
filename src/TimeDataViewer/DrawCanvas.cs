using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using TimeDataViewer.Spatial;

namespace TimeDataViewer
{
    public class DrawCanvas : Canvas
    {
        private Rect? _clip;
        private readonly Pen _selectedPen = new() { Brush = Brushes.Black, Thickness = 4 };
        private readonly Dictionary<TimelineSeries, IList<Rect>> _dict = new();
        private readonly Dictionary<TimelineSeries, IList<Rect>> _selectedDict = new();

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (_dict.Count != 0)
            {
                foreach (var series in _dict.Keys)
                {
                    var pen = new Pen() { Brush = series.StrokeBrush };

                    foreach (var item in _dict[series])
                    {
                        context.DrawRectangle(series.FillBrush, pen, item);
                    }
                }
            }

            if (_selectedDict.Count != 0)
            {
                foreach (var series in _selectedDict.Keys)
                {
                    foreach (var item in _selectedDict[series])
                    {
                        context.DrawRectangle(series.FillBrush, _selectedPen, item);
                    }
                }
            }
        }

        public void RenderSeries(IEnumerable<Series> series)
        {
            _dict.Clear();
            _selectedDict.Clear();

            foreach (var s in series)
            {
                List<Rect> list1 = new List<Rect>();
                List<Rect> list2 = new List<Rect>();

                var innserSeries = ((Core.TimelineSeries)s.InternalSeries);

                foreach (var item in innserSeries.MyRectList)
                {
                    CreateClippedRectangle(innserSeries.MyClippingRect, ToRect(item), list1);
                }

                _dict.Add((TimelineSeries)s, list1);

                foreach (var item in innserSeries.MySelectedRectList)
                {
                    CreateClippedRectangle(innserSeries.MyClippingRect, ToRect(item), list2);
                }

                _selectedDict.Add((TimelineSeries)s, list2);
            }

            InvalidateVisual();
        }

        protected void CreateClippedRectangle(OxyRect clippingRectangle, Rect rect, IList<Rect> list)
        {
            if (SetClip(clippingRectangle))
            {
                list.Add(rect);
                ResetClip();
                return;
            }

            var clippedRect = ClipRect(rect, clippingRectangle);
            if (clippedRect == null)
            {
                return;
            }

            list.Add(clippedRect.Value);
        }

        protected bool SetClip(OxyRect clippingRect)
        {
            _clip = ToRect(clippingRect);
            return false;//true;
        }

        protected static Rect? ClipRect(Rect rect, OxyRect clippingRectangle)
        {
            if (rect.Right < clippingRectangle.Left)
            {
                return null;
            }

            if (rect.Left > clippingRectangle.Right)
            {
                return null;
            }

            if (rect.Top > clippingRectangle.Bottom)
            {
                return null;
            }

            if (rect.Bottom < clippingRectangle.Top)
            {
                return null;
            }

            var width = rect.Width;
            var left = rect.Left;
            var top = rect.Top;
            var height = rect.Height;

            if (left + width > clippingRectangle.Right)
            {
                width = clippingRectangle.Right - left;
            }

            if (left < clippingRectangle.Left)
            {
                width = rect.Right - clippingRectangle.Left;
                left = clippingRectangle.Left;
            }

            if (top < clippingRectangle.Top)
            {
                height = rect.Bottom - clippingRectangle.Top;
                top = clippingRectangle.Top;
            }

            if (top + height > clippingRectangle.Bottom)
            {
                height = clippingRectangle.Bottom - top;
            }

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return null;
            }

            return new Rect(left, top, width, height);
        }

        protected void ResetClip()
        {
            _clip = null;
        }

        protected static Rect ToRect(OxyRect r)
        {
            return new Rect(r.Left, r.Top, r.Width, r.Height);
        }
    }
}
