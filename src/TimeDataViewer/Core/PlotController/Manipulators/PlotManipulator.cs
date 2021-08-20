using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public abstract class PlotManipulator<T> : ManipulatorBase<T> where T : OxyInputEventArgs
    {
        protected PlotManipulator(IPlotView view) : base(view)
        {
            PlotView = view;
        }

        public IPlotView PlotView { get; private set; }

        protected Axis XAxis { get; set; }

        protected Axis YAxis { get; set; }

        protected DataPoint InverseTransform(double x, double y)
        {
            if (XAxis != null)
            {
                return XAxis.InverseTransform(x, y, YAxis);
            }

            if (YAxis != null)
            {
                return new DataPoint(0, YAxis.InverseTransform(y));
            }

            return new DataPoint();
        }

        protected void AssignAxes()
        {
            Axis xaxis = null;
            Axis yaxis = null;

            if (PlotView.ActualModel != null)
            {
                PlotView.ActualModel.GetAxesFromPoint(out xaxis, out yaxis);
            }

            XAxis = xaxis;
            YAxis = yaxis;
        }
    }
}
