#nullable disable
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class Satellite : BaseEntity, IDrawable, ITargetable, IChildren
    {
        private RenderModel _renderModel;        
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public RenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (Frame.Parent.State is SatelliteAnimator)
                {
                    if (Frame.Parent.State is IFrameable frameable)
                    {
                        return frameable.ModelMatrix.Inverse;
                    }
                }
                return dmat4.Identity.Inverse;
            }
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {              
                renderer.DrawSatellite(dc, RenderModel, Frame.State.AbsoluteModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
