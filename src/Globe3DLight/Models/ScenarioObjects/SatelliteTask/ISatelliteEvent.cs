using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.ScenarioObjects
{
    public interface ISatelliteEvent : IObservableObject
    {
        DateTime Begin { get; set; }

        TimeSpan Duration { get; set; }
    }
}
