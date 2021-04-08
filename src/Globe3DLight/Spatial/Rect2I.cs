#nullable enable
using System;
using System.Globalization;

namespace Globe3DLight.Spatial
{
    public struct Rect2I
    {
        private static readonly Rect2I s_empty = new();
        private int _x;
        private int _y;
        private int _width;
        private int _height;

        public Rect2I(int x, int y, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public Rect2I(Point2I location, SizeI size)
        {
            _x = location.X;
            _y = location.Y;
            _width = size.Width;
            _height = size.Height;
        }

        public Rect2I(Point2I point1, Point2I point2)
        {
            _x = point1.X;
            _y = point1.Y;
            _width = point2.X - point1.X;
            _height = point2.Y - point1.Y;
        }

        public static Rect2I Empty => s_empty;

        public static Rect2I FromLTRB(int left, int top, int right, int bottom)
        {
            return new(left, top, right - left, bottom - top);
        }

        public Point2I Location
        {
            get => new(X, Y);
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        public Point2I RightBottom => new(Right, Bottom);

        public Point2I RightTop => new(Right, Top);

        public Point2I LeftBottom => new(Left, Bottom);

        public SizeI Size
        {
            get => new(Width, Height);
            set
            {
                _width = value.Width;
                _height = value.Height;
            }
        }

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

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

        public int Left => _x;

        public int Top => _y;

        public int Right => _x + _width;

        public int Bottom => _y + _height;

        public bool IsEmpty => _height == 0 && _width == 0 && _x == 0 && _y == 0;

        public override bool Equals(object? obj)
        {
            if (!(obj is Rect2I))
                return false;

            var comp = (Rect2I)obj;
            return (comp.X == X) && (comp.Y == Y) && (comp.Width == Width) && (comp.Height == Height);
        }

        public static bool operator ==(Rect2I left, Rect2I right)
        {
            return (left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height);
        }

        public static bool operator !=(Rect2I left, Rect2I right)
        {
            return !(left == right);
        }

        public bool Contains(long x, long y)
        {
            return _x <= x && x < _x + _width && _y <= y && y < _y + _height;
        }

        public bool Contains(Point2I pt)
        {
            return Contains(pt.X, pt.Y);
        }

        public bool Contains(Rect2I rect)
        {
            return (_x <= rect.X) && ((rect.X + rect.Width) <= (_x + _width)) && (_y <= rect.Y) && ((rect.Y + rect.Height) <= (_y + _height));
        }

        public override int GetHashCode()
        {
            if (IsEmpty)
            {
                return 0;
            }

            return ((_x ^ ((_y << 13) | (_y >> 0x13))) ^ ((_width << 0x1a) | (_width >> 6))) ^ ((_height << 7) | (_height >> 0x19));
        }

        public void Inflate(int width, int height)
        {
            _x -= width;
            _y -= height;
            _width += 2 * width;
            _height += 2 * height;
        }

        public void Inflate(SizeI size)
        {
            Inflate(size.Width, size.Height);
        }

        public static Rect2I Inflate(Rect2I rect, int x, int y)
        {
            var r = rect;
            r.Inflate(x, y);
            return r;
        }

        public void Intersect(Rect2I rect)
        {
            var result = Intersect(rect, this);

            _x = result.X;
            _y = result.Y;
            _width = result.Width;
            _height = result.Height;
        }

        public static Rect2I Intersect(Rect2I a, Rect2I b)
        {
            int x1 = Math.Max(a.X, b.X);
            int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Max(a.Y, b.Y);
            int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rect2I(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        public bool IntersectsWith(Rect2I rect)
        {
            return (rect.X < _x + _width) && (_x < (rect.X + rect.Width)) && (rect.Y < _y + _height) && (_y < rect.Y + rect.Height);
        }

        public static Rect2I Union(Rect2I a, Rect2I b)
        {
            int x1 = Math.Min(a.X, b.X);
            int x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            int y1 = Math.Min(a.Y, b.Y);
            int y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new(x1, y1, x2 - x1, y2 - y1);
        }

        public void Offset(Point2I pos)
        {
            Offset(pos.X, pos.Y);
        }

        public void OffsetNegative(Point2I pos)
        {
            Offset(-pos.X, -pos.Y);
        }

        public void Offset(int x, int y)
        {
            _x += x;
            _y += y;
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
               ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
               ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}
