using System;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class ZoomRectangleManipulator : MouseManipulator
    {
        private OxyRect zoomRectangle;

        public ZoomRectangleManipulator(IPlotView plotView) : base(plotView)
        {
        }

        private bool IsZoomEnabled { get; set; }

        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);
            if (!IsZoomEnabled)
            {
                return;
            }

            PlotView.SetCursorType(CursorType.Default);
            PlotView.HideZoomRectangle();

            if (zoomRectangle.Width > 10 && zoomRectangle.Height > 10)
            {
                var p0 = InverseTransform(zoomRectangle.Left, zoomRectangle.Top);
                var p1 = InverseTransform(zoomRectangle.Right, zoomRectangle.Bottom);

                if (XAxis != null)
                {
                    XAxis.Zoom(p0.X, p1.X);
                }

                if (YAxis != null)
                {
                    YAxis.Zoom(p0.Y, p1.Y);
                }

                PlotView.InvalidatePlot();
            }

            e.Handled = true;
        }

        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            if (!IsZoomEnabled)
            {
                return;
            }

            var plotArea = PlotView.ActualModel.PlotArea;

            var x = Math.Min(StartPosition.X, e.Position.X);
            var w = Math.Abs(StartPosition.X - e.Position.X);
            var y = Math.Min(StartPosition.Y, e.Position.Y);
            var h = Math.Abs(StartPosition.Y - e.Position.Y);

            if (XAxis == null || !XAxis.IsZoomEnabled)
            {
                x = plotArea.Left;
                w = plotArea.Width;
            }

            if (YAxis == null || !YAxis.IsZoomEnabled)
            {
                y = plotArea.Top;
                h = plotArea.Height;
            }

            zoomRectangle = new OxyRect(x, y, w, h);
            PlotView.ShowZoomRectangle(zoomRectangle);
            e.Handled = true;
        }

        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            IsZoomEnabled = (XAxis != null && XAxis.IsZoomEnabled)
                     || (YAxis != null && YAxis.IsZoomEnabled);

            if (IsZoomEnabled)
            {
                zoomRectangle = new OxyRect(StartPosition.X, StartPosition.Y, 0, 0);
                PlotView.ShowZoomRectangle(zoomRectangle);
                PlotView.SetCursorType(GetCursorType());
                e.Handled = true;
            }
        }

        private CursorType GetCursorType()
        {
            if (XAxis == null)
            {
                return CursorType.ZoomVertical;
            }

            if (YAxis == null)
            {
                return CursorType.ZoomHorizontal;
            }

            return CursorType.ZoomRectangle;
        }
    }
}
