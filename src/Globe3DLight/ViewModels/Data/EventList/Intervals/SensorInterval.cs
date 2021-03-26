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
        private Scan _scan;
        private readonly int _direction;
        private readonly dmat4 _mat0;
        private readonly dmat4 _mat1;

        public SensorInterval(double t0, double t1, double gam1Deg, double gam2Deg, double range1, double range2, 
            dmat4 modelMatrix0, dmat4 modelMatrix1) : base(t0, t1)
        {
            _direction = (gam1Deg >= 0.0 && gam2Deg >= 0.0) ? 1 : -1;

            double yScale1 = -range1;
            double yScale2 = -range2;
            double angRot1 = -Math.Abs(glm.Radians(gam1Deg)) * _direction;
            double angRot2 = -Math.Abs(glm.Radians(gam2Deg)) * _direction;

            var p0 = new dvec3(0.0, Math.Cos(angRot1) * yScale1, Math.Sin(angRot1) * yScale1);
            var p1 = new dvec3(0.0, Math.Cos(angRot2) * yScale2, Math.Sin(angRot2) * yScale2);

            _shoot = new Shoot(){ p0 = p0, p1 = p1 };

            _mat0 = modelMatrix0;
            _mat1 = modelMatrix1;

            _scan = new Scan()
            {
                p0 = (modelMatrix0 * p0.ToVec4()).ToDvec3(),
                p1 = (modelMatrix1 * p0.ToVec4()).ToDvec3(),
                p2 = (modelMatrix1 * p1.ToVec4()).ToDvec3(),
                p3 = (modelMatrix0 * p1.ToVec4()).ToDvec3(),
            };
        }

        public ISensorEventState Animate(double t)
        {
            var len = (_mat1.Column3 - _mat0.Column3).Length;
            var x0 = ((t - EndTime) / (EndTime - BeginTime)) * len;
            var x1 = (1.0 - x0) * len;
            
            var norm = (_mat1.Column3 - _mat0.Column3).Normalized.ToDvec3();

            var v0 = norm * x0;
            var v1 = norm * x1;

            var p0 = _shoot.p0;
            var p1 = _shoot.p1;
            
            _scan = new Scan()
            {
                p0 = p0 - v0,
                p1 = p0 + v1,
                p2 = p1 + v1,
                p3 = p1 - v0,
            };

            return new SensorEventState(t, _shoot, _scan, _direction);
        }
    }
}
