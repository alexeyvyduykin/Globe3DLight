#nullable disable
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class Spacebox : BaseEntity, IDrawable
    {
        private SpaceboxRenderModel _renderModel;        
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public SpaceboxRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                renderer.DrawSpacebox(dc, RenderModel, Frame.State.ModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
