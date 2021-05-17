#nullable disable
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
using Globe3DLight.ViewModels.Time;

namespace Globe3DLight.ViewModels.Containers
{
    public class InvalidateScenarioEventArgs : EventArgs { }

    public delegate void InvalidateScenarioEventHandler(object sender, InvalidateScenarioEventArgs e);

    public class ScenarioContainerViewModel : BaseContainerViewModel
    {
        private readonly InvalidateScenarioEventArgs _invalidateScenarioEventArgs;
       // private ImmutableArray<LogicalViewModel> _logicalRoot;
        private IDataUpdater _updater;
        private ImmutableArray<BaseEntity> _entities;
        private BaseEntity _currentEntity;
        private ImmutableArray<SatelliteTask> _tasks;
        private SatelliteTask _currentTask;
       // private LogicalViewModel _currentLogical;
        private ISceneState _sceneState;
        private TimePresenter _timePresenter;
        private double _width;
        private double _height;
        private ImmutableArray<FrameViewModel> _frameRoot;
        private FrameViewModel _currentFrame;

        public event InvalidateScenarioEventHandler InvalidateScenarioHandler;

        public ScenarioContainerViewModel()
        {
            _invalidateScenarioEventArgs = new InvalidateScenarioEventArgs();
            
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Width) || e.PropertyName == nameof(Height))
                {
                    if (SceneState.Camera is IArcballCamera arcballCamera)
                    {
                        arcballCamera.Resize((int)Width, (int)Height);
                    }
                }

                if (e.PropertyName == nameof(Tasks))
                {
                    AddTasks(((ScenarioContainerViewModel)s).Tasks);
                }

                if (e.PropertyName == nameof(CurrentTask))
                {
                    AddCurrentTask(((ScenarioContainerViewModel)s).CurrentTask);
                }
            };
        }

        public void InvalidateScenario() => InvalidateScenarioHandler?.Invoke(this, _invalidateScenarioEventArgs);

        //public ImmutableArray<LogicalViewModel> LogicalRoot
        //{
        //    get => _logicalRoot;
        //    set => RaiseAndSetIfChanged(ref _logicalRoot, value);
        //}

        //public LogicalViewModel CurrentLogical
        //{
        //    get => _currentLogical;
        //    set => RaiseAndSetIfChanged(ref _currentLogical, value);
        //}

        public ImmutableArray<FrameViewModel> FrameRoot
        {
            get => _frameRoot;
            set => RaiseAndSetIfChanged(ref _frameRoot, value);
        }

        public FrameViewModel CurrentFrame
        {
            get => _currentFrame;
            set => RaiseAndSetIfChanged(ref _currentFrame, value);
        }

        public IDataUpdater Updater
        {
            get => _updater;
            set => RaiseAndSetIfChanged(ref _updater, value);
        }

        public ImmutableArray<BaseEntity> Entities
        {
            get => _entities;
            set => RaiseAndSetIfChanged(ref _entities, value);
        }

        public ImmutableArray<SatelliteTask> Tasks
        {
            get => _tasks;
            set => RaiseAndSetIfChanged(ref _tasks, value);
        }

        public SatelliteTask CurrentTask
        {
            get => _currentTask;
            set => RaiseAndSetIfChanged(ref _currentTask, value);
        }

        public BaseEntity CurrentEntity
        {
            get => _currentEntity;
            set => RaiseAndSetIfChanged(ref _currentEntity, value);
        }

        public ISceneState SceneState
        {
            get => _sceneState;
            set => RaiseAndSetIfChanged(ref _sceneState, value);
        }

        public double Width
        {
            get => _width;
            set => RaiseAndSetIfChanged(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => RaiseAndSetIfChanged(ref _height, value);
        }

        public TimePresenter TimePresenter
        {
            get => _timePresenter;
            set => RaiseAndSetIfChanged(ref _timePresenter, value);
        }

        public void SetCameraTo(ITargetable target)
        {
            var behaviours = SceneState.CameraBehaviours;
            var targetType = target.GetType();

            if (behaviours.ContainsKey(targetType))
            {
                // save behaviour for current target type
                var (_, func) = behaviours[SceneState.Target.GetType()];
                behaviours[SceneState.Target.GetType()] = (SceneState.Camera.Eye, func);

                var newBehaviour = behaviours[targetType];
                SceneState.Camera.LookAt(newBehaviour.eye, dvec3.Zero, dvec3.UnitY);
                SceneState.Target = target;
            }
        }

        public void LogicalUpdate()
        {
            //if (TimePresenter.Timer.IsRunning == true)
            {
                Updater.Update(TimePresenter.Timer.CurrentTime, FrameRoot.Single());
            }
        }

        private void AddCurrentTask(SatelliteTask currentTask)
        {
            SetCameraTo(currentTask.Satellite);
        }

        private void AddTasks(ImmutableArray<SatelliteTask> tasks)
        {
            foreach (var task in tasks)
            {
                task.PropertyChanged += Task_PropertyChanged;
            }

            void Task_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(SatelliteTask.IsVisible))
                {
                    var task = sender as SatelliteTask;
                    if (task.IsVisible == true)
                    {
                        foreach (var item in Tasks)
                        {
                            if (item != task)
                            {
                                item.IsVisible = false;
                            }
                        }

                        CurrentTask = task;
                    }
                }

                if (e.PropertyName == nameof(SatelliteTask.SelectedEvent))
                {
                    var task = sender as SatelliteTask;

                    if (TimePresenter.Timer.IsRunning == true)
                    {
                        TimePresenter.OnPause();
                    }

                    if (task.SelectedEvent != null)
                    {
                        var time = task.SelectedEvent.Epoch.AddSeconds(task.SelectedEvent.BeginTime);//task.SelectedEvent.Begin;
                        var begin = TimePresenter.Begin;

                        TimePresenter.Update((time - begin).TotalSeconds);
                    }
                }

            }
        }


        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            foreach (var scObj in Entities)
            {
                isDirty |= scObj.IsDirty();
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            foreach (var scObj in Entities)
            {
                scObj.Invalidate();
            }
        }

        public override IDisposable Subscribe(IObserver<(object sender, PropertyChangedEventArgs e)> observer)
        {
            var mainDisposable = new CompositeDisposable();
            var disposablePropertyChanged = default(IDisposable);
            var disposableTimePresenter = default(IDisposable);
            var disposableShapes = default(CompositeDisposable);

            ObserveSelf(Handler, ref disposablePropertyChanged, mainDisposable);
            ObserveObject(_timePresenter, ref disposableTimePresenter, mainDisposable, observer);
            ObserveList(_entities, ref disposableShapes, mainDisposable, observer);

            void Handler(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(TimePresenter))
                {
                    ObserveObject(_timePresenter, ref disposableTimePresenter, mainDisposable, observer);
                }

                if (e.PropertyName == nameof(Entities))
                {
                    ObserveList(_entities, ref disposableShapes, mainDisposable, observer);
                }

                observer.OnNext((sender, e));
            }

            return mainDisposable;
        }
    }
}
