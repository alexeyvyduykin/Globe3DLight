using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Input
{
    public abstract class InputSource : IInputSource
    {        
        public IObservable<InputArgs> LeftDown { get; set; }
     
        public IObservable<InputArgs> LeftUp { get; set; }
     
        public IObservable<InputArgs> RightDown { get; set; }
    
        public IObservable<InputArgs> RightUp { get; set; }
     
        public IObservable<InputArgs> Move { get; set; }
    }
}
