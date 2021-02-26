using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using System.ComponentModel;
using Globe3DLight.ScenarioObjects;
using System.Linq;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using Globe3DLight.Time;

namespace Globe3DLight.Editor
{
    public class ProjectObserver : IDisposable
    {
        private readonly IProjectEditor _editor;
        private readonly Action _invalidateContainer;    
        private readonly Action _invalidateScenario;
        private readonly Action _invalidateShapes;
        private readonly Action _invalidateCamera;

        public ProjectObserver(IProjectEditor editor)
        {
            if (editor?.Project != null)
            {
                _editor = editor;

                _invalidateContainer = () => { };            
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
                if (_editor.Project.CurrentScenario.SceneState.Camera is Scene.IArcballCamera)
                {
                    var w = _editor.Project.CurrentScenario.Width;
                    var h = _editor.Project.CurrentScenario.Height;

                    ((Scene.IArcballCamera)_editor.Project.CurrentScenario.SceneState.Camera).Resize((int)w, (int)h);
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
            if (e.PropertyName == nameof(IProjectContainer.Scenarios))
            {
                var project = sender as IProjectContainer;
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
            if (e.PropertyName == nameof(IScenarioContainer.ScenarioObjects))
            {
                var layer = sender as IScenarioContainer;
                Remove(layer.ScenarioObjects);
                Add(layer.ScenarioObjects);
            }

            if(e.PropertyName == nameof(IScenarioContainer.TimePresenter))
            {
                var timePresenter = sender as ITimePresenter;
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

        private void SceneTimer_OnUpdate(double t)
        {
            var root = _editor?.Project?.CurrentScenario.LogicalTreeNodeRoot.SingleOrDefault();

            if(root != null)
            {
                UpdateData(t, root);
            }
        }

        private void UpdateData(double t, ILogicalTreeNode node)
        {
            if(node.Data is IAnimator animator)
            {
                animator.Animate(t);
            }

            foreach (var item in node.Children)
            {
                UpdateData(t, item);
            }
        }

        private void Add(IProjectContainer project)
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

        private void Remove(IProjectContainer project)
        {
            if (project == null)
            {
                return;
            }

            project.PropertyChanged -= ObserveProject;


            if (project.Scenarios != null)
            {
                foreach (var scenario in project.Scenarios)
                {
                    Remove(scenario);
                }
            }
        }

        private void Add(IScenarioContainer scenario)
        {
            if (scenario == null)
            {
                return;
            }

            scenario.PropertyChanged += ObserveScenario;

            scenario.PropertyChanged += ObserveCamera;

            Add(scenario.SceneState);

            Add(scenario.TimePresenter);

            if (scenario.ScenarioObjects != null)
            {
                Add(scenario.ScenarioObjects);
            }

            scenario.InvalidateScenarioHandler += ObserveInvalidateScenario;
        }



        private void Remove(IScenarioContainer scenario)
        {
            if (scenario == null)
            {
                return;
            }

            scenario.PropertyChanged -= ObserveScenario;

            scenario.PropertyChanged -= ObserveCamera;
           
            Remove(scenario.SceneState);

            Remove(scenario.TimePresenter);

            if (scenario.ScenarioObjects != null)
            {
                Remove(scenario.ScenarioObjects);
            }

            scenario.InvalidateScenarioHandler -= ObserveInvalidateScenario;
        }

        private void Add(ISceneState sceneState)
        {
            if(sceneState == null)
            {
                return;
            }

            sceneState.PropertyChanged += ObserveSceneState;

            //Add(sceneState.Camera);

        }

        private void Remove(ISceneState sceneState)
        {
            if (sceneState == null)
            {
                return;
            }

            sceneState.PropertyChanged -= ObserveSceneState;

            //Remove(sceneState.Camera);

        }

        private void Add(ITimePresenter timePresenter)
        {
            if(timePresenter == null)
            {
                return;
            }
            
            timePresenter/*SceneTimer*/.OnUpdate += SceneTimer_OnUpdate;

            timePresenter.PropertyChanged += ObserveTimePresenter;
        }

        private void Remove(ITimePresenter timePresenter)
        {
            if (timePresenter == null)
            {
                return;
            }

            timePresenter/*SceneTimer*/.OnUpdate -= SceneTimer_OnUpdate;

            timePresenter.PropertyChanged -= ObserveTimePresenter;
        }

        private void Add(ICamera camera)
        {
            if(camera == null)
            {
                return;
            }

            camera.PropertyChanged += ObserveCamera;

        }

        private void Remove(ICamera camera)
        {
            if (camera == null)
            {
                return;
            }

            camera.PropertyChanged -= ObserveCamera;
        }

        private void Add(IScenarioObject shape)
        {
            if (shape == null)
            {
                return;
            }

            shape.PropertyChanged += ObserveShape;

        }

        private void Remove(IScenarioObject shape)
        {
            if (shape == null)
            {
                return;
            }

            shape.PropertyChanged -= ObserveShape;

        }






        private void Add(IEnumerable<IScenarioContainer> scenarios)
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

        private void Remove(IEnumerable<IScenarioContainer> scenarios)
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

        private void Add(IEnumerable<IScenarioObject> shapes)
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

        private void Remove(IEnumerable<IScenarioObject> shapes)
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
            if (e.PropertyName == nameof(IScenarioContainer.Width)
                || e.PropertyName == nameof(IScenarioContainer.Height))
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
            throw new NotImplementedException();
        }
    }
}
