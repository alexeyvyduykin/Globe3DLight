using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using GlmSharp;

namespace Globe3DLight.ScenarioObjects
{
    public class Orbit : BaseScenarioObject, IOrbit
    {
        private IOrbitRenderModel _renderModel;    
        private ILogicalTreeNode _logicalTreeNode;
     
        public IOrbitRenderModel RenderModel
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
                if (LogicalTreeNode.State is IOrbitState orbitState)
                {                    
                    RenderModel.Vertices = orbitState.Vertices.Select(s => new dvec3(s.x, s.y, s.z)).ToList();

                    renderer.DrawOrbit(dc, RenderModel, dmat4.Identity, scene);
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
