using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Renderer;
using Globe3DLight;
using GlmSharp;
using Extensions;
//using System.Drawing;



namespace Globe3DLight.Scene
{
  //  public class TestObject
  //  {
  //      private readonly Polyline polyline;
  //      private ShapefileRenderer _states;

  //      private bool dirty = false;

  //      public TestObject()
  //      {
  //          polyline = new Polyline();

  //          dirty = true;
  //      }

  //      private void Clean(Context context)
  //      {
  //          Mesh mesh = new PolylineMesh();

  //          polyline.Set(context, mesh);

  //          Ellipsoid globeShape = Ellipsoid.ScaledWgs84;
  //          _states = new ShapefileRenderer("110m_admin_0_countries.shp", context, globeShape,
  //              new ShapefileAppearance()
  //              {
  //                  PolylineColor = Color.Red,
  //                  PolylineWidth = 3.0,
  //                  PolylineOutlineWidth = 2.0
  //              });

  //          dirty = false;
  //      }

  //      public void Render(Context context, SceneState scene)
  //      {
  //          if (dirty == true)
  //              Clean(context);

  //          //_states.Render(context, scene);
  //          ////polyline.Render(context, scene);

  //          //GL.MatrixMode(MatrixMode.Projection);
  //          //GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

  //          //GL.MatrixMode(MatrixMode.Modelview);
  //          //GL.LoadMatrix(scene.ViewMatrix.Values1D);

  //          //GL.Begin(BeginMode.Lines);

  //          //double x, y, z;
  //          //Conversion.SphericalToCartesian(30.0, ExtraMath.ToRadians(151.2086), ExtraMath.ToRadians(-33.8683), out x, out y, out z);

  //          ////GL.Vertex3(0.0, 0.0, 0.0);
  //          ////GL.Vertex3(x, y, z);

  //          //GL.End();


  //      }
  //  }

  //  public class PolylineMesh : Mesh
  //  {
  //      public PolylineMesh() : base()
  //      {

  //          VertexAttributePosition4 positionsAttribute = new VertexAttributePosition4();
  //          base.Attributes.Add(positionsAttribute);
  //          IList<vec4> positionsRef = positionsAttribute.Values;
  //          positionsRef.Add(new vec4(-12.0f, -12.0f, 12.0f, 1.0f));
  //          positionsRef.Add(new vec4(-12.0f, 12.0f, 12.0f, 1.0f));
  //          positionsRef.Add(new vec4(12.0f, 12.0f, 12.0f, 1.0f));
  //          positionsRef.Add(new vec4(12.0f, -12.0f, 12.0f, 1.0f));
  //          positionsRef.Add(new vec4(12.0f, -12.0f, -12.0f, 1.0f));
  //          positionsRef.Add(new vec4(12.0f, 12.0f, -12.0f, 1.0f));


  //          //positionsRef.Add(new vec4(-10.0f, -10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4(-10.0f,  10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4( -5.0f, -10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4( -5.0f,  10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4(  0.0f, -10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4(  0.0f,  10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4(  5.0f, -10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4(  5.0f,  10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4( 10.0f, -10.0f, -10.0f, 1.0f));
  //          //positionsRef.Add(new vec4( 10.0f,  10.0f, -10.0f, 1.0f));

  //          VertexAttributeColor colorsAttribute = new VertexAttributeColor();
  //          base.Attributes.Add(colorsAttribute);
  //          IList<vec4> colorsRef = colorsAttribute.Values;

  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));
  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));
  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));
  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));
  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));
  //          colorsRef.Add(new vec4(0.78f, 0.12f, 0.02f, 1.0f));

  //          IndicesUnsignedShort indicesBase = new IndicesUnsignedShort();
  //          base.Indices = indicesBase;
  //          IList<ushort> indicesRef = indicesBase.Values;

  //          ushort[] indicesArr = { 0, 1, 2, 0xFFFF, 3, 4, 5 };
  //          //indicesRef = indicesArr;
  //          indicesRef.Add(0);
  //          indicesRef.Add(1);
  //          indicesRef.Add(2);
  //          indicesRef.Add(0xFFFF);
  //          indicesRef.Add(3);                       
  //          indicesRef.Add(4);
  //          indicesRef.Add(5);

  //          base.PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip;





  //      }
  //  }

  //  public class Polyline : IDisposable
  //  {
  //      private Uniform<float> u_fillDistance;
  //      private Uniform<mat4> u_mvp;
  //      private Uniform<mat4> u_viewportMatrix;
  //      private Uniform<mat4> u_viewportOrtho;
  //      private Uniform<float> u_perspectiveNearPlaneDistance;

  //      private readonly DrawState drawState;
  //      private OpenTK.Graphics.OpenGL.PrimitiveType primitiveType;

  //      public Polyline()
  //      {
  //          drawState = new DrawState();
  ////          drawState.RenderState.FacetCulling.Enabled = false;

  //          drawState.RenderState.PrimitiveRestart.Enabled = true;
  //          drawState.RenderState.PrimitiveRestart.Index = 0xFFFF;

  //          Width = 2.0;
  //      }
       
  //      public void Set(Context context, Mesh mesh)
  //      {
  //          if (mesh == null)
  //          {
  //              throw new ArgumentNullException("mesh");
  //          }

  //          if (mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.Lines &&
  //              mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop &&
  //              mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip)
  //          {
  //              throw new ArgumentException("mesh.PrimitiveType must be Lines, LineLoop, or LineStrip.", "mesh");
  //          }

  //          if (!mesh.Attributes.Contains("POSITION") &&
  //              !mesh.Attributes.Contains("COLOR"))
  //          {
  //              throw new ArgumentException("mesh.Attributes should contain attributes named \"position\" and \"color\".", "mesh");
  //          }

  //          if (drawState.ShaderProgram == null)
  //          {
  //              drawState.ShaderProgram = Device.CreateShaderProgram(
  //                  EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.Polyline.PolylineVS.glsl"),
  //                  EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.Polyline.PolylineGS.glsl"),
  //                  EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.Polyline.PolylineFS.glsl"));
  //              u_fillDistance = (Uniform<float>)drawState.ShaderProgram.Uniforms["u_fillDistance"];
  //              u_mvp = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_mvp"];
                
  //              u_viewportMatrix = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_viewportMatrix"];
  //              u_viewportOrtho = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_viewportOrtho"];


  //              u_perspectiveNearPlaneDistance = (Uniform<float>)drawState.ShaderProgram.Uniforms["u_perspectiveNearPlaneDistance"];

  //          }

  //          drawState.VertexArray = context.CreateVertexArray(mesh, drawState.ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw);
  //          primitiveType = mesh.PrimitiveType;
  //      }

  //      private dmat4 ViewportMatrix(Rectangle viewport, double nearDepthRange, double farDepthRange)
  //      {
  //          double halfWidth = viewport.Width * 0.5;
  //          double halfHeight = viewport.Height * 0.5;
  //          double halfDepth = (farDepthRange - nearDepthRange) * 0.5;

  //          //        return new dmat4(
  //          //halfWidth, 0.0, 0.0, viewport.Left + halfWidth,
  //          //0.0, halfHeight, 0.0, viewport.Top + halfHeight,
  //          //0.0, 0.0, halfDepth, nearDepthRange + halfDepth,
  //          //0.0, 0.0, 0.0, 1.0);

  //          return new dmat4(
  //              halfWidth, 0.0, 0.0, 0.0,
  //              0.0, halfHeight, 0.0, 0.0,
  //              0.0, 0.0, halfDepth, 0.0,
  //              viewport.Left + halfWidth, viewport.Top + halfHeight, nearDepthRange + halfDepth, 1.0);
  //      }

  //      //    public dmat4 ComputeViewportOrthographicMatrix(Rectangle viewport)
  //      //    {
  //      //        //
  //      //        // Bottom and top swapped:  MS -> OpenGL
  //      //        //
  //      //        //return CreateOrthographicOffCenter(
  //      //        //    viewport.Left, viewport.Right,
  //      //        //    viewport.Top, viewport.Bottom,
  //      //        //    0.0, 1.0);

  //      //        return OrthographicProjection(
  //      //viewport.Left, viewport.Right,
  //      //viewport.Bottom, viewport.Top,
  //      //0.0, 1.0);
  //      //    }

  //      //    public dmat4 PerspectiveProjectionFOV(double fovy, double aspect, double zNear, double zFar)
  //      //    {
  //      //        if (fovy <= 0.0 || fovy > Math.PI)
  //      //        {
  //      //            throw new ArgumentOutOfRangeException("fovy", "fovy must be in [0, PI).");
  //      //        }

  //      //        if (aspect <= 0.0)
  //      //        {
  //      //            throw new ArgumentOutOfRangeException("aspect", "aspect must be greater than zero.");
  //      //        }

  //      //        if (zNear <= 0.0)
  //      //        {
  //      //            throw new ArgumentOutOfRangeException("zNear", "zNear must be greater than zero.");
  //      //        }

  //      //        if (zFar <= 0.0)
  //      //        {
  //      //            throw new ArgumentOutOfRangeException("zFar", "zFar must be greater than zero.");
  //      //        }

  //      //        double bottom = Math.Tan(fovy * 0.5);
  //      //        double f = 1.0 / bottom;

  //      //        return new dmat4(
  //      //            f / aspect, 0.0, 0.0, 0.0,
  //      //            0.0, f, 0.0, 0.0,
  //      //            0.0, 0.0, (zFar + zNear) / (zNear - zFar), (2.0 * zFar * zNear) / (zNear - zFar),
  //      //            0.0, 0.0, -1.0, 0.0);
  //      //    }

  //      //    public dmat4 OrthographicProjection(double left, double right, double bottom, double top, double zNear, double zFar)
  //      //    {
  //      //        double a = 1.0 / (right - left);
  //      //        double b = 1.0 / (top - bottom);
  //      //        double c = 1.0 / (zFar - zNear);

  //      //        double tx = -(right + left) * a;
  //      //        double ty = -(top + bottom) * b;
  //      //        double tz = -(zFar + zNear) * c;

  //      //        //return new dmat4(
  //      //        //    2.0 * a, 0.0, 0.0, tx,
  //      //        //    0.0, 2.0 * b, 0.0, ty,
  //      //        //    0.0, 0.0, -2.0 * c, tz,
  //      //        //    0.0, 0.0, 0.0, 1.0);

  //      //        return new dmat4(
  //      //            2.0 * a, 0.0, 0.0, 0.0,
  //      //            0.0, 2.0 * b, 0.0, 0.0,
  //      //            0.0, 0.0, -2.0 * c, 0.0,
  //      //            tx, ty, tz, 1.0);
  //      //    }

  //      public mat4 OrthographicProjection(Rectangle viewport)
  //      {
  //          return mat4.Ortho(viewport.Left, viewport.Right, viewport.Top, viewport.Bottom, 0.0f, 1.0f);
  //      }

  //      //    public dmat4 ModelViewPerspectiveMatrix(Camera camera)
  //      //    {
  //      //        return PerspectiveMatrix(camera) * ViewMatrix(camera);
  //      //    }

  //      //    public dmat4 ViewMatrix(Camera camera)
  //      //    {
  //      //        return dmat4.LookAt(camera.Eye, camera.Target, camera.Up);
  //      //    }

  //      //    public dmat4 PerspectiveMatrix(Camera camera)
  //      //    {
  //      //        return dmat4.Perspective(camera.FieldOfViewY, camera.AspectRatio,
  //      //                camera.PerspectiveNearPlaneDistance, camera.PerspectiveFarPlaneDistance);

  //      //            //return PerspectiveProjectionFOV(camera.FieldOfViewY, camera.AspectRatio,
  //      //            //    camera.PerspectiveNearPlaneDistance, camera.PerspectiveFarPlaneDistance);
  //      //    }

  //      public void Render(Context context, SceneState scene)
  //      {
  //          if (drawState.ShaderProgram != null)
  //          {
  //              mat4 viewportMatrix = ViewportMatrix(scene.Viewport, 0.0, 1.0).ToMat4();



  //              //   dmat4 model =/* dmat4.Rotate(-90.0f, vec3.UnitY) **/ dmat4.Rotate(-90.0f, vec3.UnitX);
  //              dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix;// * model;
  //              mat4 ortho = OrthographicProjection(scene.Viewport);

  //              u_perspectiveNearPlaneDistance.Value = (float)scene.Camera.PerspectiveNearPlaneDistance;
  //              u_fillDistance.Value = (float)(Width * 0.5 * scene.HighResolutionSnapScale);

  //              u_mvp.Value = mvp.ToMat4();
  //              u_viewportMatrix.Value = viewportMatrix;
  //              u_viewportOrtho.Value = ortho;

  //              context.Draw(primitiveType, drawState, scene);
  //          }
  //      }

  //      public bool Wireframe
  //      {
  //          get { return drawState.RenderState.RasterizationMode == PolygonMode.Line/* RasterizationMode.Line*/; }
  //          set { drawState.RenderState.RasterizationMode = value ? PolygonMode.Line/* RasterizationMode.Line*/ : PolygonMode.Fill/* RasterizationMode.Fill*/; }
  //      }

  //      public double Width { get; set; }

  //      #region IDisposable Members

  //      public void Dispose()
  //      {
  //          if (drawState.ShaderProgram != null)
  //          {
  //              drawState.ShaderProgram.Dispose();
  //          }

  //          if (drawState.VertexArray != null)
  //          {
  //              drawState.VertexArray.Dispose();
  //          }
  //      }

  //      #endregion

  //  }
}
