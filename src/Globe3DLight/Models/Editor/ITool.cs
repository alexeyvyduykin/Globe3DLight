using Globe3DLight.Input;

namespace Globe3DLight.Editor
{
    public interface ITool
    {   
        // Handle mouse left button down events.
        void LeftDown(InputArgs args);

        // Handle mouse left button up events.
        void LeftUp(InputArgs args);
  
        // Handle mouse right button down events.
        void RightDown(InputArgs args);
 
        // Handle mouse right button up events.
        void RightUp(InputArgs args);

        // Handle mouse move events.
        void Move(InputArgs args);
    }
}
