namespace TimeDataViewer.Core
{
    public static class ControllerExtensions
    {
        // Binds the specified mouse button to the specified command.
        public static void BindMouseDown(this IController controller, OxyMouseButton mouseButton, IViewCommand<OxyMouseDownEventArgs> command)
        {
            controller.Bind(new OxyMouseDownGesture(mouseButton), command);
        }

        // Binds the mouse enter event to the specified command.
        public static void BindMouseEnter(this IController controller, IViewCommand<OxyMouseEventArgs> command)
        {
            controller.Bind(new OxyMouseEnterGesture(), command);
        }

        // Binds the mouse wheel event to the specified command.
        public static void BindMouseWheel(this IController controller, IViewCommand<OxyMouseWheelEventArgs> command)
        {
            controller.Bind(new OxyMouseWheelGesture(), command);
        }

        // Unbinds the specified mouse down gesture.
        public static void UnbindMouseDown(this IController controller, OxyMouseButton mouseButton)
        {
            controller.Unbind(new OxyMouseDownGesture(mouseButton));
        }

        // Unbinds the mouse enter gesture.
        public static void UnbindMouseEnter(this IController controller)
        {
            controller.Unbind(new OxyMouseEnterGesture());
        }

        // Unbinds the mouse wheel gesture.
        public static void UnbindMouseWheel(this IController controller)
        {
            controller.Unbind(new OxyMouseWheelGesture());
        }
    }
}
