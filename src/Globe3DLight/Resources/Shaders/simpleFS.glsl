#version 330

in vec2 v_texCoord;
uniform sampler2D textureId;

out vec4 color;

 
void main()
{
  color = texture(textureId, v_texCoord);
}
