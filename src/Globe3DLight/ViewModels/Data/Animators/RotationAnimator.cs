using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Data.Database;
using System.Diagnostics;


namespace Globe3DLight.Data.Animators
{
    public interface IRotationData : IData, IAnimator
    {
        dmat4 RotationMatrix { get; }

        double GamDEG { get; }
    }

    public class RotationAnimator : ObservableObject, IRotationData
    {
        private readonly IRotationDatabase _rotationDatabase;
        private readonly ContinuousEvents<RotationState> _rotationEvents;

        private dmat4 _rotationMatrix;
        private double _gamDEG;

        public dmat4 RotationMatrix 
        {
            get => _rotationMatrix; 
            protected set => Update(ref _rotationMatrix, value);
        }

        public double GamDEG
        {
            get => _gamDEG;
            protected set => Update(ref _gamDEG, value);
        }

        public RotationAnimator(IRotationDatabase rotationDatabase)
        {
            this._rotationDatabase = rotationDatabase;
            this._rotationEvents = create(rotationDatabase.Rotations);
        }

        private ContinuousEvents<RotationState> create(IList<RotationRecord> rotations)
        {
            var rotationEvents = new ContinuousEvents<RotationState>() { MissMode = MissMode.LastActive };

            double lastAngle = 0.0;

            foreach (var item in rotations)
            {
                rotationEvents.AddFrom(new RotationState()
                {
                    t = item.BeginTime,
                    Angle = lastAngle,
                });

                rotationEvents.AddTo(new RotationState()
                {
                    t = item.EndTime,
                    Angle = item.Angle,
                });

                lastAngle = item.Angle;
            }

            return rotationEvents;
        }
    

        private dmat4 Rotation(double t)
        {
            GamDEG = 0.0;

            if (_rotationEvents.IsActiveState == true)
            {
                var activeState = _rotationEvents.ActiveState;

                GamDEG = activeState.Angle;

                Debug.WriteLine(string.Format("RotationMatrix({0}): t = {1}, gamDEG = {2}", Name, t, GamDEG));
            }

            return dmat4.Rotate(-glm.Radians(GamDEG), new dvec3(1.0f, 0.0f, 0.0f));
        }

        public void Animate(double t)
        {
            _rotationEvents.Update(t);

            RotationMatrix = Rotation(t);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
