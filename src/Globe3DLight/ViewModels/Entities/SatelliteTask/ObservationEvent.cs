﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Entities
{

    public class ObservationEvent : BaseSatelliteEvent, IObservationEvent
    {
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}