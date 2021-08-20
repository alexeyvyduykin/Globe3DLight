#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Globe3DLight.ViewModels.Editors;
using TimeDataViewer.Core;

namespace Globe3DLight.ViewModels.Entities
{
    public class LabelItem
    {
        public string Label { get; set; }
    }

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
        private ObservableCollection<LabelItem> _labels;
        private DateTime _epoch;
        private DateTime TimeOrigin { get; } = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        private List<Rotation> _timelineRotations;
        private List<Observation> _timelineObservations;
        private List<Transmission> _timelineTransmissions;
        private double _beginScenario;
        private double _endScenario;
        private double _begin;
        private double _duration;
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

            _labels = new ObservableCollection<LabelItem>()
            {
                new() { Label = "Rotation" },
                new() { Label = "Observation" },
                new() { Label = "Transmission" }
            };
        }

        public ObservableCollection<LabelItem> Labels
        {
            get => _labels;
            set => this.RaiseAndSetIfChanged(ref _labels, value);
        }

        public double BeginScenario
        {
            get => _beginScenario;
            set => this.RaiseAndSetIfChanged(ref _beginScenario, value);
        }

        public double EndScenario
        {
            get => _endScenario;
            set => this.RaiseAndSetIfChanged(ref _endScenario, value);
        }

        public double Begin
        {
            get => _begin;
            set => this.RaiseAndSetIfChanged(ref _begin, value);
        }

        public double Duration
        {
            get => _duration;
            set => this.RaiseAndSetIfChanged(ref _duration, value);
        }

        public DateTime Epoch
        {
            get => _epoch;
            set => this.RaiseAndSetIfChanged(ref _epoch, value);
        }

        // HACK: For test, full rework
        public void Filtering(Filter filter)
        {
            Events = filter.Filtering(_sourceEvents);
            SelectedEvent = Events.FirstOrDefault();

            List<Rotation> rotations = new List<Rotation>();
            List<Observation> observations = new List<Observation>();
            List<Transmission> transmissions = new List<Transmission>();

            foreach (var item in _rotations)
            {
                rotations.Add(new Rotation()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            foreach (var item in _observations)
            {
                observations.Add(new Observation()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            foreach (var item in _transmissions)
            {
                transmissions.Add(new Transmission()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            Epoch = _rotations.FirstOrDefault().Epoch;

            var min = rotations.Min(s => ToTotalDays(s.BeginTime, Epoch));
            min = Math.Min(observations.Min(s => ToTotalDays(s.BeginTime, Epoch)), min);
            min = Math.Min(transmissions.Min(s => ToTotalDays(s.BeginTime, Epoch)), min);

            var max = rotations.Max(s => ToTotalDays(s.EndTime, Epoch));
            max = Math.Max(observations.Max(s => ToTotalDays(s.EndTime, Epoch)), max);
            max = Math.Max(transmissions.Max(s => ToTotalDays(s.EndTime, Epoch)), max);

            TimelineRotations = new List<Rotation>(rotations);
            TimelineObservations = new List<Observation>(observations);
            TimelineTransmissions = new List<Transmission>(transmissions);

            BeginScenario = ToTotalDays(Epoch.Date, TimeOrigin);
            EndScenario = BeginScenario + 2;

            Begin = ToTotalDays(Epoch, TimeOrigin);
            Duration = 1.0;
        }
        
        public List<Rotation> TimelineRotations
        {
            get => _timelineRotations;
            set => RaiseAndSetIfChanged(ref _timelineRotations, value);
        }

        public List<Observation> TimelineObservations
        {
            get => _timelineObservations;
            set => RaiseAndSetIfChanged(ref _timelineObservations, value);
        }

        public List<Transmission> TimelineTransmissions
        {
            get => _timelineTransmissions;
            set => RaiseAndSetIfChanged(ref _timelineTransmissions, value);
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

        public static double ToTotalDays(DateTime value, DateTime timeOrigin)
        {
            return (value - timeOrigin).TotalDays + 1;
        }
    }

    public class Rotation : TimelineItem
    {
        public string Category => "Rotation";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Observation : TimelineItem
    {
        public string Category => "Observation";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Transmission : TimelineItem
    {
        public string Category => "Transmission";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
