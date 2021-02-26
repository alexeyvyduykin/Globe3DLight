using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal struct ColorMask : IEquatable<ColorMask>
    {
        public ColorMask(bool red, bool green, bool blue, bool alpha)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.alpha = alpha;
        }

        public bool Red
        {
            get { return red; }
        }

        public bool Green
        {
            get { return green; }
        }

        public bool Blue
        {
            get { return blue; }
        }

        public bool Alpha
        {
            get { return alpha; }
        }

        #region IEquatable Members

        public bool Equals(ColorMask other)
        {
            return
                red == other.red &&
                green == other.green &&
                blue == other.blue &&
                alpha == other.alpha;
        }

        #endregion

        public static bool operator ==(ColorMask left, ColorMask right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ColorMask left, ColorMask right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorMask)
            {
                return Equals((ColorMask)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return red.GetHashCode() ^ green.GetHashCode() ^ blue.GetHashCode() ^ alpha.GetHashCode();
        }

        private readonly bool red;
        private readonly bool green;
        private readonly bool blue;
        private readonly bool alpha;
    }
}
