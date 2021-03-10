using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface IJ2000State : IState, IAnimator
    {      
        dmat4 ModelMatrix { get; }

        double AngleDEG { get; }
    }


    public class J2000Animator : ObservableObject, IJ2000State
    {        
        private double _angleDeg0;
        private double _angleDeg;

        private dmat4 _modelMatrix;
        private DateTime _epoch;

        public J2000Animator(J2000Data data)
        {
            _angleDeg0 = data.AngleDeg;
            _epoch = data.Epoch;
        }

        public dmat4 ModelMatrix 
        {
            get => _modelMatrix; 
            protected set => Update(ref _modelMatrix, value);
        }

        public double AngleDEG 
        {
            get => _angleDeg;
            protected set => Update(ref _angleDeg, value);
        }

        public void Animate(double t)
        {
            double w = 7.292115085e-5;
            AngleDEG = _angleDeg0 + glm.Degrees(w * t);
            while (AngleDEG > 360.0)
                AngleDEG -= 360.0;

            this.ModelMatrix = dmat4.Rotate(glm.Radians(AngleDEG), new dvec3(0.0, 1.0, 0.0));
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
