using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    public static class ContainmentTests
    {
        public static bool PointInsideTriangle(dvec2 point, dvec2 p0, dvec2 p1, dvec2 p2)
        {
            //
            // Implementation based on http://www.blackpawn.com/texts/pointinpoly/default.html.
            //
            dvec2 v0 = (p1 - p0);
            dvec2 v1 = (p2 - p0);
            dvec2 v2 = (point - p0);

            double dot00 = dvec2.Dot(v0, v0);
            double dot01 = dvec2.Dot(v0, v1);
            double dot02 = dvec2.Dot(v0, v2);
            double dot11 = dvec2.Dot(v1, v1);
            double dot12 = dvec2.Dot(v1, v2);

            double q = 1.0 / (dot00 * dot11 - dot01 * dot01);
            double u = (dot11 * dot02 - dot01 * dot12) * q;
            double v = (dot00 * dot12 - dot01 * dot02) * q;

            return (u > 0) && (v > 0) && (u + v < 1);
        }

        /// <summary>
        /// The pyramid's base points should be in counterclockwise winding order.
        /// </summary>
        public static bool PointInsideThreeSidedInfinitePyramid(
            dvec3 point,
            dvec3 pyramidApex,
            dvec3 pyramidBase0,
            dvec3 pyramidBase1,
            dvec3 pyramidBase2)
        {
            dvec3 v0 = pyramidBase0 - pyramidApex;
            dvec3 v1 = pyramidBase1 - pyramidApex;
            dvec3 v2 = pyramidBase2 - pyramidApex;

            //
            // Face normals
            //
            dvec3 n0 = dvec3.Cross(v1, v0);
            dvec3 n1 = dvec3.Cross(v2, v1);
            dvec3 n2 = dvec3.Cross(v0, v2);

            dvec3 planeToPoint = point - pyramidApex;

            

            return ((dvec3.Dot(planeToPoint, n0) < 0) && (dvec3.Dot(planeToPoint, n1) < 0) && (dvec3.Dot(planeToPoint, n2) < 0));
        }
    }

}
