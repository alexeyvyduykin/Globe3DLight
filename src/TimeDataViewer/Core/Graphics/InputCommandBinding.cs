namespace TimeDataViewer.Core
{
    public class InputCommandBinding
    {
        public InputCommandBinding(OxyInputGesture gesture, IViewCommand command)
        {
            Gesture = gesture;
            Command = command;
        }

        // Gets the gesture. 
        public OxyInputGesture Gesture { get; private set; }

        // Gets the command.   
        public IViewCommand Command { get; private set; }
    }
}
