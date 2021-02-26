using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    internal class EventInterval<T> where T : EventState
    {
        public EventInterval(T state0, T state1)
        {
            this.StateBegin = state0;
            this.StateEnd = state1;

            this.TimeBegin = state0.t;
            this.TimeEnd = state1.t;
        }

        public bool IsRange(double t)
        {
            if (t >= TimeBegin && t <= TimeEnd)
                return true;

            return false;
        }

        public bool IsForward(double t)
        {
            if (t > TimeEnd)
                return true;

            return false;
        }

        public bool IsBackward(double t)
        {
            if (t < TimeBegin)
                return true;

            return false;
        }

        public T InRange(double t)
        {
            return StateBegin.FromHit(StateBegin, StateEnd, t) as T;
        }

        public double TimeBegin { get; set; }
        public double TimeEnd { get; set; }

        public T StateBegin { get; set; }
        public T StateEnd { get; set; }
    }
}
