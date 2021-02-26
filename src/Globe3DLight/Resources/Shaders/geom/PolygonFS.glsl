#version 330
        
in vec3 v_worldPosition;
in vec3 v_positionToLight;
in vec3 v_positionToEye;
out vec4 fragmentColor;

uniform vec4 u_diffuseSpecularAmbientShininess;
uniform vec3 u_globeOneOverRadiiSquared;
uniform vec4 u_color;

float LightIntensity(vec3 normal, vec3 toLight, vec3 toEye, vec4 diffuseSpecularAmbientShininess)
{
    vec3 toReflectedLight = reflect(-toLight, normal);

    float diffuse = max(dot(toLight, normal), 0.0);
    float specular = max(dot(toReflectedLight, toEye), 0.0);
    specular = pow(specular, diffuseSpecularAmbientShininess.w);

    return (diffuseSpecularAmbientShininess.x * diffuse) +
            (diffuseSpecularAmbientShininess.y * specular) +
            diffuseSpecularAmbientShininess.z;
}

vec3 GeodeticSurfaceNormal(vec3 positionOnEllipsoid, vec3 oneOverEllipsoidRadiiSquared)
{
    return normalize(positionOnEllipsoid * oneOverEllipsoidRadiiSquared);
}

void main()
{
	//vec3 normal = GeodeticSurfaceNormal(v_worldPosition, u_globeOneOverRadiiSquared);
    //float intensity = LightIntensity(normal,  normalize(v_positionToLight), normalize(v_positionToEye), u_diffuseSpecularAmbientShininess);

	fragmentColor = vec4(/*intensity **/ u_color.rgb, u_color.a);
}