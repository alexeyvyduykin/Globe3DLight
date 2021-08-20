using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public abstract class MouseManipulator : PlotManipulator<OxyMouseEventArgs>
    {
        protected MouseManipulator(IPlotView plotView)
            : base(plotView)
        {
        }

        public ScreenPoint StartPosition { get; protected set; }

        public override void Started(OxyMouseEventArgs e)
        {
            AssignAxes();
            base.Started(e);
            StartPosition = e.Position;
        }
    }
}
