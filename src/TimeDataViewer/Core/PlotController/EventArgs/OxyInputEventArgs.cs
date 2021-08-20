using System;

namespace TimeDataViewer.Core
{
    public abstract class OxyInputEventArgs : EventArgs
    {
        // Gets or sets a value indicating whether the event was handled.      
        public bool Handled { get; set; }
    }
}
