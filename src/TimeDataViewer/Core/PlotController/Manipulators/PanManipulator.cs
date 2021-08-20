using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class PanManipulator : MouseManipulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PanManipulator" /> class.
        /// </summary>
        /// <param name="plotView">The plot view.</param>
        public PanManipulator(IPlotView plotView)
            : base(plotView)
        {
        }

        /// <summary>
        /// Gets or sets the previous position.
        /// </summary>
        private ScreenPoint PreviousPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether panning is enabled.
        /// </summary>
        private bool IsPanEnabled { get; set; }

        /// <summary>
        /// Occurs when a manipulation is complete.
        /// </summary>
        /// <param name="e">The <see cref="OxyInputEventArgs" /> instance containing the event data.</param>
        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);
            if (!IsPanEnabled)
            {
                return;
            }

            View.SetCursorType(CursorType.Default);
            e.Handled = true;
        }

        /// <summary>
        /// Occurs when the input device changes position during a manipulation.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            if (!IsPanEnabled)
            {
                return;
            }

            if (XAxis != null)
            {
                XAxis.Pan(PreviousPosition, e.Position);
            }

            if (YAxis != null)
            {
                YAxis.Pan(PreviousPosition, e.Position);
            }

            PlotView.InvalidatePlot(false);
            PreviousPosition = e.Position;
            e.Handled = true;
        }

        /// <summary>
        /// Occurs when an input device begins a manipulation on the plot.
        /// </summary>
        /// <param name="e">The <see cref="OxyPlot.OxyMouseEventArgs" /> instance containing the event data.</param>
        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            PreviousPosition = e.Position;

            IsPanEnabled = (XAxis != null && XAxis.IsPanEnabled)
                                || (YAxis != null && YAxis.IsPanEnabled);

            if (IsPanEnabled)
            {
                View.SetCursorType(CursorType.Pan);
                e.Handled = true;
            }
        }
    }
}
