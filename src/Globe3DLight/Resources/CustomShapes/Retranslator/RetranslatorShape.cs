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
    //public class RetranslatorShape : ISceneShape
    //{
    //    private readonly IShaderProgram _shaderProgram;

    //    private readonly IRenderState _renderState = new RenderState();
    //    private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();
       

    //    private readonly IUniform<mat4> u_mvp;
    //    private readonly IUniform<vec4> u_color;

    //    // DrawState
    //    public IShaderProgram ShaderProgram => _shaderProgram;
    //    public IRenderState RenderState => _renderState;
    //    public IList<IVertexArray> VertexArray => _vertexArray;

    //    public IList<IMesh> Meshes { get; set; }

    //    public RetranslatorShape(IDevice device)
    //    {
    //        this._shaderProgram = device.CreateShaderProgram();
            
    //        _shaderProgram.VertexShaderSource =   
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.GroundStation.meshVS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.GroundStation.meshFS.glsl");
            
    //        u_mvp = ((IUniform<mat4>)_shaderProgram.Uniforms["u_mvp"]);
    //        u_color = ((IUniform<vec4>)_shaderProgram.Uniforms["u_color"]);

    //        u_color.Value = new vec4(0.0f, 1.0f, 0.0f, 1.0f);


    //    }

    //    private bool dirty = true;
        
    //    private void Clean(IRenderContext context)
    //    {
    //        if (dirty)
    //        {
    //            VertexArray.Clear();
    //            VertexArray.Add(context.CreateVertexArray_NEW(Meshes.SingleOrDefault(), ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw));
    //            RenderState.FacetCulling = new FacetCulling()
    //            {
    //                Face = CullFaceMode.Front,               
    //                FrontFaceWindingOrder = Meshes.SingleOrDefault().FrontFaceWindingOrder,
    //            };
    //            dirty = false;
    //        }
    //    }

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        Clean(context);

    //        u_mvp.Value = scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4() * modelMatrix.ToMat4();
    //        context.Draw(PrimitiveType.Triangles, _renderState, _vertexArray.SingleOrDefault(), _shaderProgram, scene);

    //        ShaderProgram.UnBind();
    //    }


    //}
}
