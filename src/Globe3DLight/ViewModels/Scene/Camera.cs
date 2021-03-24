using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Scene;

namespace Globe3DLight.ViewModels.Scene
{
    public abstract class BaseCamera : ViewModelBase, ICamera
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
            set => RaiseAndSetIfChanged(ref _eye, value); 
        }

        public dvec3 Target 
        {
            get => _target; 
            set => RaiseAndSetIfChanged(ref _target, value); 
        }

        public dvec3 Up
        {
            get => _up;
            set => RaiseAndSetIfChanged(ref _up, value); 
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
    }

}
