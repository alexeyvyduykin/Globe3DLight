using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class OxyMouseEventArgs : OxyInputEventArgs
    {
        /// <summary>
        /// Gets or sets the position of the mouse cursor.
        /// </summary>
        public ScreenPoint Position { get; set; }

        /// <summary>
        /// Gets or sets the view where the event occurred.
        /// </summary>
        public IView View { get; set; }
    }
}
