using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Globe3DLight.Entities
{
    public class SatelliteTask : ObservableObject, ISatelliteTask
    {
        private readonly IList<ISatelliteEvent> _sourceEvents;

        private bool _isVisible;
        private bool _hasRotations;
        private bool _hasObservations;
        private bool _hasTransmission;
        private string _searchString;

        private IList<ISatelliteEvent> _events;
        private ISatelliteEvent _selectedEvent;
        private ISatellite _satellite;

        public SatelliteTask(IList<ISatelliteEvent> events)
        {
            var sortEvents = events.OrderBy(s => s.Begin).ToList();

            _sourceEvents = sortEvents;
            _events = sortEvents;
            _selectedEvent = sortEvents.FirstOrDefault();
        }
             
        private IList<ISatelliteEvent> CreateFrom(IList<ISatelliteEvent> source)
        {
            Func<ISatelliteEvent, bool> rotationPredicate = (s => (HasRotations == true) ? s is IRotationEvent : false);
            Func<ISatelliteEvent, bool> observationPredicate = (s => (HasObservations == true) ? s is IObservationEvent : false);
            Func<ISatelliteEvent, bool> transmissionPredicate = (s => (HasTransmissions == true) ? s is ITransmissionEvent : false);
            Func<ISatelliteEvent, bool> namePredicate =
                (s => (string.IsNullOrEmpty(SearchString) == false) ? s.Name.Contains(SearchString) : true);

            Func<ISatelliteEvent, bool> combined = s => rotationPredicate(s) || observationPredicate(s) || transmissionPredicate(s);

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
            set => Update(ref _isVisible, value); 
        }

        public bool HasRotations
        {
            get => _hasRotations;
            set 
            {
                Update(ref _hasRotations, value);
                Update();
            }
        }
        
        public bool HasObservations
        {
            get => _hasObservations;
            set 
            {
                Update(ref _hasObservations, value);
                Update();
            }
        }
        
        public bool HasTransmissions
        {
            get => _hasTransmission;
            set              
            {
                Update(ref _hasTransmission, value);
                Update();
            }
        }
        
        public string SearchString 
        {
            get => _searchString;
            set 
            {
                Update(ref _searchString, value);
                Update();
            }
        }

        public ISatellite Satellite 
        {
            get => _satellite; 
            set => Update(ref _satellite, value); 
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

        public virtual bool ShouldSerializeHasObservations() => true;
        public virtual bool ShouldSerializeHasRotations() => true;
        public virtual bool ShouldSerializeHasTransmissions() => true;
        public virtual bool ShouldSerializeSearchString() => true;
        public virtual bool ShouldSerializeEvents() => true;
        public virtual bool ShouldSerializeSelectedEvent() => _selectedEvent != null;
    }
}
