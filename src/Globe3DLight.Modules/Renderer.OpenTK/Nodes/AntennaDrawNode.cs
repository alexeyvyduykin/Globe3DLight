using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Scene;
using Globe3DLight.Renderer.OpenTK.Core;
//using Globe3DScene;
using OpenTK.Graphics.OpenGL;
using Globe3DLight.Data.Animators;

namespace Globe3DLight.Renderer.OpenTK
{

    internal class AntennaDrawNode : DrawNode, Globe3DLight.Renderer.IAntennaDrawNode
    {

       // private readonly ShaderProgram sp;
        private readonly Scene.IAntennaRenderModel _antenna;

        public Scene.IAntennaRenderModel Antenna => _antenna;

        public AntennaDrawNode(Scene.IAntennaRenderModel antenna)
        {
            this._antenna = antenna;
        }
  
        public override void UpdateGeometry()
        {
            //_scan = Sensor.Scan;
           // _shoot = Sensor.Shoot;
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {

            dmat4 modelView = scene.ViewMatrix;// * modelMatrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(modelView.Values1D);

            dvec4 source = modelMatrix.Column3;
            dvec3 target = Antenna.TargetPostion;

            GL.Color3(1.0, 0.0, 0.0);
            
            GL.PushAttrib(AttribMask.EnableBit);
            
            GL.LineWidth(3.0f);
            GL.LineStipple(1, 0xFF00);        
            GL.Enable(EnableCap.LineStipple);


            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(source.x, source.y, source.z);
            GL.Vertex3(target.x, target.y, target.z);
            GL.End();
          
            GL.LineWidth(1.0f);

          //  GL.Disable(EnableCap.LineStipple);

            GL.PopAttrib();
        }

    }
}
