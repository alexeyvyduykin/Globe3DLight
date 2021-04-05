#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.SceneTimer
{
    internal class Timer
    {
        protected Stopwatch _timer;

        public event EventHandler? OnReset;
 
        public Timer() 
        {
            _timer = new Stopwatch();
        }

        public void Start() => _timer.Start();        

        public void Pause() => _timer.Stop();        

        public virtual void Reset()
        {
            _timer.Reset();

            OnReset?.Invoke(null, EventArgs.Empty);
        }

        public virtual double Time() => _timer.Elapsed.TotalSeconds;
        
        public bool IsRunning() => _timer.IsRunning;        
    }

    internal class AdvancedTimer : Timer
    {
        private long _normalElapsedTime;
        private double _acceleratedElapsedTime;
        private double _acceleration;

        public AdvancedTimer() : base()
        {          
            _normalElapsedTime = 0;      
            _acceleratedElapsedTime = 0.0;         
            _acceleration = 1.0;   
        }

        public override double Time()
        {
            long newNormalElapsedTime = _timer.ElapsedTicks;

            _acceleratedElapsedTime += ((double)(newNormalElapsedTime - _normalElapsedTime) / Stopwatch.Frequency) * _acceleration;

            _normalElapsedTime = newNormalElapsedTime;

            return _acceleratedElapsedTime;
        }

        public void SetElapsedTime(double dt)
        {
            _acceleratedElapsedTime = dt;
        }

        public override void Reset()
        {
            base.Reset();
            _normalElapsedTime = 0;
            _acceleratedElapsedTime = 0.0;
            _acceleration = 1.0;
        }

        public void Faster()
        {
            if (_acceleration < 4096.0)
            {
                _acceleration *= 2.0;
            }
        }

        public void Slower()
        {
            if (_acceleration > 1.0)
            {
                _acceleration /= 2.0;
            }
        }
    }
}
