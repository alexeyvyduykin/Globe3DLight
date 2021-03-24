using Globe3DLight.Input;

namespace Globe3DLight.ViewModels.Editor
{
    public class ProjectEditorInputTarget : IInputTarget
    {
        private readonly ProjectEditorViewModel _editor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectEditorInputTarget"/> class.
        /// </summary>
        /// <param name="editor">The current <see cref="IProjectEditor"/> object.</param>
        public ProjectEditorInputTarget(ProjectEditorViewModel editor)
        {
            _editor = editor;
        }

        /// <inheritdoc/>
        public void LeftDown(InputArgs args) => _editor?.CurrentTool?.LeftDown(args);

        /// <inheritdoc/>
        public void LeftUp(InputArgs args) => _editor?.CurrentTool?.LeftUp(args);

        /// <inheritdoc/>
        public void RightDown(InputArgs args) => _editor?.CurrentTool?.RightDown(args);

        /// <inheritdoc/>
        public void RightUp(InputArgs args) => _editor?.CurrentTool?.RightUp(args);

        /// <inheritdoc/>
        public void Move(InputArgs args) => _editor?.CurrentTool?.Move(args);

        /// <inheritdoc/>
        public bool IsLeftDownAvailable()
        {
            return _editor?.Project?.CurrentScenario != null;
                //&& _editor.Project.CurrentScenario.IsVisible;
        }

        /// <inheritdoc/>
        public bool IsLeftUpAvailable()
        {
            return _editor?.Project?.CurrentScenario != null;
                //&& _editor.Project.CurrentScenario.IsVisible;
        }

        /// <inheritdoc/>
        public bool IsRightDownAvailable()
        {
            return _editor?.Project?.CurrentScenario != null;
                //&& _editor.Project.CurrentScenario.IsVisible;
        }

        /// <inheritdoc/>
        public bool IsRightUpAvailable()
        {
            return _editor?.Project?.CurrentScenario != null;
                //&& _editor.Project.CurrentScenario.IsVisible;
        }

        /// <inheritdoc/>
        public bool IsMoveAvailable()
        {
            return _editor.Project?.CurrentScenario != null;
                //&& _editor.Project.CurrentScenario.IsVisible;
        }
    }
}
