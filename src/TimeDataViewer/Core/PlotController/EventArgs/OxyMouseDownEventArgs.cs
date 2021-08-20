namespace TimeDataViewer.Core
{
    public class OxyMouseDownEventArgs : OxyMouseEventArgs
    {
        // Gets or sets the mouse button that has changed.      
        public OxyMouseButton ChangedButton { get; set; }

        // Gets or sets the hit test result.        
        public HitTestResult HitTestResult { get; set; } // TODO: REMOVE THIS?
    }
}
