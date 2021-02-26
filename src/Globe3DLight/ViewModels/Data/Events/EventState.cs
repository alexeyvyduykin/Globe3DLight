using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public abstract class EventState
    {
        internal abstract EventState FromHit(EventState state0, EventState state1, double t);

        public double t { get; set; }
    }



}
