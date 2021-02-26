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

    //public class SunShape : ISceneShape
    //{
    //    private readonly IShaderProgram _shaderProgram;
    //    private readonly IRenderState _renderState = new RenderState();
    //    private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();


    //    private readonly IUniform<vec2> u_dims;
    //    private readonly IUniform<vec3> u_center;
    //    private readonly IUniform<float> u_NoiseZ;
    //    private readonly IUniform<float> u_ColorMapU;
    //    private readonly IUniform<float> u_ColorMapV;
    //    private readonly IUniform<vec3> u_ColorMult;
    //    private readonly IUniform<mat4> u_view;
    //    private readonly IUniform<mat4> u_proj;

    //    private const double DSUN = 1392684.0;
    //    private const double TSUN = 5778.0;
        
    //    private float DT = 0.0f;

    //    private const double toKM = 637.8;
    //    private const double diameter = 13926840.0;

    //    private const double range = 149600000.0 * 1.0 / toKM;// / 6378.0;//637.8;

    //    private const float temperature = 5778.0f;
    //    private const float colorMapU = (temperature - 800.0f) / 29200.0f;

    //    private const float multColor = 1.0f;
    //    private vec3 colorMult = new vec3(1.0f, 1.0f, 1.0f) * multColor;
        
    //    public ITexture SunGlow { get; set; }

    //    // DrawState
    //    public IShaderProgram ShaderProgram => _shaderProgram;
    //    public IRenderState RenderState => _renderState;
    //    public IList<IVertexArray> VertexArray => _vertexArray;

    //    public IList<IMesh> Meshes { get; set; }


    //    public SunShape(IDevice device)
    //    {
    //        this._shaderProgram = device.CreateShaderProgram();

    //        _shaderProgram.VertexShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Sun.sunVS.glsl");
    //        _shaderProgram.FragmentShaderSource =
    //            EmbeddedResources.GetText("Globe3D.Resources.CustomShaderPrograms.Sun.sunFS.glsl");

    //        u_dims = (Uniform<vec2>)_shaderProgram.Uniforms["u_dims"];
    //        u_center = (Uniform<vec3>)_shaderProgram.Uniforms["u_center"];
    //        u_NoiseZ = (Uniform<float>)_shaderProgram.Uniforms["unNoiseZ"];
    //        u_ColorMapU = (Uniform<float>)_shaderProgram.Uniforms["unColorMapU"];
    //        u_ColorMapV = (Uniform<float>)_shaderProgram.Uniforms["unColorMapV"];
    //        u_ColorMult = (Uniform<vec3>)_shaderProgram.Uniforms["unColorMult"];
    //        u_view = (Uniform<mat4>)_shaderProgram.Uniforms["u_view"];
    //        u_proj = (Uniform<mat4>)_shaderProgram.Uniforms["u_proj"];

    //    }

    //    public void SetUniforms(dmat4 modelMatrix, ISceneState scene)
    //    {
    //        dvec3 sunPosition_WS = new dvec3(glm.Normalized(scene.LightPosition) * range);
    //        dvec3 sunPosition_KM = sunPosition_WS * toKM;

    //        dvec3 cameraPosition_WS = scene.Camera.Eye;
    //        dvec3 cameraPosition_KM = scene.Camera.Eye * toKM;

    //        double distance = glm.Length(sunPosition_KM - cameraPosition_KM);

    //        float aspectRatio = 1.0f;
    //        double size = calculateGlowSize(diameter, temperature, distance);// - 4.0;
    //        vec2 dims = new vec2((float)size, (float)size * aspectRatio);

    //        DT += 0.0005f;
    //        if (DT >= 10000.0f)
    //            DT = 0.0f;

    //        u_dims.Value = dims;
    //        u_center.Value = sunPosition_WS.ToVec3();
    //        u_NoiseZ.Value = DT;
    //        u_ColorMapU.Value = colorMapU;
    //        u_ColorMapV.Value = -1.0f;
    //        u_ColorMult.Value = colorMult;
    //        u_view.Value = scene.ViewMatrix.ToMat4();
    //        u_proj.Value = scene.ProjectionMatrix.ToMat4();
    //    }
        
    //    private double calculateGlowSize(double diameter, double temperature, double distance)
    //    {
    //        // Georg's magic formula              
    //        double d = distance; // Distance
    //        double D = diameter * DSUN;
    //        double L = (D * D) * Math.Pow(temperature / TSUN, 4.0); // Luminosity

    //        return 0.016 * Math.Pow(L, 0.25) / Math.Pow(d, 0.5); // Size
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
    //                Face = CullFaceMode.Back,
    //                FrontFaceWindingOrder = Meshes.SingleOrDefault().FrontFaceWindingOrder,
    //            };

    //            RenderState.Blending = new Blending()
    //            {
    //                Enabled = true,                
    //                SourceRGBFactor = BlendingFactorSrc.One,                
    //                DestinationRGBFactor = BlendingFactorDest.One,           
    //            };

    //            //   drawState.RenderState.Blending.SourceAlphaFactor = BlendingFactorSrc.One;
    //            //   drawState.RenderState.Blending.DestinationAlphaFactor = BlendingFactorDest.One;

    //            dirty = false;
    //        }
    //    }

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        Clean(context);
     
    //        SetUniforms(modelMatrix, scene);

    //        context.TextureUnits[0].Texture = SunGlow;
    //        context.Draw(PrimitiveType.Triangles, _renderState, _vertexArray.SingleOrDefault(), _shaderProgram, scene);

    //        ShaderProgram.UnBind();
    //    }
    //}
}
