#nullable disable
using System.Linq;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.ViewModels.Entities
{
    public class Orbit : BaseEntity, IDrawable
    {
        private OrbitRenderModel _renderModel;       
        private FrameViewModel _frame;

        public FrameViewModel Frame
        {
            get => _frame;
            set => RaiseAndSetIfChanged(ref _frame, value);
        }

        public Orbit()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(RenderModel) || e.PropertyName == nameof(Frame))
                {
                    if (RenderModel is not null && Frame is not null && Frame.State is not null && Frame.State is OrbitState state)
                    {
                        RenderModel.Vertices = state.Vertices.Select(s => new dvec3(s.x, s.y, s.z)).ToList();
                    }
                }
            };
        }

        public OrbitRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                renderer.DrawOrbit(dc, RenderModel, Frame.State.ModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
