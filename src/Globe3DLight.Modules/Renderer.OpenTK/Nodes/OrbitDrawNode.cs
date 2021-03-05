using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Scene;
using GlmSharp;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class OrbitDrawNode : DrawNode, Globe3DLight.Renderer.IOrbitDrawNode
    {
      
        private readonly IOrbitRenderModel _orbit;

        public IOrbitRenderModel Orbit => _orbit;

        public OrbitDrawNode(IOrbitRenderModel orbit)
        {
            _orbit = orbit;
        }

        public override void UpdateGeometry() { }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(scene.ViewMatrix.Values1D);

            GL.Color3(1.0, 0.0, 0.0);
            GL.LineWidth(3.0f);

            GL.Begin(PrimitiveType.LineLoop);

            foreach (var pos in Orbit.Vertices)
            {
                GL.Vertex3(pos.x, pos.y, pos.z);
            }

            GL.LineWidth(1.0f);
            GL.End();
        }

    }
}
