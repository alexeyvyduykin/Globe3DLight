using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using Globe3DLight.Data.Animators;


namespace Globe3DLight.ScenarioObjects
{
    public class GroundObjectList : BaseScenarioObject, IGroundObjectList, IDrawable
    {
        private bool _isVisible;
        private IGroundObjectListRenderModel _renderModel;
  
        private ILogicalTreeNode _logicalTreeNode;


        public IGroundObjectListRenderModel RenderModel 
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

        public void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        {
            if (IsVisible == true)
            {
                if (LogicalTreeNode.Data is IGroundObjectListData groundObjectListData)
                {
                    var parent = (ILogicalTreeNode)LogicalTreeNode.Owner;
                    if (parent.Data is IJ2000Data j2000Data)
                    {
                        var m = j2000Data.ModelMatrix;

                        var matrices = groundObjectListData.ModelMatrices.Select(s => m * s.Value);
                 
                        renderer.DrawGroundObjectList(dc, RenderModel, matrices, scene);
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
