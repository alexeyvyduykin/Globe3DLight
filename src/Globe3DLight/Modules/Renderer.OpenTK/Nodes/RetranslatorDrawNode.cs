using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using B = Globe3DLight.Renderer.OpenTK.Core;
using Globe3DLight.Models.Scene;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Geometry;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Renderer.OpenTK
{
   
    internal class RetranslatorDrawNode : DrawNode, IRetranslatorDrawNode
    {
        private readonly B.Context _context;
        private B.Device _device;
        // private readonly IMesh mesh;
        private readonly B.DrawState drawState;
        private readonly B.ShaderProgram sp;

        private bool dirty;
        private readonly double _scale;
        private readonly B.Uniform<mat4> u_mvp;
        private readonly B.Uniform<vec4> u_color;


        public RetranslatorRenderModel Retranslator { get; set; }

        private readonly string meshVS = @"
#version 330

layout (location = 0) in vec3 POSITION;

uniform mat4 u_mvp;

void main(void)
{
gl_Position = u_mvp * vec4(POSITION, 1.0);
}";
        private readonly string meshFS = @"
#version 330

out vec4 color;

uniform vec4 u_color;

void main(void)
{
color = u_color;
}";

        public RetranslatorDrawNode(RetranslatorRenderModel retranslator)
        {
            this.Retranslator = retranslator;

            _context = new B.Context(/*null, 600, 600*/);

            _device = new B.Device();

            _scale = retranslator.Scale;

            dirty = true;

            sp = _device.CreateShaderProgram(meshVS, meshFS);

            u_mvp = ((B.Uniform<mat4>)sp.Uniforms["u_mvp"]);
            u_color = ((B.Uniform<vec4>)sp.Uniforms["u_color"]);

            u_color.Value = new vec4(0.0f, 1.0f, 0.0f, 1.0f);

            drawState = new B.DrawState();
            drawState.ShaderProgram = sp;


        }
        public override void UpdateGeometry()    
        {
            if (dirty)
            {
                var mesh = Retranslator.Mesh;// new SolidSphere(1.0f, 32, 32);

                drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                drawState.RenderState.FacetCulling.FrontFaceWindingOrder =
                    (mesh.FrontFaceWindingOrder == FrontFaceDirection.Cw) ? A.FrontFaceDirection.Cw : A.FrontFaceDirection.Ccw;
                dirty = false;
            }
        }
        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)   
        {         
            dmat4 model = modelMatrix * dmat4.Scale(new dvec3(_scale, _scale, _scale));
            dmat4 view = scene.ViewMatrix;
            dmat4 mvp = scene.ProjectionMatrix * view * model;

            u_mvp.Value = mvp.ToMat4();


            _context.Draw(A.PrimitiveType.Triangles, drawState, scene);

            B.ShaderProgram.UnBind();
        }

     
    }
}
