using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public interface IRotationEventState : IEventState
    {
        double Angle { get; }
    }

    public class RotationEventState : IRotationEventState
    {
        private readonly double _angle;
        private readonly double _t;

        public RotationEventState(double t, double angle)
        {
            _t = t;
            _angle = angle;
        }

        public double Time => _t;

        public double Angle => _angle;
    }
}
