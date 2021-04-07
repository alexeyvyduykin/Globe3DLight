#nullable disable
using GlmSharp;

namespace Globe3DLight.ViewModels.Data
{
    public class BaseState : ViewModelBase //: LogicalViewModel
    {
        private dmat4 _modelMatrix;
        private BaseState _parent;

        public dmat4 ModelMatrix
        {
            get => _modelMatrix;
            protected set => RaiseAndSetIfChanged(ref _modelMatrix, value);
        }

        public BaseState Parent
        {
            get => _parent;
            set => RaiseAndSetIfChanged(ref _parent, value);
        }

        public dmat4 AbsoluteModelMatrix => GetAbsoluteModelMatrix();

        protected dmat4 GetAbsoluteModelMatrix()
        {
            var state = Parent;
            var modelMatrix = _modelMatrix;

            while (state is not null)
            {
                modelMatrix = state.ModelMatrix * modelMatrix;

                state = state.Parent;
            }

            return modelMatrix;
        }
    }
}
