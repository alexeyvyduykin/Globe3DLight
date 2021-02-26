using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.Timer
{
    public class Timer : ITimer
    {
        protected readonly Stopwatch _timer;
        
        public virtual double CurrentTime => _timer.Elapsed.TotalSeconds;

        public bool IsRunning => _timer.IsRunning;

        public Timer()
        {
            _timer = new Stopwatch();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public virtual void Reset()
        {
            _timer.Reset();
        }

        public virtual void SetTime(double t) { }
    }


    public class AcceleratedTimer : Timer, IAcceleratedTimer
    {
        private long _normalElapsedTime;
        private double _acceleratedElapsedTime;
        private double _accelerationStep;

        public AcceleratedTimer()
        {
            _normalElapsedTime = 0;
            _acceleratedElapsedTime = 0.0; 
            _accelerationStep = 1.0;
        }

        public override double CurrentTime
        {
            get
            {
                long newNormalElapsedTime = _timer.ElapsedTicks;

                _acceleratedElapsedTime += ((double)(newNormalElapsedTime - _normalElapsedTime) / Stopwatch.Frequency) * _accelerationStep;

                _normalElapsedTime = newNormalElapsedTime;

                return _acceleratedElapsedTime;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _normalElapsedTime = 0;
            _acceleratedElapsedTime = 0.0;
            _accelerationStep = 1.0;
        }

        public void Faster()
        {
            if (_accelerationStep >= 4096.0)
            {
                return;
            }
            _accelerationStep *= 2.0;
        }

        public void Slower()
        {
            if (_accelerationStep <= 1.0)
            {
                return;
            }
            _accelerationStep /= 2.0;
        }

        public override void SetTime(double t) 
        {
            _acceleratedElapsedTime = t;
        }
    }
}
