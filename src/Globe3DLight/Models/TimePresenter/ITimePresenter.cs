using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Timer;



namespace Globe3DLight.Time
{
    public delegate void TimeHandler(double t);

    public interface ITimePresenter : IObservableObject
    {
        //DateTime BeginTime { get; set; }
        //TimeSpan TimeSpan { get; set; }
        //double CurrentTime { get; set; }

        TimeInterval TimeInterval { get; set; }
        SliderInterval SliderInterval { get; set; }
        ITimer Timer { get; set; }

        public event TimeHandler OnUpdate;

        void Update(double t);

        void OnReset();
        void OnPlay();
        void OnPause();
        void OnSlower();
        void OnFaster();

    }
}
