#version 330

uniform sampler2D u_diffuseMap;
uniform sampler2D u_specularMap;
uniform sampler2D u_normalMap;
uniform sampler2D u_nightMap;

uniform float u_offsetU;
uniform float u_offsetV;

uniform mat4x2 u_modelZToClipCoordinates;

in vec2 v_texCoord;

in vec3 v_normal;  
in vec3 v_lightDir_CS;
in vec3 v_viewDir_CS;

in vec3 v_lightDir_TS;
in vec3 v_viewDir_TS; 

in float v_diffuse;

in vec3 v_worldPosition;

out vec4 color;


const float Terminator = 7.0;
const float InvTerminator = 1.0 / (2.0 * Terminator);

const vec4 MaterialAmbient = vec4(0.1, 0.1, 0.1, 1.0);
const float MaterialShininess = 5.0;

const vec4 LightAmbient = vec4(1.0, 1.0, 1.0, 1.0); // vec4(0.6, 0.6, 0.1, 1.0);
const vec4 LightDiffuse = vec4(1.0, 1.0, 1.0, 1.0); // vec4(1.0, 1.0, 0.75, 1.0);
const vec4 LightSpecular = vec4(1.0, 1.0, 1.0, 1.0); // vec4(0.6, 0.6, 0.6, 1.0);

float Pi = 3.1415926535897932384626433832795;
float OneOverTwoPi = 1.0 / (2.0 * Pi);
float OneOverPi = 1.0 / Pi;

vec3 GeodeticSurfaceNormal(vec3 positionOnEllipsoid, vec3 oneOverEllipsoidRadiiSquared)
{
    return normalize(positionOnEllipsoid * oneOverEllipsoidRadiiSquared);
}

vec2 ComputeTextureCoordinates(vec3 normal)
{
    //return vec2(atan(normal.y, normal.x) * OneOverTwoPi + 0.5, asin(normal.z) * OneOverPi + 0.5);

	return vec2(atan(normal.x, normal.z) * OneOverTwoPi + 0.5, asin(normal.y) * OneOverPi + 0.5);
}

float ComputeWorldPositionDepth(vec3 position, mat4x2 modelZToClipCoordinates)
{ 
    vec2 v = modelZToClipCoordinates * vec4(position, 1);   // clip coordinates
    v.x /= v.y;                                             // normalized device coordinates
    v.x = (v.x + 1.0) * 0.5;
    return v.x;
}

vec2 u_gridResolution = vec2(0.25, 0.25);
vec2 u_gridLineWidth = vec2(1.0, 1.0);

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



           vec3 oneOverRadiiSquared = vec3(
                1.0 / (10.0 * 10.0),
                1.0 / (10.0 * 10.0),
                1.0 / (10.0 * 10.0));

    vec3 normalOther = GeodeticSurfaceNormal(v_worldPosition, oneOverRadiiSquared);
    vec2 textureCoordinate = ComputeTextureCoordinates(normalOther);

	vec2 distanceToLine = mod(textureCoordinate, u_gridResolution);
    vec2 dx = abs(dFdx(textureCoordinate));
    vec2 dy = abs(dFdy(textureCoordinate));
    vec2 dF = vec2(max(dx.s, dy.s), max(dx.t, dy.t)) * u_gridLineWidth;


vec2 newTexCoord = v_texCoord;
newTexCoord.y = 1.0 - newTexCoord.y;

//newTexCoord = textureCoordinate;
//newTexCoord.y = 1.0 - newTexCoord.y;


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


    if (any(lessThan(distanceToLine, dF)))
    {
        color = vec4(1.0, 0.0, 0.0, 1.0);
    }
	else
	{
	  color = finalColor; 
	}


//gl_FragColor = diffuse + specular + vec4(0.18, 0.18, 0.18, 1.0);
color = finalColor; 
}
