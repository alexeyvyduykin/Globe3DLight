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
        private readonly Shoot _shoot;
        private readonly int _direction;

        public SensorInterval(double t0, double t1, double gam1Deg, double gam2Deg, double range1, double range2) : base(t0, t1)
        {
            _direction = (gam1Deg >= 0.0 && gam2Deg >= 0.0) ? 1 : -1;

            double yScale1 = -range1;
            double yScale2 = -range2;
            double angRot1 = -Math.Abs(glm.Radians(gam1Deg)) * _direction;
            double angRot2 = -Math.Abs(glm.Radians(gam2Deg)) * _direction;
            double dx = 0.02;

            _shoot = new Shoot()
            {
                p0 = new dvec3(-dx * _direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),    // * pls - для обхода TRIANGLE_STRIP по часов стр.                
                p1 = new dvec3(-dx * _direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                p2 = new dvec3(dx * _direction, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2),
                p3 = new dvec3(dx * _direction, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1),
            };
        }

        public ISensorEventState Animate(double t)
        {
            return new SensorEventState(t, _shoot, _direction);
        }
    }
}
