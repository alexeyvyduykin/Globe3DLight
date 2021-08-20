using System;

namespace TimeDataViewer.Core
{
    public class AxisChangedEventArgs : EventArgs
    {
        public AxisChangedEventArgs(AxisChangeTypes changeType, double deltaMinimum, double deltaMaximum)
        {
            ChangeType = changeType;
            DeltaMinimum = deltaMinimum;
            DeltaMaximum = deltaMaximum;
        }

        // Gets the type of the change.
        public AxisChangeTypes ChangeType { get; private set; }

        // Gets the delta for the minimum.
        public double DeltaMinimum { get; private set; }

        // Gets the delta for the maximum.
        public double DeltaMaximum { get; private set; }
    }
}
