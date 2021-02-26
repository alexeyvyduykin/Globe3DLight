using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    public static class IntersectionTests
    {
        public static bool TryRayPlane(
            dvec3 rayOrigin,
            dvec3 rayDirection,
            dvec3 planeNormal,
            double planeD,
            out dvec3 intersectionPoint)
        {
            double denominator = dvec3.Dot(planeNormal, rayDirection);// planeNormal.Dot(rayDirection);

            if (Math.Abs(denominator) < 0.00000000000000000001)
            {
                //
                // Ray is parallel to plane.  The ray may be in the polygon's plane.
                //
                intersectionPoint = dvec3.Zero;
                return false;
            }

            double t = (-planeD - dvec3.Dot(planeNormal, rayOrigin) /*planeNormal.Dot(rayOrigin)*/) / denominator;

            if (t < 0)
            {
                intersectionPoint = dvec3.Zero;
                return false;
            }

            intersectionPoint = rayOrigin + (t * rayDirection);
            return true;
        }
    }
}
