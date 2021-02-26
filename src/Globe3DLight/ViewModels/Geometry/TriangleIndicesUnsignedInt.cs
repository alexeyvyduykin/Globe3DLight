using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Globe3DLight.Geometry
{
    public struct TriangleIndicesUnsignedInt : IEquatable<TriangleIndicesUnsignedInt>
    {
        public TriangleIndicesUnsignedInt(uint ui0, uint ui1, uint ui2)
        {
            this.ui0 = ui0;
            this.ui1 = ui1;
            this.ui2 = ui2;
        }

        public TriangleIndicesUnsignedInt(int i0, int i1, int i2)
        {
            if (i0 < 0)
            {
                throw new ArgumentOutOfRangeException("i0");
            }

            if (i1 < 0)
            {
                throw new ArgumentOutOfRangeException("i1");
            }

            if (i2 < 0)
            {
                throw new ArgumentOutOfRangeException("i2");
            }

            ui0 = (uint)i0;
            ui1 = (uint)i1;
            ui2 = (uint)i2;
        }

        public TriangleIndicesUnsignedInt(TriangleIndicesUnsignedInt other)
        {
            ui0 = other.UI0;
            ui1 = other.UI1;
            ui2 = other.UI2;
        }

        public static bool operator ==(TriangleIndicesUnsignedInt left, TriangleIndicesUnsignedInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TriangleIndicesUnsignedInt left, TriangleIndicesUnsignedInt right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "i0: {0} i1: {1} i2: {2}", ui0, ui1, ui2);
        }

        public override int GetHashCode()
        {
            return ui0.GetHashCode() ^ ui1.GetHashCode() ^ ui2.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TriangleIndicesUnsignedInt))
                return false;

            return this.Equals((TriangleIndicesUnsignedInt)obj);
        }

        #region IEquatable<TriangleIndices> Members

        public bool Equals(TriangleIndicesUnsignedInt other)
        {
            return
                (ui0.Equals(other.UI0)) &&
                (ui1.Equals(other.UI1)) &&
                (ui2.Equals(other.UI2));
        }

        #endregion

        public int I0
        {
            get { return (int)ui0; }
        }

        public int I1
        {
            get { return (int)ui1; }
        }

        public int I2
        {
            get { return (int)ui2; }
        }

        public uint UI0
        {
            get { return ui0; }
        }

        public uint UI1
        {
            get { return ui1; }
        }

        public uint UI2
        {
            get { return ui2; }
        }

        private uint ui0;
        private uint ui1;
        private uint ui2;
    }
}
