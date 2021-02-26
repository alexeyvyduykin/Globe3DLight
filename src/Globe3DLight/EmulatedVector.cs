using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using System.Globalization;
using Extensions;

namespace Globe3DLight
{
    public struct EmulatedVector3D : IEquatable<EmulatedVector3D>
    {
        public EmulatedVector3D(dvec3 v)
        {
            _high = v.ToVec3();
            _low = (v - _high.ToDVec3()).ToVec3();
        }

        public vec3 High
        {
            get { return _high; }
        }

        public vec3 Low
        {
            get { return _low; }
        }

        public bool Equals(EmulatedVector3D other)
        {
            return _high == other._high && _low == other._low;
        }

        public static bool operator ==(EmulatedVector3D left, EmulatedVector3D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EmulatedVector3D left, EmulatedVector3D right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is EmulatedVector3D)
            {
                return Equals((EmulatedVector3D)obj);
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "({0}, {1})", High, Low);
        }

        public override int GetHashCode()
        {
            return _high.GetHashCode() ^ _low.GetHashCode();
        }

        private readonly vec3 _high;
        private readonly vec3 _low;
    }
}
