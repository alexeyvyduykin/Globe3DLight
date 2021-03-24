using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Globe3DLight.ViewModels.Entities
{
    public class SatelliteTask : ViewModelBase
    {
        private readonly IList<BaseSatelliteEvent> _sourceEvents;

        private bool _isVisible;
        private bool _hasRotations;
        private bool _hasObservations;
        private bool _hasTransmission;
        private string _searchString;

        private IList<BaseSatelliteEvent> _events;
        private BaseSatelliteEvent _selectedEvent;
        private Satellite _satellite;

        public SatelliteTask(IList<BaseSatelliteEvent> events)
        {
            var sortEvents = events.OrderBy(s => s.Begin).ToList();

            _sourceEvents = sortEvents;
            _events = sortEvents;
            _selectedEvent = sortEvents.FirstOrDefault();
        }
             
        private IList<BaseSatelliteEvent> CreateFrom(IList<BaseSatelliteEvent> source)
        {
            Func<BaseSatelliteEvent, bool> rotationPredicate = (s => (HasRotations == true) ? s is RotationEvent : false);
            Func<BaseSatelliteEvent, bool> observationPredicate = (s => (HasObservations == true) ? s is ObservationEvent : false);
            Func<BaseSatelliteEvent, bool> transmissionPredicate = (s => (HasTransmissions == true) ? s is TransmissionEvent : false);
            Func<BaseSatelliteEvent, bool> namePredicate =
                (s => (string.IsNullOrEmpty(SearchString) == false) ? s.Name.Contains(SearchString) : true);

            Func<BaseSatelliteEvent, bool> combined = s => rotationPredicate(s) || observationPredicate(s) || transmissionPredicate(s);

            return source.Where(combined).Where(namePredicate).ToList();
        }

        private void Update()
        {
            Events = CreateFrom(_sourceEvents);
            SelectedEvent = Events.FirstOrDefault();
        }

        public bool IsVisible 
        {
            get => _isVisible; 
            set => RaiseAndSetIfChanged(ref _isVisible, value); 
        }

        public bool HasRotations
        {
            get => _hasRotations;
            set 
            {
                RaiseAndSetIfChanged(ref _hasRotations, value);
                Update();
            }
        }
        
        public bool HasObservations
        {
            get => _hasObservations;
            set 
            {
                RaiseAndSetIfChanged(ref _hasObservations, value);
                Update();
            }
        }
        
        public bool HasTransmissions
        {
            get => _hasTransmission;
            set              
            {
                RaiseAndSetIfChanged(ref _hasTransmission, value);
                Update();
            }
        }
        
        public string SearchString 
        {
            get => _searchString;
            set 
            {
                RaiseAndSetIfChanged(ref _searchString, value);
                Update();
            }
        }

        public Satellite Satellite 
        {
            get => _satellite; 
            set => RaiseAndSetIfChanged(ref _satellite, value); 
        }

        public IList<BaseSatelliteEvent> Events 
        {
            get => _events; 
            set => RaiseAndSetIfChanged(ref _events, value); 
        }
       
        public BaseSatelliteEvent SelectedEvent 
        {
            get => _selectedEvent;
            set => RaiseAndSetIfChanged(ref _selectedEvent, value); 
        }
    }
}
