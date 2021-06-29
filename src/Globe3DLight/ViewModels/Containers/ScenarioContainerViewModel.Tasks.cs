using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;

namespace Globe3DLight.ViewModels.Containers
{
    public partial class ScenarioContainerViewModel
    {
        private ImmutableArray<SatelliteTask> _tasks;
        private SatelliteTask _currentTask;

        public ImmutableArray<SatelliteTask> Tasks
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
                    SetCameraTo(_currentTask.Satellite);
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
                    if (SceneTimerEditor.Timer.IsRunning == true)
                    {
                        SceneTimerEditor.OnPause();
                    }

                    var time = task.SelectedEvent.Epoch.AddSeconds(task.SelectedEvent.BeginTime);//task.SelectedEvent.Begin;
                    var begin = SceneTimerEditor.Begin;

                    SceneTimerEditor.Update((time - begin).TotalSeconds);
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
