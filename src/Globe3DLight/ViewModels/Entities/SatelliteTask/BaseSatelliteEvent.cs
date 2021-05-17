using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.ViewModels.Entities
{
    public abstract class BaseSatelliteEvent : ViewModelBase
    {
        private DateTime _epoch;
        private double _beginTime;
        private double _endTime;

        public DateTime Epoch
        {
            get => _epoch;
            set => RaiseAndSetIfChanged(ref _epoch, value);
        }

        public double BeginTime
        {
            get => _beginTime;
            set => RaiseAndSetIfChanged(ref _beginTime, value);
        }

        public double EndTime
        {
            get => _endTime;
            set => RaiseAndSetIfChanged(ref _endTime, value);
        }

        public DateTime Begin => Epoch.AddSeconds(_beginTime);

        public TimeSpan Duration => Epoch.AddSeconds(_endTime) - Epoch.AddSeconds(_beginTime);
    }
}
