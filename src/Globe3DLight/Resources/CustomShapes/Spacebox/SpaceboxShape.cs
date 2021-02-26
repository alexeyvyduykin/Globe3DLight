using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Scene;
using System.Collections.Immutable;
using Globe3DLight.Geometry;
using System.Linq;

namespace Globe3DLight.Resources
{
//    public class SpaceboxShape : ISceneShape
//    {
//        private readonly IShaderProgram _shaderProgram;
//        private readonly IRenderState _renderState = new RenderState();
//        private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();


//        private float width = 25000.0f;//200000.0f;
            
//        // DrawState
//        public IShaderProgram ShaderProgram => _shaderProgram;
//        public IRenderState RenderState => _renderState;
//        public IList<IVertexArray> VertexArray => _vertexArray;

//        public IList<IMesh> Meshes { get; set; }

//        public int IdCubemapText { get; set; }

//        public SpaceboxShape(IDevice device)            
//        {
//            this._shaderProgram = device.CreateShaderProgram();

//            _shaderProgram.VertexShaderSource =
//                EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Spacebox.spaceboxVS.glsl");
//            _shaderProgram.FragmentShaderSource =
//                EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Spacebox.spaceboxFS.glsl");            
//        }

//        private bool dirty = true;
//        private void Clean(IRenderContext context)
//        {
//            if (dirty)
//            {
//                VertexArray.Clear();
//                VertexArray.Add(context.CreateVertexArray_NEW(Meshes.SingleOrDefault(), ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw));

//                RenderState.FacetCulling = new FacetCulling() 
//                {                        
//                    Face = CullFaceMode.Front,                
//                    FrontFaceWindingOrder = Meshes.SingleOrDefault().FrontFaceWindingOrder,                       
//                };
               
//                RenderState.DepthMask = false;

//                dirty = false;
//            }
//        }


//        public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
//        {

//            Clean(context);

//            ShaderProgram.Bind();

//            ShaderProgram.SetUniform("u_mvp", scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * mat4.Identity);

//            /*
//GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
//GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
//GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
// */

//            // skybox cube      
//            ShaderProgram.SetUniform("u_spacebox", 0);
//            context.TEMP__ActiveTextureForSpacebox(IdCubemapText);

//            context.Draw(PrimitiveType.Triangles, _renderState, _vertexArray.SingleOrDefault(), _shaderProgram, scene);

//            ShaderProgram.UnBind();
//        }




//    }
}
