#version 330

layout (location = 0) in vec2 POSITION;

// Uniforms
uniform mat4 u_view;
uniform mat4 u_proj;
uniform vec3 u_center;
uniform vec2 u_dims;
//uniform mat4 u_mvp;

// Output
out vec2 fPosition;

void main() {

fPosition = POSITION;
    
gl_Position = u_proj * u_view * vec4(u_center, 1.0);
gl_Position /= gl_Position.w;
gl_Position.xy += POSITION * u_dims;


//gl_Position = u_mvp * vec4(POSITION.x * u_dims, POSITION.y * u_dims, 0.0, 1.0);

}