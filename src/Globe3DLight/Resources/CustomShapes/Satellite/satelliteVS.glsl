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
}