#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec2 TEXCOORD;

uniform mat4 u_mvp; 

out vec2 v_texCoord;

void main()
{
  v_texCoord = TEXCOORD;
  gl_Position = u_mvp * vec4(POSITION, 1.0);
}