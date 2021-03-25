using Globe3DLight.ViewModels.Containers;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Entities
{
    public class Earth : BaseEntity, IDrawable, ITargetable
    {   
        private EarthRenderModel _renderModel;
        private FrameRenderModel _frameRenderModel;
        private BaseState _logical;

        public EarthRenderModel RenderModel 
        {
            get => _renderModel; 
            set => RaiseAndSetIfChanged(ref _renderModel, value); 
        }

        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => RaiseAndSetIfChanged(ref _frameRenderModel, value);
        }

        public BaseState Logical
        {
            get => _logical; 
            set => RaiseAndSetIfChanged(ref _logical, value); 
        }

        public dmat4 InverseAbsoluteModel => dmat4.Identity.Inverse;

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                renderer.DrawFrame(dc, FrameRenderModel, Logical.ModelMatrix, scene);

                renderer.DrawEarth(dc, RenderModel, Logical.ModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
