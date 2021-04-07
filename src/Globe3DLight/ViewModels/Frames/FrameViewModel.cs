#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class FrameViewModel : BaseContainerViewModel, IDrawable
    {
        private FrameViewModel _parent;
        private ImmutableArray<FrameViewModel> _children;
        private BaseState _state;
        private FrameRenderModel _renderModel;

        public FrameViewModel Parent
        {
            get => _parent;
            set => RaiseAndSetIfChanged(ref _parent, value);
        }

        public ImmutableArray<FrameViewModel> Children
        {
            get => _children;
            set => RaiseAndSetIfChanged(ref _children, value);
        }

        public BaseState State
        {
            get => _state;
            set => RaiseAndSetIfChanged(ref _state, value);
        }

        public FrameRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true && State is not null)
            {
                renderer.DrawFrame(dc, RenderModel, State.AbsoluteModelMatrix, scene);            
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;            
        }
    }
}
