namespace TimeDataViewer.Core
{
    public interface IController
    {
        // Handles mouse down events.
        bool HandleMouseDown(IView view, OxyMouseDownEventArgs args);

        // Handles mouse move events.
        bool HandleMouseMove(IView view, OxyMouseEventArgs args);

        // Handles mouse up events.
        bool HandleMouseUp(IView view, OxyMouseEventArgs args);

        // Handles mouse enter events.
        bool HandleMouseEnter(IView view, OxyMouseEventArgs args);

        // Handles mouse leave events.
        bool HandleMouseLeave(IView view, OxyMouseEventArgs args);

        // Handles mouse wheel events.
        bool HandleMouseWheel(IView view, OxyMouseWheelEventArgs args);

        // Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        void AddMouseManipulator(IView view, ManipulatorBase<OxyMouseEventArgs> manipulator, OxyMouseDownEventArgs args);

        // Adds the specified mouse hover manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        void AddHoverManipulator(IView view, ManipulatorBase<OxyMouseEventArgs> manipulator, OxyMouseEventArgs args);

        // Binds the specified command to the specified mouse down gesture. Removes old bindings to the gesture.
        void Bind(OxyMouseDownGesture gesture, IViewCommand<OxyMouseDownEventArgs> command);

        // Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        void Bind(OxyMouseEnterGesture gesture, IViewCommand<OxyMouseEventArgs> command);

        // Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        void Bind(OxyMouseWheelGesture gesture, IViewCommand<OxyMouseWheelEventArgs> command);

        // Unbinds the specified gesture.
        void Unbind(OxyInputGesture gesture);

        // Unbinds the specified command from all gestures.
        void Unbind(IViewCommand command);

        // Unbinds all commands.     
        void UnbindAll();
    }
}
