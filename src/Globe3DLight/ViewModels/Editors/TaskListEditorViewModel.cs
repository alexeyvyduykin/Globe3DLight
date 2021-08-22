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
using ReactiveUI;
using DynamicData.Binding;
using DynamicData;
using Avalonia.OpenGL;
using System.Reactive.Linq;

namespace Globe3DLight.ViewModels.Editors
{
    public class SatelliteTaskFilter 
    {
        public SatelliteTaskFilter()
        {
            IsRotation = true;
            IsObservation = true;
            IsTransmission = true;
            SearchString = string.Empty;    
        }
        
        public bool IsRotation { get; set; }

        public bool IsObservation { get; set; }

        public bool IsTransmission { get; set; }

        public string SearchString { get; set; }

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

    public class TaskListEditorViewModel : ReactiveObject
    {      
        private SatelliteTask? _currentTask;
        private ScenarioContainerViewModel _scenario;
        private bool _isRotation;
        private bool _isObservation;
        private bool _isTransmission;
        private string _searchString;
       
        public ObservableCollection<SatelliteTask> Tasks { get; set; }

        public TaskListEditorViewModel(ScenarioContainerViewModel scenario)
        {            
            Tasks = new ObservableCollection<SatelliteTask>();  
            _scenario = scenario;

            _currentTask = null;

            _isRotation = true;
            _isObservation = true;
            _isTransmission = true;
            _searchString = string.Empty;

            this.WhenAnyValue(x => x.IsRotation, x => x.IsObservation, x => x.IsTransmission, x => x.SearchString).Subscribe(_ =>
            {
                if (CurrentTask != null)
                {
                    ActivateFilterFor(CurrentTask);
                }
            });


            this.WhenAnyValue(x => x.CurrentTask).Subscribe(task =>
            {
                if(task == null)
                {
                    return;
                }

                ActivateFilterFor(task);

                _scenario.SetCameraTo(task.Satellite);
            });


            Tasks.ToObservableChangeSet().AutoRefresh(task => task.IsVisible).Subscribe(changeSet =>
            {
                var task = changeSet.SingleOrDefault().Item.Current;
             
                if(task == null)
                {
                    return;
                }

                if (task.IsVisible == true)
                {
                    CurrentTask = task;
                }
            });

            Tasks.ToObservableChangeSet().AutoRefresh(task => task.SelectedEvent).Subscribe(changeSet =>
            {
                var task = changeSet.SingleOrDefault().Item.Current;
             
                var selectedEvent = task.SelectedEvent;

                if (task == null || selectedEvent == null)
                {
                    return;
                }

                if (_scenario.SceneTimerEditor.Timer.IsRunning == true)
                {
                    _scenario.SceneTimerEditor.OnPause();
                }

                var time = selectedEvent.Epoch.AddSeconds(selectedEvent.BeginTime);
                var begin = _scenario.SceneTimerEditor.Begin;

                _scenario.SceneTimerEditor.Update((time - begin).TotalSeconds);
            });
        }

        private void ActivateFilterFor(SatelliteTask task)
        {
            var filter = new SatelliteTaskFilter()
            {
                IsObservation = IsObservation,
                IsRotation = IsRotation,
                IsTransmission = IsTransmission,
                SearchString = SearchString
            };

            task.Filtering(filter);
        }

        public bool IsRotation
        {
            get => _isRotation;
            set => this.RaiseAndSetIfChanged(ref _isRotation, value);
        }

        public bool IsObservation
        {
            get => _isObservation;
            set => this.RaiseAndSetIfChanged(ref _isObservation, value);
        }

        public bool IsTransmission
        {
            get => _isTransmission;
            set => this.RaiseAndSetIfChanged(ref _isTransmission, value);
        }

        public string SearchString
        {
            get => _searchString;
            set => this.RaiseAndSetIfChanged(ref _searchString, value);
        }

        //public ObservableCollection<SatelliteTask> Tasks
        //{
        //    get => _tasks;
        //    set => this.RaiseAndSetIfChanged(ref _tasks, value);               
        //}

        public SatelliteTask? CurrentTask
        {
            get => _currentTask;
            set => this.RaiseAndSetIfChanged(ref _currentTask, value);                            
        }
    }
}
