using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Renderer;
using Globe3DLight.Data.Animators;
using GlmSharp;

namespace Globe3DLight.ScenarioObjects
{
    public class GroundStation : BaseScenarioObject, IGroundStation, IDrawable
    {
        private bool _isVisible;
        private IGroundStationRenderModel _renderModel;
        private UniqueName _uniqueName;

        private ILogicalTreeNode _logicalTreeNode;
      
        public IGroundStationRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }
        public bool IsVisible
        {
            get => _isVisible;
            set => Update(ref _isVisible, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode;
            set => Update(ref _logicalTreeNode, value);
        }
        public UniqueName UniqueName
        {
            get => _uniqueName;
            set => Update(ref _uniqueName, value); 
        }

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (LogicalTreeNode.Data is IGroundStationData groundStationData)
                {
                    var parent = (ILogicalTreeNode)LogicalTreeNode.Owner;
                    if (parent.Data is IJ2000Data j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var groundStationModelMatrix = m * groundStationData.ModelMatrix;

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
