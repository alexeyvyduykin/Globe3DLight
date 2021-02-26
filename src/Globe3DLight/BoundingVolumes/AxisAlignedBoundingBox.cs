using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    public struct AxisAlignedBoundingBox
    {
        public AxisAlignedBoundingBox(IEnumerable<dvec3> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }

            double minimumX = double.MaxValue;
            double minimumY = double.MaxValue;
            double minimumZ = double.MaxValue;

            double maximumX = -double.MaxValue;
            double maximumY = -double.MaxValue;
            double maximumZ = -double.MaxValue;

            foreach (dvec3 position in positions)
            {
                if (position.x < minimumX)
                {
                    minimumX = position.x;
                }

                if (position.x > maximumX)
                {
                    maximumX = position.x;
                }

                if (position.y < minimumY)
                {
                    minimumY = position.y;
                }

                if (position.y > maximumY)
                {
                    maximumY = position.y;
                }

                if (position.z < minimumZ)
                {
                    minimumZ = position.z;
                }

                if (position.z > maximumZ)
                {
                    maximumZ = position.z;
                }
            }

            dvec3 minimum = new dvec3(minimumX, minimumY, minimumZ);
            dvec3 maximum = new dvec3(maximumX, maximumY, maximumZ);

            if ((minimum > maximum).All)   // if (minimum > maximum)
            {
                ExtraMath.Swap(ref minimum, ref maximum);
            }

            _minimum = minimum;
            _maximum = maximum;
        }

        public dvec3 Minimum
        {
            get { return _minimum; }
        }

        public dvec3 Maximum
        {
            get { return _maximum; }
        }

        public dvec3 Center
        {
            get { return (Minimum + Maximum) * 0.5; }
        }

        private readonly dvec3 _minimum;
        private readonly dvec3 _maximum;
    }

}
