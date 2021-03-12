using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using B = Globe3DLight.Renderer.OpenTK.Core;
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
        private bool _dirty;
        private readonly B.ShaderProgram _sp;
        private readonly B.DrawState _drawState;   
        private readonly double _scale;

        private readonly B.Uniform<mat4> u_mvp;
        private readonly B.Uniform<vec4> u_color;

        public GroundStationDrawNode(Scene.IGroundStationRenderModel groundStation)
        {
            this.GroundStation = groundStation;

            _context = new B.Context();

            _scale = groundStation.Scale;

            _dirty = true;

            _device = new B.Device();

            _sp = _device.CreateShaderProgram(groundStationVS, groundStationFS);

            u_mvp = ((B.Uniform<mat4>)_sp.Uniforms["u_mvp"]);
            u_color = ((B.Uniform<vec4>)_sp.Uniforms["u_color"]);

            u_color.Value = new vec4(0.0f, 0.7f, 0.0f, 1.0f);


            A.GL.BindAttribLocation(_sp.Handle, (int)0, "POSITION");

            _drawState = new B.DrawState();
            _drawState.ShaderProgram = _sp;
        }

        public override void UpdateGeometry()
        {
            if (_dirty)
            {
                var mesh = GroundStation.Mesh;// new SolidSphere(1.0f, 16, 16);// 32, 32);

                _drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, _drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                _drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                _drawState.RenderState.FacetCulling.FrontFaceWindingOrder = 
                    (mesh.FrontFaceWindingOrder == Geometry.FrontFaceDirection.Cw) ? A.FrontFaceDirection.Cw : A.FrontFaceDirection.Ccw;
                _dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, Scene.ISceneState scene)
        {
          //  sp.Bind();
                
            var model = modelMatrix * dmat4.Scale(new dvec3(_scale, _scale, _scale));
            var view = scene.ViewMatrix;
            var mvp = scene.ProjectionMatrix * view * model;

            u_mvp.Value = mvp.ToMat4();

            _context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

            B.ShaderProgram.UnBind();
        }
    }

}
