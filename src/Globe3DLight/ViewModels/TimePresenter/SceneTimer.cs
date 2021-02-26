using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace Globe3DLight//.SceneTimer
{
    //public class AdvancedSceneTimer : ObservableObject, ISceneTimer
    //{
    //    private enum TimerMode { Stop, Pause, Play };
  
    //    private readonly AdvancedTimer _advancedTimer;
    //    private readonly System.Timers.Timer _timerThread;
    //    private string _stringTime;
    //    private double _sliderTimeMinumum = 0.0;
    //    private double _sliderTime = 0.0;
    //    private double _sliderTimeMaximum = 1000.0;
    //    private double _currentTime;

    //    private TimerMode _timerMode = TimerMode.Stop;

    //    DateTime BeginTime { get; set; } = DateTime.Now;
    //    TimeSpan Span { get; set; } = TimeSpan.FromDays(1.0);
        
    //    public event TimerHandler OnUpdate;
        
    //    public AdvancedSceneTimer()
    //    {          
    //        _advancedTimer = new AdvancedTimer();

    //        // 1000 milliseconds = 1 sec
    //        _timerThread = new System.Timers.Timer(1000.0 / 30.0);

    //        _advancedTimer.OnReset += SimulationTimer_OnReset;

    //        _timerThread.Elapsed += TimerThread_Elapsed;
    //        _timerThread.AutoReset = true;
    //        _timerThread.Enabled = true;

    //        CurrentTime = 0.0;
    //    }

    //    private void SimulationTimer_OnReset(object sender, EventArgs e)
    //    {
    //        CurrentTime = 0.0;

    //        OnUpdate?.Invoke(CurrentTime);   
    //    }

    //    private void TimerThread_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    //    {
    //      //  double current = Stopwatch.GetTimestamp() - last;

    //        if (_advancedTimer.IsRunning() == true)
    //        {
    //            // it's tick -> *.Time() do t += dt;
    //            CurrentTime = _advancedTimer.Time();

    //         //   var startTicks = Stopwatch.GetTimestamp();

    //            // Do stuff
    //            OnUpdate?.Invoke(CurrentTime);

    //           // var ticks = Stopwatch.GetTimestamp() - startTicks;

    //           // UpdateTicks = string.Format("update globeviewer, ticks: {0}", ticks);

    //          //  ThreadElapsed = string.Format("thread elapsed, seconds: {0}", current / Stopwatch.Frequency);
    //        }

    //      //  last = Stopwatch.GetTimestamp();
    //    }

    //    public void OnReset() 
    //    {
    //        _advancedTimer.Reset();
    //    }

    //    public void OnPlay() 
    //    {
    //        _advancedTimer.Start();
    //    }

    //    public void OnPause() 
    //    {
    //        _advancedTimer.Pause();
    //    }

    //    public void OnSlower() 
    //    {
    //        _advancedTimer.Slower();
    //    }

    //    public void OnFaster() 
    //    {
    //        _advancedTimer.Faster();
    //    }

    //    public double CurrentTime
    //    {
    //        get => _currentTime;            
    //        set
    //        {
    //            this.Update(ref _currentTime, value);

    //            StringTime = ToCurrentTime(_currentTime);      
    //        }
    //    }

    //    public string StringTime
    //    {
    //        get => _stringTime;
    //        set => Update(ref _stringTime, value); 
    //    }

    //    public double SliderTimeMinumum
    //    {
    //        get => _sliderTimeMinumum; 
    //        set => Update(ref _sliderTimeMinumum, value); 
    //    }

    //    public double SliderTimeMaximum
    //    {
    //        get => _sliderTimeMaximum; 
    //        set => Update(ref _sliderTimeMaximum, value); 
    //    }

    //    public double SliderTime 
    //    {
    //        get => _sliderTime;
    //        set
    //        {
    //            this.Update(ref _sliderTime, value);

    //            var _currentTimeNEW = value * Span.TotalSeconds / SliderTimeMaximum;

    //            StringTime = ToCurrentTime(_currentTimeNEW);

    //            _advancedTimer.SetElapsedTime(_currentTimeNEW);

    //            if (_advancedTimer.IsRunning() == false)
    //            {
    //                OnUpdate?.Invoke(_currentTimeNEW);
    //            }

    //            this.Update(ref _currentTime, _currentTimeNEW);
    //        }
    //    }

    //    private string ToCurrentTime(double tsec)
    //    {
    //        var dt0 = BeginTime;// DateTime.FromOADate(JulianDateBegin - 2415018.5);

    //        var dt = dt0.AddSeconds(tsec);

    //        return dt.ToLongDateString() + " " + dt.ToLongTimeString();
    //    }

    //    public override object Copy(IDictionary<object, object> shared)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
