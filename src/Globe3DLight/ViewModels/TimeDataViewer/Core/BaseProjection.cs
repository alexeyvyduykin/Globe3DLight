#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public abstract class BaseProjection
    {
        public BaseProjection() { }

        // get pixel coordinates from lat/lng
        public abstract Point2I FromSchedulerPointToPixel(double x, double y, double zoom);

        // gets lat/lng coordinates from pixel coordinates
        public abstract Point2D FromPixelToSchedulerPoint(int x, int y, double zoom);

        // get pixel coordinates from SchedulerPoint
        public Point2I FromSchedulerPointToPixel(Point2D p, double zoom)
        {
            return FromSchedulerPointToPixel(p.X, p.Y, zoom);
        }

        // gets SchedulerPoint coordinates from pixel coordinates
        public Point2D FromPixelToSchedulerPoint(Point2I p, double zoom)
        {
            return FromPixelToSchedulerPoint(p.X, p.Y, zoom);
        }

        // gets boundaries    
        public virtual RectI Bounds { get; protected set; }

        public abstract void UpdateBounds(RectI bounds);

        public virtual RectD Level0 { get; protected set; }

        public abstract void UpdateLevel0(RectD level0);

        // The ground resolution indicates the distance (in meters) on the ground that’s represented by a single pixel in the map.
        // For example, at a ground resolution of 10 meters/pixel, each pixel represents a ground distance of 10 meters.
        //public virtual double GetGroundResolution(int zoom, double latitude)
        //{
        //    return (Math.Cos(latitude * (Math.PI / 180)) * 2 * Math.PI * Axis) / GetTileMatrixSizePixel(zoom).Width;
        //}

        // Clips a number to the specified minimum and maximum values.
        protected static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clip(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }
    }
}
