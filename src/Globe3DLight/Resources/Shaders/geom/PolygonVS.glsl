#version 330

layout(location = 0) in vec4 POSITION;

out vec3 v_worldPosition;
out vec3 v_positionToLight;
out vec3 v_positionToEye;

uniform mat4 u_mvp;
uniform vec3 u_cameraEye;
uniform vec3 u_cameraLightPosition;

void main()                     
{
    gl_Position = u_mvp * POSITION; 

    v_worldPosition = POSITION.xyz;
    v_positionToLight = u_cameraLightPosition - v_worldPosition;
    v_positionToEye = u_cameraEye - v_worldPosition;
}