#nullable enable
using System.Globalization;

namespace TimeDataViewer.Spatial
{
    public struct SizeI
    {
        private static readonly SizeI s_empty = new();
        private int _width;
        private int _height;

        public SizeI(Point2I pt)
        {
            _width = pt.X;
            _height = pt.Y;
        }

        public SizeI(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public static SizeI Empty => s_empty;

        public static SizeI operator +(SizeI sz1, SizeI sz2)
        {
            return Add(sz1, sz2);
        }

        public static SizeI operator -(SizeI sz1, SizeI sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(SizeI sz1, SizeI sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        public static bool operator !=(SizeI sz1, SizeI sz2)
        {
            return !(sz1 == sz2);
        }

        public static explicit operator Point2I(SizeI size)
        {
            return new Point2I(size.Width, size.Height);
        }

        public bool IsEmpty => _width == 0 && _height == 0;

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public static SizeI Add(SizeI sz1, SizeI sz2)
        {
            return new SizeI(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static SizeI Subtract(SizeI sz1, SizeI sz2)
        {
            return new SizeI(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is SizeI))
                return false;

            var comp = (SizeI)obj;
            // Note value types can't have derived classes, so we don't need to            
            return (comp._width == _width) && (comp._height == _height);
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }

            return (_width.GetHashCode() ^ _height.GetHashCode());
        }

        public override string ToString()
        {
            return "{Width=" + _width.ToString(CultureInfo.CurrentCulture) + ", Height=" + _height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
