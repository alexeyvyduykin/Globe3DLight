﻿using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Scene;
using Globe3DLight.Models.Renderer;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.Renderer.OpenTK
{
    
    internal class SatelliteDrawNode : DrawNode, ISatelliteDrawNode
    { 
        private readonly string satelliteVS = @"
#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec3 NORMAL;
layout (location = 2) in vec2 TEXCOORD;

uniform mat4 u_view;
uniform mat4 u_model;
uniform mat3 u_normalMatrix;
uniform mat4 u_modelView;
uniform mat4 u_mvp;
//uniform vec3 u_lightPosition_WS;
uniform vec3 u_cameraPosition_WS;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}light;

out vec3 v_lightDir_CS;
out vec3 v_viewDir_CS;
out vec3 v_normal_CS;
out vec2 v_texCoord;

void main(void)
{
vec4 vertex_MS = vec4(POSITION.xyz, 1.0);
vec3 vertex_WS = (u_model * vertex_MS).xyz;
vec3 vertex_CS = (u_modelView * vertex_MS).xyz;

//vec4 lightPos_WS = vec4(light.position, 1.0);
vec3 lightPosition_CS  = (u_view * light.position).xyz;
vec3 cameraPosition_CS = vec3(0.0, 0.0, 0.0);

//v_lightDir_WS = light.position - vertex_WS;
//v_viewDir_WS  = u_cameraPosition_WS - vertex_WS;

v_lightDir_CS = lightPosition_CS - vertex_CS;
v_viewDir_CS  = cameraPosition_CS - vertex_CS;

//v_normal_WS    = (u_model * vec4(NORMAL, 0.0)).xyz;
v_normal_CS    = (u_modelView * vec4(NORMAL, 0.0)).xyz;

v_texCoord = TEXCOORD;

gl_Position    = u_mvp * vertex_MS;
}";
        private readonly string satelliteFS = @"
#version 330

in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;
in vec3 v_normal_CS;
in vec2 v_texCoord;

out vec4 color;

uniform struct Material
{
  sampler2D texture;

  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
  vec4 emission;
  float shininess;
} material;

uniform float u_isTexture;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}light;

// Light
//uniform vec4 lightAmbient;//    = vec4(1.0, 1.0, 1.0, 1.0);
//uniform vec4 lightDiffuse;//    = vec4(1.0, 1.0, 1.0, 1.0);
//uniform vec4 lightSpecular;//   = vec4(1.0, 1.0, 1.0, 1.0);

void main(void)
{
vec4 finalColor;
vec4 sampler = texture2D(material.texture, v_texCoord);

vec3 n = normalize(v_normal_CS);
vec3 l = normalize(v_lightDir_CS);
vec3 v = normalize(v_viewDir_CS);

finalColor = material.emission;

//finalColor += material.ambient * light.ambient;

float NdotL = max(dot( n, l ), 0.0);
finalColor += material.diffuse * light.diffuse * NdotL;


float materialShininess = 20.0f; // material.shininess;

float RdotVpow = max(pow(dot(reflect(-l, n), v), materialShininess), 0.0);
finalColor += material.specular * light.specular * RdotVpow;

if( u_isTexture == 1.0 )
  finalColor *= sampler;  

color = finalColor;
}";       
        private readonly B.ShaderProgram _sp;
        private readonly Model _model;  
        private readonly double _scale;// = 0.009;// 0.002;
        private readonly B.Device _device;
        private bool _dirty;
        private readonly ICache<string, int> _textureCache;
        private IModelRenderer _modelRenderer;

        public SatelliteDrawNode(RenderModel satellite, ICache<string, int> textureCache)
        {
            Satellite = satellite;

            _textureCache = textureCache;

            _scale = satellite.Scale;

            _model = satellite.Model;

            _device = new B.Device();

            _modelRenderer = new ModelRenderer(_model, textureCache);

            _sp = _device.CreateShaderProgram(satelliteVS, satelliteFS);

            _dirty = true;

            A.GL.BindAttribLocation(_sp.Handle, (int)0, "POSITION");
            A.GL.BindAttribLocation(_sp.Handle, (int)1, "NORMAL");
            A.GL.BindAttribLocation(_sp.Handle, (int)2, "TEXCOORD");
        }

        public RenderModel Satellite { get; set; }

        private void SetUniforms(dmat4 modelMatrix, ISceneState scene)
        {
            var model = modelMatrix * dmat4.Scale(new dvec3(_scale, _scale, _scale));
            var view = scene.ViewMatrix;
            var normalMatrix = (new dmat3((view * model).Inverse).Transposed);
            var mvp = scene.ProjectionMatrix * view * model;
            var modelView = view * model;

            _sp.SetUniform("u_model", model.ToMat4());
            _sp.SetUniform("u_view", view.ToMat4());
            _sp.SetUniform("u_normalMatrix", normalMatrix.ToMat3());
            _sp.SetUniform("u_modelView", modelView.ToMat4());
            _sp.SetUniform("u_mvp", mvp.ToMat4());

            _sp.SetUniform("light.position", scene.LightPosition.ToVec4());
            _sp.SetUniform("light.ambient", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
            _sp.SetUniform("light.diffuse", new vec4(1.0f, 1.0f, 1.0f, 1.0f));
            _sp.SetUniform("light.specular", new vec4(0.7f, 0.7f, 0.7f, 1.0f));      
        }
       
        public override void UpdateGeometry()
        {
            if (_dirty == true)
            {
                _modelRenderer.SetupTextures();

                _dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, 
            ISceneState scene)
        {
            if (_dirty == false)
            {
                _sp.Bind();

                SetUniforms(modelMatrix, scene);
             
                _modelRenderer.Draw(_sp);

                B.ShaderProgram.UnBind();
            }
        }  
    }


}
