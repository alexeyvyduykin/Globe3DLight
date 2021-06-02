using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Timer;
using System.Diagnostics;

namespace Globe3DLight.ViewModels.Time
{
    public enum TimerMode { Play, Stop, Pause };

    public delegate void TimeEventHandler(object? sender, TimeEventArgs e);

    public class TimeEventArgs : EventArgs
    {
        private readonly double _time;
        public TimeEventArgs(double time)
        {
            _time = time;
        }

        public double Time => _time;
    }

    public class TimePresenter : ViewModelBase
    {
        private ITimer _timer;  
        private DateTime _begin;
        private TimeSpan _duration;
        private readonly System.Timers.Timer _timerThread;
        private double _currentTime;    
        private TimerMode _timerMode;
        private DateTime _currentDateTime;

        public TimePresenter(ITimer timer, DateTime begin, TimeSpan duration)
        {
            _timer = timer;
            _begin = begin;
            _duration = duration;
            _currentTime = 0.0;
            _currentDateTime = begin;

            _timerMode = TimerMode.Stop;

            // 1000 milliseconds = 1 sec
            _timerThread = new System.Timers.Timer(1000.0 / 60.0);

            _timerThread.Elapsed += TimerThreadElapsed;
            
            _timerThread.AutoReset = true;
            _timerThread.Enabled = true;
        }

        public TimerMode TimerMode
        {
            get => _timerMode;
            set => RaiseAndSetIfChanged(ref _timerMode, value);
        }

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

        public double CurrentTime 
        { 
            get => _currentTime; 
            protected set => RaiseAndSetIfChanged(ref _currentTime, value); 
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => RaiseAndSetIfChanged(ref _currentDateTime, value);
        }

        protected virtual void TimerThreadElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentTime = _timer.CurrentTime;      
            CurrentDateTime = _begin.AddSeconds(CurrentTime);                  
        }

        public ITimer Timer
        {
            get => _timer;
            protected set => RaiseAndSetIfChanged(ref _timer, value);
        }

        public virtual void Update(double t) 
        {
            Timer.SetTime(t);
        }
    
        public void OnReset()
        {
            TimerMode = TimerMode.Stop;
            _timer.Reset();
        }

        public void OnPlay()
        {
            TimerMode = TimerMode.Play;
            _timer.Start();
        }

        public void OnPause()
        {
            TimerMode = TimerMode.Pause;
            _timer.Pause();
        }

        public void OnSlower()
        {
            if (TimerMode == TimerMode.Play)
            {
                if (_timer is IAcceleratedTimer acceleratedTimer)
                {
                    acceleratedTimer.Slower();
                }
            }
        }

        public void OnFaster()
        {
            if (TimerMode == TimerMode.Play)
            {
                if (_timer is IAcceleratedTimer acceleratedTimer)
                {
                    acceleratedTimer.Faster();
                }
            }
        }
    }
}
