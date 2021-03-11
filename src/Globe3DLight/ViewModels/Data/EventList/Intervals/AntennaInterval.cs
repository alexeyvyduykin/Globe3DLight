using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    internal class AntennaInterval : BaseEventInterval, IAnimatableInterval<IAntennaEventState>
    {
        private readonly string _target;

        public AntennaInterval(double t0, double t1, string target) : base(t0, t1)
        {
            _target = target;
        }

        public IAntennaEventState Animate(double t)
        {
            return new AntennaEventState(t, _target);
        }
    }
}
