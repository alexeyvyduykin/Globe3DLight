#version 330

layout(location = 0) in vec4 POSITION;
layout(location = 1) in vec4 COLOR;

out vec4 gs_Color;
//uniform mat4 u_mvp;

void main()                     
{
	gl_Position = POSITION;
	gs_Color = COLOR;
}