using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;
using GlmSharp;

namespace Globe3DLight.ViewModels.Data
{
    public interface IAntennaEventState : IEventState
    {
        string Target { get; }

        dvec3 Position { get; }
    }

    public class AntennaEventState : IAntennaEventState
    {
        private readonly double _t;
        private readonly string _target;
        private readonly dvec3 _position;

        public AntennaEventState(double t, string target, dvec3 position)
        {
            _t = t;
            _target = target;
            _position = position;
        }

        public double Time => _t;

        public string Target => _target;

        public dvec3 Position => _position;
    }
}
