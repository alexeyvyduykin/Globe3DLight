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
    //public class EarthShape : ISceneShape, IDisposable
    //{
    //    private readonly IShaderProgram _shaderProgram;

    //    private readonly IRenderState _renderState = new RenderState();
    //    private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();

    //    private readonly IUniform<mat4> u_model;
    //    private readonly IUniform<mat4> u_mvp;
    //    private readonly IUniform<mat3> u_normalMatrix;
    //    private readonly IUniform<mat4> u_view;
    //    private readonly IUniform<mat4> u_modelView;
    //    private readonly IUniform<vec4> u_lightPosition;
    //    private readonly IUniform<mat4x2> u_modelZToClipCoordinates;

    //    public ImmutableArray<ITexture> DiffuseMaps { get; set; }
    //    public ImmutableArray<ITexture> SpecularMaps { get; set; }
    //    public ImmutableArray<ITexture> NormalMaps { get; set; }
    //    public ImmutableArray<ITexture> NightMaps { get; set; }

  


    //    // DrawState
    //    public IShaderProgram ShaderProgram => _shaderProgram;
    //    public IRenderState RenderState => _renderState;
    //    public IList<IVertexArray> VertexArray => _vertexArray;


    //    public IList<IMesh> Meshes { get; set; }

    //    public EarthShape(IDevice device)
    //    {
    //        this._shaderProgram = device.CreateShaderProgram();

    //        _shaderProgram.VertexShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Earth.earthGridVS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Earth.earthGridFS.glsl");

    //        u_model = (IUniform<mat4>)_shaderProgram.Uniforms["u_model"];
    //        u_mvp = ((IUniform<mat4>)_shaderProgram.Uniforms["u_mvp"]);
    //        u_normalMatrix = (IUniform<mat3>)_shaderProgram.Uniforms["u_normalMatrix"];
    //        u_view = ((IUniform<mat4>)_shaderProgram.Uniforms["u_view"]);
    //        u_modelView = ((IUniform<mat4>)_shaderProgram.Uniforms["u_modelView"]);
    //        u_lightPosition = ((IUniform<vec4>)_shaderProgram.Uniforms["u_lightPosition"]);            
    //    }

    //    public void SetUniforms(dmat4 modelMatrix, ISceneState sceneState)
    //    {
    //        dmat4 model = modelMatrix;
    //        dmat4 view = sceneState.ViewMatrix;
    //        dmat3 normalMatrix = (new dmat3((view * model).Inverse).Transposed);
    //        dmat4 mvp = sceneState.ProjectionMatrix * view * model;
    //        dmat4 modelView = view * model;

    //        u_model.Value = model.ToMat4();
    //        u_view.Value = view.ToMat4();
    //        u_normalMatrix.Value = normalMatrix.ToMat3();
    //        u_modelView.Value = modelView.ToMat4();
    //        u_mvp.Value = mvp.ToMat4();

    //        u_lightPosition.Value = sceneState.LightPosition.ToVec4();
    //    }

    //    bool dirty = true;
    //    private void Clean(IRenderContext context)
    //    {
    //        if (dirty == true)
    //        {
    //            RenderState.FacetCulling =
    //                new FacetCulling()
    //                {
    //                    Enabled = true,
    //                    Face = CullFaceMode.Back,
    //                    FrontFaceWindingOrder = Meshes.SingleOrDefault().FrontFaceWindingOrder,
    //                };
                
    //            VertexArray.Clear();
    //            for (int i = 0; i < Meshes.Count; i++)
    //            {
    //                var va = context.CreateVertexArray_NEW(Meshes[i], _shaderProgram.VertexAttributes, BufferUsageHint.StaticDraw);
    //                VertexArray.Add(va);
    //            }

    //            dirty = false;
    //        }
    //    }

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        Clean(context);
    
    //        SetUniforms(modelMatrix, scene);

    //        //int[] order = { 3, 2, 0, 1, 4, 5 };
    //        //int[] order = { 0, 1, 2, 3, 4, 5 };

    //        for (int i = 0; i < Meshes.Count; i++)
    //        {
    //            context.TextureUnits[0].Texture = DiffuseMaps[i];
    //            _shaderProgram.SetUniform<int>("u_diffuseMap", 0);

    //            context.TextureUnits[1].Texture = SpecularMaps[i];
    //            _shaderProgram.SetUniform<int>("u_specularMap", 1);

    //            context.TextureUnits[2].Texture = NormalMaps[i];
    //            _shaderProgram.SetUniform<int>("u_normalMap", 2);

    //            context.TextureUnits[3].Texture = NightMaps[i];
    //            _shaderProgram.SetUniform<int>("u_nightMap", 3);

    //            context.Draw(PrimitiveType.Triangles, _renderState, _vertexArray[i], _shaderProgram, scene);
    //        }

    //        ShaderProgram.UnBind();
    //    }

    //    public void Dispose()
    //    {
    //        _shaderProgram?.Dispose();

    //        for (int i = 0; i < VertexArray.Count; i++)
    //        {                             
    //            VertexArray[i]?.Dispose();                
    //        }
    //    }


    //}


}
