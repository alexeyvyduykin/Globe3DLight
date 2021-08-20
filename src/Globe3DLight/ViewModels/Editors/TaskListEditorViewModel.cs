using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reactive.Disposables;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Containers;

namespace Globe3DLight.ViewModels.Editors
{
    public class Filter : ViewModelBase
    {
        private bool _isRotation;
        private bool _isObservation;
        private bool _isTransmission;
        private string _searchString;
        private readonly TaskListEditorViewModel _taskListEditor;

        public Filter(TaskListEditorViewModel taskListEditor)
        {
            _taskListEditor = taskListEditor;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsRotation) || e.PropertyName == nameof(IsObservation) ||
                e.PropertyName == nameof(IsTransmission) || e.PropertyName == nameof(SearchString))
                {
                    if (_taskListEditor.CurrentTask != null)
                    {
                        _taskListEditor.CurrentTask.Filtering(this);
                    }
                }
            };
        }
        
        public bool IsRotation
        {
            get => _isRotation;
            set => RaiseAndSetIfChanged(ref _isRotation, value);
        }

        public bool IsObservation
        {
            get => _isObservation;
            set => RaiseAndSetIfChanged(ref _isObservation, value);
        }

        public bool IsTransmission
        {
            get => _isTransmission;
            set => RaiseAndSetIfChanged(ref _isTransmission, value);
        }

        public string SearchString
        {
            get => _searchString;
            set => RaiseAndSetIfChanged(ref _searchString, value);
        }

        public IList<BaseSatelliteEvent> Filtering(IList<BaseSatelliteEvent> source)
        {
            Func<BaseSatelliteEvent, bool> rotationPredicate = (s => (IsRotation == true) ? s is RotationEvent : false);
            Func<BaseSatelliteEvent, bool> observationPredicate = (s => (IsObservation == true) ? s is ObservationEvent : false);
            Func<BaseSatelliteEvent, bool> transmissionPredicate = (s => (IsTransmission == true) ? s is TransmissionEvent : false);
            Func<BaseSatelliteEvent, bool> namePredicate =
                (s => (string.IsNullOrEmpty(SearchString) == false) ? s.Name.Contains(SearchString) : true);

            Func<BaseSatelliteEvent, bool> combined = s => rotationPredicate(s) || observationPredicate(s) || transmissionPredicate(s);

            return source.Where(combined).Where(namePredicate).ToList();
        }
    }

    public class TaskListEditorViewModel : ViewModelBase
    {
        private ObservableCollection<SatelliteTask> _tasks;
        private SatelliteTask _currentTask;
        private ScenarioContainerViewModel _scenario;

        public TaskListEditorViewModel(ScenarioContainerViewModel scenario)
        {
            Filter = new Filter(this) { IsObservation = true, IsRotation = true, IsTransmission = true };
            _tasks = new ObservableCollection<SatelliteTask>();// ImmutableArray.Create<SatelliteTask>();    
            _scenario = scenario;          
        }

        public Filter Filter { get; set; }

        public ObservableCollection<SatelliteTask> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    RaiseAndSetIfChanged(ref _tasks, value);

                    foreach (var task in _tasks)
                    {
                        task.PropertyChanged += TaskVisibilityChangedEvent;
                        task.PropertyChanged += TaskSelectedEventChangedEvent;
                    }                  
                }
            }
        }

        public SatelliteTask CurrentTask
        {
            get => _currentTask;
            set
            {
                if (_currentTask != value)
                {
                    RaiseAndSetIfChanged(ref _currentTask, value);

                    _currentTask.Filtering(Filter);

                    _scenario.SetCameraTo(_currentTask.Satellite);
                }
            }
        }

        private void TaskSelectedEventChangedEvent(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SatelliteTask.SelectedEvent))
            {
                var task = sender as SatelliteTask;

                if (task.SelectedEvent is not null)
                {
                    if (_scenario.SceneTimerEditor.Timer.IsRunning == true)
                    {
                        _scenario.SceneTimerEditor.OnPause();
                    }

                    var time = task.SelectedEvent.Epoch.AddSeconds(task.SelectedEvent.BeginTime);//task.SelectedEvent.Begin;
                    var begin = _scenario.SceneTimerEditor.Begin;

                    _scenario.SceneTimerEditor.Update((time - begin).TotalSeconds);
                }
            }
        }

        private void TaskVisibilityChangedEvent(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SatelliteTask.IsVisible))
            {
                var task = sender as SatelliteTask;
                if (task.IsVisible == true)
                {
                    CurrentTask = task;
                }
            }
        }
    }
}
