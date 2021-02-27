using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.Data
{
    public class RotationEventState : EventState
    {
        internal override EventState FromHit(EventState state0, EventState state1, double t)
        {
            double tB = state0.t;
            double tE = state1.t;
            double Angle0 = (state0 as RotationEventState).Angle;
            double Angle1 = (state1 as RotationEventState).Angle;

            float d = (float)(t - tB) / (float)(tE - tB);

            double dGAM = Math.Abs(Angle0 - Angle1);
            int pls = Angle1 >= 0.0 ? 1 : -1;


            Debug.WriteLine(string.Format("Rotation: t = {0}", t));


            return new RotationEventState()
            {
                t = t,
                Angle = Angle0 + dGAM * d * (pls),
            };




            // this.t = t;
            // this.Angle = Angle0 + dGAM * d * pls;
        }

        public double Angle { get; internal set; }
    }
}
