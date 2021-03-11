using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    internal class EventInterval<T> where T : IEventState
    {
        private readonly double _beginTime;
        private readonly double _endTime;
        private readonly T _beginState;
        private readonly T _endState;

        public EventInterval(T state0, T state1)
        {
            _beginState = state0;
            _endState = state1;

            _beginTime = state0.t;
            _endTime = state1.t;
        }

        public T BeginState => _beginState;

        public T EndState => _endState;

        public bool IsRange(double t) => (t >= _beginTime && t <= _endTime);
        
        public bool IsForward(double t) => (t > _endTime);

        public bool IsBackward(double t) => (t < _beginTime);

        public T InRange(double t) => (T)_beginState.FromHit(_beginState, _endState, t);        
    }
}
