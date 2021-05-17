#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

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
        private BaseSatelliteEvent? _selectedEvent;
        private Satellite _satellite;
        private ObservableCollection<BaseSatelliteEvent> _rotations;
        private ObservableCollection<BaseSatelliteEvent> _observations;
        private ObservableCollection<BaseSatelliteEvent> _transmissions;

        public SatelliteTask(Satellite satellite, IList<BaseSatelliteEvent> events)
        {
            var sortEvents = events.OrderBy(s => s.BeginTime/*Begin*/).ToList();

            _satellite = satellite;
            _sourceEvents = sortEvents;
            _events = sortEvents;
            _selectedEvent = sortEvents.FirstOrDefault();
            _searchString = string.Empty;

            _rotations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is RotationEvent));
            _observations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is ObservationEvent));
            _transmissions = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is TransmissionEvent));

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(HasRotations) || e.PropertyName == nameof(HasObservations) || 
                e.PropertyName == nameof(HasTransmissions) || e.PropertyName == nameof(SearchString))
                {
                    Events = CreateFrom(_sourceEvents);
                    SelectedEvent = Events.FirstOrDefault();
                }
            };
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

        public bool IsVisible 
        {
            get => _isVisible; 
            set => RaiseAndSetIfChanged(ref _isVisible, value); 
        }

        public bool HasRotations
        {
            get => _hasRotations;
            set => RaiseAndSetIfChanged(ref _hasRotations, value);
        }
        
        public bool HasObservations
        {
            get => _hasObservations;
            set => RaiseAndSetIfChanged(ref _hasObservations, value);
        }
        
        public bool HasTransmissions
        {
            get => _hasTransmission;
            set => RaiseAndSetIfChanged(ref _hasTransmission, value);
        }
        
        public string SearchString 
        {
            get => _searchString;
            set => RaiseAndSetIfChanged(ref _searchString, value);
        }

        public Satellite Satellite 
        {
            get => _satellite; 
            set => RaiseAndSetIfChanged(ref _satellite, value); 
        }

        public ObservableCollection<BaseSatelliteEvent> Rotations
        {
            get => _rotations;
            set => RaiseAndSetIfChanged(ref _rotations, value);
        }

        public ObservableCollection<BaseSatelliteEvent> Observations
        {
            get => _observations;
            set => RaiseAndSetIfChanged(ref _observations, value);
        }

        public ObservableCollection<BaseSatelliteEvent> Transmissions
        {
            get => _transmissions;
            set => RaiseAndSetIfChanged(ref _transmissions, value);
        }

        public IList<BaseSatelliteEvent> Events 
        {
            get => _events; 
            set => RaiseAndSetIfChanged(ref _events, value); 
        }
       
        public BaseSatelliteEvent? SelectedEvent 
        {
            get => _selectedEvent;
            set => RaiseAndSetIfChanged(ref _selectedEvent, value); 
        }
    }
}
