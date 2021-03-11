using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public class SensorEventState : IEventState
    {
        private readonly IShoot _shoot;
        private readonly int _direction;
        private readonly double _gam1RAD;
        private readonly double _gam2RAD;
        private readonly double _range1;
        private readonly double _range2;
        private readonly double _t;
     
        public SensorEventState(double t, double gam1RAD, double gam2RAD, double range1, double range2)
        {
            _t = t;
            _gam1RAD = gam1RAD;
            _gam2RAD = gam2RAD;
            _range1 = range1;
            _range2 = range2;

            _direction = (_gam1RAD >= 0.0 && _gam2RAD >= 0.0) ? 1 : -1;

            double yScale1 = -_range1;
            double yScale2 = -_range2;
            double angRot1 = -Math.Abs(_gam1RAD) * _direction;
            double angRot2 = -Math.Abs(_gam2RAD) * _direction;
            double dx = 0.02;

            _shoot = new Shoot()
            {
                p0 = new dvec3(-dx * _direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),    // * pls - для обхода TRIANGLE_STRIP по часов стр.                
                p1 = new dvec3(-dx * _direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                p2 = new dvec3(dx * _direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                p3 = new dvec3(dx * _direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),
            };
        }

        protected SensorEventState(IShoot shoot, int direction)
        {
            _shoot = shoot;
            _direction = direction;
        }

        public double t => _t;

        public IShoot Shoot => _shoot;

        public int Direction => _direction;

        public IEventState FromHit(IEventState state0, IEventState state1, double t)
        {
            return new SensorEventState(_shoot, _direction);
        }
    }

}
