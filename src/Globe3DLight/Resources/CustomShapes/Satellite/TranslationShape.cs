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
    //public class TranslationShape : ISceneShape
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


    //    public vec3 PosSource { get; set; }
    //    public vec3 PosTarget { get; set; }

    //    public TranslationShape(IDevice device)
    //    {
    //        this._device = device;
    //        //this._shaderProgram = device.CreateShaderProgram();
    //    }

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        dmat4 modelView = scene.ViewMatrix;// * modelMatrix;

    //        context.LoadProjectionMatrix(scene.ProjectionMatrix);
    //        context.LoadModelviewMatrix(modelView);

    //        context.DrawLine(PosSource, PosTarget, Colors.Yellow, 3.0f);
    //    }
    //}   
    

    //public class SatelliteTranslationModelSimple : RenderModel// SatelliteTranslationModel
    //{
    //    public SatelliteTranslationModelSimple()
    //    {
    //        protoCircleSize = 36;

    //        initCircle();

    //        sp1 = new ShaderProgram(
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyVS.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.Translation.translationGS_Edit.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyFS.glsl"));

    //        va = new VertexArray();
    //        VertexBuffer vb = new VertexBuffer(BufferUsageHint.StaticDraw, ArraySizeInBytes.Size<vec3>(protoCircle.ToArray()));
    //        vb.CopyFromSystemMemory(protoCircle);
    //        va.Attributes[0] = new VertexBufferAttribute(vb, VertexAttribPointerType.Float, 3);
    //    }

    //    //public override void Render(ISatelliteData data, Context context, SceneState scene)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    public void Render(dmat4 modelMatrix, vec4 PositionTargetTS, float ScaleSource, float ScaleTarget, Context context, SceneState scene)
    //    {
    //        GL.Enable(EnableCap.CullFace);
    //        GL.CullFace(CullFaceMode.Front);
    //        GL.FrontFace(FrontFaceDirection.Ccw/*Cw*/);

    //        GL.Enable(EnableCap.Blend);
    //        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

    //        va.Bind();
    //        va.Clean();

    //        sp1.Bind();
    //        SetUnoform(modelMatrix, PositionTargetTS, ScaleSource, ScaleTarget, scene);
    //        GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop/*BeginMode.LineLoop*/, 0, protoCircleSize);

    //        GL.Disable(EnableCap.Blend);
    //        GL.Disable(EnableCap.CullFace);
    //    }

    //    private void SetUnoform(dmat4 modelMatrix, vec4 PositionTargetTS, float ScaleSource, float ScaleTarget, SceneState scene)
    //    {
    //        dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * modelMatrix;
    //        sp1.SetUniform("u_mvp", mvp.ToMat4());

    //        sp1.SetUniform("u_positionTarget", PositionTargetTS);
    //        sp1.SetUniform("u_scaleSource", ScaleSource);
    //        sp1.SetUniform("u_scaleTarget", ScaleTarget);
    //    }

    //    private void initCircle()
    //    {
    //        double curAngle, dAngle = glm.Radians(360.0 / protoCircleSize);

    //        protoCircle = new vec3[protoCircleSize];

    //        for (int i = 0; i < 36; i++)
    //        {
    //            curAngle = 2.0 * Math.PI - dAngle * i; // for clockwise direction (??? not work ???)
    //            protoCircle[i] = new vec3((float)Math.Cos(curAngle), 0.0f, (float)-Math.Sin(curAngle));
    //        }
    //    }

    //    public override void Render(dmat4 modelMatrix, Context context, SceneState scene)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public override void Render(ISceneObjectData data, Context context, SceneState scene)
    //    //{
    //    //    throw new NotImplementedException();
    //    //}

    //    private int protoCircleSize;

    //    private vec3[] protoCircle;

    //    private readonly ShaderProgram sp1;
    //    private readonly VertexArray va;
    //}

    //public class SatelliteTranslationModelDefault : SatelliteTranslationModel
    //{
    //    public SatelliteTranslationModelDefault()
    //    {
    //        protoCircleSize = 36;

    //        initCircle();

    //        sp1 = new ShaderProgram(
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyVS.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.Translation.translationGS.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyFS.glsl"));

    //        //sp2 = new ShaderProgram(
    //        //    EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyVS.glsl"),
    //        //    EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.Translation.translationRingsGS.glsl"),
    //        //    EmbeddedResources.GetText("Globe3D.Scene.Renderables.SatelliteBehaviour.Shaders.emptyFS.glsl"));

    //        va = new VertexArray();
    //        VertexBuffer vb = new VertexBuffer(BufferUsageHint.StaticDraw, ArraySizeInBytes.Size<vec3>(protoCircle.ToArray()));
    //        vb.CopyFromSystemMemory(protoCircle);
    //        va.Attributes[0] = new VertexBufferAttribute(vb, VertexAttribPointerType.Float, 3);
    //    }

    //    public override void Render(ISatelliteData data, Context context, SceneState scene)
    //    {
    //        if (data.Translation.Enable)
    //        {
    //            GL.Enable(EnableCap.CullFace);
    //            GL.CullFace(CullFaceMode.Front);
    //            GL.FrontFace(FrontFaceDirection.Cw);

    //            GL.Enable(EnableCap.Blend);
    //            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

    //            va.Bind();
    //            va.Clean();

    //            sp1.Bind();
    //            SetUnoform1(data.Translation, scene);
    //            GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop/*BeginMode.LineLoop*/, 0, protoCircleSize);

    //            GL.Disable(EnableCap.Blend);
    //            GL.Disable(EnableCap.CullFace);

    //            //GL.Enable(EnableCap.Blend);
    //            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

    //            //sp2.Bind();
    //            //SetUniform2(data.Translation, scene);
    //            //GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop /*BeginMode.LineLoop*/, 0, protoCircleSize);
    //            //ShaderProgram.UnBind();

    //            //GL.BindVertexArray(0);

    //            //deltaRingsStep += 0.002f;
    //            //if (deltaRingsStep >= 0.2f)
    //            //    deltaRingsStep = 0.0f;
    //            //GL.Disable(EnableCap.Blend);            
    //        }
    //    }

    //    private void SetUnoform1(TranslationData data, SceneState scene)
    //    {
    //        dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * data.ModelTarget;
    //        sp1.SetUniform("u_mvp", mvp.ToMat4());

    //        sp1.SetUniform("u_matrixSource", data.CircleSource_TS.ToMat4());
    //        sp1.SetUniform("u_matrixTarget", data.CircleTarget_TS.ToMat4());
    //        sp1.SetUniform("u_scaleSource", data.ScaleSource);
    //        sp1.SetUniform("u_scaleTarget", data.ScaleTarget);
    //    }

    //    private void SetUniform2(TranslationData data, SceneState scene)
    //    {
    //        //dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix;

    //        //sp2.SetUniform("u_deltaStep", deltaRingsStep);
    //        //sp2.SetUniform("u_matrixSource", data.CircleSource_TS.ToMat4());
    //        //sp2.SetUniform("u_matrixTarget", data.CircleTarget_TS.ToMat4());
    //        //sp2.SetUniform("u_scaleSource", data.ScaleSource);
    //        //sp2.SetUniform("u_scaleTarget", data.ScaleTarget);

    //        //sp2.SetUniform("u_mvp", mvp.ToMat4());
    //    }

    //    public void Render(dmat4 modelMatrixTarget, dmat4 CircleSourceTS, float ScaleSource, dmat4 CircleTargetTS, float ScaleTarget, Context context, SceneState scene)
    //    {
    //        GL.Enable(EnableCap.CullFace);
    //        GL.CullFace(CullFaceMode.Front);
    //        GL.FrontFace(FrontFaceDirection.Cw);

    //        GL.Enable(EnableCap.Blend);
    //        GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

    //        va.Bind();
    //        va.Clean();

    //        sp1.Bind();
    //        SetUnoform1(modelMatrixTarget, CircleSourceTS, ScaleSource, CircleTargetTS, ScaleTarget, scene);
    //        GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop/*BeginMode.LineLoop*/, 0, protoCircleSize);

    //        GL.Disable(EnableCap.Blend);
    //        GL.Disable(EnableCap.CullFace);
    //    }

    //    private void SetUnoform1(dmat4 modelMatrixTarget, dmat4 CircleSourceTS, float ScaleSource, dmat4 CircleTargetTS, float ScaleTarget, SceneState scene)
    //    {
    //        dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * modelMatrixTarget;
    //        sp1.SetUniform("u_mvp", mvp.ToMat4());

    //        sp1.SetUniform("u_matrixSource", CircleSourceTS);
    //        sp1.SetUniform("u_matrixTarget", CircleTargetTS);
    //        sp1.SetUniform("u_scaleSource", ScaleSource);
    //        sp1.SetUniform("u_scaleTarget", ScaleTarget);
    //    }

    //    private void initCircle()
    //    {
    //        float curAngle, dAngle = glm.Radians(360.0f / (float)protoCircleSize);

    //        protoCircle = new vec3[protoCircleSize];

    //        for (int i = 0; i < 36; i++)
    //        {
    //            curAngle = dAngle * i;
    //            protoCircle[i] = new vec3((float)Math.Cos(curAngle), 0.0f, (float)-Math.Sin(curAngle));
    //        }
    //    }

    //    public override void Render(dmat4 modelMatrix, Context context, SceneState scene)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    private int protoCircleSize;

    //    private vec3[] protoCircle;

    //    //   private dmat4 circleSource_TS;
    //    //   private dmat4 circleTarget_TS;

    //    //private dmat4 modelSource;
    //    //      private dmat4 modelTarget;

    //    //    private float scaleSource;
    //    //     private float scaleTarget;

    //    private readonly ShaderProgram sp1;//, sp2;
    //    private readonly VertexArray va;

    //    private float deltaRingsStep = 0.0f;
    //}

}
