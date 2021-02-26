#version 330

uniform sampler2D u_diffuseMap;
uniform sampler2D u_specularMap;
uniform sampler2D u_normalMap;
uniform sampler2D u_nightMap;

uniform float u_offsetU;
uniform float u_offsetV;


in vec2 v_texCoord;

in vec3 v_normal;  
in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;

in vec3 v_lightDir_TS;
in vec3 v_viewDir_TS; 

in float v_diffuse;


out vec4 color;


const float Terminator = 7.0;
const float InvTerminator = 1.0 / (2.0 * Terminator);

const vec4 MaterialAmbient = vec4(0.1, 0.1, 0.1, 1.0);
const float MaterialShininess = 5.0;

const vec4 LightAmbient = vec4(1.0, 1.0, 1.0, 1.0); // vec4(0.6, 0.6, 0.1, 1.0);
const vec4 LightDiffuse = vec4(1.0, 1.0, 1.0, 1.0); // vec4(1.0, 1.0, 0.75, 1.0);
const vec4 LightSpecular = vec4(1.0, 1.0, 1.0, 1.0); // vec4(0.6, 0.6, 0.6, 1.0);


void main(void)
{
vec3 normal = normalize(v_normal);            

vec3 normalizedViewDir = normalize(v_viewDir_TS);   
vec3 normalizedLightDir = normalize(v_lightDir_TS);

vec3 normViewDir_CS = normalize(v_viewDir_CS);
vec3 normLightDir_CS = normalize(v_lightDir_CS);

float px = (v_texCoord.x - u_offsetU) * 4.0 - 1.0;
float py = (v_texCoord.y - u_offsetV) * 2.0 - 1.0;

//vec2 newTexCoord = vec2( px, py);

vec2 newTexCoord = v_texCoord;
newTexCoord.y = 1.0 - newTexCoord.y;

vec4 earthDiffSampler = texture2D(u_diffuseMap, newTexCoord.st);


//float distX = v_texCoord.x - 0.25;
//if(distX < 0.0)
//  distX += 1.00;

//newTexCoord = vec2( distX, v_texCoord.y);



vec4 earthSpecSampler = texture2D(u_specularMap, newTexCoord.st);
vec4 earthNormSampler = texture2D(u_normalMap, newTexCoord.st);
vec4 earthNightSampler = texture2D(u_nightMap, newTexCoord.st);

vec3 pixelNormal = normalize( ( earthNormSampler ).xyz * 2.0 - 1.0 );

float diff = clamp(dot(normalizedLightDir, pixelNormal), 0.0, 1.0);
//float diff = clamp(dot(normLightDir_CS, normal ), 0.0, 1.0);

float spec = max(pow(dot(reflect(-normLightDir_CS, normal), normViewDir_CS), MaterialShininess), 0.0);

vec4 ambient  = MaterialAmbient * LightAmbient;
vec4 diffuse  = diff * earthDiffSampler * LightDiffuse;
vec4 specular = spec * earthSpecSampler * LightSpecular;

vec4 finalColor = ambient + diffuse + specular;

if( v_diffuse < -Terminator )
{
finalColor = earthNightSampler;
}

if( abs(v_diffuse) <= Terminator )
{
finalColor = mix(earthNightSampler, finalColor, (v_diffuse + Terminator) * InvTerminator);
}

//gl_FragColor = diffuse + specular + vec4(0.18, 0.18, 0.18, 1.0);
color = finalColor; 
}
