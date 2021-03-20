using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;

namespace Globe3DLight.Entities
{
    public class Satellite : BaseEntity, ISatellite
    {
        private ISatelliteRenderModel _renderModel;
        private IFrameRenderModel _frameRenderModel;
        private ILogical _logical;
    
        public ISatelliteRenderModel RenderModel 
        {
            get => _renderModel; 
            set => Update(ref _renderModel, value);
        }

        public ILogical Logical
        {
            get => _logical; 
            set => Update(ref _logical, value);
        }
        public IFrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel; 
            set => Update(ref _frameRenderModel, value); 
        }



        public dmat4 InverseAbsoluteModel 
        {
            get
            {
                if(((ILogical)_logical?.Owner).State is IFrameable frameable)
                {
                    return frameable.ModelMatrix.Inverse;
                }

                return dmat4.Identity.Inverse;
            }
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IRotationState rotationData)
                {
                    var parent = (ILogical)Logical.Owner;
                    if (parent.State is ISatelliteState satelliteState)
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
