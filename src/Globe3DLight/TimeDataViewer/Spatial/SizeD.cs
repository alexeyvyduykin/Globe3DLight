#nullable enable
using System.Globalization;

namespace TimeDataViewer.Spatial
{
    public struct SizeD
    {
        private static readonly SizeD s_empty = new();
        private double _width;
        private double _height;

        public SizeD(Point2D pt)
        {
            _width = pt.X;
            _height = pt.Y;
        }

        public SizeD(double width, double height)
        {
            _width = width;
            _height = height;
        }

        public static SizeD Empty => s_empty;

        public static SizeD operator +(SizeD sz1, SizeD sz2)
        {
            return Add(sz1, sz2);
        }

        public static SizeD operator -(SizeD sz1, SizeD sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(SizeD sz1, SizeD sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        public static bool operator !=(SizeD sz1, SizeD sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator Point2D(SizeD size)
        {
            return new Point2D(size.Width, size.Height);
        }

        public bool IsEmpty => _width == 0 && _height == 0;

        public double Width
        {
            get => _width;
            set => _width = value;
        }

        public double Height
        {
            get => _height;
            set => _height = value;
        }

        public static SizeD Add(SizeD sz1, SizeD sz2)
        {
            return new SizeD(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static SizeD Subtract(SizeD sz1, SizeD sz2)
        {
            return new SizeD(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is SizeD))
                return false;

            var comp = (SizeD)obj;
            // Note value types can't have derived classes, so we don't need to         
            return (comp._width == _width) && (comp._height == _height);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }

            return (Width.GetHashCode() ^ Height.GetHashCode());
        }

        public override string ToString()
        {
            return "{Width=" + _width.ToString(CultureInfo.CurrentCulture) + ", Height=" + _height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
