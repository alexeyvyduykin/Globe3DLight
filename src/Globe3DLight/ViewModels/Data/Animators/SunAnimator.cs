using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class SunAnimator : BaseState, IAnimator
    {     
        private readonly dvec3 _position0;
        private readonly dvec3 _position1;
        private readonly double _timeBegin;
        private readonly double _timeEnd;
        private dvec3 _position;

        public SunAnimator(SunData data)
        {
            _position0 = data.Position0;
            _position1 = data.Position1;
            _timeBegin = data.TimeBegin;
            _timeEnd = data.TimeEnd;
        }

        public dvec3 Position 
        {
            get => _position; 
            protected set => RaiseAndSetIfChanged(ref _position, value); 
        }

        private dvec3 GetPosition(double t)
        {
            double tCur = t;// base.LocalTime;
    
            double coef = tCur / (_timeEnd - _timeBegin);

            dvec3 p = _position0 + (_position1 - _position0) * coef;

          //  p = glm.Normalized(p);

          //  p *= 60.0f;//range;

            return p;
        }

        public void Animate(double t)
        {
            Position = GetPosition(t);

            ModelMatrix = dmat4.Translate(Position);
        }
    }
}
