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
    //public class SatelliteShape : ISceneShape
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

    //    public double Scale { get; set; }

    //    private readonly IList<int> vaos = new List<int>();

    //    public SatelliteShape(IDevice device)
    //    {
    //        this._device = device;

    //        this._shaderProgram = device.CreateShaderProgram();

    //        _shaderProgram.VertexShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.Satellite.satelliteVS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShapes.Satellite.satelliteFS.glsl");

    //        _shaderProgram.BindAttribLocation((int)0, "POSITION");
    //        _shaderProgram.BindAttribLocation((int)1, "NORMAL");
    //        _shaderProgram.BindAttribLocation((int)2, "TEXCOORD");
    //    }

    //    private void SetUniforms(dmat4 modelMatrix, ISceneState scene)
    //    {
    //        dmat4 model = modelMatrix * dmat4.Scale(new dvec3(Scale, Scale, Scale));
    //        dmat4 view = scene.ViewMatrix;
    //        dmat3 normalMatrix = (new dmat3((view * model).Inverse).Transposed);
    //        dmat4 mvp = scene.ProjectionMatrix * view * model;
    //        dmat4 modelView = view * model;

    //        _shaderProgram.SetUniform("u_model", model.ToMat4());
    //        _shaderProgram.SetUniform("u_view", view.ToMat4());
    //        _shaderProgram.SetUniform("u_normalMatrix", normalMatrix.ToMat3());
    //        _shaderProgram.SetUniform("u_modelView", modelView.ToMat4());
    //        _shaderProgram.SetUniform("u_mvp", mvp.ToMat4());

    //        _shaderProgram.SetUniform("light.position", scene.LightPosition.ToVec4());
    //        _shaderProgram.SetUniform("light.ambient", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //        _shaderProgram.SetUniform("light.diffuse", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //        _shaderProgram.SetUniform("light.specular", new vec4(0.7f, 0.7f, 0.7f, 1.0f));
    //    }

    //    private bool dirty = true;

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        if(dirty == true)
    //        {
    //            for (int i = 0; i < Meshes.Count; i++)
    //            {
    //                context.SetupMesh(Meshes[i], out int vao, out int vbo, out int ebo);
    //                vaos.Add(vao);
    //            }
    //            dirty = false;
    //        }

    //        ShaderProgram.Bind();

    //        SetUniforms(modelMatrix, scene);

    //        for (int i = 0; i < Meshes.Count; i++)
    //        {            
    //            context.DrawModel(ShaderProgram, Meshes[i], vaos[i]);
    //        }
           
    //        ShaderProgram.UnBind();
    //    }

    //}


    //public class SatelliteGraphicsComponent : IGraphicsComponent
    //{
    //    public SatelliteGraphicsComponent()
    //    {
    //        //LoaderASE loader = new LoaderASE();
    //        //modelObject = loader.loadFrom(Resources.pathModelSatellite);

    //        modelObject = new Globe3D.Renderer.Model.Model("resources/models/satellite_v3.ase"/*  "C:/resource/models/satellite_v3.ase"*/);

    //        sp = Device.CreateShaderProgram(
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.Satellite.Shaders.satelliteVS.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.Satellite.Shaders.satelliteFS.glsl"));

    //        //  modelObject.materials[5].texture = Functions.textureLoad(Resources.pathTextureLocator);
    //        //  modelObject.materials[6].texture = Functions.textureLoad(Resources.pathTextureMiniSolarPanel);
    //        //  modelObject.materials[7].texture = Functions.textureLoad(Resources.pathTextureSolarPanel);

    //        //modelObject.materials[5].texture = (int)SOIL_load_OGL_texture(Resources.pathTextureLocator, 0, 0, SOIL_FLAG_INVERT_Y);
    //        //modelObject.materials[6].texture = (int)SOIL_load_OGL_texture(Resources.pathTextureMiniSolarPanel, 0, 0, 0);
    //        //modelObject.materials[7].texture = (int)SOIL_load_OGL_texture(Resources.pathTextureSolarPanel, 0, 0, 0);

    //        GL.BindAttribLocation(sp.Handle, (int)0, "POSITION");
    //        GL.BindAttribLocation(sp.Handle, (int)1, "NORMAL");
    //        GL.BindAttribLocation(sp.Handle, (int)2, "TEXCOORD");
    //    }

    //    public void Render(GameObjectCollection objs, Context context, SceneState scene) { }

    //    public void Render(GameObject obj, Context context, SceneState scene)
    //    {


    //        sp.Bind();

    //        SetUniforms(obj, scene);


    //        modelObject.draw(sp);

    //        //for (int i = 0; i < meshes.Count; i++)
    //        //{
    //        //    int iMat = modelObject.meshes[i].material;
    //        //    MyShaderProgram.MaterialSetup(prog, modelObject.materials[iMat]);


    //        //    GL.BindVertexArray(meshes[i].vao);
    //        //    GL.DrawElements(BeginMode.Triangles, meshes[i].numIndices,DrawElementsType.UnsignedShort, 0);
    //        //    GL.BindVertexArray(0);
    //        //}

    //        ShaderProgram.UnBind();
    //    }

    //    void SetUniforms(GameObject obj, SceneState sceneState)
    //    {
    //        dmat4 model = obj.ModelMatrix * dmat4.Scale(new dvec3(scale, scale, scale));
    //        dmat4 view = sceneState.ViewMatrix;
    //        dmat3 normalMatrix = (new dmat3((view * model).Inverse).Transposed);
    //        dmat4 mvp = sceneState.ProjectionMatrix * view * model;
    //        dmat4 modelView = view * model;

    //        sp.SetUniform("u_model", model.ToMat4());
    //        sp.SetUniform("u_view", view.ToMat4());
    //        sp.SetUniform("u_normalMatrix", normalMatrix.ToMat3());
    //        sp.SetUniform("u_modelView", modelView.ToMat4());
    //        sp.SetUniform("u_mvp", mvp.ToMat4());

    //        sp.SetUniform("light.position", sceneState.LightPosition.ToVec4());
    //        sp.SetUniform("light.ambient", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //        sp.SetUniform("light.diffuse", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
    //        sp.SetUniform("light.specular", new vec4(0.7f, 0.7f, 0.7f, 1.0f));
    //    }

    //    private ShaderProgram sp;
    //    private Globe3D.Renderer.Model.Model modelObject;
    //    private double scale = 0.002;
    //}

}
