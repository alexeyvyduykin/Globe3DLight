using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;

namespace Globe3DLight.ScenarioObjects
{
    public class Retranslator : BaseScenarioObject, IRetranslator
    {       
        private IRetranslatorRenderModel _renderModel;
        private ILogicalTreeNode _logicalTreeNode;

        public IRetranslatorRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode;
            set => Update(ref _logicalTreeNode, value);
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (LogicalTreeNode.State is IRetranslatorState retranslatorData)
                {
                    var m = retranslatorData.ModelMatrix;

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
