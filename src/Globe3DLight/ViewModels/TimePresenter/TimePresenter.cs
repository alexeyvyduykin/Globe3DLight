using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Timer;
using System.Diagnostics;

namespace Globe3DLight.Time
{
    public class TimePresenter : ObservableObject
    {
        private ITimer _timer;  
        private DateTime _begin;
        private TimeSpan _duration;
        private readonly System.Timers.Timer _timerThread;
        private double _currentTime;
        private string _currentTimeString;

        public TimePresenter(ITimer timer, DateTime begin, TimeSpan duration)
        {
            _timer = timer;
            _begin = begin;
            _duration = duration;
            _currentTime = 0.0;
            _currentTimeString = string.Empty;

            // 1000 milliseconds = 1 sec
            _timerThread = new System.Timers.Timer(1000.0 / 60.0);

            _timerThread.Elapsed += TimerThreadElapsed;
            
            _timerThread.AutoReset = true;
            _timerThread.Enabled = true;
        }

        public DateTime Begin 
        {
            get => _begin; 
            protected set => Update(ref _begin, value); 
        }

        public TimeSpan Duration 
        {
            get => _duration; 
            protected set => Update(ref _duration, value); 
        }

        public string CurrentTimeString
        {
            get => _currentTimeString;
            protected set => Update(ref _currentTimeString, value);
        }

        public double CurrentTime 
        { 
            get => _currentTime; 
            protected set => Update(ref _currentTime, value); 
        }

        protected virtual void TimerThreadElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentTime = _timer.CurrentTime;
       
            var dt = Begin.AddSeconds(CurrentTime);
            CurrentTimeString = dt.ToLongDateString() + " " + dt.ToLongTimeString();               
        }

        public ITimer Timer
        {
            get => _timer;
            protected set => Update(ref _timer, value);
        }

        public virtual void Update(double t) 
        {
            Timer.SetTime(t);
        }
    
        public void OnReset()
        {
            _timer.Reset();
        }

        public void OnPlay()
        {
            _timer.Start();
        }

        public void OnPause()
        {
            _timer.Pause();
        }

        public void OnSlower()
        {
            if(_timer is IAcceleratedTimer acceleratedTimer)
            {
                acceleratedTimer.Slower();
            }            
        }

        public void OnFaster()
        {
            if (_timer is IAcceleratedTimer acceleratedTimer)
            {
                acceleratedTimer.Faster();
            }        
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
