namespace TimeDataViewer.Core
{
    public class PlotController : ControllerBase, IPlotController
    {
        public PlotController()
        {
            // Zoom rectangle bindings: MMB
            this.BindMouseDown(OxyMouseButton.Middle, PlotCommands.ZoomRectangle);

            // Pan bindings: RMB
            this.BindMouseDown(OxyMouseButton.Right, PlotCommands.PanAt);

            // Select bindings: LMB
            this.BindMouseDown(OxyMouseButton.Left, PlotCommands.Select);

            // Tracker bindings:
            this.BindMouseEnter(PlotCommands.HoverSnapTrack);

            // Zoom in/out binding: mouse wheels
            this.BindMouseWheel(PlotCommands.ZoomWheel);
        }
    }
}
