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
    //public class GroundStationShape : ISceneShape
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

    //    public GroundStationShape(IDevice device)
    //    {
    //        this._shaderProgram = device.CreateShaderProgram();

    //        _shaderProgram.VertexShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.GroundStation.meshVS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.GroundStation.meshFS.glsl");

    //        u_mvp = ((IUniform<mat4>)_shaderProgram.Uniforms["u_mvp"]);
    //        u_color = ((IUniform<vec4>)_shaderProgram.Uniforms["u_color"]);

    //        u_color.Value = new vec4(1.0f, 0.0f, 0.0f, 1.0f);
    //    }

    //    private bool dirty = true;
    //    private void Clean(IRenderContext context)
    //    {
    //        if (dirty)
    //        {
    //            VertexArray.Clear();
    //            VertexArray.Add(context.CreateVertexArray_NEW(Meshes.FirstOrDefault(), ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw));
    //            RenderState.FacetCulling = new FacetCulling()
    //            {
    //                Face = CullFaceMode.Front,
    //                FrontFaceWindingOrder = Meshes.FirstOrDefault().FrontFaceWindingOrder,
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

    /*попытка добавить GroundStation как модель*/
    //public class GroundStationModelDefault_old : RenderModel// GroundStationModel //IRenderObject<SatelliteData>
    //{
    //    public GroundStationModelDefault_old()
    //    {
    //        //dirty = true;

    //        //    sp = Device.CreateShaderProgram(    
    //        //        EmbeddedResources.GetText("Globe3D.Scene.Renderables.Meshes.Sphere.meshVS.glsl"),   
    //        //         EmbeddedResources.GetText("Globe3D.Scene.Renderables.Meshes.Sphere.meshFS.glsl"));

    //        modelObject = new Globe3D.Renderer.Model.Model("C:/resource/models/GroundStation02.obj");

    //        sp = Device.CreateShaderProgram(
    //EmbeddedResources.GetText("Globe3D.Scene.Renderables.Satellite.Shaders.satelliteVS.glsl"),
    //EmbeddedResources.GetText("Globe3D.Scene.Renderables.Satellite.Shaders.satelliteFS.glsl"));

    //        GL.BindAttribLocation(sp.Handle, (int)0, "POSITION");
    //        GL.BindAttribLocation(sp.Handle, (int)1, "NORMAL");
    //        GL.BindAttribLocation(sp.Handle, (int)2, "TEXCOORD");

    //        //u_mvp = ((Uniform<mat4>)sp.Uniforms["u_mvp"]);
    //        //u_color = ((Uniform<vec4>)sp.Uniforms["u_color"]);

    //        //u_color.Value = new vec4(0.0f, 1.0f, 0.0f, 1.0f);

    //        // drawState = new DrawState();
    //        // drawState.ShaderProgram = sp;

    //        //mesh = new SolidSphere(0.06f, 16, 16);
    //    }

    //    //private void Clean(Context context)
    //    //{
    //    //    if (dirty)
    //    //    {
    //    //        drawState.VertexArray = context.CreateVertexArray_NEW(mesh, drawState.ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw);
    //    //        drawState.RenderState.FacetCulling.Face = CullFaceMode.Front;
    //    //        drawState.RenderState.FacetCulling.FrontFaceWindingOrder = mesh.FrontFaceWindingOrder;
    //    //        dirty = false;
    //    //    }
    //    //}

    //    //void SetUniforms(IGroundStationData data, SceneState sceneState)
    //    //{
    //    //    dmat4 model = data.ModelMatrix * dmat4.Scale(new dvec3(scale, scale, scale));
    //    //    dmat4 view = sceneState.ViewMatrix;
    //    //    dmat3 normalMatrix = (new dmat3((view * model).Inverse).Transposed);
    //    //    dmat4 mvp = sceneState.ProjectionMatrix * view * model;
    //    //    dmat4 modelView = view * model;

    //    //    sp.SetUniform("u_model", model.ToMat4());
    //    //    sp.SetUniform("u_view", view.ToMat4());
    //    //    sp.SetUniform("u_normalMatrix", normalMatrix.ToMat3());
    //    //    sp.SetUniform("u_modelView", modelView.ToMat4());
    //    //    sp.SetUniform("u_mvp", mvp.ToMat4());

    //    //    sp.SetUniform("light.position", sceneState.LightPosition.ToVec4());
    //    //    sp.SetUniform("light.ambient", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //    //    sp.SetUniform("light.diffuse", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //    //    sp.SetUniform("light.specular", new vec4(0.7f, 0.7f, 0.7f, 1.0f));
    //    //}

    //    //public override void Render(List<GroundStationData> data, Context context, SceneState scene)
    //    //{
    //    //    //Clean(context);

    //    //    sp.Bind();

    //    //    for (int i = 0; i < data.Count; i++)
    //    //    {
    //    //        SetUniforms(data[i], scene);

    //    //        modelObject.draw(sp);
    //    //    }
    //    //    //mat4 tempMatrix = scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4();

    //    //    //for (int i = 0; i < objs.Count; i++)
    //    //    //{
    //    //    //    u_mvp.Value = tempMatrix * objs[i].ModelMatrix.ToMat4();
    //    //    //    context.Draw(BeginMode.Triangles, drawState, scene);
    //    //    //}

    //    //    ShaderProgram.UnBind();
    //    //}

    //    //public override void Render(IGroundStationData data, Context context, SceneState scene)
    //    //{
    //    //    //Clean(context);

    //    //    sp.Bind();

    //    //    SetUniforms(data, scene);

    //    //    modelObject.draw(sp);

    //    //    //mat4 tempMatrix = scene.ProjectionMatrix.ToMat4() * scene.ViewMatrix.ToMat4();

    //    //    //for (int i = 0; i < objs.Count; i++)
    //    //    //{
    //    //    //    u_mvp.Value = tempMatrix * objs[i].ModelMatrix.ToMat4();
    //    //    //    context.Draw(BeginMode.Triangles, drawState, scene);
    //    //    //}

    //    //    ShaderProgram.UnBind();
    //    //}

    //    public override void Render(dmat4 modelMatrix, Context context, SceneState scene)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //private readonly Mesh mesh;
    //    //private readonly DrawState drawState;
    //    private readonly ShaderProgram sp;

    //    //private bool dirty;

    //    //private readonly Uniform<mat4> u_mvp;
    //    //private readonly Uniform<vec4> u_color;

    //    private Globe3D.Renderer.Model.Model modelObject;
    //    private double scale = 0.05;
    //}
}
