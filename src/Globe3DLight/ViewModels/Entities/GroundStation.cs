using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Renderer;
using GlmSharp;

namespace Globe3DLight.Entities
{
    public class GroundStation : BaseEntity, IGroundStation
    { 
        private IGroundStationRenderModel _renderModel; 
        private IFrameRenderModel _frameRenderModel;
        private ILogical _logical;
      
        public IGroundStationRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public IFrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => Update(ref _frameRenderModel, value);
        }

        public ILogical Logical
        {
            get => _logical;
            set => Update(ref _logical, value);
        }
        public dmat4 InverseAbsoluteModel
        {
            get
            {
                if (_logical?.State is IFrameable)
                {
                    if (Logical.State is IGroundStationState groundStationData)
                    {
                        var collection = Logical.Owner;
                        var parent = (ILogical)collection.Owner;
                        if (parent.State is IJ2000State j2000Data)
                        {                      
                            var modelMatrix = j2000Data.ModelMatrix * groundStationData.ModelMatrix;
                            return modelMatrix.Inverse;
                        }
                    }                   
                }

                return dmat4.Identity.Inverse;
            }
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IGroundStationState groundStationData)
                {
                    var collection = Logical.Owner;
                    var parent = (ILogical)collection.Owner;
                    if (parent.State is IJ2000State j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var groundStationModelMatrix = m * groundStationData.ModelMatrix;

                        renderer.DrawFrame(dc, FrameRenderModel, groundStationModelMatrix, scene);

                        renderer.DrawGroundStation(dc, RenderModel, groundStationModelMatrix, scene);
                    }
                }
            }
        }



        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
