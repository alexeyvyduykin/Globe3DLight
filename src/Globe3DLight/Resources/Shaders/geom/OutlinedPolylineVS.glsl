#version 330

layout(location = 0) in vec4 POSITION;
layout(location = 1) in vec4 COLOR;
layout(location = 2) in vec4 OUTLINECOLOR;
//in vec4 v_outlineColor;

out vec4 gsColor;
out vec4 gsOutlineColor;

void main()                     
{
	gl_Position = POSITION;
	gsColor = COLOR;
	gsOutlineColor = OUTLINECOLOR;// v_outlineColor;
}