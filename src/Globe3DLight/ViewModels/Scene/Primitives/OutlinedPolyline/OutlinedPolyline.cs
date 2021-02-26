using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using Globe3DLight.Renderer;
using GlmSharp;
using Extensions;
//using System.Drawing;

namespace Globe3DLight.Scene
{
   // public class OutlinedPolyline : IDisposable
   // {
   //     private Uniform<float> u_fillDistance;
   //     private Uniform<float> u_outlineDistance;

   //     private Uniform<mat4> u_mvp;
   //     private Uniform<mat4> u_viewportMatrix;
   //     private Uniform<mat4> u_viewportOrtho;

   //     private readonly DrawState drawState;
   //     private OpenTK.Graphics.OpenGL.PrimitiveType primitiveType;

   //     public OutlinedPolyline()
   //     {
   //         drawState = new DrawState();
   ////       drawState.RenderState.FacetCulling.Enabled = false;

   //         drawState.RenderState.PrimitiveRestart.Enabled = true;
   //         drawState.RenderState.PrimitiveRestart.Index = 0xFFFF;

   //         Width = 1;
   //         OutlineWidth = 1;
   //     }

   //     public void Set(Context context, Mesh mesh)
   //     {
   //         //Verify.ThrowIfNull(context);

   //         if (mesh == null)
   //         {
   //             throw new ArgumentNullException("mesh");
   //         }

   //         if (mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.Lines &&
   //             mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop &&
   //             mesh.PrimitiveType != OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip)
   //         {
   //             throw new ArgumentException("mesh.PrimitiveType must be Lines, LineLoop, or LineStrip.", "mesh");
   //         }

   //         if (!mesh.Attributes.Contains("POSITION") &&
   //             !mesh.Attributes.Contains("COLOR") &&
   //             !mesh.Attributes.Contains("OUTLINECOLOR"))
   //         {
   //             throw new ArgumentException("mesh.Attributes should contain attributes named \"position\", \"color\", and \"outlineColor\".", "mesh");
   //         }

   //         if (drawState.ShaderProgram == null)
   //         {
   //             drawState.ShaderProgram = Device.CreateShaderProgram(
   //                 EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.OutlinedPolyline.OutlinedPolylineVS.glsl"),
   //                 EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.OutlinedPolyline.OutlinedPolylineGS.glsl"),
   //                 EmbeddedResources.GetText("Globe3D.Scene.Renderables.Primitives.OutlinedPolyline.OutlinedPolylineFS.glsl"));
   //             u_fillDistance = (Uniform<float>)drawState.ShaderProgram.Uniforms["u_fillDistance"];
   //             u_outlineDistance = (Uniform<float>)drawState.ShaderProgram.Uniforms["u_outlineDistance"];

   //             u_mvp = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_mvp"];

   //             u_viewportMatrix = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_viewportMatrix"];
   //             u_viewportOrtho = (Uniform<mat4>)drawState.ShaderProgram.Uniforms["u_viewportOrtho"];
   //         }

   //         drawState.VertexArray = context.CreateVertexArray(mesh, drawState.ShaderProgram.VertexAttributes, BufferUsageHint.StaticDraw);
   //         primitiveType = mesh.PrimitiveType;
   //     }

   //     private dmat4 ViewportMatrix(Rectangle viewport, double nearDepthRange, double farDepthRange)
   //     {
   //         double halfWidth = viewport.Width * 0.5;
   //         double halfHeight = viewport.Height * 0.5;
   //         double halfDepth = (farDepthRange - nearDepthRange) * 0.5;

   //         //        return new dmat4(
   //         //halfWidth, 0.0, 0.0, viewport.Left + halfWidth,
   //         //0.0, halfHeight, 0.0, viewport.Top + halfHeight,
   //         //0.0, 0.0, halfDepth, nearDepthRange + halfDepth,
   //         //0.0, 0.0, 0.0, 1.0);

   //         return new dmat4(
   //             halfWidth, 0.0, 0.0, 0.0,
   //             0.0, halfHeight, 0.0, 0.0,
   //             0.0, 0.0, halfDepth, 0.0,
   //             viewport.Left + halfWidth, viewport.Top + halfHeight, nearDepthRange + halfDepth, 1.0);
   //     }

   //     public mat4 OrthographicProjection(Rectangle viewport)
   //     {
   //         return mat4.Ortho(viewport.Left, viewport.Right, viewport.Top, viewport.Bottom, 0.0f, 1.0f);
   //     }

   //     public void Render(Context context, SceneState scene)
   //     {
   //         //Verify.ThrowIfNull(context);
   //         //Verify.ThrowIfNull(sceneState);

   //         if (drawState.ShaderProgram != null)
   //         {
   //             double fillDistance = Width * 0.5 * scene.HighResolutionSnapScale;
   //             u_fillDistance.Value = (float)(fillDistance);
   //             u_outlineDistance.Value = (float)(fillDistance + (OutlineWidth * scene.HighResolutionSnapScale));

   //             dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix;
   //             mat4 ortho = OrthographicProjection(scene.Viewport);
   //             mat4 viewportMatrix = ViewportMatrix(scene.Viewport, 0.0, 1.0).ToMat4();

   //             u_mvp.Value = mvp.ToMat4();
   //             u_viewportMatrix.Value = viewportMatrix;
   //             u_viewportOrtho.Value = ortho;

   //             context.Draw(primitiveType, drawState, scene);
   //         }
   //     }

   //     public double Width { get; set; }

   //     public double OutlineWidth { get; set; }

   //     public bool Wireframe
   //     {
   //         get { return drawState.RenderState.RasterizationMode == PolygonMode.Line; }
   //         set { drawState.RenderState.RasterizationMode = value ? PolygonMode.Line : PolygonMode.Fill; }
   //     }

   //     #region IDisposable Members

   //     public void Dispose()
   //     {
   //         if (drawState.ShaderProgram != null)
   //         {
   //             drawState.ShaderProgram.Dispose();
   //         }

   //         if (drawState.VertexArray != null)
   //         {
   //             drawState.VertexArray.Dispose();
   //         }
   //     }

   //     #endregion


   // }
}
