using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ScenarioObjects;


namespace Globe3DLight.ScenarioObjects
{
    public interface ISatelliteTask : IObservableObject
    {
        bool HasRotations { get; set; }

        bool HasObservations { get; set; }

        bool HasTransmissions { get; set; }

        string SearchString { get; set; }

        IList<ISatelliteEvent> Events { get; set; }

        ISatelliteEvent SelectedEvent { get; set; }
    }








}
