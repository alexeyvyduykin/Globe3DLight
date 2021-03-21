using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Entities;
using Globe3DLight.Time;
using GlmSharp;
using Globe3DLight.Data;
using System.ComponentModel;

namespace Globe3DLight.Containers
{
    public class ScenarioContainer : ObservableObject
    {
        public event InvalidateScenarioEventHandler InvalidateScenarioHandler;

        private ImmutableArray<Logical> _logicalTreeNodeRoot;
        private IDataUpdater _updater;
        private bool _isExpanded = true;
        private ImmutableArray<BaseEntity> _entities;
        private BaseEntity _currentEntity;
        private ImmutableArray<SatelliteTask> _tasks;
        private SatelliteTask _currentTask;

        private Logical _currentLogicalTreeNode;
        private ISceneState _sceneState;
        private TimePresenter _timePresenter;

        private double _width;

        private double _height;

        public ImmutableArray<Logical> LogicalTreeNodeRoot
        {
            get => _logicalTreeNodeRoot;
            set => Update(ref _logicalTreeNodeRoot, value);
        }

        public Logical CurrentLogicalTreeNode
        {
            get => _currentLogicalTreeNode;
            set => Update(ref _currentLogicalTreeNode, value);
        }

        public IDataUpdater Updater
        {
            get => _updater;
            set => Update(ref _updater, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Update(ref _isExpanded, value);
        }

        public ImmutableArray<BaseEntity> Entities
        {
            get => _entities;
            set => Update(ref _entities, value);
        }

        public ImmutableArray<SatelliteTask> Tasks
        {
            get => _tasks;
            set
            {
                if (Update(ref _tasks, value) == true)
                {
                    AddTasks(value);
                }
            }
        }

        public SatelliteTask CurrentTask
        {
            get => _currentTask;
            set
            {
                if (Update(ref _currentTask, value) == true)
                {
                    AddCurrentTask(value);
                }
            }
        }

        public BaseEntity CurrentEntity
        {
            get => _currentEntity;
            set => Update(ref _currentEntity, value);
        }

        public ISceneState SceneState
        {
            get => _sceneState;
            set => Update(ref _sceneState, value);
        }

        public double Width
        {
            get => _width;
            set => Update(ref _width, value);
        }

        public double Height
        {
            get => _height;
            set => Update(ref _height, value);
        }

        public TimePresenter TimePresenter
        {
            get => _timePresenter;
            set => Update(ref _timePresenter, value);
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
                Updater.Update(TimePresenter.Timer.CurrentTime, LogicalTreeNodeRoot.SingleOrDefault());
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

                    if(TimePresenter.Timer.IsRunning == true)
                    {
                        TimePresenter.OnPause();
                    }

                    if (task.SelectedEvent != null)
                    {
                        var time = task.SelectedEvent.Begin;
                        var begin = TimePresenter.Begin;

                        TimePresenter.Update((time - begin).TotalSeconds);
                    }
                }
       
            }    
        }

        public void InvalidateScenario() => InvalidateScenarioHandler?.Invoke(this, new InvalidateScenarioEventArgs());

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
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

    
        public virtual bool ShouldSerializeTimePresenter() => true;
        public virtual bool ShouldSerializeSceneState() => true;
        public virtual bool ShouldSerializeLogicalTreeNodeRoot() => true;
        public virtual bool ShouldSerializeCurrentLogicalTreeNode() => _currentLogicalTreeNode != null;
        public virtual bool ShouldSerializeEntities() => true;
        public virtual bool ShouldSerializeSatelliteTasks() => true;
        public virtual bool ShouldSerializeCurrentEntity() => _currentEntity != null;
        public virtual bool ShouldSerializeWidth() => _width != default;
        public virtual bool ShouldSerializeHeight() => _height != default;
        public virtual bool ShouldSerializeIsExpanded() => _isExpanded != default;
    }
}
