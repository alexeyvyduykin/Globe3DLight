using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Entities
{
    public class Earth : BaseEntity, IDrawable, ITargetable
    {   
        private EarthRenderModel _renderModel;
        private FrameRenderModel _frameRenderModel;
        private Logical _logical;

        public EarthRenderModel RenderModel 
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

        public dmat4 InverseAbsoluteModel => dmat4.Identity.Inverse;

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IJ2000State j2000Data)
                {
                    renderer.DrawFrame(dc, FrameRenderModel, j2000Data.ModelMatrix, scene);

                    renderer.DrawEarth(dc, RenderModel, j2000Data.ModelMatrix, scene);
                }
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
