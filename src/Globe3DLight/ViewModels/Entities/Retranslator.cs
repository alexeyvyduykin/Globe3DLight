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
    public class Retranslator : BaseEntity, IRetranslator
    {       
        private IRetranslatorRenderModel _renderModel;
        private ILogical _logical;

        public IRetranslatorRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public ILogical Logical
        {
            get => _logical;
            set => Update(ref _logical, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (Logical.State is IRetranslatorState retranslatorData)
                {
                    //var m = retranslatorData.ModelMatrix;

                    renderer.DrawRetranslator(dc, RenderModel, retranslatorData.ModelMatrix, scene);
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
