using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;

namespace Globe3DLight.Entities
{
    public class GroundObject : BaseEntity, IDrawable, ITargetable
    {
        private GroundObjectRenderModel _renderModel; 
        private FrameRenderModel _frameRenderModel;
        private Logical _logical;

        public GroundObjectRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public FrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => Update(ref _frameRenderModel, value);
        }

        public Logical Logical
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
                    if (Logical.State is IGroundObjectState groundObjectState)
                    {
                        var collection = Logical.Owner;
                        var parent = (Logical)collection.Owner;
                        if (parent.State is IJ2000State j2000Data)
                        {
                            var modelMatrix = j2000Data.ModelMatrix * groundObjectState.ModelMatrix;
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
                if (Logical.State is IGroundObjectState groundObjectState)
                {
                    var collection = Logical.Owner;
                    var parent = (Logical)collection.Owner;
                    if (parent.State is IJ2000State j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var matrix = m * groundObjectState.ModelMatrix;

                        renderer.DrawFrame(dc, FrameRenderModel, matrix, scene);

                        renderer.DrawGroundObject(dc, RenderModel, matrix, scene);
                    }
                }
            }
        }
   
        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
