using System;

namespace TimeDataViewer.Core
{
    public abstract class OxyInputGesture : IEquatable<OxyInputGesture>
    {
        public abstract bool Equals(OxyInputGesture other);
    }
}
