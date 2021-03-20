using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Entities
{
    public abstract class BaseSatelliteEvent : ObservableObject, ISatelliteEvent
    {
        private DateTime _begin;
        private TimeSpan _duration;

        public DateTime Begin 
        {
            get => _begin; 
            set => Update(ref _begin, value); 
        }

        public TimeSpan Duration 
        {
            get => _duration; 
            set => Update(ref _duration, value); 
        }
    }
}
