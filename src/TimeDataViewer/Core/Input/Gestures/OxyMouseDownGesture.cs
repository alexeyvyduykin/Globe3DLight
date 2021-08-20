namespace TimeDataViewer.Core
{
    public class OxyMouseDownGesture : OxyInputGesture
    {
        public OxyMouseDownGesture(OxyMouseButton mouseButton)
        {
            MouseButton = mouseButton;
        }

        // Gets the mouse button.    
        public OxyMouseButton MouseButton { get; private set; }

        // Indicates whether the current object is equal to another object of the same type.
        public override bool Equals(OxyInputGesture other)
        {
            return other is OxyMouseDownGesture mg && mg.MouseButton == MouseButton;
        }
    }
}
