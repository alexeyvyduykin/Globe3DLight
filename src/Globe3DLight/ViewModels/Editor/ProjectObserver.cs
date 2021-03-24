using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using System.ComponentModel;
using Globe3DLight.ViewModels.Entities;
using System.Linq;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Time;

namespace Globe3DLight.ViewModels.Editor
{
    public class ProjectObserver : IDisposable
    {
        private readonly ProjectEditorViewModel _editor;
        //private readonly Action _invalidateContainer;    
        private readonly Action _invalidateScenario;
        private readonly Action _invalidateShapes;
        private readonly Action _invalidateCamera;

        public ProjectObserver(ProjectEditorViewModel editor)
        {
            if (editor?.Project != null)
            {
                _editor = editor;

                //_invalidateContainer = () => { };            
                _invalidateScenario = () => Invalidate();
                _invalidateShapes = () => Invalidate();
                _invalidateCamera = () => InvalidateCamera();

                Add(_editor.Project);
            }
        }
        private void Invalidate()
        {
            if (_editor?.Project?.CurrentScenario != null)
            {
                _editor.Project.CurrentScenario.InvalidateScenario();
            }
        }
        
        private void InvalidateCamera()
        {
            if (_editor?.Project?.CurrentScenario != null)
            {
                if (_editor.Project.CurrentScenario.SceneState.Camera is IArcballCamera arcballCamera)
                {
                    var w = _editor.Project.CurrentScenario.Width;
                    var h = _editor.Project.CurrentScenario.Height;

                    arcballCamera.Resize((int)w, (int)h);
                }
            }
        }

        private void MarkAsDirty()
        {
            if (_editor != null)
            {
               // _editor.IsProjectDirty = true;
            }
        }

        private void ObserveProject(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProjectContainerViewModel.Scenarios))
            {
                var project = sender as ProjectContainerViewModel;
                Remove(project.Scenarios);
                Add(project.Scenarios);
            }

            _invalidateShapes();
            MarkAsDirty();
        }
        private void ObserveInvalidateScenario(object sender, InvalidateScenarioEventArgs e)
        {
            _editor?.CanvasPlatform?.InvalidateControl?.Invoke();
        }

        private void ObserveScenario(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ScenarioContainerViewModel.Entities))
            {
                var layer = sender as ScenarioContainerViewModel;
                Remove(layer.Entities);
                Add(layer.Entities);
            }

            if(e.PropertyName == nameof(ScenarioContainerViewModel.TimePresenter))
            {
                var timePresenter = sender as TimePresenter;
                Remove(timePresenter);
                Add(timePresenter);
            }

            _invalidateScenario();
            MarkAsDirty();
        }

        private void ObserveShape(object sender, PropertyChangedEventArgs e)
        {
            _invalidateShapes();
            MarkAsDirty();
        }

        //private void SceneTimer_OnUpdate(double t)
        //{
        //    var root = _editor?.Project?.CurrentScenario.LogicalTreeNodeRoot.SingleOrDefault();

        //    if(root != null)
        //    {
        //        _editor.Updater.Update(t, root);
        //    }
        //}

        private void Add(ProjectContainerViewModel project)
        {
            if (project == null)
            {
                return;
            }

            project.PropertyChanged += ObserveProject;

            if (project.Scenarios != null)
            {            
                foreach (var scenario in project.Scenarios)
                {
                    Add(scenario);
                }
            }
        }

        //private void Remove(IProjectContainer project)
        //{
        //    if (project == null)
        //    {
        //        return;
        //    }

        //    project.PropertyChanged -= ObserveProject;


        //    if (project.Scenarios != null)
        //    {
        //        foreach (var scenario in project.Scenarios)
        //        {
        //            Remove(scenario);
        //        }
        //    }
        //}

        private void Add(ScenarioContainerViewModel scenario)
        {
            if (scenario == null)
            {
                return;
            }

            scenario.PropertyChanged += ObserveScenario;

            scenario.PropertyChanged += ObserveCamera;

            Add(scenario.SceneState);

            Add(scenario.TimePresenter);

            if (scenario.Entities != null)
            {
                Add(scenario.Entities);
            }

            scenario.InvalidateScenarioHandler += ObserveInvalidateScenario;
        }



        private void Remove(ScenarioContainerViewModel scenario)
        {
            if (scenario == null)
            {
                return;
            }

            scenario.PropertyChanged -= ObserveScenario;

            scenario.PropertyChanged -= ObserveCamera;
           
            Remove(scenario.SceneState);

            Remove(scenario.TimePresenter);

            if (scenario.Entities != null)
            {
                Remove(scenario.Entities);
            }

            scenario.InvalidateScenarioHandler -= ObserveInvalidateScenario;
        }

        private void Add(ISceneState sceneState)
        {
            if(sceneState == null)
            {
                return;
            }

            //sceneState.PropertyChanged += ObserveSceneState;

            //Add(sceneState.Camera);

        }

        private void Remove(ISceneState sceneState)
        {
            if (sceneState == null)
            {
                return;
            }

            //sceneState.PropertyChanged -= ObserveSceneState;

            //Remove(sceneState.Camera);

        }

        private void Add(TimePresenter timePresenter)
        {
            if(timePresenter == null)
            {
                return;
            }
            
            //timePresenter.OnUpdate += SceneTimer_OnUpdate;

            timePresenter.PropertyChanged += ObserveTimePresenter;
        }

        private void Remove(TimePresenter timePresenter)
        {
            if (timePresenter == null)
            {
                return;
            }

            //timePresenter.OnUpdate -= SceneTimer_OnUpdate;

            timePresenter.PropertyChanged -= ObserveTimePresenter;
        }

        private void Add(BaseEntity shape)
        {
            if (shape == null)
            {
                return;
            }

            shape.PropertyChanged += ObserveShape;

        }

        private void Remove(BaseEntity shape)
        {
            if (shape == null)
            {
                return;
            }

            shape.PropertyChanged -= ObserveShape;

        }






        private void Add(IEnumerable<ScenarioContainerViewModel> scenarios)
        {
            if (scenarios == null)
            {
                return;
            }

            foreach (var scenario in scenarios)
            {
                Add(scenario);
            }
        }

        private void Remove(IEnumerable<ScenarioContainerViewModel> scenarios)
        {
            if (scenarios == null)
            {
                return;
            }

            foreach (var scenario in scenarios)
            {
                Remove(scenario);
            }
        }

        private void Add(IEnumerable<BaseEntity> shapes)
        {
            if (shapes == null)
            {
                return;
            }

            foreach (var shape in shapes)
            {
                Add(shape);
            }
        }

        private void Remove(IEnumerable<BaseEntity> shapes)
        {
            if (shapes == null)
            {
                return;
            }

            foreach (var shape in shapes)
            {
                Remove(shape);
            }
        }

        private void ObserveCamera(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ScenarioContainerViewModel.Width)
                || e.PropertyName == nameof(ScenarioContainerViewModel.Height))
            {
                _invalidateCamera();
                MarkAsDirty();
            }
        }

        private void ObserveTimePresenter(object sender, PropertyChangedEventArgs e)
        {
          //  if (e.PropertyName == nameof(ITimePresenter.CurrentTime))
          //  {       
          //      if(sender is ITimePresenter timePresenter)
          //      {
                    //timePresenter.Invalidate();
                    //timePresenter.Notify();
                 //   Remove(timePresenter);
                 //   Add(timePresenter);
          //      }
          //  }
        }

        private void ObserveSceneState(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ISceneState.Camera))
            {
                _invalidateCamera();
                MarkAsDirty();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
