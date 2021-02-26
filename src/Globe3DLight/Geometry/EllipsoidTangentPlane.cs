using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Extensions;

namespace Globe3DLight
{
    public class EllipsoidTangentPlane
    {
        public EllipsoidTangentPlane(Ellipsoid ellipsoid, IEnumerable<dvec3> positions)
        {
            if (ellipsoid == null)
            {
                throw new ArgumentNullException("ellipsoid");
            }

            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }

            if (!CollectionAlgorithms.EnumerableCountGreaterThanOrEqual(positions, 1))
            {
                throw new ArgumentOutOfRangeException("positions", "At least one position is required.");
            }

            AxisAlignedBoundingBox box = new AxisAlignedBoundingBox(positions);

            _origin = ellipsoid.ScaleToGeodeticSurface(box.Center);
            _normal = ellipsoid.GeodeticSurfaceNormal(_origin);
            _d = /*-_origin.Dot(_origin);*/ -dvec3.Dot(_origin, _origin);
            _yAxis = /*_origin.Cross(_origin.MostOrthogonalAxis).Normalize();*/ dvec3.Cross(_origin, _origin.MostOrthogonalAxis()).Normalized;
            _xAxis = /*_yAxis.Cross(_origin).Normalize();*/ dvec3.Cross(_yAxis, _origin).Normalized;
        }

        public ICollection<dvec2> ComputePositionsOnPlane(IEnumerable<dvec3> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException("positions");
            }

            IList<dvec2> positionsOnPlane = new List<dvec2>(CollectionAlgorithms.EnumerableCount(positions));

            foreach (dvec3 position in positions)
            {
                dvec3 intersectionPoint;

                if (IntersectionTests.TryRayPlane(dvec3.Zero, position.Normalized, _normal, _d, out intersectionPoint))
                {                    
                    dvec3 v = intersectionPoint - _origin;
                    positionsOnPlane.Add(new dvec2(dvec3.Dot(_xAxis, v), dvec3.Dot(_yAxis, v)));
                }
                else
                {
                    // Ray does not intersect plane
                }
            }

            return positionsOnPlane;
        }

        public dvec3 Origin { get { return _origin; } }
        public dvec3 Normal { get { return _normal; } }
        public double D { get { return _d; } }
        public dvec3 XAxis { get { return _xAxis; } }
        public dvec3 YAxis { get { return _yAxis; } }

        private dvec3 _origin;
        private dvec3 _normal;
        private double _d;
        private dvec3 _xAxis;
        private dvec3 _yAxis;
    }

}
