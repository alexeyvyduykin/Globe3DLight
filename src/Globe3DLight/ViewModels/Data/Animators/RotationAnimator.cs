using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using System.Diagnostics;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    public class RotationAnimator : BaseState, IAnimator
    {      
        private readonly IEventList<RotationInterval> _rotationEvents;
        private double _gamDEG;

        public RotationAnimator(RotationData data)
        {
            _rotationEvents = Create(data.Rotations);
        }

        public double GamDEG
        {
            get => _gamDEG;
            protected set => RaiseAndSetIfChanged(ref _gamDEG, value);
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
            
            ModelMatrix = dmat4.Rotate(-glm.Radians(GamDEG), dvec3.UnitX);
        }
    }
}
