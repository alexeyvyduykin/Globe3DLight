#nullable enable
using System;
using System.Globalization;

namespace TimeDataViewer.Spatial
{
    public struct Point2D : IComparable<Point2D>
    {
        public readonly double X;
        public readonly double Y;
        private static readonly Point2D s_empty = new();

        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point2D Empty => s_empty;

        public bool IsEmpty => X == 0 && Y == 0;

        public void Deconstruct(out double x, out double y)
        {
            x = this.X;
            y = this.Y;
        }

        public static Point2D FromXY(double x, double y)
        {
            return new Point2D(x, y);
        }

        public static Point2D operator +(Point2D point1, Point2D point2)
        {
            return new Point2D(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static Point2D operator -(Point2D point1, Point2D point2)
        {
            return new Point2D(point1.X - point2.X, point1.Y - point2.Y);
        }

        public double DistanceTo(Point2D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return (double)Math.Sqrt((dx * dx) + (dy * dy));
        }

        public double AngleBetween(Point2D other)
        {
            double angle = (double)Math.Atan2(other.Y - Y, other.X - X);
            double result = angle * 180.0 / Math.PI;
            if (result < 0.0)
                result += 360.0;
            return result;
        }

        public Point2D RotateAt(Point2D center, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180.0);
            double cosTheta = (double)Math.Cos(angleInRadians);
            double sinTheta = (double)Math.Sin(angleInRadians);
            return new Point2D(
                (cosTheta * (X - center.X)) - (sinTheta * (Y - center.Y)) + center.X,
                (sinTheta * (X - center.X)) + (cosTheta * (Y - center.Y)) + center.Y);
        }

        public Point2D ProjectOnLine(Point2D a, Point2D b)
        {
            double x1 = a.X, y1 = a.Y, x2 = b.X, y2 = b.Y, x3 = X, y3 = Y;
            double px = x2 - x1, py = y2 - y1, dAB = (px * px) + (py * py);
            double u = (((x3 - x1) * px) + ((y3 - y1) * py)) / dAB;
            double x = x1 + (u * px), y = y1 + (u * py);
            return new Point2D(x, y);
        }

        public Point2D NearestOnLine(Point2D a, Point2D b)
        {
            double ax = X - a.X;
            double ay = Y - a.Y;
            double bx = b.X - a.X;
            double by = b.Y - a.Y;
            double t = ((ax * bx) + (ay * by)) / ((bx * bx) + (by * by));
            if (t < 0.0)
            {
                return new Point2D(a.X, a.Y);
            }
            else if (t > 1.0)
            {
                return new Point2D(b.X, b.Y);
            }
            return new Point2D((bx * t) + a.X, (by * t) + a.Y);
        }

        public bool IsOnLine(Point2D a, Point2D b)
        {
            double minX = (double)Math.Min(a.X, b.X);
            double maxX = (double)Math.Max(a.X, b.X);
            double minY = (double)Math.Min(a.Y, b.Y);
            double maxY = (double)Math.Max(a.Y, b.Y);
            return minX <= X && X <= maxX && minY <= Y && Y <= maxY;
        }

        public RectD ExpandToRect(double radius)
        {
            double size = radius * 2;
            return new RectD(X - radius, Y - radius, size, size);
        }

        public static bool operator <(Point2D p1, Point2D p2)
        {
            return p1.X < p2.X || (p1.X == p2.X && p1.Y < p2.Y);
        }

        public static bool operator >(Point2D p1, Point2D p2)
        {
            return p1.X > p2.X || (p1.X == p2.X && p1.Y > p2.Y);
        }

        public int CompareTo(Point2D other)
        {
            return (this > other) ? -1 : ((this < other) ? 1 : 0);
        }

        public bool Equals(Point2D other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Point2D ? Equals((Point2D)obj) : false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat(X, CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, Y);
        }
    }
}
