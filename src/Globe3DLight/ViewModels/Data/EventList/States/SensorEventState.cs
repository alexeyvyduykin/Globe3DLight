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

        int Direction { get; }
    }

    public class SensorEventState : ISensorEventState
    {
        private readonly Shoot _shoot;
        private readonly int _direction;
        private readonly double _t;

        public SensorEventState(double t, Shoot shoot, int direction)
        {
            _t = t;
            _shoot = shoot;
            _direction = direction;
        }

        public double Time => _t;

        public Shoot Shoot => _shoot;

        public int Direction => _direction;
    }
}
