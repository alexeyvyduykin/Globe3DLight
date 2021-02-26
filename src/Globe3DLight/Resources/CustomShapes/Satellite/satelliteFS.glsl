#version 330

in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;
in vec3 v_normal_CS;
in vec2 v_texCoord;

out vec4 color;

uniform struct Material
{
  sampler2D texture;

  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
  vec4 emission;
  float shininess;
} material;

uniform float u_isTexture;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}light;

// Light
//uniform vec4 lightAmbient;//    = vec4(1.0, 1.0, 1.0, 1.0);
//uniform vec4 lightDiffuse;//    = vec4(1.0, 1.0, 1.0, 1.0);
//uniform vec4 lightSpecular;//   = vec4(1.0, 1.0, 1.0, 1.0);

void main(void)
{
vec4 finalColor;
vec4 sampler = texture2D(material.texture, v_texCoord);

vec3 n = normalize(v_normal_CS);
vec3 l = normalize(v_lightDir_CS);
vec3 v = normalize(v_viewDir_CS);

finalColor = material.emission;

//finalColor += material.ambient * light.ambient;

float NdotL = max(dot( n, l ), 0.0);
finalColor += material.diffuse * light.diffuse * NdotL;

float RdotVpow = max(pow(dot(reflect(-l, n), v), material.shininess), 0.0);
finalColor += material.specular * light.specular * RdotVpow;

if( u_isTexture == 1.0 )
  finalColor *= sampler;  

color = finalColor;
}