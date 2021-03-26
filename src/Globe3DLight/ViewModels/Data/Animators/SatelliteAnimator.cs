using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Linq;
using Globe3DLight.Models.Data;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public class SatelliteAnimator : BaseState, IAnimator, IFrameable
    {
        private readonly IList<(double x, double y, double z, double vx, double vy, double vz, double u)> _records;
        private readonly double _timeBegin;
        private readonly double _timeEnd;
        private readonly double _timeStep;
        private dvec3 _position;   

        public SatelliteAnimator(SatelliteData data)
        {
            _records = data.Records.Select(s => (s[0], s[1], s[2], s[3], s[4], s[5], s[6])).ToList();
            _timeBegin = data.TimeBegin;
            _timeEnd = data.TimeEnd;
            _timeStep = data.TimeStep;
        }

        public dvec3 Position
        {
            get => _position;
            protected set => RaiseAndSetIfChanged(ref _position, value);
        }

        private dvec3 GetPosition(double t)
        {
            double tCur = t;// base.LocalTime;

            int n = (int)Math.Floor(tCur / _timeStep);

            //  dvec3 pn = positions[n];
            //  dvec3 pk = positions[n + 1];

            dvec3 p;
            double OrbitRadius;

            if (n == _records.Count - 1) // для времени t равного Tend
            {
                p = new dvec3(_records[n].y, _records[n].z, _records[n].x);

                OrbitRadius = p.Length;              
            }
            else
            {
                dvec3 pn = new dvec3(_records[n].y, _records[n].z, _records[n].x);
                dvec3 pk = new dvec3(_records[n + 1].y, _records[n + 1].z, _records[n + 1].x);
              
                OrbitRadius = pn.Length;

                double coef = (tCur - _timeStep * n) / _timeStep;

                p = pn + (pk - pn) * coef;
            }

            p = glm.Normalized(p);

            p *= OrbitRadius;

            return p;
        }

        private dmat4 OrbitalMatrix(double t)
        {
            double tCur = t;// base.LocalTime;

            int n = (int)Math.Floor(tCur / _timeStep);

            var arr1 = _records[n];
            // double[] arr2 = Array[n + 1];

            dvec3 v1 = glm.Normalized(new dvec3(arr1.y, arr1.z, arr1.x));
            dvec3 v2 = glm.Normalized(new dvec3(arr1.vy, arr1.vz, arr1.vx));
            dvec3 v3 = dvec3.Cross(v1, v2);

            dmat4 m = new dmat4(v2, v1, v3);

            double u, un;
            if (n == _records.Count - 1) // для времени t равного Tend
            {
                u = _records[n].u;
                un = u;
            }
            else
            {
                un = _records[n].u;
                double uk = _records[n + 1].u;

                double coef = (tCur - _timeStep * n) / _timeStep;

                u = un + (uk - un) * coef;
            }

            m = m * dmat4.Rotate(-(u - un), new dvec3(0.0f, 0.0f, 1.0f));

            //m.m00 = -v2.x; m.m10 = v2.y; m.m20 = -v3.y; m.m30 = 0.0;
            //m.m01 = v1.x; m.m11 = v1.y; m.m21 = v1.z; m.m31 = 0.0;
            //m.m02 = -v3.x; m.m12 = v2.z; m.m22 = -v3.z; m.m32 = 0.0;
            //m.m03 = 0.0;  m.m13 = 0.0;  m.m23 = 0.0;  m.m33 = 1.0;

            //  uCurrent = arr1[6];// u;

            if (double.IsNaN(m.Length) == true) // for retranslators
            {
                m = dmat4.Identity;
            }

            return m;// Conversion.CartesianToOrbital(inclination, u, -om);
        }

        public void Animate(double t)
        {
            Position = GetPosition(t);
            
            var translate = dmat4.Translate(Position);

            var rotation = OrbitalMatrix(t);

            ModelMatrix = translate * rotation;        
        }
    }
}
