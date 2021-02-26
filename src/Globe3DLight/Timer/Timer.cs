using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Globe3DLight.SceneTimer
{
    internal class Timer
    {
        public Timer()
        {         
        }

        public void Start()
        {
            timer.Start();
        }

        public void Pause()
        {
            timer.Stop();
        }

        public virtual void Reset()
        {
            timer.Reset();

            if (OnReset != null)
            {
                OnReset(null, EventArgs.Empty);
            }
        }

        public virtual double Time()
        {
            return timer.Elapsed.TotalSeconds;
        }

        public bool IsRunning()
        {
        
            return timer.IsRunning;
        }

        public event EventHandler OnReset;

        protected Stopwatch timer = new Stopwatch();

    }

    internal class AdvancedTimer : Timer
    {
        public override double Time()
        {
            long newNormalElapsedTime = timer.ElapsedTicks;

            acceleratedElapsedTime += ((double)(newNormalElapsedTime - normalElapsedTime) / Stopwatch.Frequency) * acceleration;

            normalElapsedTime = newNormalElapsedTime;

            return acceleratedElapsedTime;
        }
        public void SetElapsedTime(double dt)
        {
            acceleratedElapsedTime = dt;
        }

        public override void Reset()
        {
            base.Reset();
            normalElapsedTime = 0;
            acceleratedElapsedTime = 0.0;
            acceleration = 1.0;
        }

        public void Faster()
        {
            if (acceleration >= 4096.0)
                return;
            acceleration *= 2.0;
        }

        public void Slower()
        {
            if (acceleration <= 1.0)
                return;
            acceleration /= 2.0;
        }

        private long normalElapsedTime = 0;
        private double acceleratedElapsedTime = 0.0;
        private double acceleration = 1.0;
    }
}
