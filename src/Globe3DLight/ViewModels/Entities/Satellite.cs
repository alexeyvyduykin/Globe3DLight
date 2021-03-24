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
        private LogicalViewModel _logical;
    
        public SatelliteRenderModel RenderModel 
        {
            get => _renderModel; 
            set => RaiseAndSetIfChanged(ref _renderModel, value);
        }

        public LogicalViewModel Logical
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
                if(((LogicalViewModel)_logical?.Owner).State is IFrameable frameable)
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
                if (Logical.State is RotationAnimator rotationData)
                {
                    var parent = (LogicalViewModel)Logical.Owner;
                    if (parent.State is SatelliteAnimator satelliteState)
                    {
                        //   double r = orbitData.Position.Length;
                        //    var orbitRadius = r * scene.WorldScale;

                        //   dmat4 translate = dmat4.Translate(glm.Normalized(orbitData.Position) * orbitRadius);

                        var orbitmodelMatrix = satelliteState.Translate * satelliteState.Rotation;//.Inverse;     

                        var satelliteModelMatrix = orbitmodelMatrix * rotationData.RotationMatrix;

                        renderer.DrawFrame(dc, FrameRenderModel, satelliteModelMatrix, scene);

                        renderer.DrawSatellite(dc, RenderModel, satelliteModelMatrix, scene);
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
