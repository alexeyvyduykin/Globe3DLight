using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Timer;

namespace Globe3DLight.Time
{
    public interface ITimePresenter : IObservableObject
    {
        DateTime Begin { get; }

        TimeSpan Duration { get; }

        ITimer Timer { get; set; }

        double CurrentTime { get; }

        string CurrentTimeString { get; }

        void Update(double t);

        void OnReset();

        void OnPlay();

        void OnPause();

        void OnSlower();

        void OnFaster();
    }
}
