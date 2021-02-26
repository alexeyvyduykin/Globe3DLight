using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class GroundObjectListDrawNode : DrawNode, Globe3DLight.Renderer.IGroundObjectListDrawNode
    {
        public Scene.IGroundObjectListRenderModel GroundObjectList { get; set; }

        public GroundObjectListDrawNode(Scene.IGroundObjectListRenderModel groundObjectList)
        {
            this.GroundObjectList = groundObjectList;
        }

        public override void UpdateGeometry()
        {

        }

        public override void Draw(object dc, IEnumerable<dmat4> modelMatrices, ISceneState scene)
        {        
            A.GL.MatrixMode(A.MatrixMode.Projection);
            A.GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

            A.GL.PointSize(2.0f);

            foreach (var item in modelMatrices)
            {
                dmat4 modelView = scene.ViewMatrix * item;

                A.GL.MatrixMode(A.MatrixMode.Modelview);
                A.GL.LoadMatrix(modelView.Values1D);

                A.GL.Color3(1.0, 0.0, 0.0);
                A.GL.Begin(A.PrimitiveType.Points);
                A.GL.Vertex3(0.0, 0.0, 0.0);               
                A.GL.End();
            }

            A.GL.PointSize(1.0f);
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, Scene.ISceneState scene)
        {

        }
    }

}
