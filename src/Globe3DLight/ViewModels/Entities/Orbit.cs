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
        private BaseState _logical;

        public Orbit()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(RenderModel) || e.PropertyName == nameof(Logical))
                {
                    if (RenderModel is not null && Logical is not null && Logical is OrbitState state)
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

        public BaseState Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                renderer.DrawOrbit(dc, RenderModel, Logical.ModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
