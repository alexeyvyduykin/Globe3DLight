using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ScenarioObjects;



namespace Globe3DLight.ScenarioObjects
{
    public class SatelliteTask : ObservableObject, ISatelliteTask
    {
        private bool _hasRotations;
        private bool _hasObservations;
        private bool _hasTransmission;
        private string _searchString;
        private IList<ISatelliteEvent> _events;
        private ISatelliteEvent _selectedEvent;

        public bool HasRotations 
        {
            get => _hasRotations; 
            set => Update(ref _hasRotations, value);
        }
        public bool HasObservations 
        {
            get => _hasObservations;
            set => Update(ref _hasObservations, value);
        }
        public bool HasTransmissions 
        {
            get => _hasTransmission; 
            set => Update(ref _hasTransmission, value); 
        }
        public string SearchString 
        {
            get => _searchString; 
            set => Update(ref _searchString, value); 
        }
        public IList<ISatelliteEvent> Events 
        {
            get => _events; 
            set => Update(ref _events, value); 
        }
        public ISatelliteEvent SelectedEvent 
        {
            get => _selectedEvent;
            set => Update(ref _selectedEvent, value); 
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
