#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using Globe3DLight.ViewModels.Editors;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Globe3DLight.ViewModels.Entities
{
    public class LabelItem
    {
        public string Label { get; set; }
    }

    public class SatelliteTask : ReactiveObject
    {
        private readonly IList<BaseSatelliteEvent> _eventsSource;
        private DateTime TimeOrigin { get; } = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        public SatelliteTask(Satellite satellite, IList<BaseSatelliteEvent> events)
        {         
            Satellite = satellite;
            _eventsSource = events;

            Events = events;
            SelectedEvent = events.FirstOrDefault();

            Rotations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is RotationEvent));
            Observations = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is ObservationEvent));
            Transmissions = new ObservableCollection<BaseSatelliteEvent>(events.Where(s => s is TransmissionEvent));

            Labels = new ObservableCollection<LabelItem>()
            {
                new() { Label = "Rotation" },
                new() { Label = "Observation" },
                new() { Label = "Transmission" }
            };
        }

        [Reactive]
        public ObservableCollection<LabelItem> Labels { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public double BeginScenario { get; set; }

        [Reactive]
        public double EndScenario { get; set; }

        [Reactive]
        public double Begin { get; set; }

        [Reactive]
        public double Duration { get; set; }

        [Reactive]
        public DateTime Epoch { get; set; }

        [Reactive]
        public List<Rotation> TimelineRotations { get; set; }

        [Reactive]
        public List<Observation> TimelineObservations { get; set; }

        [Reactive]
        public List<Transmission> TimelineTransmissions { get; set; }

        [Reactive]
        public bool IsVisible { get; set; }

        [Reactive]
        public Satellite Satellite { get; set; }

        [Reactive]
        public ObservableCollection<BaseSatelliteEvent> Rotations { get; set; }

        public ObservableCollection<BaseSatelliteEvent> Observations { get; set; }

        [Reactive]
        public ObservableCollection<BaseSatelliteEvent> Transmissions { get; set; }

        [Reactive]
        public IList<BaseSatelliteEvent> Events { get; set; }

        [Reactive]
        public BaseSatelliteEvent? SelectedEvent { get; set; }

        // HACK: For test, full rework
        public void Filtering(SatelliteTaskFilter filter)
        {
            Events = filter.Filtering(_eventsSource);
            SelectedEvent = Events.FirstOrDefault();

            List<Rotation> rotations = new List<Rotation>();
            List<Observation> observations = new List<Observation>();
            List<Transmission> transmissions = new List<Transmission>();

            foreach (var item in Rotations)
            {
                rotations.Add(new Rotation()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            foreach (var item in Observations)
            {
                observations.Add(new Observation()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            foreach (var item in Transmissions)
            {
                transmissions.Add(new Transmission()
                {
                    BeginTime = item.Begin,
                    EndTime = item.Begin + item.Duration,
                });
            }

            Epoch = Rotations.FirstOrDefault().Epoch;

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

        public static double ToTotalDays(DateTime value, DateTime timeOrigin)
        {
            return (value - timeOrigin).TotalDays + 1;
        }
    }

    public class Rotation
    {
        public string Category => "Rotation";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Observation
    {
        public string Category => "Observation";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }

    public class Transmission
    {
        public string Category => "Transmission";

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}
