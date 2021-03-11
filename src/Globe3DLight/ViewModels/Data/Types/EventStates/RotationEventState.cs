using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.Data
{
    public class RotationEventState : IEventState
    {
        private readonly double _angle;
        private readonly double _t;

        public RotationEventState(double t, double angle)
        {
            _t = t;
            _angle = angle;
        }

        public IEventState FromHit(IEventState state0, IEventState state1, double t)
        {
            double tB = state0.t;
            double tE = state1.t;
            double angle0 = ((RotationEventState)state0).Angle;
            double angle1 = ((RotationEventState)state1).Angle;

            float d = (float)(t - tB) / (float)(tE - tB);

            double dGAM = Math.Abs(angle0 - angle1);
            
            int pls = angle1 >= 0.0 ? 1 : -1;

            var angle = angle0 + dGAM * d * (pls);

            return new RotationEventState(t, angle);
        }

        public double t => _t;

        public double Angle => _angle;
    }
}
