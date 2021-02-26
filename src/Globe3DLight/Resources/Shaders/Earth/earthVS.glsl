#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec3 NORMAL;
layout (location = 2) in vec2 TEXCOORD;
layout (location = 3) in vec3 TANGENT;

uniform mat4 u_model;
uniform mat4 u_mvp;
uniform mat3 u_normalMatrix;
uniform mat4 u_view;
uniform mat4 u_modelView;

uniform vec4 u_lightPosition;     // lightPosition_WS

out vec2 v_texCoord;
out vec3 v_lightDir_CS;
out vec3 v_viewDir_CS;
out vec3 v_normal;


out vec3 v_lightDir_TS;
out vec3 v_viewDir_TS;

out float v_diffuse;

float RE = 10.0;// 11.903203;

void main(void)
{
  vec4 vertex    = vec4(normalize(POSITION.xyz) * RE, 1.0);
  vec3 vertex_CS = (u_modelView * vertex).xyz;
  vec3 vertex_WS = (u_model * vertex).xyz;

  v_normal = (u_normalMatrix*NORMAL).xyz;
  v_texCoord = TEXCOORD;
  vec3 normal = (u_model * vec4(NORMAL, 0.0)).xyz;

v_lightDir_CS = ( u_view * u_lightPosition ).xyz - vertex_CS;
v_viewDir_CS = -vertex_CS;

vec3 v_lightDir_WS = vec3(u_lightPosition) - vertex_WS;;
v_diffuse = dot(v_lightDir_WS, normal);

vec3 n = normalize( u_normalMatrix * NORMAL );
vec3 t = normalize( u_normalMatrix * TANGENT );
vec3 b = normalize( u_normalMatrix * cross( NORMAL, TANGENT.xyz) );

vec3 temp = ( u_view * u_lightPosition ).xyz - vertex_CS;

v_lightDir_TS.x = dot( temp, t );
v_lightDir_TS.y = dot( temp, b );
v_lightDir_TS.z = dot( temp, n );

v_viewDir_TS.x = dot( -vertex_CS, t );
v_viewDir_TS.y = dot( -vertex_CS, b );
v_viewDir_TS.z = dot( -vertex_CS, n );

gl_Position = u_mvp * vertex;
}

