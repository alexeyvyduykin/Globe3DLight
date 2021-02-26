using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using B = Globe3DLight.Renderer.OpenTK.Core;
//using Globe3DScene;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class GroundStationDrawNode : DrawNode, Globe3DLight.Renderer.IGroundStationDrawNode
    {
        private B.Device _device;
        private B.Context _context;
        public Scene.IGroundStationRenderModel GroundStation { get; set; }

        private readonly string groundStationVS = @"
#version 330

layout (location = 0) in vec3 POSITION;

uniform mat4 u_mvp;

void main(void)
{
gl_Position = u_mvp * vec4(POSITION, 1.0);
}";

        private readonly string groundStationFS = @"
#version 330

out vec4 color;

uniform vec4 u_color;

void main(void)
{
color = u_color;
}";
        private bool dirty;
        private readonly B.ShaderProgram sp;
        private readonly B.DrawState drawState;   
        private readonly double _scale;

        private readonly B.Uniform<mat4> u_mvp;
        private readonly B.Uniform<vec4> u_color;

        public GroundStationDrawNode(Scene.IGroundStationRenderModel groundStation)
        {
            this.GroundStation = groundStation;

            _context = new B.Context();

            _scale = groundStation.Scale;

            dirty = true;

            _device = new B.Device();

            sp = _device.CreateShaderProgram(groundStationVS, groundStationFS);

            u_mvp = ((B.Uniform<mat4>)sp.Uniforms["u_mvp"]);
            u_color = ((B.Uniform<vec4>)sp.Uniforms["u_color"]);

            u_color.Value = new vec4(0.0f, 1.0f, 0.0f, 1.0f);


            A.GL.BindAttribLocation(sp.Handle, (int)0, "POSITION");

            drawState = new B.DrawState();
            drawState.ShaderProgram = sp;
        }

        public override void UpdateGeometry()
        {
            if (dirty)
            {
                var mesh = GroundStation.Mesh;// new SolidSphere(1.0f, 16, 16);// 32, 32);

                drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                drawState.RenderState.FacetCulling.FrontFaceWindingOrder = 
                    (mesh.FrontFaceWindingOrder == Geometry.FrontFaceDirection.Cw) ? A.FrontFaceDirection.Cw : A.FrontFaceDirection.Ccw;
                dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, Scene.ISceneState scene)
        {
          //  sp.Bind();
                
            dmat4 model = modelMatrix * dmat4.Scale(new dvec3(_scale, _scale, _scale));
            dmat4 view = scene.ViewMatrix;
            dmat4 mvp = scene.ProjectionMatrix * view * model;

            u_mvp.Value = mvp.ToMat4();

            _context.Draw(A.PrimitiveType.Triangles, drawState, scene);

         //   ShaderProgram.UnBind();
        }
    }

}
