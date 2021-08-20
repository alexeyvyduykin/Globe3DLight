using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using TimeDataViewer.Spatial;
using Path = Avalonia.Controls.Shapes.Path;

namespace TimeDataViewer
{
    public class CanvasRenderContext
    {
        private const int _maxFiguresPerGeometry = 16;
        private const int _maxPolylinesPerLine = 64;
        private const int _minPointsPerPolyline = 16;
        private readonly Dictionary<Color, IBrush> _abrushCache = new Dictionary<Color, IBrush>();
        private readonly Canvas _canvas;
        private readonly Rect? _clip;
        private readonly string _currentToolTip;

        public CanvasRenderContext(Canvas canvas)
        {
            _canvas = canvas;
            UseStreamGeometry = false; // Temporarily disabled because of Avalonia bug
            RendersToScreen = true;
            BalancedLineDrawingThicknessLimit = 3.5;
        }

        // Gets or sets the thickness limit for "balanced" line drawing.  
        public double BalancedLineDrawingThicknessLimit { get; set; }

        // Gets or sets a value indicating whether to use stream geometry for lines and polygons rendering.
        public bool UseStreamGeometry { get; set; }

        // Gets or sets a value indicating whether the context renders to screen.
        public bool RendersToScreen { get; set; }

        public void DrawLine(IList<ScreenPoint> points, Color stroke, double[] dashArray, bool aliased)
        {
            if (1.0 < BalancedLineDrawingThicknessLimit)
            {
                DrawLineBalanced(points, stroke, dashArray, aliased);
                return;
            }

            var e = CreateAndAdd<Polyline>();
            SetStroke(e, stroke, dashArray, 0, aliased);

            e.Points = ToPointCollection(points, aliased);
        }

        public void DrawLine(double x0, double y0, double x1, double y1, Pen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            DrawLine(new[] { new ScreenPoint(x0, y0), new ScreenPoint(x1, y1) }, ((ImmutableSolidColorBrush)pen.Brush).Color, GetDashArray(pen.DashStyle), aliased);
        }

        public void DrawLine(Point p0, Point p1, Pen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }

            if (1.0 < BalancedLineDrawingThicknessLimit)
            {
                DrawLineBalanced(new List<Point>() { p0, p1 }, pen, aliased);
                return;
            }

            var e = CreateAndAdd<Line>();

            e.Stroke = pen.Brush;
            e.StrokeThickness = pen.Thickness;

            e.StartPoint = ToPoint(p0, aliased);
            e.EndPoint = ToPoint(p1, aliased);
        }

        private void DrawLineBalanced(IList<Point> points, Pen stroke, bool aliased)
        {
            // balance the number of points per polyline and the number of polylines
            var numPointsPerPolyline = Math.Max(points.Count / _maxPolylinesPerLine, _minPointsPerPolyline);

            var polyline = CreateAndAdd<Polyline>();
            polyline.Stroke = stroke.Brush;
            polyline.StrokeThickness = stroke.Thickness;
            var pc = new List<Point>(numPointsPerPolyline);

            var n = points.Count;
            double lineLength = 0;
            var dashPatternLength = (stroke.DashStyle != null) ? stroke.DashStyle.Dashes.Sum() : 0;
            var last = new Point();
            for (int i = 0; i < n; i++)
            {
                var p = aliased ? ToPixelAlignedPoint(points[i]) : points[i];
                pc.Add(p);

                // alt. 1
                if (stroke.DashStyle != null)
                {
                    if (i > 0)
                    {
                        var delta = p - last;
                        var dist = Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
                        lineLength += dist;
                    }

                    last = p;
                }

                // use multiple polylines with limited number of points to improve Avalonia performance
                if (pc.Count >= numPointsPerPolyline)
                {
                    polyline.Points = pc;

                    if (i < n - 1)
                    {
                        // alt.2
                        ////if (dashArray != null)
                        ////{
                        ////    lineLength += GetLength(polyline);
                        ////}

                        // start a new polyline at last point so there is no gap (it is not necessary to use the % operator)
                        var dashOffset = dashPatternLength > 0 ? lineLength / 1.0/*thickness*/ : 0;
                        polyline = CreateAndAdd<Polyline>();
                        polyline.Stroke = stroke.Brush;
                        polyline.StrokeThickness = stroke.Thickness;
                        pc = new List<Point>(numPointsPerPolyline) { pc.Last() };
                    }
                }
            }

            if (pc.Count > 1 || n == 1)
            {
                polyline.Points = pc;
            }
        }

        private static Point ToPoint(Point pt, bool aliased)
        {
            return aliased ? ToPixelAlignedPoint(pt) : pt;
        }

        private static Point ToPixelAlignedPoint(Point pt)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            // TODO: issue 10221 - should consider line thickness and logical to physical size of pixels
            return new Point(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
        }

        public void DrawLineSegments(IList<ScreenPoint> points, Pen pen, bool aliased = true)
        {
            if (pen == null)
            {
                return;
            }
            if (pen.Brush is ImmutableSolidColorBrush brush1)
            {
                DrawLineSegments(points, brush1.Color, GetDashArray(pen.DashStyle), aliased);
            }
            else if (pen.Brush is SolidColorBrush brush2)
            {
                DrawLineSegments(points, brush2.Color, GetDashArray(pen.DashStyle), aliased);
            }
        }

        public static double[] GetDashArray(IDashStyle style)
        {
            if (style == DashStyle.Dash)
            {
                return new double[] { 4, 1 };
            }
            else if (style == DashStyle.Dot)
            {
                return new double[] { 1, 1 };
            }

            return null;
        }

        // Draws line segments defined by points (0,1) (2,3) (4,5) etc.
        // This should have better performance than calling DrawLine for each segment.
        public void DrawLineSegments(IList<ScreenPoint> points, Color stroke, double[] dashArray, bool aliased)
        {
            if (UseStreamGeometry)
            {
                DrawLineSegmentsByStreamGeometry(points, stroke, dashArray, aliased);
                return;
            }

            Path path = null;
            PathGeometry pathGeometry = null;

            var count = 0;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (path == null)
                {
                    path = CreateAndAdd<Path>();
                    SetStroke(path, stroke, dashArray, 0, aliased);
                    pathGeometry = new PathGeometry();
                }

                var figure = new PathFigure { StartPoint = ToPoint(points[i], aliased), IsClosed = false };
                figure.Segments.Add(new LineSegment { Point = ToPoint(points[i + 1], aliased) });
                pathGeometry.Figures.Add(figure);

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > _maxFiguresPerGeometry || dashArray != null)
                {
                    path.Data = pathGeometry;
                    path = null;
                    count = 0;
                }
            }

            if (path != null)
            {
                path.Data = pathGeometry;
            }
        }

        public void DrawRectangle(OxyRect rect, IBrush? fill, IPen? stroke)
        {
            var e = CreateAndAdd<Rectangle>(rect.Left, rect.Top);

            if (stroke != null)
            {
                e.Stroke = stroke.Brush;
                e.StrokeThickness = stroke.Thickness;
            }

            if (fill != null)
            {
                e.Fill = fill;
            }

            e.Width = rect.Width;
            e.Height = rect.Height;

            Canvas.SetLeft(e, rect.Left);
            Canvas.SetTop(e, rect.Top);
        }

        public void DrawText(ScreenPoint p, TextBlock textBlock, OxySize? maxSize)
        {
            var tb = Add<TextBlock>(textBlock);

            double dx = 0;
            double dy = 0;

            if (maxSize != null || textBlock.HorizontalAlignment != HorizontalAlignment.Left || textBlock.VerticalAlignment != VerticalAlignment.Top)
            {
                tb.Measure(new Size(1000, 1000));
                var size = tb.DesiredSize;
                if (maxSize != null)
                {
                    if (size.Width > maxSize.Value.Width + 1e-3)
                    {
                        size = size.WithWidth(Math.Max(maxSize.Value.Width, 0));
                    }

                    if (size.Height > maxSize.Value.Height + 1e-3)
                    {
                        size = size.WithHeight(Math.Max(maxSize.Value.Height, 0));
                    }

                    tb.Width = size.Width;
                    tb.Height = size.Height;
                }

                if (textBlock.HorizontalAlignment == HorizontalAlignment.Center)
                {
                    dx = -size.Width / 2;
                }

                if (textBlock.HorizontalAlignment == HorizontalAlignment.Right)
                {
                    dx = -size.Width;
                }

                if (textBlock.VerticalAlignment == VerticalAlignment.Center)
                {
                    dy = -size.Height / 2;
                }

                if (textBlock.VerticalAlignment == VerticalAlignment.Bottom)
                {
                    dy = -size.Height;
                }
            }

            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(dx, dy));
            transform.Children.Add(new TranslateTransform(p.X, p.Y));
            tb.RenderTransform = transform;
            if (tb.Clip != null)
            {
                tb.Clip.Transform = new MatrixTransform(tb.RenderTransform.Value.Invert());
            }
            tb.RenderTransformOrigin = new RelativePoint(0.0, 0.0, RelativeUnit.Relative);
        }

        // Measures the size of the specified text.
        public OxySize MeasureText(TextBlock textBlock)
        {
            if (string.IsNullOrEmpty(textBlock.Text))
            {
                return OxySize.Empty;
            }

            textBlock.Measure(new Size(1000, 1000));

            return new OxySize(textBlock.DesiredSize.Width, textBlock.DesiredSize.Height);
        }

        // Creates an element of the specified type and adds it to the canvas.
        private T CreateAndAdd<T>(double clipOffsetX = 0, double clipOffsetY = 0) where T : Control, new()
        {
            // TODO: here we can reuse existing elements in the canvas.Children collection
            var element = new T();

            if (_clip != null)
            {
                element.Clip = new RectangleGeometry(new Rect(_clip.Value.X - clipOffsetX, _clip.Value.Y - clipOffsetY, _clip.Value.Width, _clip.Value.Height));
            }

            _canvas.Children.Add(element);

            ApplyToolTip(element);
            return element;
        }

        private T Add<T>(T element, double clipOffsetX = 0, double clipOffsetY = 0) where T : Control, new()
        {
            if (_clip != null)
            {
                element.Clip = new RectangleGeometry(new Rect(_clip.Value.X - clipOffsetX, _clip.Value.Y - clipOffsetY, _clip.Value.Width, _clip.Value.Height));
            }

            _canvas.Children.Add(element);

            ApplyToolTip(element);

            return element;
        }

        // Applies the current tool tip to the specified element.
        private void ApplyToolTip(Control element)
        {
            if (!string.IsNullOrEmpty(_currentToolTip))
            {
                ToolTip.SetTip(element, _currentToolTip);
            }
        }

        // Draws the line segments by stream geometry.
        private void DrawLineSegmentsByStreamGeometry(IList<ScreenPoint> points, Color stroke, double[] dashArray, bool aliased)
        {
            StreamGeometry streamGeometry = null;
            StreamGeometryContext streamGeometryContext = null;

            var count = 0;

            for (int i = 0; i + 1 < points.Count; i += 2)
            {
                if (streamGeometry == null)
                {
                    streamGeometry = new StreamGeometry();
                    streamGeometryContext = streamGeometry.Open();
                }

                streamGeometryContext.BeginFigure(ToPoint(points[i], aliased), false);
                streamGeometryContext.LineTo(ToPoint(points[i + 1], aliased));

                count++;

                // Must limit the number of figures, otherwise drawing errors...
                if (count > _maxFiguresPerGeometry || dashArray != null)
                {
                    streamGeometryContext.Dispose();
                    var path = CreateAndAdd<Path>();
                    SetStroke(path, stroke, dashArray, 0, aliased);
                    path.Data = streamGeometry;
                    streamGeometry = null;
                    count = 0;
                }
            }

            if (streamGeometry != null)
            {
                streamGeometryContext.Dispose();
                var path = CreateAndAdd<Path>();
                SetStroke(path, stroke, null, 0, aliased);
                path.Data = streamGeometry;
            }
        }

        private IBrush GetCachedBrush(Color color)
        {
            if (color.A == 0)
            {
                return null;
            }

            if (!_abrushCache.TryGetValue(color, out IBrush brush))
            {
                brush = new SolidColorBrush(color);
                _abrushCache.Add(color, brush);
            }

            return brush;
        }

        // Sets the stroke properties of the specified shape object.        
        private void SetStroke(Shape shape, Color stroke, IEnumerable<double> dashArray = null, double dashOffset = 0, bool aliased = false)
        {
            if (stroke != null)
            {
                shape.Stroke = GetCachedBrush(stroke);
                shape.StrokeJoin = PenLineJoin.Miter;
                shape.StrokeThickness = 1.0;

                if (dashArray != null)
                {
                    shape.StrokeDashArray = new global::Avalonia.Collections.AvaloniaList<double>(dashArray);
                    shape.StrokeDashOffset = dashOffset;
                }
            }
        }
        // Draws the line using the MaxPolylinesPerLine and MinPointsPerPolyline properties.

        private void DrawLineBalanced(IList<ScreenPoint> points, Color stroke, double[] dashArray, bool aliased)
        {
            // balance the number of points per polyline and the number of polylines
            var numPointsPerPolyline = Math.Max(points.Count / _maxPolylinesPerLine, _minPointsPerPolyline);

            var polyline = CreateAndAdd<Polyline>();
            SetStroke(polyline, stroke, dashArray, 0, aliased);
            var pc = new List<Point>(numPointsPerPolyline);

            var n = points.Count;
            double lineLength = 0;
            var dashPatternLength = (dashArray != null) ? dashArray.Sum() : 0;
            var last = new Point();
            for (int i = 0; i < n; i++)
            {
                var p = aliased ? ToPixelAlignedPoint(points[i]) : ToPoint(points[i]);
                pc.Add(p);

                // alt. 1
                if (dashArray != null)
                {
                    if (i > 0)
                    {
                        var delta = p - last;
                        var dist = Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y));
                        lineLength += dist;
                    }

                    last = p;
                }

                // use multiple polylines with limited number of points to improve Avalonia performance
                if (pc.Count >= numPointsPerPolyline)
                {
                    polyline.Points = pc;

                    if (i < n - 1)
                    {
                        // alt.2
                        ////if (dashArray != null)
                        ////{
                        ////    lineLength += GetLength(polyline);
                        ////}

                        // start a new polyline at last point so there is no gap (it is not necessary to use the % operator)
                        var dashOffset = dashPatternLength > 0 ? lineLength / 1.0/*thickness*/ : 0;
                        polyline = CreateAndAdd<Polyline>();
                        SetStroke(polyline, stroke, dashArray, dashOffset, aliased);
                        pc = new List<Point>(numPointsPerPolyline) { pc.Last() };
                    }
                }
            }

            if (pc.Count > 1 || n == 1)
            {
                polyline.Points = pc;
            }
        }

        // Converts a <see cref="Point2D" /> to a <see cref="Point" />.
        private static Point ToPoint(ScreenPoint pt)
        {
            return new Point(pt.X, pt.Y);
        }

        // Converts a <see cref="Point2D" /> to a pixel aligned<see cref="Point" />.
        private static Point ToPixelAlignedPoint(ScreenPoint pt)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            // TODO: issue 10221 - should consider line thickness and logical to physical size of pixels
            return new Point(0.5 + (int)pt.X, 0.5 + (int)pt.Y);
        }

        // Converts a <see cref="Point2D" /> to a <see cref="Point" />.
        private static Point ToPoint(ScreenPoint pt, bool aliased)
        {
            return aliased ? ToPixelAlignedPoint(pt) : ToPoint(pt);
        }

        // Creates a point collection from the specified points.
        private static List<Point> ToPointCollection(IEnumerable<ScreenPoint> points, bool aliased)
        {
            return new List<Point>(aliased ? points.Select(ToPixelAlignedPoint) : points.Select(ToPoint));
        }
    }
}
