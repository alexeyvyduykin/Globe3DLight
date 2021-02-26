#version 330

layout(lines) in;
layout(triangle_strip, max_vertices = 6) out;
//layout(line_strip, max_vertices = 2) out;

uniform mat4 u_mvp;

uniform vec4 u_positionTarget;
uniform float u_scaleSource;
uniform float u_scaleTarget;

out vec4 clr;

//const vec4 color = vec4(1.0, 1.0, 0.0, 0.95);
const vec4 color = vec4(1.0, 1.0, 0.0, 0.20);

vec4 scaleVector(vec4 v, float scale);

void main ()
{
  vec4 v0 = gl_in[0].gl_Position;
  vec4 v2 = gl_in[1].gl_Position;
  
  v0 =  scaleVector(v0, u_scaleSource); 
  v2 =  scaleVector(v2, u_scaleSource); 
  
  vec4 v1 = gl_in[0].gl_Position;
  vec4 v3 = gl_in[1].gl_Position;  
  
  v1 = u_positionTarget + scaleVector(v1, u_scaleTarget); 
  v3 = u_positionTarget + scaleVector(v3, u_scaleTarget); 


  gl_Position = u_mvp * v0;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * v1;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * v2;
  clr = color;
  EmitVertex();
  EndPrimitive();

  gl_Position = u_mvp * v2;
  clr = color;
  EmitVertex();
	
  gl_Position = u_mvp * v1;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * v3;
  clr = color;
  EmitVertex();
  EndPrimitive();

}


vec4 scaleVector(vec4 v, float scale)
{
  vec4 res;
  res.x = v.x * scale;
  res.y = v.y;
  res.z = v.z * scale;
  res.w = v.w;
  
  return res;
}
