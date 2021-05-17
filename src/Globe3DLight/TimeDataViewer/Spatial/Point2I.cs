#nullable enable
using System;
using System.Globalization;

namespace TimeDataViewer.Spatial
{
    public struct Point2I : IComparable<Point2I>
    {
        private int _x;
        private int _y;
        private static readonly Point2I s_empty = new();

        public Point2I(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Point2I(SizeI sz)
        {
            _x = sz.Width;
            _y = sz.Height;
        }

        public static Point2I Empty => s_empty;

        public bool IsEmpty => _x == 0 && _y == 0;

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


        public static explicit operator SizeI(Point2I p)
        {
            return new SizeI(p.X, p.Y);
        }

        public static Point2I operator +(Point2I pt, SizeI sz)
        {
            return Add(pt, sz);
        }

        public static Point2I operator -(Point2I pt, SizeI sz)
        {
            return Subtract(pt, sz);
        }

        public static bool operator ==(Point2I left, Point2I right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Point2I left, Point2I right)
        {
            return !(left == right);
        }

        public static bool operator <(Point2I p1, Point2I p2)
        {
            return p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y);
        }

        public static bool operator >(Point2I p1, Point2I p2)
        {
            return p1.X > p2.X || (p1.X == p2.X && p1.Y > p2.Y);
        }

        public static Point2I Add(Point2I pt, SizeI sz)
        {
            return new Point2I(pt.X + sz.Width, pt.Y + sz.Height);
        }

        public static Point2I Subtract(Point2I pt, SizeI sz)
        {
            return new Point2I(pt.X - sz.Width, pt.Y - sz.Height);
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Point2I))
                return false;
            Point2I comp = (Point2I)obj;
            return comp.X == X && comp.Y == Y;
        }

        public override int GetHashCode()
        {
            return (int)(_x.GetHashCode() ^ _y.GetHashCode());
        }

        public void Offset(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public void Offset(Point2I p)
        {
            Offset(p.X, p.Y);
        }

        public void OffsetNegative(Point2I p)
        {
            Offset(-p.X, -p.Y);
        }

        public override string ToString()
        {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) + "}";
        }

        public int CompareTo(Point2I other)
        {
            return (this > other) ? -1 : ((this < other) ? 1 : 0);
        }
    }
}
