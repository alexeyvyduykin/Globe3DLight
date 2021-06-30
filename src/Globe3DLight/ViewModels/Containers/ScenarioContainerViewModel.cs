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
using Globe3DLight.ViewModels.Editors;

namespace Globe3DLight.ViewModels.Containers
{
    public class InvalidateScenarioEventArgs : EventArgs { }

    public delegate void InvalidateScenarioEventHandler(object sender, InvalidateScenarioEventArgs e);

    public enum ScenarioMode { Visual, Task };

    public partial class ScenarioContainerViewModel : BaseContainerViewModel
    {
        private readonly InvalidateScenarioEventArgs _invalidateScenarioEventArgs;
       // private ImmutableArray<LogicalViewModel> _logicalRoot;
        private IDataUpdater _updater;

        private GroundObjectList _groundObjectList;
       // private LogicalViewModel _currentLogical;
        private ISceneState _sceneState;
        private SceneTimerEditorViewModel _sceneTimerEditor;
        private TaskListEditorViewModel _taskListEditor; 
        private OutlinerEditorViewModel _outlinerEditor;
        private double _width;
        private double _height;

        private ScenarioMode _currentScenarioMode;

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
            };

            PropertyChanged += ScenarioModeChangedEvent;
        }

        private void ScenarioModeChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ScenarioContainerViewModel.CurrentScenarioMode))
            {
                if(CurrentScenarioMode == ScenarioMode.Task)
                {
                    if (TaskListEditor.CurrentTask != null)
                    {
                        SetCameraTo(TaskListEditor.CurrentTask.Satellite);
                    }
                }
            }
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

        public IDataUpdater Updater
        {
            get => _updater;
            set => RaiseAndSetIfChanged(ref _updater, value);
        }

        public GroundObjectList GroundObjectList
        {
            get => _groundObjectList;
            set => RaiseAndSetIfChanged(ref _groundObjectList, value);
        }

        public ISceneState SceneState
        {
            get => _sceneState;
            set => RaiseAndSetIfChanged(ref _sceneState, value);
        }

        public ScenarioMode CurrentScenarioMode
        {
            get => _currentScenarioMode;
            set => RaiseAndSetIfChanged(ref _currentScenarioMode, value);
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

        public SceneTimerEditorViewModel SceneTimerEditor
        {
            get => _sceneTimerEditor;
            set => RaiseAndSetIfChanged(ref _sceneTimerEditor, value);
        }

        public TaskListEditorViewModel TaskListEditor
        {
            get => _taskListEditor;
            set => RaiseAndSetIfChanged(ref _taskListEditor, value);
        }

        public OutlinerEditorViewModel OutlinerEditor
        {
            get => _outlinerEditor;
            set => RaiseAndSetIfChanged(ref _outlinerEditor, value);
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
                var root = OutlinerEditor.FrameRoot.Single();
                Updater.Update(SceneTimerEditor.Timer.CurrentTime, root);
            }
        }



        public override bool IsDirty()
        {
            var isDirty = base.IsDirty();

            foreach (var scObj in OutlinerEditor.Entities)
            {
                isDirty |= scObj.IsDirty();
            }

            return isDirty;
        }

        public override void Invalidate()
        {
            base.Invalidate();

            foreach (var scObj in OutlinerEditor.Entities)
            {
                scObj.Invalidate();
            }
        }

        public int AbsolutePositionX { get; set; }

        public int AbsolutePositionY { get; set; }

        public int ZIndex { get; set; }

        public override IDisposable Subscribe(IObserver<(object sender, PropertyChangedEventArgs e)> observer)
        {
            var mainDisposable = new CompositeDisposable();
            var disposablePropertyChanged = default(IDisposable);
            var disposableTimePresenter = default(IDisposable);
           
            ObserveSelf(Handler, ref disposablePropertyChanged, mainDisposable);
            ObserveObject(_sceneTimerEditor, ref disposableTimePresenter, mainDisposable, observer);
            OutlinerEditor.Subscribe(observer);

            void Handler(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(ScenarioContainerViewModel.SceneTimerEditor))
                {
                    ObserveObject(_sceneTimerEditor, ref disposableTimePresenter, mainDisposable, observer);
                }

                observer.OnNext((sender, e));
            }

            return mainDisposable;
        }
    }
}
