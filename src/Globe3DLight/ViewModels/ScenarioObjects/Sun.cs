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
    public class Sun : BaseScenarioObject, ISun, IDrawable
    {
        private ISunRenderModel _renderModel;
        private bool _isVisible;
        private ILogicalTreeNode _logicalTreeNode;
        private IDataProvider _provider;


        public ISunRenderModel RenderModel 
        {
            get => _renderModel; 
            set => Update(ref _renderModel, value);
        }
        public bool IsVisible 
        {
            get => _isVisible;
            set => Update(ref _isVisible, value); 
        }      
        public IDataProvider Provider 
        {
            get => _provider; 
            set => Update(ref _provider, value);
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
                if (LogicalTreeNode.State is ISunState sunData)
                {
                    double r = sunData.Position.Length;
                    var orbitRadius = r;// * scene.WorldScale;
                    
                    scene.LightPosition = new dvec4(glm.Normalized(sunData.Position) * /*50000000.0*/orbitRadius/1000.0, 1.0);

                    //var sunPos = glm.Normalized(sunData.Position) * 160000.0;
                    var modelMatrix = dmat4.Translate(/*sunPos*/sunData.Position);

                    renderer.DrawSun(dc, RenderModel, modelMatrix, scene);
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
