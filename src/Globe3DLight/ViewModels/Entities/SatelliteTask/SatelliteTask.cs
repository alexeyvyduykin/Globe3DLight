#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Globe3DLight.ViewModels.Editors;

namespace Globe3DLight.ViewModels.Entities
{
    public class SatelliteTask : ViewModelBase
    {
        private readonly IList<BaseSatelliteEvent> _sourceEvents;
        private bool _isVisible;
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
       
            _rotations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is RotationEvent));
            _observations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is ObservationEvent));
            _transmissions = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is TransmissionEvent));
        }
             
        public void Filtering(Filter filter)
        {
            Events = filter.Filtering(_sourceEvents);
            SelectedEvent = Events.FirstOrDefault();
        }

        public bool IsVisible 
        {
            get => _isVisible; 
            set => RaiseAndSetIfChanged(ref _isVisible, value); 
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
