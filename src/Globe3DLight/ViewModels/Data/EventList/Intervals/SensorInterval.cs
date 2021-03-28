using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    internal class SensorInterval : BaseEventInterval, IAnimatableInterval<ISensorEventState>
    { 
        private readonly Scan _scan;
        private readonly int _direction;

        public SensorInterval(double t0, double t1, int direction, Scan scan) : base(t0, t1)
        {
            _direction = direction;
            _scan = scan;
        }

        public ISensorEventState Animate(double t)
        {        
            return new SensorEventState(t, _scan, _direction);
        }
    }
}
