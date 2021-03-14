using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Timer;
using System.Diagnostics;

namespace Globe3DLight.Time
{
    public class TimeInterval : ObservableObject
    {
        private DateTime _beginDateTime;
        private TimeSpan _timeSpan;
        private double _currentTime;


        public DateTime BeginTime
        {
            get => _beginDateTime;
            set => Update(ref _beginDateTime, value);
        }
        public TimeSpan TimeSpan
        {
            get => _timeSpan;
            set => Update(ref _timeSpan, value);
        }
        public double CurrentTime
        {
            get => _currentTime;
            set
            {
                Update(ref _currentTime, value);
                //Notify();
            }
        }

        public TimeInterval(double current, DateTime begin, TimeSpan duration)
        {
            _currentTime = current;
            _beginDateTime = begin;
            _timeSpan = duration;
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

    public class SliderInterval : ObservableObject
    {
        private readonly TimePresenter _timePresenter;

        private int _min;
        private int _max;
        private int _value;
        public int Min
        {
            get => _min;
            set => Update(ref _min, value);
        }
        public int Max
        {
            get => _max;
            set => Update(ref _max, value);
        }
        public int Value
        {
            get => _value;
            set                 
            {
                FromSlider(value);
                Update(ref _value, value); 
            }
        }

        public SliderInterval(TimePresenter timePresenter, int min, int max)
        {
            _timePresenter = timePresenter;               
            _min = min;
            _max = max;
            _value = (int)(_timePresenter.TimeInterval.CurrentTime * max / _timePresenter.TimeInterval.TimeSpan.TotalSeconds);
        }
        private void FromSlider(int value)
        {
            var t = value * _timePresenter.TimeInterval.TimeSpan.TotalSeconds / Max;

            _timePresenter.Update(t);
        }
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

    public class TimePresenter : ObservableObject, ITimePresenter
    {
     //   private DateTime _beginDateTime;
     //   private TimeSpan _timeSpan;
     //   private double _currentTime;
        private ITimer _timer;
        private readonly System.Timers.Timer _timerThread;

        //private enum TimerMode { Stop, Pause, Play };       

        //private string _stringTime;       
        //private TimerMode _timerMode = TimerMode.Stop;

        //public DateTime BeginTime 
        //{
        //    get => _beginDateTime; 
        //    set => Update(ref _beginDateTime, value); 
        //} 
        //public TimeSpan TimeSpan 
        //{ 
        //    get => _timeSpan; 
        //    set => Update(ref _timeSpan, value); 
        //} 
        //public double CurrentTime
        //{
        //    get => _currentTime;
        //    set 
        //    { 
        //        Update(ref _currentTime, value);
        //        //Notify();
        //    }
        //}

        private TimeInterval _timeInterval;
        public TimeInterval TimeInterval 
        {
            get => _timeInterval;
            set => Update(ref _timeInterval, value);
        }

        private SliderInterval _sliderInterval;
        public SliderInterval SliderInterval
        {
            get => _sliderInterval;
            set => Update(ref _sliderInterval, value);             
        }

        public ITimer Timer
        {
            get => _timer;
            set => Update(ref _timer, value);
        }

        public event TimeHandler OnUpdate;

        private readonly DateTime _begin;
        private readonly TimeSpan _duration;

        public TimePresenter(DateTime begin, TimeSpan duration)
        {
            // 1000 milliseconds = 1 sec
            _timerThread = new System.Timers.Timer(1000.0 / 30.0);

            _timerThread.Elapsed += TimerThread_Elapsed;
            _timerThread.AutoReset = true;
            _timerThread.Enabled = true;

          //  _currentTime = 0.0;
            _begin = begin;
            _duration = duration;

            TimeInterval = new TimeInterval(0.0, _begin, _duration);
            SliderInterval = new SliderInterval(this, 0, 1000);
        }


        public void Update(double t) 
        {
            Timer.SetTime(t);

            TimeInterval = new TimeInterval(t, _begin, _duration);

            if (Timer.IsRunning == false)
            {
                OnUpdate?.Invoke(t);
            }
        }

        private double last = 0.0;

        private void TimerThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {            
            if (_timer.IsRunning == true)
            {
                // it's tick -> *.Time() do t += dt;
               // CurrentTime = _timer.CurrentTime;

                TimeInterval = new TimeInterval(_timer.CurrentTime, _begin, _duration);

                SliderInterval = new SliderInterval(this, 0, 1000);
                //   var startTicks = Stopwatch.GetTimestamp();

     //           double current = Stopwatch.GetTimestamp() - last;

                // Do stuff
     //           OnUpdate?.Invoke(TimeInterval.CurrentTime/* CurrentTime*/);

      //          last = Stopwatch.GetTimestamp();

                // var ticks = Stopwatch.GetTimestamp() - startTicks;

                // UpdateTicks = string.Format("update globeviewer, ticks: {0}", ticks);

                //  ThreadElapsed = string.Format("thread elapsed, seconds: {0}", current / Stopwatch.Frequency);

     //           Debug.WriteLine(string.Format("Elapsed, seconds: {0}", current / Stopwatch.Frequency));
            }        
        }

        public void OnReset()
        {
            _timer.Reset();

          //  CurrentTime = 0.0;

            TimeInterval = new TimeInterval(0.0, _begin, _duration);
            SliderInterval = new SliderInterval(this, 0, 1000);

            OnUpdate?.Invoke(TimeInterval.CurrentTime/* CurrentTime*/);
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
