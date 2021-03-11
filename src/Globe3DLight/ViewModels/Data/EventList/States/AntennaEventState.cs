using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface IAntennaEventState : IEventState
    {
        string Target { get; }
    }

    public class AntennaEventState : IAntennaEventState
    {
        private readonly double _t;
        private readonly string _target;

        public AntennaEventState(double t, string target)
        {
            _t = t;
            _target = target;
        }

        public double Time => _t;

        public string Target => _target;
    }
}
