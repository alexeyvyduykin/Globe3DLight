namespace TimeDataViewer.Core
{
    public class ZoomStepManipulator : MouseManipulator
    {
        public ZoomStepManipulator(IPlotView plotView) : base(plotView)
        {
        }

        public bool FineControl { get; set; }

        public double Step { get; set; }

        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            var isZoomEnabled = (XAxis != null && XAxis.IsZoomEnabled) || (YAxis != null && YAxis.IsZoomEnabled);

            if (!isZoomEnabled)
            {
                return;
            }

            var current = InverseTransform(e.Position.X, e.Position.Y);

            var scale = Step;
            if (FineControl)
            {
                scale *= 3;
            }

            scale = 1 + scale;

            // make sure the zoom factor is not negative
            if (scale < 0.1)
            {
                scale = 0.1;
            }

            if (XAxis != null)
            {
                XAxis.ZoomAt(scale, current.X);
            }

            if (YAxis != null)
            {
                YAxis.ZoomAt(scale, current.Y);
            }

            PlotView.InvalidatePlot(false);
            e.Handled = true;
        }
    }
}
