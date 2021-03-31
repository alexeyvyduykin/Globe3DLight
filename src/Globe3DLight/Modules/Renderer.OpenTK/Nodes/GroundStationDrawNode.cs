using System;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.Renderer.OpenTK.Core;
using Globe3DLight.ViewModels.Geometry.Models;
using Globe3DLight.ViewModels.Scene;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class GroundStationDrawNode : DrawNode, IGroundStationDrawNode
    {
        private Device _device;
        //private B.Context _context;  
        private readonly string groundStationVS = @"
#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec3 NORMAL;

uniform mat4 u_view;
uniform mat4 u_model;
uniform mat3 u_normalMatrix;
uniform mat4 u_modelView;
uniform mat4 u_mvp;
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

void main(void)
{
vec4 vertex_MS = vec4(POSITION.xyz, 1.0);
vec3 vertex_WS = (u_model * vertex_MS).xyz;
vec3 vertex_CS = (u_modelView * vertex_MS).xyz;

vec3 lightPosition_CS = (u_view * light.position).xyz;
vec3 cameraPosition_CS = vec3(0.0, 0.0, 0.0);

v_lightDir_CS = lightPosition_CS - vertex_CS;
v_viewDir_CS = cameraPosition_CS - vertex_CS;

v_normal_CS = (u_modelView * vec4(NORMAL, 0.0)).xyz;

gl_Position = u_mvp * vertex_MS;
}";
        private readonly string groundStationFS = @"
#version 330

in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;
in vec3 v_normal_CS;

out vec4 color;

uniform struct Material
{
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
  vec4 emission;
  float shininess;
} material;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}light;

void main(void)
{
vec4 finalColor;

vec3 n = normalize(v_normal_CS);
vec3 l = normalize(v_lightDir_CS);
vec3 v = normalize(v_viewDir_CS);

finalColor = material.emission;

float NdotL = max(dot( n, l ), 0.0);
finalColor += material.diffuse * light.diffuse * NdotL;

float materialShininess = 20.0f; // material.shininess;

float RdotVpow = max(pow(dot(reflect(-l, n), v), materialShininess), 0.0);
finalColor += material.specular * light.specular * RdotVpow;

color = finalColor;
}";
        private bool _dirty;
        private readonly ShaderProgram _sp;
        //private readonly B.DrawState _drawState;
        private readonly double _scale;
        private readonly IMesh _mesh;
        private ModelRenderer__ _modelRenderer;
        //private readonly B.Uniform<mat4> u_mvp;
        //private readonly B.Uniform<vec4> u_color;

        public GroundStationDrawNode(GroundStationRenderModel groundStation)
        {
            GroundStation = groundStation;

            //_context = new B.Context();
            _device = new Device();
            _dirty = true;

            _mesh = groundStation.Mesh;
            _scale = groundStation.Scale;

            _sp = _device.CreateShaderProgram(groundStationVS, groundStationFS);

            _modelRenderer = new ModelRenderer__(_mesh);

            //   u_mvp = ((B.Uniform<mat4>)_sp.Uniforms["u_mvp"]);
            //   u_color = ((B.Uniform<vec4>)_sp.Uniforms["u_color"]);

            //   u_color.Value = new vec4(0.565f, 0.537f, 0.518f, 1.0f); // #908984

            //_drawState = new B.DrawState();
            //_drawState.ShaderProgram = _sp;

            A.GL.BindAttribLocation(_sp.Handle, (int)0, "POSITION");
            A.GL.BindAttribLocation(_sp.Handle, (int)1, "NORMAL");
        }

        public GroundStationRenderModel GroundStation { get; set; }

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
            if (_dirty)
            {
                //_drawState.VertexArray = _context.CreateVertexArray_NEW(_mesh, _drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);
                //_drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Front;
                //_drawState.RenderState.FacetCulling.FrontFaceWindingOrder = A.FrontFaceDirection.Cw;
                _dirty = false;
            }
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            _sp.Bind();

            SetUniforms(modelMatrix, scene);

            _modelRenderer.Draw(_sp);

            //_context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

            ShaderProgram.UnBind();
        }
    }

    internal class ModelRenderer__
    {
        private readonly IMesh _mesh;
        private int _vao, _vbo, _ebo;

        public ModelRenderer__(IMesh mesh)
        {
            _mesh = mesh;
            SetupMeshes();
        }

        private struct Vertex
        {
            public vec3 position;
            public vec3 normal;
        }

        public void Draw(ShaderProgram sp)
        {
            sp.SetUniform("material.ambient", new vec4(0.0f, 0.0f, 0.0f, 1.0f));
            sp.SetUniform("material.diffuse", new vec4(0.565f, 0.537f, 0.518f, 1.0f));
            sp.SetUniform("material.specular", new vec4(0.0f, 0.0f, 0.0f, 1.0f));
            sp.SetUniform("material.emission", new vec4(0.0f, 0.0f, 0.0f, 1.0f));
            sp.SetUniform("material.shininess", 10.0f);

            // Draw mesh
            A.GL.BindVertexArray(_vao);
            A.GL.DrawElements(A.BeginMode.Triangles, _mesh.Indices.Count, A.DrawElementsType.UnsignedShort, 0);
            A.GL.BindVertexArray(0);
        }

        private void SetupMeshes()
        {
            var mesh = _mesh;

            var vertices = new Vertex[mesh.Vertices.Count];

            for (int j = 0; j < mesh.Vertices.Count; j++)
            {
                vertices[j] = new Vertex()
                {
                    position = mesh.Vertices[j],
                    normal = mesh.Normals[j]
                };
            }

            // Create buffers/arrays
            _vao = A.GL.GenVertexArray();
            _vbo = A.GL.GenBuffer();
            _ebo = A.GL.GenBuffer();

            A.GL.BindVertexArray(_vao);
            // Load data into vertex buffers
            A.GL.BindBuffer(A.BufferTarget.ArrayBuffer, _vbo);
            // A great thing about structs is that their memory layout is sequential for all its items.
            // The effect is that we can simply pass a pointer to the struct and it translates perfectly to a glm::vec3/2 array which
            // again translates to 3/2 floats which translates to a byte array.
            A.GL.BufferData(A.BufferTarget.ArrayBuffer, new IntPtr(ArraySizeInBytes.Size<Vertex>(vertices.ToArray())),
                vertices.ToArray(), A.BufferUsageHint.StaticDraw);

            A.GL.BindBuffer(A.BufferTarget.ElementArrayBuffer, _ebo);
            A.GL.BufferData(A.BufferTarget.ElementArrayBuffer, new IntPtr(ArraySizeInBytes.Size<ushort>(mesh.Indices.ToArray())),
                mesh.Indices.ToArray(), A.BufferUsageHint.StaticDraw);

            // Set the vertex attribute pointers
            // Vertex Positions           
            A.GL.VertexAttribPointer((int)0, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, 0);
            A.GL.EnableVertexAttribArray((int)0);
            // Vertex Normals            
            A.GL.VertexAttribPointer((int)1, 3, A.VertexAttribPointerType.Float, false, SizeInBytes<Vertex>.Value, SizeInBytes<vec3>.Value);
            A.GL.EnableVertexAttribArray((int)1);

            A.GL.BindVertexArray(0);
        }
    }
}
