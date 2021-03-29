﻿using System;
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
        private readonly B.DrawState _drawState;
        private readonly B.ShaderProgram _sp;
        private bool _dirty;
        private readonly double _scale;
        private readonly B.Uniform<mat4> u_mvp;
        private readonly B.Uniform<vec4> u_color;    
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
            Retranslator = retranslator;

            _context = new B.Context();

            _device = new B.Device();

            _scale = retranslator.Scale;

            _dirty = true;

            _sp = _device.CreateShaderProgram(meshVS, meshFS);

            u_mvp = ((B.Uniform<mat4>)_sp.Uniforms["u_mvp"]);
            u_color = ((B.Uniform<vec4>)_sp.Uniforms["u_color"]);

            u_color.Value = new vec4(0.094f, 0.647f, 0.345f, 1.0f); // #18A558

            _drawState = new B.DrawState();
            _drawState.ShaderProgram = _sp;
        }

        public RetranslatorRenderModel Retranslator { get; set; }

        public override void UpdateGeometry()    
        {
            if (_dirty)
            {
                var mesh = Retranslator.Mesh;// new SolidSphere(1.0f, 32, 32);

                _drawState.VertexArray = _context.CreateVertexArray_NEW(mesh, _drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                _drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                _drawState.RenderState.FacetCulling.FrontFaceWindingOrder =
                    (mesh.FrontFaceWindingOrder == FrontFaceDirection.Cw) ? A.FrontFaceDirection.Cw : A.FrontFaceDirection.Ccw;
                _dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)   
        {         
            var model = modelMatrix * dmat4.Scale(new dvec3(_scale, _scale, _scale));
            var view = scene.ViewMatrix;
            var mvp = scene.ProjectionMatrix * view * model;

            u_mvp.Value = mvp.ToMat4();

            _context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

            B.ShaderProgram.UnBind();
        }     
    }
}
