using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.ViewModels.Entities
{
    public abstract class BaseSatelliteEvent : ViewModelBase
    {
        private DateTime _begin;
        private TimeSpan _duration;

        public DateTime Begin 
        {
            get => _begin; 
            set => RaiseAndSetIfChanged(ref _begin, value); 
        }

        public TimeSpan Duration 
        {
            get => _duration; 
            set => RaiseAndSetIfChanged(ref _duration, value); 
        }
    }
}
