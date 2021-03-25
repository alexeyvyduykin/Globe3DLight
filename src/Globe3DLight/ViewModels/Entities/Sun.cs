using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Models.Renderer;
using GlmSharp;
using Globe3DLight.Models.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Entities
{
    public class Sun : BaseEntity, IDrawable
    {
        private SunRenderModel _renderModel;
        private LogicalViewModel _logical;

        public SunRenderModel RenderModel 
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
                if (Logical is SunAnimator sunData)
                {
                    double r = sunData.Position.Length;
                    var orbitRadius = r;// * scene.WorldScale;
                    
                    scene.LightPosition = new dvec4(glm.Normalized(sunData.Position) * /*50000000.0*/orbitRadius/1000.0, 1.0);

                    //var sunPos = glm.Normalized(sunData.Position) * 160000.0;
                    var modelMatrix = dmat4.Translate(/*sunPos*/sunData.Position);

                    renderer.DrawSun(dc, RenderModel, modelMatrix, scene);
                }
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
