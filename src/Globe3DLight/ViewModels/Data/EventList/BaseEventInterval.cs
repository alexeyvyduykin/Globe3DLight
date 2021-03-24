using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Data
{
    internal abstract class BaseEventInterval : IEventInterval
    {
        private readonly double _beginTime;
        private readonly double _endTime;

        public BaseEventInterval(double t0, double t1)
        {
            _beginTime = t0;
            _endTime = t1;
        }

        public double BeginTime => _beginTime;

        public double EndTime => _endTime;

        public bool IsRange(double t) => (t >= _beginTime && t <= _endTime);
        
        public bool IsForward(double t) => (t > _endTime);

        public bool IsBackward(double t) => (t < _beginTime);
    }
}
