using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;

namespace Globe3DLight.Data.Animators
{
    public interface ISunData : IData, IAnimator
    {
        dvec3 Position { get; }

        dmat4 ModelMatrix { get; }
    }


    public class SunAnimator : ObservableObject, ISunData
    {
        private readonly ISunDatabase _sunDatabase;

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
        public SunAnimator(ISunDatabase sunDatabase)
        {
            this._sunDatabase = sunDatabase;

            this._position0 = sunDatabase.Position0;
            this._position1 = sunDatabase.Position1;
            this._timeBegin = sunDatabase.TimeBegin;
            this._timeEnd = sunDatabase.TimeEnd;
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
