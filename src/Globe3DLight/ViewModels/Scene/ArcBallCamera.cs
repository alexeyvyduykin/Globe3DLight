using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Spatial;
using Globe3DLight.Models.Scene;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;

namespace Globe3DLight.ViewModels.Scene
{
    public class ArcballCamera : BaseCamera, IArcballCamera
    {
        private dvec3 _rotateBegin;
        private dvec3 _rotateEnd;
        private double _adjustWidth;
        private double _adjustHeight;
        private double _width;
        private double _height;
        private dmat4 _translation;
        private dmat4 _rotation;
        private dmat4 _view;
        private dquat _orientation;
        private dquat _orientationSaved;
        private double _radius = 1.0;

        public ArcballCamera(dvec3 eye, dvec3 target, dvec3 up)
        {
            LookAt(eye, target, up);
        }

        public double AdjustWidth
        {
            get => _adjustWidth;
            set => RaiseAndSetIfChanged(ref _adjustWidth, value);
        }

        public double AdjustHeight
        {
            get => _adjustHeight;
            set => RaiseAndSetIfChanged(ref _adjustHeight, value);
        }

        public double Width
        {
            get => _width;
            set => RaiseAndSetIfChanged(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => RaiseAndSetIfChanged(ref _height, value);
        }

        public override dmat4 ViewMatrix => _view;

        public override void LookAt(dvec3 eye, dvec3 target, dvec3 up)
        {
            Eye = eye;
            Target = target;
            Up = up;
            _view = dmat4.LookAtSafe(Eye, Target, Up);
            _translation = dmat4.Translate(0.0, 0.0, -glm.Length(Eye));
            _orientation = dquat.FromMat4(_view);
            _rotation = ToMatrixRotate(_orientation);
        }

        public void Resize(int width, int height)
        {
            Width = width;
            Height = height;

            if ((double)height / (double)width >= 1.0)
            {
                height = width;
            }
            else
            {
                width = height;
            }

          //  AdjustWidth = 1.0 / ((width - 1) * 0.5);
          //  AdjustHeight = 1.0 / ((height - 1) * 0.5);

            AdjustWidth = 2.0f / width;
            AdjustHeight = 2.0f / height;
        }

        public void Zoom(double deltaZ)
        {
            _translation *= dmat4.Translate(new dvec3(0.0, 0.0, -deltaZ));
            _view = _translation * _rotation;

            var zAxis = new dvec3(_view.m02, _view.m12, _view.m22);
            Eye = -zAxis * _translation.m32;

            var range = (Eye - Target).Length;
            _radius = 1.0 / (0.38 * (range - 1.0)) + 1.4;      
        }
       
        public ivec2 Point0 { get; set; }

        public ivec2 Point1 { get; set; }

        public void RotateBegin(int x, int y)
        {
            _orientationSaved = _orientation;
            _rotateBegin = MapToSphere(x, y);

            Point0 = new ivec2(x, y);
            Point1 = new ivec2(x, y);
        }

        public void Rotate(int x, int y)
        {
            var ThisQuat = Drag(x, y);
            _orientation = ThisQuat * _orientationSaved;

            _rotation = ToMatrixRotate(_orientation);
            _view = _translation * _rotation;

            dvec3 zAxis = new dvec3(_view.m02, _view.m12, _view.m22);

            Eye = -zAxis * _translation.m32;

            Point1 = new ivec2(x, y);
        }

        public void RotateEnd(int x, int y)
        {
            Point0 = new ivec2(x, y);
            Point1 = new ivec2(x, y);
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

            xs = q.x * s; ys = q.y * s; zs = q.z * s;
            wx = q.w * xs; wy = q.w * ys; wz = q.w * zs;
            xx = q.x * xs; xy = q.x * ys; xz = q.x * zs;
            yy = q.y * ys; yz = q.y * zs; zz = q.z * zs;

            m.m00 = 1.0 - (yy + zz); m.m01 = xy + wz; m.m02 = xz - wy; m.m03 = 0.0;
            m.m10 = xy - wz; m.m11 = 1.0 - (xx + zz); m.m12 = yz + wx; m.m13 = 0.0;
            m.m20 = xz + wy; m.m21 = yz - wx; m.m22 = 1.0 - (xx + yy); m.m23 = 0.0;
            m.m30 = 0.0; m.m31 = 0.0; m.m32 = 0.0; m.m33 = 1.0;
            return m;
        }
        
        public dvec3 MapToSphere(int x, int y)
        {
            //hyperboloid mapping taken from https://www.opengl.org/wiki/Object_Mouse_Trackball

            double pX = x * _adjustWidth - 1.0;
            double pY = y * _adjustHeight - 1.0;

            dvec3 P = new dvec3(pX, -pY, 0);

            //sphere radius
            // const double radius = 0.5;
            // const double radius_squared = radius * radius;
            var radius_squared = _radius * _radius;
        
            double XY_squared = P.x * P.x + P.y * P.y;

            if (XY_squared <= 0.5 * radius_squared)
                P.z = Math.Sqrt(radius_squared - XY_squared);  // Pythagore
            else
                P.z = (0.5 * (radius_squared)) / Math.Sqrt(XY_squared);  // hyperboloid

            return P;
        }

        private dvec3 MapToSphere_old(int x, int y)
        {        
            //Adjust point coords and scale down to range of [-1 ... 1]
            double newX = (x * AdjustWidth) - 1.0;
            double newY = 1.0 - (y * AdjustHeight);

            //Compute the square of the length of the vector to the point from the center
            var length2 = (newX * newX) + (newY * newY);
            var radius2 = _radius * _radius;

            // Если точка отображена за пределами сферы (length > radius squared)
            if (length2 > radius2)
            {              
                //Вычисление коэфф. нормализации(radius / sqrt(length))
                var norm = _radius / Math.Sqrt(length2);

                //Return the "normalized" vector, a point on the sphere
                return new(newX * norm, newY * norm, 0.0);
            }
            else    //Else it's on the inside
            {
                //Return a vector to a point mapped inside the sphere sqrt(radius squared - length)
                return new(newX, newY, Math.Sqrt(radius2 - length2));
            }  
        }

        private dquat Drag(int x, int y)
        {
            const double epsilon = 1.0e-5;

            dquat qRotation = new dquat();

            _rotateEnd = MapToSphere(x, y);

            dvec3 tempStVec, tempEnVec;
            tempStVec.x = 0.0;
            tempStVec.y = 0.0;
            tempStVec.z = 1.0;

            tempEnVec.x = _rotateEnd.x - _rotateBegin.x;
            tempEnVec.y = _rotateEnd.y - _rotateBegin.y;
            tempEnVec.z = 1.0;

            dvec3 Perp = dvec3.Cross(tempStVec, tempEnVec);

            double lll = glm.Length(Perp);// Perp.magnitude();
                                          //Compute the length of the perpendicular vector
            if (lll > epsilon)    //if its non-zero
            {
                //We're ok, so return the perpendicular vector as the transform after all
                qRotation.x = Perp.x;
                qRotation.y = Perp.y;
                qRotation.z = Perp.z;
                //In the quaternion values, w is cosine (theta / 2), where theta is rotation angle
                qRotation.w = dvec3.Dot(_rotateBegin, _rotateEnd);
            }
            else                                    //if its zero
            {
                //The begin and end vectors coincide, so return an identity transform
                qRotation.x =
                qRotation.y =
                qRotation.z =
                qRotation.w = 0.0;
            }
            return qRotation;
        }
    }
}
