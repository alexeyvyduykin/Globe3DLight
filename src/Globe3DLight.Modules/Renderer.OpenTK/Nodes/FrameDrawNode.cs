using GlmSharp;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class FrameDrawNode : DrawNode, Globe3DLight.Renderer.IFrameDrawNode, IDisposable
    {

        private IFrameRenderModel _frame;

        public IFrameRenderModel Frame { get => _frame; set => _frame = value; }

        public FrameDrawNode(IFrameRenderModel frame)
        {
            this._frame = frame;
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            dmat4 modelView = scene.ViewMatrix * modelMatrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(modelView.Values1D);

            var scale = Frame.Scale;
            //GL.Scale(scale, scale, scale);

            // zAxis                   
            GL.Color3(0.0, 0.0, 1.0);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 1.0 * scale);
            GL.End();


            //	glRasterPos3f(0.0, 0.0, 1.2);
            //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'z');

            // xAxis      
            //GL.Rotate(90, 0, 1, 0);          
            GL.Color3(1.0, 0.0, 0.0);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(1.0 * scale, 0.0, 0.0);
            GL.End();


            //	glRasterPos3f(0.0, 0.0, 1.2);
            //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'x');

            // yAxis      
            //GL.Rotate(-90, 1, 0, 0);           
            GL.Color3(0.0, 1.0, 0.0);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 1.0 * scale, 0.0);
            GL.End();

            //	glRasterPos3f(0.0, 0.0, 1.2);
            //	glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'y');
        }

        public override void UpdateGeometry()
        {
            
        }
    }
}
