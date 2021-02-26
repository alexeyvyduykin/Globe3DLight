#version 330

layout (location = 0) in vec3 POSITION;

uniform mat4 u_mvp;

out vec3 v_texCoords;

void main(void)
{
  gl_Position = u_mvp * vec4(POSITION, 1.0);
  v_texCoords = POSITION;
}