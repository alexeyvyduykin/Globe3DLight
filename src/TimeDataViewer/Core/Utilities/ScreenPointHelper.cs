using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public static class ScreenPointHelper
    {
        public static ScreenPoint FindPointOnLine(ScreenPoint p, ScreenPoint p1, ScreenPoint p2)
        {
            double dx = p2._x - p1._x;
            double dy = p2._y - p1._y;
            double u = FindPositionOnLine(p, p1, p2);

            if (double.IsNaN(u))
            {
                u = 0;
            }

            if (u < 0)
            {
                u = 0;
            }

            if (u > 1)
            {
                u = 1;
            }

            return new ScreenPoint(p1._x + (u * dx), p1._y + (u * dy));
        }

        public static double FindPositionOnLine(ScreenPoint p, ScreenPoint p1, ScreenPoint p2)
        {
            double dx = p2._x - p1._x;
            double dy = p2._y - p1._y;
            double u1 = ((p._x - p1._x) * dx) + ((p._y - p1._y) * dy);
            double u2 = (dx * dx) + (dy * dy);

            if (u2 < 1e-6)
            {
                return double.NaN;
            }

            return u1 / u2;
        }
    }
}
