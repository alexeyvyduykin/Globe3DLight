#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    // the base projection
    public class CommonSchedulerProjection : BaseProjection
    {
        public static readonly CommonSchedulerProjection Instance = new CommonSchedulerProjection();

        public CommonSchedulerProjection() { }

        // int minZoom = 0;
        // int maxZoom = 3;

        //   List<double> secsInZoom = new List<double>() { 86400.0, 3600.0, 60.0, 1.0 };


        //double secsInWidth(double zoom)
        //{
        //    zoom = Math.Min(Math.Max(zoom, minZoom), maxZoom);

        //    double remainder = zoom % 1;

        //    if(remainder == 0.0) // zoom => integer
        //    {
        //        return secsInZoom[(int)zoom];
        //    }
        //    else // zoom => double
        //    {
        //        var secsL = secsInZoom[(int)zoom];
        //        var secsR = secsInZoom[(int)zoom + 1];

        //        return secsL + (secsR - secsL) * remainder;
        //    }            
        //}


        public override Point2I FromSchedulerPointToPixel(double px, double py, double zoom)
        {
            px = Clip(px, Level0.Left, Level0.Right);
            py = Clip(py, Level0.Bottom, Level0.Top);

            int x = (int)(/*Level0.Left +*/ Bounds.Width * px * (zoom + 1.0) / Level0.Width);
            int y = (int)(Level0.Height/*Bounds.Height*/ - py);

            return new Point2I(x, y);
        }

        public override Point2D FromPixelToSchedulerPoint(int x, int y, double zoom)
        {
            x = Clip(x, 0, (int)(Bounds.Width * (zoom + 1.0)));
            y = Clip(y, 0, (int)Level0.Height/*Bounds.Height*/);

            double px = Level0.Left + Level0.Width * x / ((Bounds.Width) * (zoom + 1.0));
            double py = Level0.Height /*Bounds.Height*/ - y;

            return new Point2D(px, py);
        }

        //double GetScaleX(int zoom)
        //{
        //    zoom = Math.Min(Math.Max(zoom, minZoom), maxZoom);

        //    if (zoom == 0) // day = 86000
        //    {
        //        return 1.0 / 86400.0;
        //    }
        //    else if (zoom == 1) // hour = 3600
        //    {
        //        return 24.0 / 86400.0;
        //        // 86400 / 24 = 3600
        //    }
        //    else if (zoom == 2) // min = 60
        //    {
        //        return 1440.0 / 86400.0;
        //        // 86400 / 1440 = 60
        //    }
        //    else // sec = 1
        //    {
        //        return 1.0;
        //    }
        //}

        public override void UpdateBounds(RectI bounds)
        {
            base.Bounds = bounds;
        }

        public override void UpdateLevel0(RectD level0)
        {
            base.Level0 = level0;
        }
    }
}
