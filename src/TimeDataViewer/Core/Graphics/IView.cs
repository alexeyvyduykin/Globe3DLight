using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public interface IView
    {
        // Gets the actual model in the view.
        Model ActualModel { get; }

        // Gets the actual controller.
        IController ActualController { get; }

        // Gets the coordinates of the client area of the view.
        OxyRect ClientArea { get; }

        // Sets the cursor type.
        void SetCursorType(CursorType cursorType);

        // Hides the zoom rectangle.
        void HideZoomRectangle();

        // Shows the zoom rectangle.
        void ShowZoomRectangle(OxyRect rectangle);
    }
}
