using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public interface ISensorEventState : IEventState
    {
        Shoot Shoot { get; }

        Scan Scan { get; }

        int Direction { get; }
    }

    public class SensorEventState : ISensorEventState
    {
        private readonly Shoot _shoot;
        private readonly Scan _scan;
        private readonly int _direction;
        private readonly double _t;

        public SensorEventState(double t, Shoot shoot, Scan scan, int direction)
        {
            _t = t;
            _shoot = shoot;
            _scan = scan;
            _direction = direction;
        }

        public double Time => _t;

        public Shoot Shoot => _shoot;

        public Scan Scan => _scan;

        public int Direction => _direction;
    }
}
