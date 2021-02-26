#version 330

uniform samplerCube u_spacebox;

in vec3 v_texCoords;

out vec4 color;

void main(void)
{
  color = texture(u_spacebox, v_texCoords);
}