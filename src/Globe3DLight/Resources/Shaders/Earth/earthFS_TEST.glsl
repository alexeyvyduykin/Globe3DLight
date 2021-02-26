#version 330

uniform sampler2D u_diffuseMap;
//uniform sampler2D u_specularMap;
//uniform sampler2D u_normalMap;
//uniform sampler2D u_nightMap;
//uniform float u_offsetU;
//uniform float u_offsetV;

uniform struct Material
{
  sampler2D texture;

  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
  vec4 emission;
  float shininess;
} u_material;

uniform struct Light
{
  vec4 position;       // lightPosition_WS
  vec4 ambient;
  vec4 diffuse;
  vec4 specular;
}u_light;

in vec2 v_texCoord;

in vec3 v_normal;  
in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;

out vec4 color;

void main(void)
{
vec3 normal = normalize(v_normal);            
vec3 normViewDir_CS = normalize(v_viewDir_CS);
vec3 normLightDir_CS = normalize(v_lightDir_CS);

//float px = (v_texCoord.x - u_offsetU) * 4.0 - 1.0;
//float py = (v_texCoord.y - u_offsetV) * 2.0 - 1.0;
//vec2 newTexCoord = vec2(px, py);
vec2 newTexCoord = v_texCoord;

newTexCoord.y = 1.0 - newTexCoord.y;

vec4 earthDiffSampler = texture2D(u_diffuseMap, newTexCoord.st);

float diff = clamp(dot(normLightDir_CS, normal ), 0.0, 1.0);
float spec = max(pow(dot(reflect(-normLightDir_CS, normal ), normViewDir_CS), u_material.shininess), 0.0);

vec4 ambient  = u_material.ambient * u_light.ambient;
vec4 diffuse  = diff * u_light.diffuse * earthDiffSampler;
vec4 specular = spec * u_light.specular;

vec4 finalColor = diffuse + specular;

color = finalColor;

//color = diffuse + specular + vec4(0.18, 0.18, 0.18, 1.0);
//color = texture2D(u_diffuseMap, newTexCoord.st);
//gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);; 
}
