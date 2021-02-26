#version 330

flat in vec4 fsColor;
out vec4 fragmentColor;

void main()
{
    fragmentColor = fsColor;
}