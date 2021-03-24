using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class EarthAnimator : ViewModelBase, IState, IAnimator
    {        
        private readonly double _angleDeg0;
        private double _angleDeg;
        private dmat4 _modelMatrix;
        private readonly DateTime _epoch;

        public EarthAnimator(EarthData data)
        {
            _angleDeg0 = data.AngleDeg;
            _epoch = data.Epoch;
        }

        public dmat4 ModelMatrix 
        {
            get => _modelMatrix; 
            protected set => RaiseAndSetIfChanged(ref _modelMatrix, value);
        }

        public double AngleDEG 
        {
            get => _angleDeg;
            protected set => RaiseAndSetIfChanged(ref _angleDeg, value);
        }

        public void Animate(double t)
        {
            double w = 7.292115085e-5;
            AngleDEG = _angleDeg0 + glm.Degrees(w * t);
            while (AngleDEG > 360.0)
                AngleDEG -= 360.0;

            ModelMatrix = dmat4.Rotate(glm.Radians(AngleDEG), new dvec3(0.0, 1.0, 0.0));
        }
    }
}
