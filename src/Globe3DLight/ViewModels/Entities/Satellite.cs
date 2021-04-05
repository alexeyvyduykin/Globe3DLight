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
        private BaseState _logical;

        public RenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public BaseState Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (_logical?.Owner is IFrameable frameable)
                {
                    return frameable.ModelMatrix.Inverse;
                }

                return dmat4.Identity.Inverse;
            }
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                renderer.DrawFrame(dc, RenderModel.Frame, Logical.AbsoluteModelMatrix, scene);
                renderer.DrawSatellite(dc, RenderModel, Logical.AbsoluteModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
