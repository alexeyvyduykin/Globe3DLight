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
    //    internal class SensorDrawNode : DrawNode, ISensorDrawNode
    //    {
    //        private readonly B.Device _device;
    //        private readonly B.ShaderProgram sp;
    //        private readonly SensorRenderModel _sensor;

    //        private readonly string sensorVS = @"
    //#version 330

    //layout (location = 0) in vec3 POSITION;

    //void main()
    //{
    //  gl_Position = vec4(POSITION, 1.0);
    //}";
    //        private readonly string sensorGS = @"
    //#version 330

    //layout(points) in;
    //layout(triangle_strip, max_vertices = 12) out;

    //uniform mat4 u_mvp;

    //uniform vec4 u_P1;
    //uniform vec4 u_P2;
    //uniform vec4 u_P3;
    //uniform vec4 u_P4;

    //out vec4 clr;

    //const vec4 color = vec4(0.0, 1.0, 1.0, 0.20);

    //void main ()
    //{
    //  gl_Position = u_mvp * gl_in[0].gl_Position;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P1;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P2;
    //  clr = color;
    //  EmitVertex();
    //  EndPrimitive();

    //  gl_Position = u_mvp * gl_in[0].gl_Position;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P2;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P3;
    //  clr = color;
    //  EmitVertex();
    //  EndPrimitive();

    //  gl_Position = u_mvp * gl_in[0].gl_Position;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P3;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P4;
    //  clr = color;
    //  EmitVertex();
    //  EndPrimitive();

    //  gl_Position = u_mvp * gl_in[0].gl_Position;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P4;
    //  clr = color;
    //  EmitVertex();

    //  gl_Position = u_mvp * u_P1;
    //  clr = color;
    //  EmitVertex();
    //  EndPrimitive();

    //}

    //";
    //        private readonly string sensorFS = @"
    //#version 330

    //in vec4 clr;
    //out vec4 color;

    //void main()
    //{
    //  color = clr;
    //}";

    //        public SensorRenderModel Sensor => _sensor;

    //        public SensorDrawNode(SensorRenderModel sensor)
    //        {
    //            _sensor = sensor;

    //            _device = new B.Device();

    //            sp = _device.CreateShaderProgram(sensorVS, sensorGS, sensorFS);

    //            A.GL.BindAttribLocation(sp.Handle, (int)0, "POSITION");
    //        }

    //        private Scan _scan;
    //        private Shoot _shoot;
    //        public override void UpdateGeometry()
    //        {
    //            _scan = Sensor.Scan;
    //            _shoot = Sensor.Shoot;
    //        }

    //        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
    //        {
    //            // Temporary solution
    //            _scan = Sensor.Scan;
    //            _shoot = Sensor.Shoot;

    //            RenderShoot(_shoot, modelMatrix, scene);
    //            RenderScan(_scan, modelMatrix, scene);
    //        }
    //        public void RenderShoot(Shoot shoot, dmat4 modelMatrix, ISceneState scene)
    //        {

    //            A.GL.Enable(A.EnableCap.CullFace);
    //            A.GL.CullFace(A.CullFaceMode.Front);
    //            A.GL.FrontFace(A.FrontFaceDirection.Cw);

    //            A.GL.Enable(A.EnableCap.Blend);
    //            A.GL.BlendFunc(A.BlendingFactorSrc.SrcAlpha, A.BlendingFactorDest.OneMinusSrcAlpha);

    //            sp.Bind();
    //            //ShaderSetup.CameraSetup(sp, scene, obj.ModelMatrix);

    //            dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * modelMatrix;
    //            sp.SetUniform("u_mvp", mvp.ToMat4());

    //            sp.SetUniform("u_P1", shoot.p0.ToVec4());
    //            sp.SetUniform("u_P2", shoot.p1.ToVec4());
    //            sp.SetUniform("u_P3", shoot.p2.ToVec4());
    //            sp.SetUniform("u_P4", shoot.p3.ToVec4());

    //            A.GL.Begin(A.PrimitiveType.Points);
    //            A.GL.Vertex3(0.0f, 0.0f, 0.0f);
    //            A.GL.End();
    //            B.ShaderProgram.UnBind();

    //            A.GL.Disable(A.EnableCap.Blend);
    //            A.GL.Disable(A.EnableCap.CullFace);
    //        }

    //        public void RenderScan(Scan scan, dmat4 modelMatrix, ISceneState scene)
    //        {
    //            dmat4 mvp = scene.ViewMatrix;// Camera.ViewMatrix;// * modelMatrix;

    //            A.GL.MatrixMode(A.MatrixMode.Projection);
    //            A.GL.LoadMatrix(scene.ProjectionMatrix.Values1D);
    //            A.GL.MatrixMode(A.MatrixMode.Modelview);
    //            A.GL.LoadMatrix(mvp/*scene.Camera.ViewMatrix*/.Values1D);

    //            A.GL.LineWidth(3.0f);
    //            A.GL.Color3(0.0f, 1.0f, 1.0f);
    //            A.GL.Begin(A.PrimitiveType.LineLoop);
    //            A.GL.Vertex3(scan.p0.Values);
    //            A.GL.Vertex3(scan.p1.Values);
    //            A.GL.Vertex3(scan.p2.Values);
    //            A.GL.Vertex3(scan.p3.Values);
    //            A.GL.End();
    //            A.GL.LineWidth(1.0f);
    //        }
    //    }

    internal class SensorDrawNode : DrawNode, ISensorDrawNode
    {
        private readonly SensorRenderModel _sensor;

        public SensorRenderModel Sensor => _sensor;

        public SensorDrawNode(SensorRenderModel sensor)
        {
            _sensor = sensor;
        }

        private Scan _scan;
        private Shoot _shoot;
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







            RenderShoot(_shoot, modelMatrix, scene);
            RenderScan(_scan, _shoot, modelMatrix, scene);
        }
        //public void RenderShoot1(Shoot shoot, dmat4 modelMatrix, ISceneState scene)
        //{

        //    A.GL.Enable(A.EnableCap.CullFace);
        //    A.GL.CullFace(A.CullFaceMode.Front);
        //    A.GL.FrontFace(A.FrontFaceDirection.Cw);

        //    A.GL.Enable(A.EnableCap.Blend);
        //    A.GL.BlendFunc(A.BlendingFactorSrc.SrcAlpha, A.BlendingFactorDest.OneMinusSrcAlpha);

        //    sp.Bind();
        //    //ShaderSetup.CameraSetup(sp, scene, obj.ModelMatrix);

        //    dmat4 mvp = scene.ProjectionMatrix * scene.ViewMatrix * modelMatrix;
        //    sp.SetUniform("u_mvp", mvp.ToMat4());

        //    sp.SetUniform("u_P1", shoot.p0.ToVec4());
        //    sp.SetUniform("u_P2", shoot.p1.ToVec4());
        //    sp.SetUniform("u_P3", shoot.p2.ToVec4());
        //    sp.SetUniform("u_P4", shoot.p3.ToVec4());

        //    A.GL.Begin(A.PrimitiveType.Points);
        //    A.GL.Vertex3(0.0f, 0.0f, 0.0f);
        //    A.GL.End();
        //    B.ShaderProgram.UnBind();

        //    A.GL.Disable(A.EnableCap.Blend);
        //    A.GL.Disable(A.EnableCap.CullFace);
        //}

        public void RenderShoot(Shoot shoot, dmat4 modelMatrix, ISceneState scene)
        {
            dmat4 mvp = scene.ViewMatrix * modelMatrix;// scene.ProjectionMatrix * scene.ViewMatrix * modelMatrix;

            A.GL.MatrixMode(A.MatrixMode.Projection);
            A.GL.LoadMatrix(scene.ProjectionMatrix.Values1D);
            A.GL.MatrixMode(A.MatrixMode.Modelview);
            A.GL.LoadMatrix(mvp.Values1D);
               
            A.GL.Enable(A.EnableCap.Blend);              
            A.GL.BlendFunc(A.BlendingFactorSrc.SrcAlpha, A.BlendingFactorDest.OneMinusSrcAlpha);

            A.GL.Color4(0.0f, 1.0f, 1.0f, 0.2f);
            A.GL.Begin(A.PrimitiveType.Triangles);

            A.GL.Vertex3(shoot.p0.Values);
            A.GL.Vertex3(0.0, 0.0, 0.0);            
            A.GL.Vertex3(shoot.p1.Values);

            A.GL.End();
        
            A.GL.Disable(A.EnableCap.Blend);         
        }


        public void RenderScan(Scan scan, Shoot shoot, dmat4 modelMatrix, ISceneState scene)
        {
            var mvp = scene.ViewMatrix * modelMatrix;

            A.GL.MatrixMode(A.MatrixMode.Projection);
            A.GL.LoadMatrix(scene.ProjectionMatrix.Values1D);
            A.GL.MatrixMode(A.MatrixMode.Modelview);
            A.GL.LoadMatrix(mvp.Values1D);

            A.GL.Color3(0.0f, 1.0f, 1.0f);
        
            A.GL.Begin(A.PrimitiveType.LineLoop);
            A.GL.Vertex3(scan.p0.Values);
            A.GL.Vertex3(scan.p1.Values);
            A.GL.Vertex3(scan.p2.Values);
            A.GL.Vertex3(scan.p3.Values);
            A.GL.End();

            //A.GL.Enable(A.EnableCap.Blend);
            //A.GL.BlendFunc(A.BlendingFactorSrc.SrcAlpha, A.BlendingFactorDest.OneMinusSrcAlpha);
            
            //A.GL.Color4(0.0f, 1.0f, 1.0f, 0.2f);

            //A.GL.Begin(A.PrimitiveType.LineLoop);
            //A.GL.Vertex3(scan.p0.Values);
            //A.GL.Vertex3(shoot.p0.Values);
            //A.GL.Vertex3(shoot.p1.Values);
            //A.GL.Vertex3(scan.p3.Values);
            //A.GL.End();

            //A.GL.Disable(A.EnableCap.Blend);
        }
    }
}
