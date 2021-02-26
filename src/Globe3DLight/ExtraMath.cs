using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    public static class ExtraMath
    {
        public const double PI = 3.14159265358979323846;
        public const double PI_2 = 6.28318530717958647692;
        public const double PI_1_2 = 1.57079632679489661923;
        public const double PI_3_2 = 4.71238898038468985769; // PI*1.5, TWOPI*0.75, 3.*PI/2.
        public const double MU = 398600.44; // km^3/cek^2  constant of a gravitational
        public const double WE = 7.292115085e-5; //скорость вращения Земли, рад/сек (Angular speed of rotation of the Earth)
        public const double RE = 6371.0;
        public const double RadiansPerDegree = Math.PI / 180.0;

        public static int Sign(double val)
        {
            if (val > 0.0) return 1;
            else if (val < 0.0) return -1;
            return 0;
        }
        public static double FmodTWOPI(double x)
        {
            while (x > PI_2) x -= PI_2;
            while (x < 0.0) x += PI_2;
            return x;
        }

        public static double ToRadians(double degrees)
        {
            return degrees * RadiansPerDegree;
        }

        public static Geodetic3D ToRadians(Geodetic3D geodetic)
        {
            return new Geodetic3D(ToRadians(geodetic.Longitude), ToRadians(geodetic.Latitude), geodetic.Height);
        }

        public static Geodetic2D ToRadians(Geodetic2D geodetic)
        {
            return new Geodetic2D(ToRadians(geodetic.Longitude), ToRadians(geodetic.Latitude));
        }

        public static double ToDegrees(double radians)
        {
            return radians / RadiansPerDegree;
        }

        public static Geodetic3D ToDegrees(Geodetic3D geodetic)
        {
            return new Geodetic3D(ToDegrees(geodetic.Longitude), ToDegrees(geodetic.Latitude), geodetic.Height);
        }

        public static Geodetic2D ToDegrees(Geodetic2D geodetic)
        {
            return new Geodetic2D(ToDegrees(geodetic.Longitude), ToDegrees(geodetic.Latitude));
        }

        public static void Swap<T>(ref T left, ref T right)
        {
            T temp = left;
            left = right;
            right = temp;
        }

        public static double AngleBetween(dvec3 left, dvec3 right)
        {
            return Math.Acos(dvec3.Dot(left.Normalized, right.Normalized));
        }

    }
}
