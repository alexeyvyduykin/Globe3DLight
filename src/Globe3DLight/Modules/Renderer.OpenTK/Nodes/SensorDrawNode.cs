using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Scene;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class SensorDrawNode : DrawNode, ISensorDrawNode
    {
        private readonly SensorRenderModel _sensor;
        private Scan _scan;
        private Shoot _shoot;

        public SensorDrawNode(SensorRenderModel sensor)
        {
            _sensor = sensor;
        }
       
        public SensorRenderModel Sensor => _sensor;

        public override void UpdateGeometry()
        {
            _scan = Sensor.Scan;
            _shoot = Sensor.Shoot;
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            // Temporary solution
            _scan = Sensor.Scan;
            _shoot = Sensor.Shoot;

            var mvp = scene.ViewMatrix * modelMatrix;
           
            A.GL.MatrixMode(A.MatrixMode.Projection);
            A.GL.LoadMatrix(scene.ProjectionMatrix.Values1D);
            A.GL.MatrixMode(A.MatrixMode.Modelview);
            A.GL.LoadMatrix(mvp.Values1D);

            A.GL.LineWidth(2.0f);

            RenderScan(_scan);
            RenderShoot(_scan, _shoot);            

            A.GL.LineWidth(1.0f);
        }

        private void RenderShoot(Scan scan, Shoot shoot)
        {
            A.GL.Disable(A.EnableCap.PolygonOffsetFill);

            A.GL.Color3(0.094, 0.604, 0.706); // #189AB4

            A.GL.Begin(A.PrimitiveType.LineLoop);

            A.GL.Vertex3(shoot.P0.Values);
            A.GL.Vertex3(shoot.Pos.Values);
            A.GL.Vertex3(shoot.P1.Values);

            A.GL.End();

            A.GL.Enable(A.EnableCap.PolygonOffsetFill);
            A.GL.PolygonOffset(1.0f, 1.0f);
      
            A.GL.Color4(0.459, 0.902, 0.855, 0.1); // #75E6DA

            A.GL.Enable(A.EnableCap.Blend);              
            A.GL.BlendFunc(A.BlendingFactorSrc.SrcAlpha, A.BlendingFactorDest.OneMinusSrcAlpha);
            
            A.GL.Begin(A.PrimitiveType.QuadStrip);

            A.GL.Vertex3(shoot.Pos.Values);
            A.GL.Vertex3(shoot.Pos.Values);
            A.GL.Vertex3(shoot.P0.Values);                    
            A.GL.Vertex3(shoot.P1.Values); 
            A.GL.Vertex3(scan.P0.Values);
            A.GL.Vertex3(scan.P3.Values);

            A.GL.End();

            A.GL.Disable(A.EnableCap.Blend);
        }

        private void RenderScan(Scan scan)
        {
            A.GL.Color3(0.831, 0.945, 0.957); // #D4F1F4

            A.GL.Begin(A.PrimitiveType.LineLoop); 
            A.GL.Vertex3(scan.P0.Values);
            A.GL.Vertex3(scan.P1.Values);
            A.GL.Vertex3(scan.P2.Values);
            A.GL.Vertex3(scan.P3.Values);
            A.GL.End();
        }
    }
}
