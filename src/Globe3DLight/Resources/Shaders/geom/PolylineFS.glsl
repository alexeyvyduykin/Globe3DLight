#version 330

flat in vec4 fs_Color;
out vec4 fragmentColor;

void main()
{
    fragmentColor = fs_Color;
}