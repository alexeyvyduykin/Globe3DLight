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

namespace Globe3DLight.ScenarioObjects
{
    public class GroundObject : BaseScenarioObject, IGroundObject
    {
        private IGroundObjectRenderModel _renderModel;
        private ILogicalTreeNode _logicalTreeNode;

        public IGroundObjectRenderModel RenderModel
        {
            get => _renderModel;
            set => Update(ref _renderModel, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode;
            set => Update(ref _logicalTreeNode, value);
        }

        public void DrawShapeCollection(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (LogicalTreeNode.State is IGroundObjectState groundObjectState)
                {
                    var parent = (ILogicalTreeNode)LogicalTreeNode.Owner;
                    if (parent.State is IJ2000State j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var states = parent.Children.Where(s => s.State is IGroundObjectState).Select(s => s.State).Cast<IGroundObjectState>();

                        var matrices = states.Select(s => m * s.ModelMatrix);

                        renderer.DrawGroundObjects(dc, RenderModel, matrices, scene);
                    }
                }
            }
        }

        
    //            if (LogicalTreeNode.State is IGroundObjectListState groundObjectListState)
    //            {
    //                var parent = (ILogicalTreeNode)LogicalTreeNode.Owner;
    //                if (parent.State is IJ2000State j2000Data)
    //                {
    //                    var m = j2000Data.ModelMatrix;

    //                    var matrices = groundObjectListState.States.Values.Select(s => m * s.ModelMatrix);
                 
    //                    renderer.DrawGroundObjectList(dc, RenderModel, matrices, scene);
    //                }
    //            }

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
