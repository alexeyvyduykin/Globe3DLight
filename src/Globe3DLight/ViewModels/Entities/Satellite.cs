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
    public class Satellite : BaseEntity, IDrawable, ITargetable, IChildren
    {
        private SatelliteRenderModel _renderModel;
        private FrameRenderModel _frameRenderModel;
        private BaseState _logical;
    
        public SatelliteRenderModel RenderModel 
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

        public dmat4 InverseAbsoluteModel 
        {
            get
            {
                if(_logical?.Owner is IFrameable frameable)
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
                renderer.DrawFrame(dc, FrameRenderModel, Logical.AbsoluteModelMatrix, scene);
                renderer.DrawSatellite(dc, RenderModel, Logical.AbsoluteModelMatrix, scene);
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
