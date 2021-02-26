#version 400

layout(lines, invocations = 5) in;
layout(line_strip, max_vertices = 2) out;

uniform mat4 u_mvp;
uniform float u_deltaStep;
uniform float u_scaleSource;
uniform float u_scaleTarget;
uniform mat4 u_matrixSource;
uniform mat4 u_matrixTarget;

out vec4 clr;
  
const vec4 color = vec4(0.8, 0.8, 0.8, 0.9);
//const vec4 color = vec4(1.0, 1.0, 0.0, 0.85);


vec4 scaleVector(vec4 v, float scale);

void main ()
{
  float step = gl_InvocationID/5.0;

  vec4 v0 = gl_in[0].gl_Position;
  vec4 v1 = gl_in[1].gl_Position;

  v0 = u_matrixSource * scaleVector(v0, u_scaleSource); 
  v1 = u_matrixSource * scaleVector(v1, u_scaleSource); 

  vec4 v2 = gl_in[0].gl_Position;
  vec4 v3 = gl_in[1].gl_Position;  
  
  v2 = u_matrixTarget * scaleVector(v2, u_scaleTarget); 
  v3 = u_matrixTarget * scaleVector(v3, u_scaleTarget); 


  vec4 res1 = vec4(1.0), 
       res2 = vec4(1.0);
  float m1, m2;

  m2 = step + u_deltaStep;
  m1 = 1.0 - m2;
  
  for( int i = 0; i < 3; i++ )
  {
    res1[i] = ( v2[i] * m2 + v0[i] * m1 ) / ( m1 + m2 );
    res2[i] = ( v3[i] * m2 + v1[i] * m1 ) / ( m1 + m2 );
  }

  gl_Position = u_mvp * res1;
  clr = color;
  EmitVertex();

  gl_Position = u_mvp * res2;
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