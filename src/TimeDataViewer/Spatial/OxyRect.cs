#nullable enable
using System;

namespace TimeDataViewer.Spatial
{
    public struct OxyRect
    {
        private readonly double _height;
        private readonly double _left;
        private readonly double _top;
        private readonly double _width;

        public OxyRect(double left, double top, double width, double height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException("width", "The width should not be negative.");
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException("height", "The height should not be negative.");
            }

            _left = left;
            _top = top;
            _width = width;
            _height = height;
        }

        public OxyRect(ScreenPoint p0, ScreenPoint p1)
            : this(Math.Min(p0.X, p1.X), Math.Min(p0.Y, p1.Y), Math.Abs(p1.X - p0.X), Math.Abs(p1.Y - p0.Y))
        {
        }

        public OxyRect(ScreenPoint p0, OxySize size)
            : this(p0.X, p0.Y, size.Width, size.Height)
        {
        }

        public double Bottom => _top + _height;

        public double Height => _height;

        public double Left => _left;

        public double Right => _left + _width;

        public double Top => _top;

        public double Width => _width;

        public ScreenPoint Center => new ScreenPoint(_left + (_width * 0.5), _top + (_height * 0.5));

        /// <summary>
        /// Returns a rectangle that is expanded by the specified thickness, in all directions.
        /// </summary>
        /// <param name="t">The thickness to apply to the rectangle.</param>
        /// <returns>The inflated <see cref="OxyRect" />.</returns>
        public OxyRect Inflate(double left, double top, double right, double bottom)
        {
            return new OxyRect(left - left, top - top, _width + left + right, _height + top + bottom);
        }

        /// <summary>
        /// Returns a rectangle that is shrunk by the specified thickness, in all directions.
        /// </summary>
        /// <param name="t">The thickness to apply to the rectangle.</param>
        /// <returns>The deflated <see cref="OxyRect" />.</returns>
        public OxyRect Deflate(double left, double top, double right, double bottom)
        {
            return new OxyRect(left + left, top + top, Math.Max(0, _width - left - right), Math.Max(0, _height - top - bottom));
        }

        public bool Contains(ScreenPoint p)
        {
            return Contains(p._x, p._y);
        }

        public bool Contains(double x, double y)
        {
            return x >= Left && x <= Right && y >= Top && y <= Bottom;
        }

        public static OxyRect Create(double x0, double y0, double x1, double y1)
        {
            return new OxyRect(Math.Min(x0, x1), Math.Min(y0, y1), Math.Abs(x1 - x0), Math.Abs(y1 - y0));
        }
    }
}
