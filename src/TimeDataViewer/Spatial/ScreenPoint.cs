#nullable enable
using System;

namespace TimeDataViewer.Spatial
{
    public struct ScreenPoint : IEquatable<ScreenPoint>
    {
        public static readonly ScreenPoint Undefined = new ScreenPoint(double.NaN, double.NaN);
        internal double _x;
        internal double _y;

        public ScreenPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double X => _x;

        public double Y => _y;

        /// <summary>
        /// Determines whether the specified point is undefined.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns><c>true</c> if the specified point is undefined; otherwise, <c>false</c> .</returns>
        public static bool IsUndefined(ScreenPoint point)
        {
            return double.IsNaN(point._x) && double.IsNaN(point._y);
        }

        public static ScreenPoint operator +(ScreenPoint p1, ScreenVector p2)
        {
            return new ScreenPoint(p1._x + p2._x, p1._y + p2._y);
        }

        public static ScreenVector operator -(ScreenPoint p1, ScreenPoint p2)
        {
            return new ScreenVector(p1._x - p2._x, p1._y - p2._y);
        }

        public static ScreenPoint operator -(ScreenPoint point, ScreenVector vector)
        {
            return new ScreenPoint(point._x - vector._x, point._y - vector._y);
        }

        public double DistanceTo(ScreenPoint point)
        {
            double dx = point._x - _x;
            double dy = point._y - _y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }

        public double DistanceToSquared(ScreenPoint point)
        {
            double dx = point._x - _x;
            double dy = point._y - _y;
            return (dx * dx) + (dy * dy);
        }

        public override string ToString()
        {
            return _x + " " + _y;
        }

        public bool Equals(ScreenPoint other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }
    }
}
