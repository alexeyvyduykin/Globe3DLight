using System;
using System.Collections.Generic;
using Globe3DLight.Models.Editor;
using Globe3DLight.Input;
using Globe3DLight.Models.Scene;

namespace Globe3DLight.ViewModels.Editor.Tools
{
    public class ToolDefault : ViewModelBase, IEditorTool
    {
        public enum State { None, Zoom, Rotate }
        private readonly IServiceProvider _serviceProvider;
        private State _currentState = State.None;

        private (double x, double y) _lastPoint;
      
        public ToolDefault(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;         
        }

        public void LeftDown(InputArgs args)
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();

            var camera = (IArcballCamera)editor.Project.CurrentScenario.SceneState.Camera;

            camera.RotateBegin((int)args.X, (int)args.Y);
         
            _currentState = State.Rotate;

            _lastPoint = (args.X, args.Y);       
        }

        public void LeftUp(InputArgs args)
        {       
            _currentState = State.None;
        }

        public void RightDown(InputArgs args)
        {
            _currentState = State.Zoom;

            _lastPoint = (args.X, args.Y);
        }

        public void RightUp(InputArgs args)
        {
            _currentState = State.None;
        }

        public void Move(InputArgs args)
        {
            var editor = _serviceProvider.GetService<ProjectEditorViewModel>();


            if (_currentState == State.Rotate)
            {
                var camera = (IArcballCamera)editor.Project.CurrentScenario.SceneState.Camera;

                camera.RotateEnd((int)args.X, (int)args.Y);
            }
            else if (_currentState == State.Zoom)
            {
                var sceneState = editor.Project.CurrentScenario.SceneState;
                var camera = (IArcballCamera)sceneState.Camera;
                var target = sceneState.Target;
                var (_, func) = sceneState.CameraBehaviours[target.GetType()];

                double value = (double)(args.Y - _lastPoint.y);
         
                var dz = func.Invoke(camera.Eye.Length);

                camera.Zoom(Math.Sign(value) * dz);
            }

            _lastPoint = (args.X, args.Y);            
        }
    }

}
