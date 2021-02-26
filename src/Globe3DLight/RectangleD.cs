using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using GlmSharp;

namespace Globe3DLight
{
    public struct RectangleD : IEquatable<RectangleD>
    {
        public RectangleD(dvec2 lowerLeft, dvec2 upperRight)
        {
            _lowerLeft = lowerLeft;
            _upperRight = upperRight;
        }

        public dvec2 LowerLeft { get { return _lowerLeft; } }
        public dvec2 UpperRight { get { return _upperRight; } }

        public static bool operator ==(RectangleD left, RectangleD right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleD left, RectangleD right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "({0}, {1})",
                _lowerLeft.ToString(), _upperRight.ToString());
        }

        public override int GetHashCode()
        {
            return _lowerLeft.GetHashCode() ^ _upperRight.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RectangleD))
                return false;

            return this.Equals((RectangleD)obj);
        }

        #region IEquatable<RectangleD> Members

        public bool Equals(RectangleD other)
        {
            return
                (_lowerLeft == other._lowerLeft) &&
                (_upperRight == other._upperRight);
        }

        #endregion

        private readonly dvec2 _lowerLeft;
        private readonly dvec2 _upperRight;
    }
}
