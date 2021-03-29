using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Scene;
using GlmSharp;
using OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class OrbitDrawNode : DrawNode, IOrbitDrawNode
    {
      
        private readonly OrbitRenderModel _orbit;

        public OrbitRenderModel Orbit => _orbit;

        public OrbitDrawNode(OrbitRenderModel orbit)
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

            GL.Color3(0.565, 0.537, 0.518); // #908984
            GL.LineWidth(2.0f);

            GL.Begin(PrimitiveType.LineLoop);

            foreach (var pos in Orbit.Vertices)
            {
                GL.Vertex3(pos.x, pos.y, pos.z);
            }
           
            GL.End();

            GL.LineWidth(1.0f);
        }
    }
}
