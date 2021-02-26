using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.SceneTimer
{
    public delegate void TimerHandler(double t);

    public interface ISceneTimer : IObservableObject
    {
        event TimerHandler OnUpdate;

        void OnReset();

        void OnPlay();

        void OnPause();

        void OnSlower();

        void OnFaster();

        string StringTime { get; set; }

        double SliderTimeMinumum { get; set; }

        double SliderTimeMaximum { get; set; }

        double SliderTime { get; set; }
    }
}
