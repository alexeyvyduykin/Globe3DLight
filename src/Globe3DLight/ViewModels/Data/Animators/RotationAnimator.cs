using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Diagnostics;


namespace Globe3DLight.Data
{
    public interface IRotationState : IState, IAnimator
    {
        dmat4 RotationMatrix { get; }

        double GamDEG { get; }
    }

    public class RotationAnimator : ObservableObject, IRotationState
    {      
        private readonly EventList<RotationEventState> _rotationEvents;

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

        public RotationAnimator(RotationData data)
        {         
            _rotationEvents = create(data.Rotations);
        }

        private static EventList<RotationEventState> create(IList<RotationRecord> rotations)
        {
            var rotationEvents = new EventList<RotationEventState>(EventMissMode.LastActive);

            double lastAngle = 0.0;

            foreach (var item in rotations)
            {
                rotationEvents.Add(
                    new RotationEventState(item.BeginTime, lastAngle), 
                    new RotationEventState(item.EndTime, item.Angle)
                    );

                lastAngle = item.Angle;
            }

            return rotationEvents;
        }
    

        private dmat4 Rotation()
        {
            GamDEG = (_rotationEvents.HasActiveState == true) ? _rotationEvents.ActiveState.Angle : 0.0;

            return dmat4.Rotate(-glm.Radians(GamDEG), new dvec3(1.0f, 0.0f, 0.0f));
        }

        public void Animate(double t)
        {
            _rotationEvents.Update(t);

            RotationMatrix = Rotation();
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
