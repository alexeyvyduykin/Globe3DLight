using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight
{
    public class Ellipsoid
    {
        //public static readonly Ellipsoid Wgs84 = new Ellipsoid(6378137.0, 6378137.0, 6356752.314245);
        public static readonly Ellipsoid ScaledWgs84 = new Ellipsoid(10.03, 10.03, 10.03/*6356752.314245 / 6378137.0*/);
        //public static readonly Ellipsoid UnitSphere = new Ellipsoid(1.0, 1.0, 1.0);

        public Ellipsoid(double x, double y, double z)
            : this(new dvec3(x, y, z))
        {
        }

        public Ellipsoid(dvec3 radii)
        {
            if ((radii.x <= 0.0) || (radii.y <= 0.0) || (radii.z <= 0.0))
            {
                throw new ArgumentOutOfRangeException("radii");
            }

            _radii = radii;
            _radiiSquared = new dvec3(
                radii.x * radii.x,
                radii.y * radii.y,
                radii.z * radii.z);
            _radiiToTheFourth = new dvec3(
                _radiiSquared.x * _radiiSquared.x,
                _radiiSquared.y * _radiiSquared.y,
                _radiiSquared.z * _radiiSquared.z);
            _oneOverRadiiSquared = new dvec3(
                1.0 / (radii.x * radii.x),
                1.0 / (radii.y * radii.y),
                1.0 / (radii.z * radii.z));
        }

        //public static Vector3D CentricSurfaceNormal(Vector3D positionOnEllipsoid)
        //{
        //    return positionOnEllipsoid.Normalize();
        //}

        public dvec3 GeodeticSurfaceNormal(dvec3 positionOnEllipsoid)
        {
            return (positionOnEllipsoid * _oneOverRadiiSquared).Normalized;
        }

        public dvec3 GeodeticSurfaceNormal(Geodetic3D geodetic)
        {
            double cosLatitude = Math.Cos(geodetic.Latitude);

            return new dvec3(
                cosLatitude * Math.Cos(geodetic.Longitude),
                cosLatitude * Math.Sin(geodetic.Longitude),
                Math.Sin(geodetic.Latitude));
        }

        //public Vector3D Radii
        //{
        //    get { return _radii; }
        //}

        //public Vector3D RadiiSquared
        //{
        //    get { return _radiiSquared; }
        //}

        public dvec3 OneOverRadiiSquared
        {
            get { return _oneOverRadiiSquared; }
        }

        //public double MinimumRadius
        //{
        //    get { return Math.Min(_radii.X, Math.Min(_radii.Y, _radii.Z)); }
        //}

        //public double MaximumRadius
        //{
        //    get { return Math.Max(_radii.X, Math.Max(_radii.Y, _radii.Z)); }
        //}

        //public double[] Intersections(Vector3D origin, Vector3D direction)
        //{
        //    direction.Normalize();

        //    // By laborious algebraic manipulation....
        //    double a = direction.X * direction.X * _oneOverRadiiSquared.X +
        //               direction.Y * direction.Y * _oneOverRadiiSquared.Y +
        //               direction.Z * direction.Z * _oneOverRadiiSquared.Z;
        //    double b = 2.0 *
        //               (origin.X * direction.X * _oneOverRadiiSquared.X +
        //                origin.Y * direction.Y * _oneOverRadiiSquared.Y +
        //                origin.Z * direction.Z * _oneOverRadiiSquared.Z);
        //    double c = origin.X * origin.X * _oneOverRadiiSquared.X +
        //               origin.Y * origin.Y * _oneOverRadiiSquared.Y +
        //               origin.Z * origin.Z * _oneOverRadiiSquared.Z - 1.0;

        //    // Solve the quadratic equation: ax^2 + bx + c = 0.
        //    // Algorithm is from Wikipedia's "Quadratic equation" topic, and Wikipedia credits
        //    // Numerical Recipes in C, section 5.6: "Quadratic and Cubic Equations"
        //    double discriminant = b * b - 4 * a * c;
        //    if (discriminant < 0.0)
        //    {
        //        // no intersections
        //        return new double[0];
        //    }
        //    else if (discriminant == 0.0)
        //    {
        //        // one intersection at a tangent point
        //        return new double[1] { -0.5 * b / a };
        //    }

        //    double t = -0.5 * (b + (b > 0.0 ? 1.0 : -1.0) * Math.Sqrt(discriminant));
        //    double root1 = t / a;
        //    double root2 = c / t;

        //    // Two intersections - return the smallest first.
        //    if (root1 < root2)
        //    {
        //        return new double[2] { root1, root2 };
        //    }
        //    else
        //    {
        //        return new double[2] { root2, root1 };
        //    }
        //}

        public dvec3 ToVector3D(Geodetic2D geodetic)
        {
            return ToVector3D(new Geodetic3D(geodetic.Longitude, geodetic.Latitude, 0.0));
        }

        public dvec3 ToVector3D(Geodetic3D geodetic)
        {
            dvec3 n = GeodeticSurfaceNormal(geodetic);
            dvec3 k = _radiiSquared * n;
            double gamma = Math.Sqrt(
                (k.x * n.x) +
                (k.y * n.y) +
                (k.z * n.z));

            dvec3 rSurface = k / gamma;
            dvec3 res = rSurface + (geodetic.Height * n);

            return new dvec3(res.y, res.z, res.x);

            //return new dvec3(
            //(_radii.x + geodetic.Height) * (Math.Cos(geodetic.Latitude) * Math.Sin(geodetic.Longitude)),
            //(_radii.y + geodetic.Height) * Math.Sin(geodetic.Latitude),
            //(_radii.z + geodetic.Height) * (Math.Cos(geodetic.Latitude) * Math.Cos(geodetic.Longitude)));

        }

        //public ICollection<Geodetic3D> ToGeodetic3D(IEnumerable<Vector3D> positions)
        //{
        //    if (positions == null)
        //    {
        //        throw new ArgumentNullException("positions");
        //    }

        //    IList<Geodetic3D> geodetics = new List<Geodetic3D>(CollectionAlgorithms.EnumerableCount(positions));

        //    foreach (Vector3D position in positions)
        //    {
        //        geodetics.Add(ToGeodetic3D(position));
        //    }

        //    return geodetics;
        //}

        //public ICollection<Geodetic2D> ToGeodetic2D(IEnumerable<Vector3D> positions)
        //{
        //    if (positions == null)
        //    {
        //        throw new ArgumentNullException("positions");
        //    }

        //    IList<Geodetic2D> geodetics = new List<Geodetic2D>(CollectionAlgorithms.EnumerableCount(positions));

        //    foreach (Vector3D position in positions)
        //    {
        //        geodetics.Add(ToGeodetic2D(position));
        //    }

        //    return geodetics;
        //}

        //public Geodetic2D ToGeodetic2D(Vector3D positionOnEllipsoid)
        //{
        //    Vector3D n = GeodeticSurfaceNormal(positionOnEllipsoid);
        //    return new Geodetic2D(
        //        Math.Atan2(n.Y, n.X),
        //        Math.Asin(n.Z / n.Magnitude));
        //}

        //public Geodetic3D ToGeodetic3D(Vector3D position)
        //{
        //    Vector3D p = ScaleToGeodeticSurface(position);
        //    Vector3D h = position - p;
        //    double height = Math.Sign(h.Dot(position)) * h.Magnitude;
        //    return new Geodetic3D(ToGeodetic2D(p), height);
        //}

        public dvec3 ScaleToGeodeticSurface(dvec3 position)
        {
            double beta = 1.0 / Math.Sqrt(
                (position.x * position.x) * _oneOverRadiiSquared.x +
                (position.y * position.y) * _oneOverRadiiSquared.y +
                (position.z * position.z) * _oneOverRadiiSquared.z);
            double n = new dvec3(
                beta * position.x * _oneOverRadiiSquared.x,
                beta * position.y * _oneOverRadiiSquared.y,
                beta * position.z * _oneOverRadiiSquared.z).Length;// Magnitude;
            double alpha = (1.0 - beta) * (position.Length/*Magnitude*/ / n);

            double x2 = position.x * position.x;
            double y2 = position.y * position.y;
            double z2 = position.z * position.z;

            double da = 0.0;
            double db = 0.0;
            double dc = 0.0;

            double s = 0.0;
            double dSdA = 1.0;

            do
            {
                alpha -= (s / dSdA);

                da = 1.0 + (alpha * _oneOverRadiiSquared.x);
                db = 1.0 + (alpha * _oneOverRadiiSquared.y);
                dc = 1.0 + (alpha * _oneOverRadiiSquared.z);

                double da2 = da * da;
                double db2 = db * db;
                double dc2 = dc * dc;

                double da3 = da * da2;
                double db3 = db * db2;
                double dc3 = dc * dc2;

                s = x2 / (_radiiSquared.x * da2) +
                    y2 / (_radiiSquared.y * db2) +
                    z2 / (_radiiSquared.z * dc2) - 1.0;

                dSdA = -2.0 *
                    (x2 / (_radiiToTheFourth.x * da3) +
                     y2 / (_radiiToTheFourth.y * db3) +
                     z2 / (_radiiToTheFourth.z * dc3));
            }
            while (Math.Abs(s) > 1e-10);

            return new dvec3(
                position.x / da,
                position.y / db,
                position.z / dc);
        }

        public dvec3 ScaleToGeocentricSurface(dvec3 position)
        {
            double beta = 1.0 / Math.Sqrt(
                (position.x * position.x) * _oneOverRadiiSquared.x +
                (position.y * position.y) * _oneOverRadiiSquared.y +
                (position.z * position.z) * _oneOverRadiiSquared.z);

            return beta * position;
        }

        //public IList<Vector3D> ComputeCurve(
        //    Vector3D start,
        //    Vector3D stop,
        //    double granularity)
        //{
        //    if (granularity <= 0.0)
        //    {
        //        throw new ArgumentOutOfRangeException("granularity", "Granularity must be greater than zero.");
        //    }

        //    Vector3D normal = start.Cross(stop).Normalize();
        //    double theta = start.AngleBetween(stop);
        //    int n = Math.Max((int)(theta / granularity) - 1, 0);

        //    List<Vector3D> positions = new List<Vector3D>(2 + n);

        //    positions.Add(start);

        //    for (int i = 1; i <= n; ++i)
        //    {
        //        double phi = (i * granularity);

        //        positions.Add(ScaleToGeocentricSurface(start.RotateAroundAxis(normal, phi)));
        //    }

        //    positions.Add(stop);

        //    return positions;
        //}

        private readonly dvec3 _radii;
        private readonly dvec3 _radiiSquared;
        private readonly dvec3 _radiiToTheFourth;
        private readonly dvec3 _oneOverRadiiSquared;
    }

}
