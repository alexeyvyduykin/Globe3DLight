#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    // defines projection   
    public abstract class SCProjection
    {
        public SCProjection() { }

        // get pixel coordinates from lat/lng
        public abstract SCPoint FromSchedulerPointToPixel(double x, double y, double zoom);

        // gets lat/lng coordinates from pixel coordinates
        public abstract Point2 FromPixelToSchedulerPoint(int x, int y, double zoom);

        // get pixel coordinates from SchedulerPoint
        public SCPoint FromSchedulerPointToPixel(Point2 p, double zoom)
        {
            return FromSchedulerPointToPixel(p.X, p.Y, zoom);
        }

        // gets SchedulerPoint coordinates from pixel coordinates
        public Point2 FromPixelToSchedulerPoint(SCPoint p, double zoom)
        {
            return FromPixelToSchedulerPoint(p.X, p.Y, zoom);
        }

        // gets boundaries    
        public virtual SCRect Bounds { get; protected set; }

        public abstract void UpdateBounds(SCRect bounds);

        public virtual SCSchedulerRect Level0 { get; protected set; }

        public abstract void UpdateLevel0(SCSchedulerRect level0);

        // The ground resolution indicates the distance (in meters) on the ground that’s represented by a single pixel in the map.
        // For example, at a ground resolution of 10 meters/pixel, each pixel represents a ground distance of 10 meters.
        //public virtual double GetGroundResolution(int zoom, double latitude)
        //{
        //    return (Math.Cos(latitude * (Math.PI / 180)) * 2 * Math.PI * Axis) / GetTileMatrixSizePixel(zoom).Width;
        //}

        #region -- math functions --

        // Clips a number to the specified minimum and maximum values.
        protected static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clip(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        #endregion
    }
}
