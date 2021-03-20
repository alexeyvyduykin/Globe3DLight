using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Entities
{
    public class RotationEvent : BaseSatelliteEvent, IRotationEvent
    {
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
