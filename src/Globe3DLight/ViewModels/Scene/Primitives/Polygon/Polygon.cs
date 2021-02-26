using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using Globe3DLight.Renderer;
using GlmSharp;
//using System.Drawing;
using Extensions;

namespace Globe3DLight.Scene
{
    //public class Polygon : IDisposable
    //{
    //    public Polygon(Context context, Ellipsoid globeShape, IEnumerable<dvec3> positions)
    //    {
    //        //Verify.ThrowIfNull(context);
    //        //Verify.ThrowIfNull(globeShape);

    //        //
    //        // Pipeline Stage 1a:  Clean up - Remove duplicate positions
    //        //
    //        List<dvec3> cleanPositions = (List<dvec3>)SimplePolygonAlgorithms.Cleanup(positions);

    //        //
    //        // Pipeline Stage 1b:  Clean up - Swap winding order
    //        //
    //        EllipsoidTangentPlane plane = new EllipsoidTangentPlane(globeShape, cleanPositions);
    //        ICollection<dvec2> positionsOnPlane = plane.ComputePositionsOnPlane(cleanPositions);
    //        if (SimplePolygonAlgorithms.ComputeWindingOrder(positionsOnPlane) == FrontFaceDirection.Cw)  
    //        {
    //            cleanPositions.Reverse();
    //        }

    //        //
    //        // Pipeline Stage 2:  Triangulate
    //        //
    //        IndicesUnsignedInt indices = EarClippingOnEllipsoid.Triangulate(cleanPositions);

    //        //
    //        // Pipeline Stage 3:  Subdivide
    //        //
    //        TriangleMeshSubdivisionResult result = TriangleMeshSubdivision.Compute(cleanPositions, indices, ExtraMath.ToRadians(1));

    //        //
    //        // Pipeline Stage 4:  Set height
    //        //
    //        VertexAttributeDoubleVector3 positionsAttribute = new VertexAttributeDoubleVector3(
    //            "POSITION", (result.Indices.Values.Count / 3) + 2);
    //        foreach (dvec3 position in result.Positions)
    //        {
    //            positionsAttribute.Values.Add(globeShape.ScaleToGeocentricSurface(position));
    //        }

    //        Mesh mesh = new Mesh();
    //        mesh.PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
    //        mesh.FrontFaceWindingOrder = OpenTK.Graphics.OpenGL.FrontFaceDirection.Ccw;  // ccw
    //        mesh.Attributes.Add(positionsAttribute);
    //        mesh.Indices = result.Indices;

    //        ShaderProgram sp = Device.CreateShaderProgram(
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.Polygon.PolygonVS.glsl"),
    //            EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.Polygon.PolygonFS.glsl"));
    //        //((Uniform<vec3>)sp.Uniforms["u_globeOneOverRadiiSquared"]).Value = globeShape.OneOverRadiiSquared.ToVec3();

    //        u_color = (Uniform<vec4>)sp.Uniforms["u_color"];
    //        u_mvp = (Uniform<mat4>)sp.Uniforms["u_mvp"];
    //        //u_cameraEye = (Uniform<vec3>)sp.Uniforms["u_cameraEye"];
    //        //u_cameraLightPosition = (Uniform<vec3>)sp.Uniforms["u_cameraLightPosition"];
    //        //u_diffuseSpecularAmbientShininess = (Uniform<vec4>)sp.Uniforms["u_diffuseSpecularAmbientShininess"];

    //        drawState = new DrawState();
    //        drawState.RenderState.Blending.Enabled = true;
    //        drawState.RenderState.Blending.SourceRGBFactor = BlendingFactorSrc.SrcAlpha;
    //        drawState.RenderState.Blending.SourceAlphaFactor = BlendingFactorSrc.SrcAlpha;
    //        drawState.RenderState.Blending.DestinationRGBFactor = BlendingFactorDest.OneMinusDstAlpha;
    //        drawState.RenderState.Blending.DestinationAlphaFactor = BlendingFactorDest.OneMinusDstAlpha;
    //        drawState.ShaderProgram = sp;
    //        meshBuffers = Device.CreateMeshBuffers(mesh, drawState.ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw);

    //        primitiveType = mesh.PrimitiveType;

    //        Color = Color.White;
    //    }

    //    private void Update(Context context)
    //    {
    //        if (meshBuffers != null)
    //        {
    //            if (drawState.VertexArray != null)
    //            {
    //                drawState.VertexArray.Dispose();
    //                drawState.VertexArray = null;
    //            }

    //            drawState.VertexArray = context.CreateVertexArray(meshBuffers);
    //            meshBuffers = null;
    //        }
    //    }

    //    public void Render(Context context, SceneState scene)
    //    {
    //        //Verify.ThrowIfNull(context);
    //        //Verify.ThrowIfNull(sceneState);

    //        Update(context);

    //        u_mvp.Value = (scene.ProjectionMatrix * scene.ViewMatrix).ToMat4();
    //        //u_cameraEye.Value = scene.Camera.Eye.ToVec3();
    //        //u_cameraLightPosition.Value = scene.Camera.Eye.ToVec3();

    //        //u_diffuseSpecularAmbientShininess.Value = new vec4(
    //        //    scene.DiffuseIntensity,    
    //        //    scene.SpecularIntensity,    
    //        //    scene.AmbientIntensity,
    //        //    scene.Shininess);

    //        context.Draw(primitiveType, drawState, scene);
    //    }

    //    public Color Color
    //    {
    //        get { return color; }

    //        set
    //        {
    //            color = value;
    //            u_color.Value = new vec4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
    //        }
    //    }

    //    public bool Wireframe
    //    {
    //        get { return drawState.RenderState.RasterizationMode == PolygonMode.Line; }
    //        set { drawState.RenderState.RasterizationMode = value ? PolygonMode.Line : PolygonMode.Fill; }
    //    }

    //    //public bool BackfaceCulling
    //    //{
    //    //    get { return _drawState.RenderState.FacetCulling.Enabled; }
    //    //    set { _drawState.RenderState.FacetCulling.Enabled = value; }
    //    //}

    //    //public bool DepthWrite
    //    //{
    //    //    get { return _drawState.RenderState.DepthMask; }
    //    //    set { _drawState.RenderState.DepthMask = value; }
    //    //}

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        drawState.ShaderProgram.Dispose();
    //        drawState.VertexArray.Dispose();
    //    }

    //    #endregion
       
    //    private Color color;

    //    private readonly Uniform<vec4> u_color;
    //    private readonly Uniform<mat4> u_mvp;

    //    private readonly Uniform<vec3> u_cameraEye;
    //    private readonly Uniform<vec3> u_cameraLightPosition;

    //    private readonly Uniform<vec4> u_diffuseSpecularAmbientShininess;

    //    private readonly DrawState drawState;
    //    private readonly OpenTK.Graphics.OpenGL.PrimitiveType primitiveType;
    //    private MeshBuffers meshBuffers;       // For passing between threads
    //}
}
