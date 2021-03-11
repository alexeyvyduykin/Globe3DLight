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
        private readonly IEventList<RotationInterval> _rotationEvents;
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
            _rotationEvents = Create(data.Rotations);
        }

        private static IEventList<RotationInterval> Create(IList<RotationRecord> rotations)
        {
            var rotationEvents = new EventList<RotationInterval>(EventMissMode.LastActive);

            double lastAngle = 0.0;

            foreach (var item in rotations)
            {
                rotationEvents.Add(new RotationInterval(item.BeginTime, item.EndTime, lastAngle, item.Angle)); 

                lastAngle = item.Angle;
            }

            return rotationEvents;
        }
    
        public void Animate(double t)
        {
            var activeInterval = _rotationEvents.ActiveInterval(t);

            GamDEG = (activeInterval != default) ? activeInterval.Animate(t).Angle : 0.0;
            
            RotationMatrix = dmat4.Rotate(-glm.Radians(GamDEG), new dvec3(1.0f, 0.0f, 0.0f));
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
