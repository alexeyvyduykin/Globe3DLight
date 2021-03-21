using Globe3DLight.Containers;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Data;

namespace Globe3DLight.Entities
{
    public class Spacebox : BaseEntity, ISpacebox
    {
        private ISpaceboxRenderModel _renderModel;
        private Logical _logical;

        public ISpaceboxRenderModel RenderModel
        {
            get => _renderModel; 
            set => Update(ref _renderModel, value);
        }

        public Logical Logical
        {
            get => _logical;
            set => Update(ref _logical, value);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IFrameState frameData)
                {
                    renderer.DrawSpacebox(dc, RenderModel, frameData.ModelMatrix, scene);
                }
            }
        }

        public bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
