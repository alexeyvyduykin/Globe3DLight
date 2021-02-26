using System;
using System.Collections.Generic;
using Globe3DLight;
using Globe3DLight.Input;
using Globe3DLight.Scene;

namespace Globe3DLight.Editor.Tools
{
    public class ToolDefault : ObservableObject, IEditorTool
    {
        public enum State { None, Zoom, Rotate }
        private readonly IServiceProvider _serviceProvider;
        private State _currentState = State.None;

        private (double x, double y) _lastPoint;
      
        public ToolDefault(IServiceProvider serviceProvider) : base()
        {
            _serviceProvider = serviceProvider;         
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void LeftDown(InputArgs args)
        {
            var editor = _serviceProvider.GetService<IProjectEditor>();

            var camera = (IArcballCamera)editor.Project.CurrentScenario.SceneState.Camera;

            camera.RotateBegin((int)args.X, (int)args.Y);
         
            _currentState = State.Rotate;

            _lastPoint = (args.X, args.Y);       
        }

        /// <inheritdoc/>
        public void LeftUp(InputArgs args)
        {       
            _currentState = State.None;
        }

        /// <inheritdoc/>
        public void RightDown(InputArgs args)
        {
            _currentState = State.Zoom;

            _lastPoint = (args.X, args.Y);
        }

        /// <inheritdoc/>
        public void RightUp(InputArgs args)
        {
            _currentState = State.None;
        }

        /// <inheritdoc/>
        public void Move(InputArgs args)
        {
            var editor = _serviceProvider.GetService<IProjectEditor>();


            if (_currentState == State.Rotate)
            {
                var camera = (IArcballCamera)editor.Project.CurrentScenario.SceneState.Camera;

                camera.RotateEnd((int)args.X, (int)args.Y);
            }
            else if (_currentState == State.Zoom)
            {
                var camera = (IArcballCamera)editor.Project.CurrentScenario.SceneState.Camera;

                double value = (double)(args.Y - _lastPoint.y);
                value /= 15.0;// 0.01;// 15.0;
            
                double c = (value != 0.0) ? value / Math.Abs(value) : 0.0;

                value = 1000/* 2000.0 */* c;

                camera.Zoom(value);
            }

            _lastPoint = (args.X, args.Y);            
        }
    }

}
