﻿using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;


namespace Globe3DLight.Data.Animators
{
    public interface IJ2000Data : IData, IAnimator
    {
        dmat4 ModelMatrix { get; }

        double AngleDEG { get; }
    }


    public class J2000Animator : ObservableObject, IJ2000Data
    {
        private readonly IJ2000Database _j2000Database;
        private readonly double _angleDeg0;
        private double _angleDeg;

        private dmat4 _modelMatrix;
        private DateTime _epoch;

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

        public J2000Animator(IJ2000Database j2000Database)
        {
            this._j2000Database = j2000Database;

            this._angleDeg0 = j2000Database.AngleDeg;
            this._epoch = j2000Database.Epoch;
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
