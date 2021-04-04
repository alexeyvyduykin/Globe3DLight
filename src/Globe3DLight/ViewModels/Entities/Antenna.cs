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
        private FrameRenderModel _frameRenderModel;
        private BaseState _logical;

        public AntennaRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public BaseState Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => RaiseAndSetIfChanged(ref _frameRenderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical is AntennaAnimator antennaAnimator)
                {
                    var rotationState = antennaAnimator.Owner;
                    if (rotationState is RotationAnimator rotationAnimator)
                    {
                        var attach = RenderModel.AttachPosition;

                        var antennaModelMatrix = rotationAnimator.AbsoluteModelMatrix * dmat4.Translate(attach);

                        renderer.DrawFrame(dc, FrameRenderModel, antennaModelMatrix, scene);

                        if (antennaAnimator.Enable == true)
                        {
                            RenderModel.AbsoluteTargetPostion = antennaAnimator.TargetPosition;

                            renderer.DrawAntenna(dc, RenderModel, antennaModelMatrix, scene);
                        }
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
