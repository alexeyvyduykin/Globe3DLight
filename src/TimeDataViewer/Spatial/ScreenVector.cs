using System;

namespace TimeDataViewer.Spatial
{
    public struct ScreenVector : IEquatable<ScreenVector>
    {
        internal double _x;
        internal double _y;

        public ScreenVector(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double Length => Math.Sqrt((_x * _x) + (_y * _y));

        public double LengthSquared => (_x * _x) + (_y * _y);

        public double X => _x;

        public double Y => _y;

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="v">The vector.</param>
        /// <param name="d">The multiplication factor.</param>
        /// <returns>The result of the operator.</returns>
        public static ScreenVector operator *(ScreenVector v, double d)
        {
            return new ScreenVector(v._x * d, v._y * d);
        }

        /// <summary>
        /// Adds a vector to another vector.
        /// </summary>
        /// <param name="v">The vector to add to.</param>
        /// <param name="d">The vector to be added.</param>
        /// <returns>The result of the operation.</returns>
        public static ScreenVector operator +(ScreenVector v, ScreenVector d)
        {
            return new ScreenVector(v._x + d._x, v._y + d._y);
        }

        /// <summary>
        /// Subtracts one specified vector from another.
        /// </summary>
        /// <param name="v">The vector to subtract from.</param>
        /// <param name="d">The vector to be subtracted.</param>
        /// <returns>The result of operation.</returns>
        public static ScreenVector operator -(ScreenVector v, ScreenVector d)
        {
            return new ScreenVector(v._x - d._x, v._y - d._y);
        }

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="v">The vector to negate.</param>
        /// <returns>The result of operation.</returns>
        public static ScreenVector operator -(ScreenVector v)
        {
            return new ScreenVector(-v._x, -v._y);
        }

        public void Normalize()
        {
            double l = Math.Sqrt((_x * _x) + (_y * _y));
            if (l > 0)
            {
                _x /= l;
                _y /= l;
            }
        }

        public override string ToString()
        {
            return _x + " " + _y;
        }

        public bool Equals(ScreenVector other)
        {
            return _x.Equals(other._x) && _y.Equals(other._y);
        }
    }
}
