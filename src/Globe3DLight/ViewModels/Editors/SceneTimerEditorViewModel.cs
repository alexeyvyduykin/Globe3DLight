using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private DateTime _epochDateTime;
        private DateTime _currentDateTime;
        private TimeSpan _duration;
        private double _sliderTimeMinumum = 0.0;
        private double _sliderTime = 0.0;
        private double _sliderTimeMaximum = 1000.0;
        private TimerMode _timerMode;

        public event TimeEventHandler UpdateTimeEvent;

        public SceneTimerEditorViewModel()
        {
            _timerMode = TimerMode.Stop;

            _epochDateTime = DateTime.Now;
            _duration = TimeSpan.FromDays(1.0);

            _currentDateTime = _epochDateTime;

            UpdateTimeEvent += SceneTimerViewModel_UpdateTimeEvent;
        }

        private void SceneTimerViewModel_UpdateTimeEvent(object? sender, TimeEventArgs e)
        {
            CurrentDateTime = _epochDateTime.AddSeconds(e.Time);
        }

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => this.RaiseAndSetIfChanged(ref _currentDateTime, value);
        }

        public double SliderTimeMinumum
        {
            get => _sliderTimeMinumum;
            set => this.RaiseAndSetIfChanged(ref _sliderTimeMinumum, value);
        }

        public double SliderTimeMaximum
        {
            get => _sliderTimeMaximum;
            set => this.RaiseAndSetIfChanged(ref _sliderTimeMaximum, value);
        }

        public double SliderTime
        {
            get => _sliderTime;
            set
            {
                var time = value * _duration.TotalSeconds / (_sliderTimeMaximum - _sliderTimeMinumum);

                UpdateTimeEvent?.Invoke(this, new TimeEventArgs(time));

                this.RaiseAndSetIfChanged(ref _sliderTime, value);
            }
        }

        public TimerMode TimerMode
        {
            get => _timerMode;
            set => this.RaiseAndSetIfChanged(ref _timerMode, value);
        }

        public void OnReset()
        {
            TimerMode = TimerMode.Stop;
        }

        public void OnPlay()
        {
            if (TimerMode == TimerMode.Stop || TimerMode == TimerMode.Pause)
            {
                TimerMode = TimerMode.Play;
            }
        }

        public void OnPause()
        {
            if (TimerMode == TimerMode.Play)
            {
                TimerMode = TimerMode.Pause;
            }
        }

        public void OnFaster() { }

        public void OnSlower() { }
    }
}
