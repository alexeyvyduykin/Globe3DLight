using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ScenarioObjects;


namespace Globe3DLight.ScenarioObjects
{
    public interface ISatelliteTask : IObservableObject
    {
        bool IsVisible { get; set; }

        bool HasRotations { get; set; }

        bool HasObservations { get; set; }

        bool HasTransmissions { get; set; }

        string SearchString { get; set; }

        ISatellite Satellite { get; set; }

        IList<ISatelliteEvent> Events { get; set; }

        ISatelliteEvent SelectedEvent { get; set; }
    }








}
