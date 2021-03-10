using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Renderer;
using GlmSharp;

namespace Globe3DLight.ScenarioObjects
{
    public class GroundStation : BaseScenarioObject, IGroundStation
    { 
        private IGroundStationRenderModel _renderModel;
        private ILogicalTreeNode _logicalTreeNode;
      
        public IGroundStationRenderModel RenderModel
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
                if (LogicalTreeNode.State is IGroundStationState groundStationData)
                {
                    var parent = (ILogicalTreeNode)LogicalTreeNode.Owner;
                    if (parent.State is IJ2000State j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var groundStationModelMatrix = m * groundStationData.ModelMatrix;

                        renderer.DrawGroundStations(dc, RenderModel, Enumerable.Repeat(groundStationModelMatrix, 1), scene);
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
