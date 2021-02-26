#version 330

layout(points) in;
layout(triangle_strip, max_vertices = 12) out;

uniform mat4 u_mvp;

uniform vec4 u_P1;
uniform vec4 u_P2;
uniform vec4 u_P3;
uniform vec4 u_P4;

out vec4 clr;

const vec4 color = vec4(0.0, 1.0, 1.0, 0.25);

void main ()
{
  gl_Position = u_mvp * gl_in[0].gl_Position;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * u_P1;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * u_P2;
  clr = color;
  EmitVertex();
  EndPrimitive();

  gl_Position = u_mvp * gl_in[0].gl_Position;
  clr = color;
  EmitVertex();
	
  gl_Position = u_mvp * u_P2;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * u_P3;
  clr = color;
  EmitVertex();
  EndPrimitive();

  gl_Position = u_mvp * gl_in[0].gl_Position;
  clr = color;
  EmitVertex();
	
  gl_Position = u_mvp * u_P3;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * u_P4;
  clr = color;
  EmitVertex();
  EndPrimitive();

  gl_Position = u_mvp * gl_in[0].gl_Position;
  clr = color;
  EmitVertex();
	
  gl_Position = u_mvp * u_P4;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * u_P1;
  clr = color;
  EmitVertex();
  EndPrimitive();

}

