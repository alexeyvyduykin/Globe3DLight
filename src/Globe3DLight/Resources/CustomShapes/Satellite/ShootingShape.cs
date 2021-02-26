using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Scene;
using System.Collections.Immutable;
using Globe3DLight.Geometry;
using System.Linq;
using Globe3DLight.Style;

namespace Globe3DLight.Resources
{
    //public class ShootingShape : ISceneShape
    //{
    //    private readonly IDevice _device;
    //    private readonly IShaderProgram _shaderProgram;

    //    private readonly IRenderState _renderState = new RenderState();
    //    private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();

    //    // DrawState
    //    public IShaderProgram ShaderProgram => _shaderProgram;
    //    public IRenderState RenderState => _renderState;
    //    public IList<IVertexArray> VertexArray => _vertexArray;

    //    public IList<IMesh> Meshes { get; set; }

    //    public ShootingShape(IDevice device)
    //    {
    //        this._device = device;
    //        this._shaderProgram = device.CreateShaderProgram();
      
    //        _shaderProgram.VertexShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.Satellite.emptyVS.glsl");
    //        _shaderProgram.GeometryShaderSource =                   
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.Satellite.shootingGS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.Satellite.emptyFS.glsl");

    //        _shaderProgram.BindAttribLocation((int)0, "POSITION");
    //    }



    //    public dmat4 OrbitalModelMatrix;
    //    public IList<dvec3> Shoot;
    //    public IList<vec3> ShootDirections;

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        context.EnableCullFace(CullFaceMode.Front, FrontFaceDirection.Cw);
    //        context.EnableBlend(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

    //        ShaderProgram.Bind();
    //        //ShaderSetup.CameraSetup(sp, scene, obj.ModelMatrix);

    //        dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * OrbitalModelMatrix;
    //        ShaderProgram.SetUniform("u_mvp", mvp.ToMat4());

    //        ShaderProgram.SetUniform("u_P1", new vec4(Shoot[0].ToVec3(), 1.0f));
    //        ShaderProgram.SetUniform("u_P2", new vec4(Shoot[1].ToVec3(), 1.0f));
    //        ShaderProgram.SetUniform("u_P3", new vec4(Shoot[2].ToVec3(), 1.0f));
    //        ShaderProgram.SetUniform("u_P4", new vec4(Shoot[3].ToVec3(), 1.0f));

    //        context.DrawPoints(new List<vec3>() { vec3.Zero });

    //        ShaderProgram.UnBind();

    //        context.DisableBlend();
    //        context.DisableCullFace();
    //        //------------------------------------------------------------------------------
    //        // render scan

    //        context.LoadProjectionMatrix(scene.ProjectionMatrix);
    //        context.LoadModelviewMatrix(scene.Camera.ViewMatrix);

    //        context.DrawLineLoop(ShootDirections, Colors.Blue, 3.0f);
    //    }
    //}
}
