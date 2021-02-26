using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Input
{
    // Defines input target contract.
    public interface IInputTarget
    {
        // Handle left down events.
        void LeftDown(InputArgs args);
  
        // Handle left up events.
        void LeftUp(InputArgs args);
 
        // Handle right down events.
        void RightDown(InputArgs args);
  
        // Handle right up events.
        void RightUp(InputArgs args);
  
        // Handle move events.
        void Move(InputArgs args);
 
        // Check if left down action is available.
        bool IsLeftDownAvailable();
     
        // Check if left up action is available.
        bool IsLeftUpAvailable();
    
        // Check if right down action is available.
        bool IsRightDownAvailable();
    
        // Check if right up action is available.
        bool IsRightUpAvailable();
   
        // Check if move action is available.
        bool IsMoveAvailable();
    }
}
