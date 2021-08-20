using System;

namespace TimeDataViewer.Core
{
    public class TrackerEventArgs : EventArgs
    {
        public TrackerHitResult? HitResult { get; set; }
    }
}
