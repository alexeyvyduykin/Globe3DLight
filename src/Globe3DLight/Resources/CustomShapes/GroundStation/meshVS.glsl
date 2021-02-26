#version 330

layout (location = 0) in vec3 POSITION;

uniform mat4 u_mvp;

void main(void)
{
gl_Position = u_mvp * vec4(POSITION, 1.0);
}