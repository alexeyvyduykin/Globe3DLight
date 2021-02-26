using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;
using System.Linq;

namespace Globe3DLight.Data.Animators
{
    public interface IRetranslatorData : IData, IAnimator
    {
        dvec3 Position { get; }
        dmat4 ModelMatrix { get; }
    }

    public class RetranslatorAnimator : ObservableObject, IRetranslatorData
    {
        private readonly IRetranslatorDatabase _retranslatorDatabase;
        private readonly IList<(double x, double y, double z, double u)> _records;
        private readonly double _timeBegin;
        private readonly double _timeEnd;
        private readonly double _timeStep;


        private dvec3 _position;
        private dmat4 _modelMatrix;
        private dmat4 _translate;
        private dmat4 _rotation;
        public dmat4 Translate
        {
            get => _translate;
            protected set => Update(ref _translate, value);
        }

        public dmat4 Rotation
        {
            get => _rotation;
            protected set => Update(ref _rotation, value);
        }


        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => Update(ref _modelMatrix, value);
        }

        public dvec3 Position
        {
            get => _position;
            protected set => Update(ref _position, value);
        }

        public RetranslatorAnimator(IRetranslatorDatabase retranslatorDatabase)
        {
            this._retranslatorDatabase = retranslatorDatabase;

            this._records = retranslatorDatabase.Records.Select(s => (s[0], s[1], s[2], s[3])).ToList();
            this._timeBegin = retranslatorDatabase.TimeBegin;
            this._timeEnd = retranslatorDatabase.TimeEnd;
            this._timeStep = retranslatorDatabase.TimeStep;
        }

        private dvec3 GetPosition(double t)
        {
            double tCur = t;// base.LocalTime;

            int n = (int)Math.Floor(tCur / _timeStep);

            //  dvec3 pn = positions[n];
            //  dvec3 pk = positions[n + 1];

            dvec3 p;
            double OrbitRadius;

            if (n == _records.Count - 1) // для времени t равного Tend
            {
                p = new dvec3(_records[n].y, _records[n].z, _records[n].x);

                OrbitRadius = p.Length;
            }
            else
            {
                dvec3 pn = new dvec3(_records[n].y, _records[n].z, _records[n].x);
                dvec3 pk = new dvec3(_records[n + 1].y, _records[n + 1].z, _records[n + 1].x);

                OrbitRadius = pn.Length;

                double coef = (tCur - _timeStep * n) / _timeStep;

                p = pn + (pk - pn) * coef;
            }

            p = glm.Normalized(p);

            p *= OrbitRadius;

            return p;
        }

        public void Animate(double t)
        {
            var pos = GetPosition(t);
            dmat4 translate = dmat4.Translate(pos);
            ModelMatrix = translate;
            Position = new dvec3(translate.Column3);
        }


        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
