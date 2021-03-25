using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using GlmSharp;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class Orbit : BaseEntity, IDrawable
    {
        private OrbitRenderModel _renderModel;    
        private LogicalViewModel _logical;
     
        public OrbitRenderModel RenderModel
        {
            get => _renderModel;
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public LogicalViewModel Logical
        {
            get => _logical;
            set => RaiseAndSetIfChanged(ref _logical, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical is OrbitState orbitState)
                {                    
                    RenderModel.Vertices = orbitState.Vertices.Select(s => new dvec3(s.x, s.y, s.z)).ToList();

                    renderer.DrawOrbit(dc, RenderModel, dmat4.Identity, scene);
                }
            }               
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
