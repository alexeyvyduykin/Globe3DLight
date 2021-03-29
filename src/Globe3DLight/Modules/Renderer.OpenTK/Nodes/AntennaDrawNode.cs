using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Scene;
using Globe3DLight.Renderer.OpenTK.Core;
using OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class AntennaDrawNode : DrawNode, IAntennaDrawNode
    {       
        private readonly AntennaRenderModel _antenna;
        public AntennaDrawNode(AntennaRenderModel antenna)
        {
            _antenna = antenna;
        }

        public AntennaRenderModel Antenna => _antenna;
  
        public override void UpdateGeometry()
        {

        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            var modelView = scene.ViewMatrix;

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(scene.ProjectionMatrix.Values1D);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(modelView.Values1D);

            var source = modelMatrix.Column3;
            var target = Antenna.AbsoluteTargetPostion;

            GL.Color3(0.094, 0.647, 0.345); // #18A558

            GL.PushAttrib(AttribMask.EnableBit);
            
            GL.LineWidth(2.0f);
            GL.LineStipple(1, 0xFF00);        
            GL.Enable(EnableCap.LineStipple);

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(source.x, source.y, source.z);
            GL.Vertex3(target.x, target.y, target.z);
            GL.End();
          
            GL.LineWidth(1.0f);

            GL.PopAttrib();
        }

    }
}
