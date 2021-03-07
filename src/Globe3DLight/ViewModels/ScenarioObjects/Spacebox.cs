using Globe3DLight.Containers;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Data;

namespace Globe3DLight.ScenarioObjects
{
    public class Spacebox : BaseScenarioObject, ISpacebox
    {
        private ISpaceboxRenderModel _renderModel;
        private ILogicalTreeNode _logicalTreeNode;

        public ISpaceboxRenderModel RenderModel
        {
            get => _renderModel; 
            set => Update(ref _renderModel, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode;
            set => Update(ref _logicalTreeNode, value);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (LogicalTreeNode.State is IFrameState frameData)
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
