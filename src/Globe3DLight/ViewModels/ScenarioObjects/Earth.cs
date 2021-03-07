using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;

namespace Globe3DLight.ScenarioObjects
{
    public class Earth : BaseScenarioObject, IEarth, IDrawable
    {   
        private IEarthRenderModel _renderModel;
        private IFrameRenderModel _frameRenderModel;
        private ILogicalTreeNode _logicalTreeNode;

        public IEarthRenderModel RenderModel 
        {
            get => _renderModel; 
            set => Update(ref _renderModel, value); 
        }

        public IFrameRenderModel FrameRenderModel
        {
            get => _frameRenderModel;
            set => Update(ref _frameRenderModel, value);
        }

        public ILogicalTreeNode LogicalTreeNode
        {
            get => _logicalTreeNode; 
            set => Update(ref _logicalTreeNode, value); 
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
                if (LogicalTreeNode.State is IJ2000State j2000Data)
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
