using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight.DataProvider.Science
{
    public static class Conversion
    {
        public static void SphericalToCartesian(double r, double lon, double lat, out double x, out double y, out double z)
        {

            // def fromLatLong(lat, lon, h, a, f):


            //double b = r;
            //double c = r;
      //      double e2 = 2 * r;
      //      double e12 = (2 * r) / (1.0 - 2 * r);

      //      double cos_lat = Math.Cos(lat);
      //      double n = r / Math.Sqrt(1.0 + e12 * cos_lat * cos_lat);
      //      double p = n * cos_lat;


      //z       = p * Math.Cos(lon);
      //x       = p * Math.Sin(lon);
      //y       = (n + 0 - e2 * n) * Math.Sin(lat);


            x = r * Math.Cos(lat) * Math.Sin(lon);
            y = r * Math.Sin(lat);
            z = r * Math.Cos(lat) * Math.Cos(lon);

        }




        public static dvec3 SphericalToCartesian(double r, double lon, double lat)
        {
            return new dvec3(                        
                r * Math.Cos(lat) * Math.Sin(lon),                        
                r * Math.Sin(lat),                        
                r * Math.Cos(lat) * Math.Cos(lon));
        }

        public static dmat4 CartesianToOrbital(double i, double u, double om)
        {
            dmat4 A;
            double w = -u;

            double sOm = Math.Sin(om);
            double cOm = Math.Cos(om);
            double sI = Math.Sin(i);
            double cI = Math.Cos(i);
            double sW = Math.Sin(w);
            double cW = Math.Cos(w);

            A.m00 = cOm * cI * cW - sOm * sW; A.m01 = -cOm * cI * sW - sOm * cW; A.m02 = cOm * sI; A.m03 = 0.0f;
            A.m10 = sI * cW; A.m11 = -sI * sW; A.m12 = -cI; A.m13 = 0.0f;
            A.m20 = sOm * cI * cW + cOm * sW; A.m21 = -sOm * cI * sW + cOm * cW; A.m22 = sOm * sI; A.m23 = 0.0f;
            A.m30 = 0.0f; A.m31 = 0.0f; A.m32 = 0.0f; A.m33 = 1.0f;

            return A;
        }

        public static void CartesianToSpherical(double x, double y, double z, out double r, out double lon, out double lat)
        {
            r = Math.Sqrt(x * x + y * y + z * z);
            lat = Math.Asin(y / r);
            lon = Math.Atan2(x, z);
        }

        public static dvec3 AbsToCartesian(dvec3 absPoint)
        {
            return new dvec3(absPoint.y, absPoint.z, absPoint.x);
        }
    }
}
