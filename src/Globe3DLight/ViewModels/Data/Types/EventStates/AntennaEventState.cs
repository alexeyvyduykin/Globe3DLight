using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public class AntennaEventState : IEventState
    {
        private readonly double _t;
        private readonly string _target;

        public AntennaEventState(double t, string target)
        {
            _t = t;
            _target = target;
        }

        public IEventState FromHit(IEventState state0, IEventState state1, double t)
        {
            return new AntennaEventState(t, _target);
        }

        public double t => _t;

        public string Target => _target;
    }
}
