using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Linq;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class RetranslatorAnimator : BaseState, IAnimator
    {     
        private readonly IList<(double x, double y, double z, double u)> _records;
        private readonly double _timeBegin;
        private readonly double _timeEnd;
        private readonly double _timeStep;     
        private dvec3 _position; 
        private dmat4 _translate;
        private dmat4 _rotation;

        public RetranslatorAnimator(RetranslatorData data)
        {
            _records = data.Records.Select(s => (s[0], s[1], s[2], s[3])).ToList();
            _timeBegin = data.TimeBegin;
            _timeEnd = data.TimeEnd;
            _timeStep = data.TimeStep;
        }

        public dmat4 Translate
        {
            get => _translate;
            protected set => RaiseAndSetIfChanged(ref _translate, value);
        }

        public dmat4 Rotation
        {
            get => _rotation;
            protected set => RaiseAndSetIfChanged(ref _rotation, value);
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

        private dmat4 OrbitalMatrix(double t, double vx, double vy, double vz)
        {
            int n = (int)Math.Floor(t / _timeStep);

            var arr1 = _records[n];
            // double[] arr2 = Array[n + 1];

            dvec3 v1 = glm.Normalized(new dvec3(arr1.y, arr1.z, arr1.x));
            dvec3 v2 = glm.Normalized(new dvec3(vy, vz, vx));
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

                double coef = (t - _timeStep * n) / _timeStep;

                u = un + (uk - un) * coef;
            }

            m = m * dmat4.Rotate(-(u - un), dvec3.UnitZ);

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

        private double GetU(double t)
        {
            int n = (int)Math.Floor(t / _timeStep);
           
            double u;
            if (n == _records.Count - 1) // для времени t равного Tend
            {
                u = _records[n].u;         
            }
            else
            {
                var un = _records[n].u;
                var uk = _records[n + 1].u;

                var coef = (t - _timeStep * n) / _timeStep;

                u = un + (uk - un) * coef;
            }

            return u;
        }

        public void Animate(double t)
        {
            var pos = GetPosition(t);
            var translation = dmat4.Translate(pos);

            var len = pos.Length;
            var angle = glm.Degrees(Math.Acos(glm.Dot(dvec3.UnitZ, -glm.Normalized(pos))));
            
            var rotation = dmat4.Rotate(-angle, dvec3.UnitY);
      
            ModelMatrix = dmat4.Translate(pos) * rotation;
            Position = new dvec3(translation.Column3);
        }

        private static dmat4 ToMatrixRotate(dquat q)
        {
            // Converts this quaternion to a rotation matrix.
            //
            //  | 1 - 2(y^2 + z^2)	     2(xy + wz)	       2(xz - wy)		0  |
            //  | 2(xy - wz)		   1 - 2(x^2 + z^2)	       2(yz + wx)	  0  |
            //  | 2(xz + wy)	         	2(yz - wx)	 1 - 2(x^2 + y^2)	  0  |
            //  | 0				          	          0					         0	 	1  |

            double n, s;
            double xs, ys, zs;
            double wx, wy, wz;
            double xx, xy, xz;
            double yy, yz, zz;

            dmat4 m = new dmat4();

            if (q.x > 1.0E+10 || q.x < -1.0E+10)
            {
                q.x = 1.0;
                q.y = 1.0;
                q.z = 1.0;
                q.w = 1.0;
            }

            n = (q.x * q.x) + (q.y * q.y) + (q.z * q.z) + (q.w * q.w);
            s = (n > 0.0) ? (2.0 / n) : 0.0;

            xs = q.x * s;
            ys = q.y * s;
            zs = q.z * s;
            wx = q.w * xs;
            wy = q.w * ys;
            wz = q.w * zs;
            xx = q.x * xs;
            xy = q.x * ys;
            xz = q.x * zs;
            yy = q.y * ys;
            yz = q.y * zs;
            zz = q.z * zs;

            m.m00 = 1.0 - (yy + zz);
            m.m01 = xy + wz;
            m.m02 = xz - wy;
            m.m03 = 0.0;
            m.m10 = xy - wz;
            m.m11 = 1.0 - (xx + zz);
            m.m12 = yz + wx;
            m.m13 = 0.0;
            m.m20 = xz + wy;
            m.m21 = yz - wx;
            m.m22 = 1.0 - (xx + yy);
            m.m23 = 0.0;
            m.m30 = 0.0;
            m.m31 = 0.0;
            m.m32 = 0.0;
            m.m33 = 1.0;
            return m;
        }

        private double[] kepkin(double a, double u)
        {
            // kep = e, a, 0.0, incl, om, u
            double[] kep = new double[6] { 0.0, a, 0.0, 0.0, 0.0, u };
            double[] kin = new double[6];

            var mu = 398600.44;
            var co = Math.Cos(kep[4]);
            var so = Math.Sin(kep[4]);
            var ci = Math.Cos(kep[3]);
            var si = Math.Sin(kep[3]);
            kin[0] = 1 + kep[0] * Math.Cos(kep[5] - kep[2]);
            kin[2] = kep[1] * (1 - kep[0] * kep[0]) / kin[0];
            kin[3] = Math.PI / 2.0 + kep[5] - Math.Atan(kep[0] * Math.Sin(kep[5] - kep[2]) / kin[0]);
            kin[0] = kep[5];
            kin[5] = Math.Sqrt(mu * (2 - kin[2] / kep[1]) / kin[2]);
            for (int i = 0; i <= 1; i++)
            {
                var cu = Math.Cos(kin[3 * i]);
                var su = Math.Sin(kin[3 * i]);
                kin[3 * i] = kin[3 * i + 2] * (co * cu - so * su * ci);
                kin[3 * i + 1] = kin[3 * i + 2] * (so * cu + co * su * ci);
                kin[3 * i + 2] *= su * si;
            }

            return kin;
        }
    }
}
