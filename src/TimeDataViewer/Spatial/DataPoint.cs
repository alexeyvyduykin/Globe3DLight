using System;

namespace TimeDataViewer.Spatial
{
    public struct DataPoint : IEquatable<DataPoint>
    {
        public static readonly DataPoint Undefined = new(double.NaN, double.NaN);

        internal readonly double _x;
        internal readonly double _y;

        public DataPoint(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double X => _x;

        public double Y => _y;

        public bool Equals(DataPoint other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }

        public override string ToString()
        {
            return _x + " " + _y;
        }

        public double DistanceTo(DataPoint other)
        {
            double dx = other.X - _x;
            double dy = other.Y - _y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }
    }
}
