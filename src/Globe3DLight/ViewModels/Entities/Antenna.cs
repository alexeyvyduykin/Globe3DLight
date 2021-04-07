#nullable disable
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class Antenna : BaseEntity, IDrawable
    {
        private AntennaRenderModel _renderModel;
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public AntennaRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Frame.State is AntennaAnimator antennaAnimator)
                {
                    if (antennaAnimator.Enable == true)
                    {
                        RenderModel.AbsoluteTargetPostion = antennaAnimator.TargetPosition;
                        renderer.DrawAntenna(dc, RenderModel, antennaAnimator.AbsoluteModelMatrix, scene);
                    }
                }
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
