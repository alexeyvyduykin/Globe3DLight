using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public abstract class BaseCamera : ObservableObject, ICamera
    {
        private dvec3 _eye;
        private dvec3 _target;
        private dvec3 _up;
        //private dvec3 _forward;
        //private dvec3 _right;

        public abstract dmat4 ViewMatrix { get; }

        public dvec3 Eye 
        {
            get => _eye; 
            set => Update(ref _eye, value); 
        }

        public dvec3 Target 
        {
            get => _target; 
            set => Update(ref _target, value); 
        }

        public dvec3 Up
        {
            get => _up;
            set => Update(ref _up, value); 
        }

        public dvec3 Forward
        {
            get { return (_target - _eye).Normalized; }
        }

        public dvec3 Right
        {
            get { return dvec3.Cross(Forward, _up).Normalized; }
        }

        public abstract void LookAt(dvec3 eye, dvec3 target, dvec3 up);

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

}
