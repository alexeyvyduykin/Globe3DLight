#version 330 

layout(lines) in;
layout(triangle_strip, max_vertices = 4) out;
//layout(line_strip, max_vertices = 2) out;

in vec4 gs_Color[];
flat out vec4 fs_Color;

uniform mat4 u_mvp;
uniform mat4 u_viewportMatrix;
uniform mat4 u_viewportOrtho;
uniform float u_perspectiveNearPlaneDistance;
uniform float u_fillDistance;

vec4 ClipToWindow(vec4 clipPosition, mat4 viewportMatrix)
{
  clipPosition.xyz /= clipPosition.w;                         // normalized device coordinates
  vec4 windowPosition = viewportMatrix * vec4(clipPosition.xyz, 1.0);
  return windowPosition;
}

void ClipLineSegmentToNearPlane(
    float nearPlaneDistance, 
    mat4 modelViewPerspectiveMatrix,
    vec4 modelP0, 
    vec4 modelP1, 
    out vec4 clipP0, 
    out vec4 clipP1,
   out bool culledByNearPlane)
{
    clipP0 = modelViewPerspectiveMatrix * modelP0;
    clipP1 = modelViewPerspectiveMatrix * modelP1;
   culledByNearPlane = false;

    float distanceToP0 = clipP0.z + nearPlaneDistance;
    float distanceToP1 = clipP1.z + nearPlaneDistance;

    if ((distanceToP0 * distanceToP1) < 0.0)
    {
        float t = distanceToP0 / (distanceToP0 - distanceToP1);
        vec3 modelV = vec3(modelP0) + t * (vec3(modelP1) - vec3(modelP0));
        vec4 clipV = modelViewPerspectiveMatrix * vec4(modelV, 1);

        if (distanceToP0 < 0.0)
        {
            clipP0 = clipV;
        }
        else
        {
            clipP1 = clipV;
        }
    }
   else if (distanceToP0 < 0.0)
   {
       culledByNearPlane = true;
   }
}

void main()
{
    vec4 clipP0;
    vec4 clipP1;
   bool culledByNearPlane;
    ClipLineSegmentToNearPlane(u_perspectiveNearPlaneDistance, 
      u_mvp,
      gl_in[0].gl_Position, gl_in[1].gl_Position, clipP0, clipP1, culledByNearPlane);

   if (culledByNearPlane)
   {
      return;
   }

    vec4 windowP0 = ClipToWindow(clipP0, u_viewportMatrix);
    vec4 windowP1 = ClipToWindow(clipP1, u_viewportMatrix);

    vec2 direction = windowP1.xy - windowP0.xy;
    vec2 normal = normalize(vec2(direction.y, -direction.x));

    vec4 v0 = vec4(windowP0.xy - (normal * u_fillDistance), -windowP0.z, 1.0);
    vec4 v1 = vec4(windowP1.xy - (normal * u_fillDistance), -windowP1.z, 1.0);
    vec4 v2 = vec4(windowP0.xy + (normal * u_fillDistance), -windowP0.z, 1.0);
    vec4 v3 = vec4(windowP1.xy + (normal * u_fillDistance), -windowP1.z, 1.0);

	//v0 /= 36.0;
	//v1 /= 36.0;
	//v2 /= 36.0;
	//v3 /= 36.0;

  //  vec4 clipP0 = u_mvp * gl_in[0].gl_Position;
  //  vec4 clipP1 = u_mvp * gl_in[1].gl_Position;

//	vec4 screenP0 = ClipToWindow(clipP0, u_viewportMatrix);
//	vec4 screenP1 = ClipToWindow(clipP1, u_viewportMatrix);

    gl_Position = u_viewportOrtho * v0; 
    fs_Color = gs_Color[0];
    EmitVertex();

    gl_Position = u_viewportOrtho * v1;
    fs_Color = gs_Color[0];
    EmitVertex();

    gl_Position = u_viewportOrtho * v2;
    fs_Color = gs_Color[0];
    EmitVertex();

    gl_Position = u_viewportOrtho * v3;
    fs_Color = gs_Color[0];
    EmitVertex();
}