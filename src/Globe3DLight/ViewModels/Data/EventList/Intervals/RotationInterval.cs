using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    internal class RotationInterval : BaseEventInterval, IAnimatableInterval<IRotationEventState>
    {
        private readonly double _angle0Deg;
        private readonly double _angle1Deg;

        public RotationInterval(double t0, double t1, double angle0Deg, double angle1Deg) : base(t0, t1)
        {
            _angle0Deg = angle0Deg;
            _angle1Deg = angle1Deg;
        }

        public IRotationEventState Animate(double t)
        {
            if (t >= BeginTime && t <= EndTime)
            {
                double d = (t - BeginTime) / (EndTime - BeginTime);

                double dGAM = Math.Abs(_angle0Deg - _angle1Deg);

                int pls = (_angle1Deg >= 0.0) ? 1 : -1;

                var angle = _angle0Deg + dGAM * d * pls;

                return new RotationEventState(t, angle);
            }
            else if (t < BeginTime)
            {
                return new RotationEventState(BeginTime, _angle0Deg);
            }
            else
            {
                return new RotationEventState(EndTime, _angle1Deg);
            }
        }
    }

}
