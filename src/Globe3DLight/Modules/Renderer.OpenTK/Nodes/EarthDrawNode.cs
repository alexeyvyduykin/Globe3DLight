#nullable enable
using System;
using System.Collections.Immutable;
using GlmSharp;
using Globe3DLight.Models.Image;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Geometry;
using Globe3DLight.ViewModels.Scene;
using A = OpenTK.Graphics.OpenGL;
using B = Globe3DLight.Renderer.OpenTK.Core;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class EarthDrawNode : DrawNode, IEarthDrawNode, IDisposable
    {
        private readonly B.Context _context; 
        private B.Device _device;
        private readonly B.Uniform<mat4> u_model;
        private readonly B.Uniform<mat4> u_mvp;
        private readonly B.Uniform<mat3> u_normalMatrix;
        private readonly B.Uniform<mat4> u_view;
        private readonly B.Uniform<mat4> u_modelView;
        private readonly B.Uniform<vec4> u_lightPosition;
        //  private readonly Uniform<mat4x2> u_modelZToClipCoordinates;
        private readonly B.Uniform<float> u_radius;
        private readonly ImmutableArray<string> _keys;
        private readonly B.Texture2D[] _maps;
        private int _currentLoadingTexture = 0;
        //private string[] keys = new string[] { "PosX", "NegZ", "NegX", "PosZ", "PosY", "NegY" };
        private bool _dirty;
        private readonly ImmutableArray<Mesh> _meshes;
        private readonly B.ShaderProgram _sp;
        private readonly B.DrawState[] _drawStates;
        private readonly string _earthGridVS = @"
#version 330

layout (location = 0) in vec3 POSITION;
layout (location = 1) in vec3 NORMAL;
layout (location = 2) in vec2 TEXCOORD;
layout (location = 3) in vec3 TANGENT;

uniform mat4 u_model;
uniform mat4 u_mvp;
uniform mat3 u_normalMatrix;
uniform mat4 u_view;
uniform mat4 u_modelView;

uniform vec4 u_lightPosition;     // lightPosition_WS

uniform float u_radius;

out vec2 v_texCoord;
out vec3 v_lightDir_CS;
out vec3 v_viewDir_CS;
out vec3 v_normal;

out vec3 v_worldPosition;

out vec3 v_lightDir_TS;
out vec3 v_viewDir_TS;

out float v_diffuse;

void main(void)
{ 
  v_worldPosition = normalize(POSITION.xyz) * u_radius;

  vec4 vertex    = vec4(normalize(POSITION.xyz) * u_radius, 1.0);
  vec3 vertex_CS = (u_modelView * vertex).xyz;
  vec3 vertex_WS = (u_model * vertex).xyz;

  v_normal = (u_normalMatrix*NORMAL).xyz;
  v_texCoord = TEXCOORD;
  vec3 normal = (u_model * vec4(NORMAL, 0.0)).xyz;

  v_lightDir_CS = ( u_view * u_lightPosition ).xyz - vertex_CS;
  v_viewDir_CS = -vertex_CS;

  vec3 v_lightDir_WS = vec3(u_lightPosition) - vertex_WS;;
  v_diffuse = dot(v_lightDir_WS, normal);

  vec3 n = normalize( u_normalMatrix * NORMAL );
  vec3 t = normalize( u_normalMatrix * TANGENT );
  vec3 b = normalize( u_normalMatrix * cross( NORMAL, TANGENT.xyz) );

  vec3 temp = ( u_view * u_lightPosition ).xyz - vertex_CS;

  v_lightDir_TS.x = dot( temp, t );
  v_lightDir_TS.y = dot( temp, b );
  v_lightDir_TS.z = dot( temp, n );

  v_viewDir_TS.x = dot( -vertex_CS, t );
  v_viewDir_TS.y = dot( -vertex_CS, b );
  v_viewDir_TS.z = dot( -vertex_CS, n );

  gl_Position = u_mvp * vertex;
}
";
        private readonly string _earthGridFS = @"
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


const float Terminator = 10000.0; // 7.0
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
}";

        public EarthDrawNode(EarthRenderModel earth)
        {
            Earth = earth;

            _context = new B.Context();
            _device = new B.Device();

            _dirty = true;
            ShowGlobe = true;

            _sp = new B.ShaderProgram(_earthGridVS, _earthGridFS);

            u_model = ((B.Uniform<mat4>)_sp.Uniforms["u_model"]);
            u_mvp = ((B.Uniform<mat4>)_sp.Uniforms["u_mvp"]);
            u_normalMatrix = ((B.Uniform<mat3>)_sp.Uniforms["u_normalMatrix"]);
            u_view = ((B.Uniform<mat4>)_sp.Uniforms["u_view"]);
            u_modelView = ((B.Uniform<mat4>)_sp.Uniforms["u_modelView"]);
            u_lightPosition = ((B.Uniform<vec4>)_sp.Uniforms["u_lightPosition"]);
            u_radius = ((B.Uniform<float>)_sp.Uniforms["u_radius"]);

            _meshes = Earth.Meshes;

            if (_meshes.Length != 6)
            {
                throw new Exception();
            }

            _drawStates = new B.DrawState[6];

            var builder = ImmutableArray.CreateBuilder<string>();

            builder.AddRange(earth.DiffuseKeys);
            builder.AddRange(earth.SpecularKeys);
            builder.AddRange(earth.NormalKeys);
            builder.AddRange(earth.NightKeys);

            _keys = builder.ToImmutable();

            _maps = new B.Texture2D[_keys.Length];
        }

        public bool ShowGlobe { get; set; }

        public EarthRenderModel Earth { get; set; }

        public override void UpdateGeometry()
        {
            if (_dirty)
            {
                var state = _device.CreateRenderState();
                state.FacetCulling.Face = A.CullFaceMode.Back;
                state.FacetCulling.FrontFaceWindingOrder = A.FrontFaceDirection.Cw; // default

                for (int i = 0; i < 6; i++)
                {                                
                    var va = _context.CreateVertexArray(_meshes[i], _sp.VertexAttributes, A.BufferUsageHint.StaticDraw);
                    _drawStates[i] = _device.CreateDrawState(state, _sp, va);
                }

                _dirty = false;
            }
        }

        public bool IsComplete => !(_currentLoadingTexture < _keys.Length);
        
        public string WaitKey => IsComplete ? string.Empty : _keys[_currentLoadingTexture];

        public int SetImage(IDdsImage image)
        {
            var class1 = new B.TextureCreator();
            var name = class1.Create(image, 0, 0);

            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.ClampToEdge);
            A.GL.TexParameter(A.TextureTarget.Texture2D, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.ClampToEdge);

            _maps[_currentLoadingTexture] = new B.Texture2D(name, A.TextureTarget.Texture2D);

            _currentLoadingTexture++;

            return name;
        }

        public void SetName(int name)
        {
            _maps[_currentLoadingTexture] = new B.Texture2D(name, A.TextureTarget.Texture2D);

            _currentLoadingTexture++;
        }

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            if (ShowGlobe == true /*&& this.Earth.IsLoading == true*/)
            {
                SetUniforms(modelMatrix, scene);

                //int[] order = { 3, 2, 0, 1, 4, 5 };
                //int[] order = { 0, 1, 2, 3, 4, 5 };

                for (int i = 0; i < 6; i++)
                {
                    _context.TextureUnits[0].Texture = _maps[i + 0];
                    _sp.SetUniform("u_diffuseMap", 0);

                    _context.TextureUnits[1].Texture = _maps[i + 6];
                    _sp.SetUniform("u_specularMap", 1);

                    _context.TextureUnits[2].Texture = _maps[i + 12];
                    _sp.SetUniform("u_normalMap", 2);

                    _context.TextureUnits[3].Texture = _maps[i + 18];
                    _sp.SetUniform("u_nightMap", 3);

                    _context.Draw(A.PrimitiveType.Triangles, _drawStates[i], scene);
                }

                B.ShaderProgram.UnBind();
            }
        }

        public override void Dispose()
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                _drawStates[i].ShaderProgram.Dispose();

                if (_drawStates[i].VertexArray != null)
                {
                    _drawStates[i].VertexArray.Dispose();
                }
            }
        }

        private void SetUniforms(dmat4 modelMatrix, ISceneState sceneState)
        {
            dmat4 model = modelMatrix;
            dmat4 view = sceneState.ViewMatrix;
            dmat3 normalMatrix = (new dmat3((view * model).Inverse).Transposed);
            dmat4 mvp = sceneState.ProjectionMatrix * view * model;
            dmat4 modelView = view * model;

            u_model.Value = model.ToMat4();
            u_view.Value = view.ToMat4();
            u_normalMatrix.Value = normalMatrix.ToMat3();
            u_modelView.Value = modelView.ToMat4();
            u_mvp.Value = mvp.ToMat4();

            u_lightPosition.Value = sceneState.LightPosition.ToVec4();

            u_radius.Value = (float)(6371.0);
        }
    }
}
