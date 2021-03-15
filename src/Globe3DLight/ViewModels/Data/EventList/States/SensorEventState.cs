﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public interface ISensorEventState : IEventState
    {
        IShoot Shoot { get; }

        int Direction { get; }
    }

    public class SensorEventState : ISensorEventState
    {
        private readonly IShoot _shoot;
        private readonly int _direction;
        private readonly double _t;

        public SensorEventState(double t, IShoot shoot, int direction)
        {
            _t = t;
            _shoot = shoot;
            _direction = direction;
        }

        public double Time => _t;

        public IShoot Shoot => _shoot;

        public int Direction => _direction;
    }
}