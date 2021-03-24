using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class SceneState : ViewModelBase, ISceneState
    {
        private float _diffuseIntensity;
        private float _specularIntensity;
        private float _ambientIntensity;
        private float _shininess;
        private float _highResolutionSnapScale;
        private ICamera _camera;
        private dvec4 _lightPosition;
        private ITargetable _target;
        private IDictionary<Type, (dvec3 eye, Func<double, double> func)> _cameraBehaviours;

        private double _fieldOfViewX;
        private double _fieldOfViewY;
        private double _aspectRatio;
        private double _perspectiveNearPlaneDistance;
        private double _perspectiveFarPlaneDistance;

        public dmat4 ViewMatrix => 
            Camera.ViewMatrix * Target.InverseAbsoluteModel;

        public dmat4 ProjectionMatrix => 
            dmat4.Perspective(FieldOfViewY, AspectRatio, PerspectiveNearPlaneDistance, PerspectiveFarPlaneDistance);

        public ICamera Camera 
        {
            get => _camera;
            set => RaiseAndSetIfChanged(ref _camera, value);
        }

        public ITargetable Target
        {
            get => _target;
            set => RaiseAndSetIfChanged(ref _target, value);
        }

        public IDictionary<Type, (dvec3 eye, Func<double, double> func)> CameraBehaviours 
        {
            get => _cameraBehaviours;
            set => RaiseAndSetIfChanged(ref _cameraBehaviours, value);
        }

        public dvec4 LightPosition 
        {
            get => _lightPosition; 
            set => RaiseAndSetIfChanged(ref _lightPosition, value); 
        } 
        public float DiffuseIntensity 
        {
            get => _diffuseIntensity;
            set => RaiseAndSetIfChanged(ref _diffuseIntensity, value); 
        }
        public float SpecularIntensity 
        {
            get => _specularIntensity; 
            set => RaiseAndSetIfChanged(ref _specularIntensity, value);
        }
        public float AmbientIntensity 
        {
            get => _ambientIntensity; 
            set => RaiseAndSetIfChanged(ref _ambientIntensity, value);
        }
        public float Shininess 
        {
            get => _shininess; 
            set => RaiseAndSetIfChanged(ref _shininess, value);
        }       
        public float HighResolutionSnapScale
        {
            get => _highResolutionSnapScale; 
            set => RaiseAndSetIfChanged(ref _highResolutionSnapScale, value); 
        }

        public SceneState() { }

        // FOV
        public double FieldOfViewX
        {
            get => _fieldOfViewX = 2.0 * Math.Atan(_aspectRatio * Math.Tan(_fieldOfViewY * 0.5)); 
            set => RaiseAndSetIfChanged(ref _fieldOfViewX, value);
        }
        public double FieldOfViewY
        {
            get => _fieldOfViewY;
            set => RaiseAndSetIfChanged(ref _fieldOfViewY, value);
        }
        public double AspectRatio
        {
            get => _aspectRatio;
            set => RaiseAndSetIfChanged(ref _aspectRatio, value);
        }
        public double PerspectiveNearPlaneDistance
        {
            get => _perspectiveNearPlaneDistance;
            set => RaiseAndSetIfChanged(ref _perspectiveNearPlaneDistance, value);
        }
        public double PerspectiveFarPlaneDistance
        {
            get => _perspectiveFarPlaneDistance;
            set => RaiseAndSetIfChanged(ref _perspectiveFarPlaneDistance, value);
        }
    }
}
