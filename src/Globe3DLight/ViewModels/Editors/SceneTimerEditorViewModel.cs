using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Timer;

namespace Globe3DLight.ViewModels.Editors
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

    public class SceneTimerEditorViewModel : ViewModelBase
    {
        private ITimer _timer;
        private TimeSpan _duration;
        private readonly System.Timers.Timer _timerThread;
        private TimerMode _timerMode;
        private DateTime _currentDateTime;
        private DateTime _begin;
        private double _currentTime;
        private double _sliderMin = 0.0;
        private double _sliderMax = 1000.0;
        private double _sliderValue = 0.0;
       
        public event TimeEventHandler UpdateTimeEvent;

        public SceneTimerEditorViewModel(ITimer timer, DateTime begin, TimeSpan duration)
        {
            _timer = timer;
            _begin = begin;
            _duration = duration;
            _currentTime = 0.0;
            _currentDateTime = begin;

            _timerMode = TimerMode.Stop;

            UpdateTimeEvent += SceneTimerViewModel_UpdateTimeEvent;

            // 1000 milliseconds = 1 sec
            _timerThread = new System.Timers.Timer(1000.0 / 60.0);

            _timerThread.Elapsed += TimerThreadElapsed;

            _timerThread.AutoReset = true;
            _timerThread.Enabled = true;
        }

        private void SceneTimerViewModel_UpdateTimeEvent(object? sender, TimeEventArgs e)
        {
         //   CurrentDateTime = _begin.AddSeconds(e.Time);
        }

        private void TimerThreadElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentTime = _timer.CurrentTime;
            CurrentDateTime = _begin.AddSeconds(CurrentTime);

            var sliderValue = (int)(CurrentTime * (_sliderMax - _sliderMin) / Duration.TotalSeconds);

            RaiseAndSetIfChanged(ref _sliderValue, sliderValue, nameof(SceneTimerEditorViewModel.SliderValue));
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => RaiseAndSetIfChanged(ref _currentDateTime, value);
        }

        public double SliderMin
        {
            get => _sliderMin;
            protected set => RaiseAndSetIfChanged(ref _sliderMin, value);
        }

        public double SliderMax
        {
            get => _sliderMax;
            protected set => RaiseAndSetIfChanged(ref _sliderMax, value);
        }

        public double SliderValue
        {
            get => _sliderValue;
            set
            {     
                var t = value * Duration.TotalSeconds / (_sliderMax - _sliderMin);
                
           //     UpdateTimeEvent?.Invoke(this, new TimeEventArgs(t));
                Update(t);

                RaiseAndSetIfChanged(ref _sliderValue, value);
            }
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

        public ITimer Timer
        {
            get => _timer;
            protected set => RaiseAndSetIfChanged(ref _timer, value);
        }

        public void Update(double t)
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
            if (TimerMode == TimerMode.Stop || TimerMode == TimerMode.Pause)
            {
                TimerMode = TimerMode.Play;
                _timer.Start();
            }
        }

        public void OnPause()
        {
            if (TimerMode == TimerMode.Play)
            {
                TimerMode = TimerMode.Pause;
                _timer.Pause();
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
    }
}
