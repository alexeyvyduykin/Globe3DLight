using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Data
{
    public interface ISunState : IState, IAnimator
    {
        dvec3 Position { get; }

        dmat4 ModelMatrix { get; }
    }


    public class SunAnimator : ObservableObject, ISunState
    {     
        private readonly dvec3 _position0;
        private readonly dvec3 _position1;
        private readonly double _timeBegin;
        private readonly double _timeEnd;

        private dvec3 _position;
        private dmat4 _modelMatrix;

        public dvec3 Position 
        {
            get => _position; 
            protected set => Update(ref _position, value); 
        }
        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => Update(ref _modelMatrix, value);
        }
        public SunAnimator(SunData data)
        {            
            _position0 = data.Position0;
            _position1 = data.Position1;
            _timeBegin = data.TimeBegin;
            _timeEnd = data.TimeEnd;
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

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
