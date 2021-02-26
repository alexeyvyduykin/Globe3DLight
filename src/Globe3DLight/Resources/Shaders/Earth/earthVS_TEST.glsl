#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec3 NORMAL;
layout (location = 2) in vec2 TEXCOORD;
layout (location = 3) in vec3 TANGENT;
//attribute vec3 BITANGENT;

uniform mat4 u_model;
uniform mat4 u_mvp;
uniform mat3 u_normalMatrix;
uniform mat4 u_view;
uniform mat4 u_modelView;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}u_light;

out vec2 v_texCoord;
out vec3 v_lightDir_CS;
out vec3 v_viewDir_CS;
out vec3 v_normal;

void main(void)
{
  vec4 vertex    = vec4(POSITION.xyz, 1.0);
  vec3 vertex_CS = (u_modelView * vertex).xyz;
  vec3 vertex_WS = (u_model * vertex).xyz;

  v_normal = (u_normalMatrix*NORMAL).xyz;
  v_texCoord = TEXCOORD;
  vec3 normal = (u_model * vec4(NORMAL, 0.0)).xyz;

v_lightDir_CS = ( u_view * u_light.position ).xyz - vertex_CS;
v_viewDir_CS = -vertex_CS;

gl_Position = u_mvp * vertex;
}
