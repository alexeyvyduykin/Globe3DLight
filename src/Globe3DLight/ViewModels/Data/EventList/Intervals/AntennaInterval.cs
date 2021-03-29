using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    internal class AntennaInterval : BaseEventInterval, IAnimatableInterval<IAntennaEventState>
    {
        private readonly string _target;
        private readonly BaseState _state;

        public AntennaInterval(double t0, double t1, string target, BaseState state) : base(t0, t1)
        {
            _target = target;
            _state = state;
        }

        public IAntennaEventState Animate(double t)
        {
            return new AntennaEventState(t, _target, _state.AbsoluteModelMatrix.Column3.ToDvec3());
        }
    }
}
