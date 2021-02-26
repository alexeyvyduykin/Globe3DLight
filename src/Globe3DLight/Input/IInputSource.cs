using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Input
{
    // Defines input source contract.  
    public interface IInputSource
    {    
        // Left down events. 
        IObservable<InputArgs> LeftDown { get; set; }
   
        // Left up events.   
        IObservable<InputArgs> LeftUp { get; set; }

        // Right down events. 
        IObservable<InputArgs> RightDown { get; set; }
        
        // Right up events.    
        IObservable<InputArgs> RightUp { get; set; }
     
        // Move events.    
        IObservable<InputArgs> Move { get; set; }
    }
}
