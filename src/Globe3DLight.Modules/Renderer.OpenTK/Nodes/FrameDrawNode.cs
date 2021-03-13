using GlmSharp;
using Globe3DLight.Scene;
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class FrameDrawNode : DrawNode, IFrameDrawNode, IDisposable
    {
        private IFrameRenderModel _frame;
       
        public FrameDrawNode(IFrameRenderModel frame)
        {
            _frame = frame;
        }

        public IFrameRenderModel Frame 
        {
            get => _frame; 
            set => _frame = value;
        }

        public override void Draw(object dc, IEnumerable<dmat4> modelMatrices, ISceneState scene)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

            var scale = Frame.Scale;

            foreach (var item in modelMatrices)
            {
                var modelView = scene.ViewMatrix * item;

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(modelView.Values1D);

                // zAxis
                GL.Color3(0.0, 0.0, 1.0);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0.0, 0.0, 0.0);
                GL.Vertex3(0.0, 0.0, 1.0 * scale);
                GL.End();

                // xAxis                    
                GL.Color3(1.0, 0.0, 0.0);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0.0, 0.0, 0.0);
                GL.Vertex3(1.0 * scale, 0.0, 0.0);
                GL.End();

                // yAxis                  
                GL.Color3(0.0, 1.0, 0.0);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0.0, 0.0, 0.0);
                GL.Vertex3(0.0, 1.0 * scale, 0.0);
                GL.End();
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            var modelView = scene.ViewMatrix * modelMatrix;

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
