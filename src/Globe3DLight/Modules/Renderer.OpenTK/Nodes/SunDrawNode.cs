using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Image;
using B = Globe3DLight.Renderer.OpenTK.Core;
using A = OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Renderer;

namespace Globe3DLight.Renderer.OpenTK
{
    internal class SunDrawNode : DrawNode, ISunDrawNode
    {
        private readonly B.Context _context;       
        private readonly B.Device _device;          
        private readonly string sunVS = @"
#version 330

layout (location = 0) in vec2 POSITION;

// Uniforms
uniform mat4 u_view;
uniform mat4 u_proj;
uniform vec3 u_center;
uniform vec2 u_dims;

// Output
out vec2 fPosition;

void main() {

fPosition = POSITION;
    
gl_Position = u_proj * u_view * vec4(u_center, 1.0);
gl_Position /= gl_Position.w;
gl_Position.xy += POSITION * u_dims;


//gl_Position = u_proj * u_view * vec4(POSITION, 1.0);

}";      
        private readonly string sunFS = @"#version 330

vec3 mod2893(vec3 x) {
  return x - floor(x * (1.0 / 289.0)) * 289.0;
}

vec4 mod2893(vec4 x) {
  return x - floor(x * (1.0 / 289.0)) * 289.0;
}

vec4 permute3(vec4 x) {
     return mod2893(((x*34.0)+1.0)*x);
}

vec4 taylorInvSqrt3(vec4 r) {
  return 1.79284291400159 - 0.85373472095314 * r;
}

float snoise(vec3 v) {
  const vec2  C = vec2(1.0/6.0, 1.0/3.0);
  const vec4  D = vec4(0.0, 0.5, 1.0, 2.0);

// First corner
  vec3 i = floor(v + dot(v, C.yyy) );
  vec3 x0 = v - i + dot(i, C.xxx) ;

// Other corners
  vec3 g = step(x0.yzx, x0.xyz);
  vec3 l = 1.0 - g;
  vec3 i1 = min( g.xyz, l.zxy );
  vec3 i2 = max( g.xyz, l.zxy );

  //   x0 = x0 - 0.0 + 0.0 * C.xxx;
  //   x1 = x0 - i1  + 1.0 * C.xxx;
  //   x2 = x0 - i2  + 2.0 * C.xxx;
  //   x3 = x0 - 1.0 + 3.0 * C.xxx;
  vec3 x1 = x0 - i1 + C.xxx;
  vec3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
  vec3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y

// Permutations
  i = mod2893(i); 
  vec4 p = permute3( permute3( permute3( 
             i.z + vec4(0.0, i1.z, i2.z, 1.0 ))
           + i.y + vec4(0.0, i1.y, i2.y, 1.0 )) 
           + i.x + vec4(0.0, i1.x, i2.x, 1.0 ));

// Gradients: 7x7 points over a square, mapped onto an octahedron.
// The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
  float n_ = 0.142857142857; // 1.0/7.0
  vec3  ns = n_ * D.wyz - D.xzx;

  vec4 j = p - 49.0 * floor(p * ns.z * ns.z);  //  mod(p,7*7)

  vec4 x_ = floor(j * ns.z);
  vec4 y_ = floor(j - 7.0 * x_ );    // mod(j,N)

  vec4 x = x_ *ns.x + ns.yyyy;
  vec4 y = y_ *ns.x + ns.yyyy;
  vec4 h = 1.0 - abs(x) - abs(y);

  vec4 b0 = vec4( x.xy, y.xy );
  vec4 b1 = vec4( x.zw, y.zw );

  //vec4 s0 = vec4(lessThan(b0,0.0))*2.0 - 1.0;
  //vec4 s1 = vec4(lessThan(b1,0.0))*2.0 - 1.0;
  vec4 s0 = floor(b0)*2.0 + 1.0;
  vec4 s1 = floor(b1)*2.0 + 1.0;
  vec4 sh = -step(h, vec4(0.0));

  vec4 a0 = b0.xzyw + s0.xzyw*sh.xxyy;
  vec4 a1 = b1.xzyw + s1.xzyw*sh.zzww;

  vec3 p0 = vec3(a0.xy,h.x);
  vec3 p1 = vec3(a0.zw,h.y);
  vec3 p2 = vec3(a1.xy,h.z);
  vec3 p3 = vec3(a1.zw,h.w);

  //Normalize gradients
  vec4 norm = taylorInvSqrt3(vec4(dot(p0,p0), dot(p1,p1), dot(p2, p2), dot(p3,p3)));
  p0 *= norm.x;
  p1 *= norm.y;
  p2 *= norm.z;
  p3 *= norm.w;

// Mix final noise value
  vec4 m = max(0.6 - vec4(dot(x0,x0), dot(x1,x1), dot(x2,x2), dot(x3,x3)), 0.0);
  m = m * m;
  return 42.0 * dot( m*m, vec4( dot(p0,x0), dot(p1,x1), 
                                dot(p2,x2), dot(p3,x3) ) );
}

float noise(vec3 position, int octaves, float frequency, float persistence) {
	float total = 0.0;
	float maxAmplitude = 0.0;
    float amplitude = 1.0;
	for (int i = 0; i < octaves; i++) {
		total += snoise(position * frequency) * amplitude;
		frequency *= 2.0;
		maxAmplitude += amplitude;
		amplitude *= persistence;
	}
	return total / maxAmplitude;
}

float absNoise(vec3 position, int octaves, float frequency, float persistence) {
	float total = 0.0;
	float maxAmplitude = 0.0;
    float amplitude = 1.0;
	for (int i = 0; i < octaves; i++) {
		total += abs(snoise(position * frequency)) * amplitude;
		frequency *= 2.0;
		maxAmplitude += amplitude;
		amplitude *= persistence;
	}
	return total / maxAmplitude;
}

float ridgedNoise(vec3 position, int octaves, float frequency, float persistence) {
	float total = 0.0;
	float maxAmplitude = 0.0;
    float amplitude = 1.0;
	for (int i = 0; i < octaves; i++) {
		total += ((1.0 - abs(snoise(position * frequency))) * 2.0 - 1.0) * amplitude;
		frequency *= 2.0;
		maxAmplitude += amplitude;
		amplitude *= persistence;
	}
	return total / maxAmplitude;
}

float squaredNoise(vec3 position, int octaves, float frequency, float persistence) {
	float total = 0.0;
	float maxAmplitude = 0.0;
    float amplitude = 1.0;
	for (int i = 0; i < octaves; i++) {
		float tmp = snoise(position * frequency);
        total += tmp * tmp * amplitude;
		frequency *= 2.0;
		maxAmplitude += amplitude;
		amplitude *= persistence;
	}
	return total / maxAmplitude;
}

float cubedNoise(vec3 position, int octaves, float frequency, float persistence) {
	float total = 0.0;
	float maxAmplitude = 0.0;
    float amplitude = 1.0;
	for (int i = 0; i < octaves; i++) {
		float tmp = snoise(position * frequency);
        total += tmp * tmp * tmp * amplitude;
		frequency *= 2.0;
		maxAmplitude += amplitude;
		amplitude *= persistence;
	}
	return total / maxAmplitude;
}




uniform sampler2D unColorMap;
uniform float unNoiseZ;
uniform float unColorMapU;
uniform float unColorMapV;

uniform vec3 unColorMult;

// Input
in vec2 fPosition;

// Output
out vec4 pColor;


void main() {   

    // ========== Change These =========
    const float spikeFrequency = 15.5;
    const float spikeShift = 0.2;
    const float spikeMult2 = 0.02;
    // =================================

    vec2 fTex = (vec2(fPosition.x, fPosition.y) + 1.0) / 2.0;
    
    vec2 nDistVec = normalize(fPosition);
    float spikeNoise = snoise(vec3(nDistVec, unNoiseZ) * spikeFrequency);
    float spikeVal = spikeNoise + spikeShift;
    
    float dist = length(fPosition);
    // Calculate brightness based on distance
    float brightness = (1.0 / (dist + 0.08)) * 0.17 - 0.18;

    brightness = max(brightness, 0.0);
    float spikeBrightness = brightness * spikeMult2 * clamp(spikeVal, 0.0, 1.0);
    
    float ovCol = (pow(1.0 - dist, 2.5) * (dist) * 3.0) * (unColorMapU + spikeNoise * spikeMult2);
    ovCol = max(ovCol, 0.0);
    float centerglow = 1.0 / pow(dist + 0.96, 40.0);
    
	// Calculate color
//vec3 temperatureColor = texture(unColorMap, vec2(unColorMapU, 1.0 - (brightness + ovCol) + 0.125)).rgb;

vec3 temperatureColor = texture(unColorMap, vec2(unColorMapU, unColorMapV)).rgb;


vec3 color = temperatureColor * unColorMult;


    vec2 ap = abs(fPosition);

    float hRay = (1.0 - (1.0 / (1.0 + exp(-((ap.y * 30.0 + 0.2) * 4 * 3.1415926) + 2 * 3.1415926))) - max(ap.x - 0.1, 0.0));
    hRay = max(hRay * 0.2, 0.0);

    pColor = vec4(color * (brightness + spikeBrightness + ovCol + hRay), 1.0);

    // Reverse the gamma
    pColor.rgb *= pColor.rgb;
}
";
        private readonly B.ShaderProgram _sp;
        private readonly B.DrawState _drawState;
        private readonly B.TextureCreator _textureCreator;
        private string _key;
        private int _textureSunGlowName;
        private bool dirty;
        private bool _isComplete = false;
        private readonly B.Uniform<vec2> u_dims;
        private readonly B.Uniform<vec3> u_center;
        private readonly B.Uniform<float> u_NoiseZ;
        private readonly B.Uniform<float> u_ColorMapU;
        private readonly B.Uniform<float> u_ColorMapV;
        private readonly B.Uniform<vec3> u_ColorMult;
        private readonly B.Uniform<mat4> u_view;
        private readonly B.Uniform<mat4> u_proj;
        private float DT = 0.0f;        
     //   private const double toKM = 637.8;
        private const double diameter = 13926840.0;
        private const float temperature = 5778.0f;
        private const float colorMapU = (temperature - 800.0f) / 29200.0f;
        //   private const double range = 149600000.0 * 1.0 / toKM;// / 6378.0;//637.8;
        private const float multColor = 1.0f;
        private vec3 colorMult = new vec3(1.0f, 1.0f, 1.0f) * multColor;

        public SunDrawNode(SunRenderModel sun)
        {
            Sun = sun;

            _device = new B.Device();
            _key = sun.SunGlowKey;

            _textureCreator = _device.CreateTextureCreator();

            _context = new B.Context();

            dirty = true;

            _sp = _device.CreateShaderProgram(sunVS, sunFS);

            u_dims = (B.Uniform<vec2>)_sp.Uniforms["u_dims"];
            u_center = (B.Uniform<vec3>)_sp.Uniforms["u_center"];
            u_NoiseZ = (B.Uniform<float>)_sp.Uniforms["unNoiseZ"];
            u_ColorMapU = (B.Uniform<float>)_sp.Uniforms["unColorMapU"];
            u_ColorMapV = (B.Uniform<float>)_sp.Uniforms["unColorMapV"];
            u_ColorMult = (B.Uniform<vec3>)_sp.Uniforms["unColorMult"];
            u_view = (B.Uniform<mat4>)_sp.Uniforms["u_view"];
            u_proj = (B.Uniform<mat4>)_sp.Uniforms["u_proj"];

            _drawState = _device.CreateDrawState(_sp);
        }

        public SunRenderModel Sun { get; set; }

        public bool IsComplete => _isComplete;

        public string WaitKey => _key;

        public override void OnDraw(object dc, dmat4 modelMatrix, ISceneState scene)
        {
            _sp.Bind();

            SetUniforms(modelMatrix, scene);

            A.GL.ActiveTexture(A.TextureUnit.Texture0);
            _sp.SetUniform("unColorMap", 0);
            A.GL.BindTexture(A.TextureTarget.Texture2D, _textureSunGlowName);

            A.GL.Enable(A.EnableCap.Blend);
            A.GL.BlendFuncSeparate(A.BlendingFactorSrc.One, A.BlendingFactorDest.Zero, A.BlendingFactorSrc.One, A.BlendingFactorDest.Zero);
            A.GL.BlendEquationSeparate(A.BlendEquationMode.FuncAdd, A.BlendEquationMode.FuncAdd);

            _context.Draw(A.PrimitiveType.Triangles, _drawState, scene);

            B.ShaderProgram.UnBind();

            A.GL.Disable(A.EnableCap.Blend);
        }

        public override void UpdateGeometry()
        {
            if (dirty)
            {
                var mesh = Sun.Billboard;

                _drawState.VertexArray = _context.CreateVertexArray(mesh, _drawState.ShaderProgram.VertexAttributes, A.BufferUsageHint.StaticDraw);

                _drawState.RenderState.FacetCulling.Face = A.CullFaceMode.Back;
                _drawState.RenderState.FacetCulling.FrontFaceWindingOrder = A.FrontFaceDirection.Cw;

                _drawState.RenderState.Blending.Enabled = true;
                _drawState.RenderState.Blending.SourceRGBFactor = A.BlendingFactorSrc.One;
                _drawState.RenderState.Blending.DestinationRGBFactor = A.BlendingFactorDest.One;

                dirty = false;
            }
        }

        private void SetUniforms(dmat4 modelMatrix, ISceneState scene)
        {
            // modelMatrix = dmat4.Translate(sunData.Position)

            var pos = glm.Normalized(modelMatrix.Column3) * 160000.0;
          //  var pos = modelMatrix.Column3 * 160000.0;

            dvec3 sunPosition_WS = pos.ToDvec3();// new dvec3(glm.Normalized(pos) * range);
            dvec3 sunPosition_KM = modelMatrix.Column3.ToDvec3();// sunPosition_WS;// * toKM;

            dvec3 cameraPosition_WS = scene.Camera.Eye;
            dvec3 cameraPosition_KM = scene.Camera.Eye;// * toKM;

            double distance = glm.Length(sunPosition_KM - cameraPosition_KM);

            float aspectRatio = 1.0f;
            double size = calculateGlowSize(diameter, temperature, distance);// - 4.0;
            vec2 dims = new vec2((float)size, (float)size * aspectRatio);

            DT += 0.0005f;
            if (DT >= 10000.0f)
                DT = 0.0f;

            u_dims.Value = dims;
            u_center.Value = sunPosition_WS.ToVec3();
            u_NoiseZ.Value = DT;
            u_ColorMapU.Value = colorMapU;
            u_ColorMapV.Value = -1.0f;
            u_ColorMult.Value = colorMult;
            u_view.Value = scene.ViewMatrix.ToMat4();
            u_proj.Value = scene.ProjectionMatrix.ToMat4();
        }

        private double calculateGlowSize(double diameter, double temperature, double distance)
        {
            double DSUN = 1392684.0;
            double TSUN = 5778.0;

            // Georg's magic formula
            double d = distance; // Distance
            double D = diameter * DSUN;
            double L = (D * D) * Math.Pow(temperature / TSUN, 4.0); // Luminosity

            return 0.016 * Math.Pow(L, 0.25) / Math.Pow(d, 0.5); // Size
        }

        public void SetName(int name)
        {
            _textureSunGlowName = name;

            _isComplete = true;
        }

        public int SetImage(IDdsImage image)
        {                  
            var class1 = new B.TextureCreator();
            _textureSunGlowName = class1.Create1(image, 0, 0);
        
            _isComplete = true;

            return  _textureSunGlowName;
        }
    }
}
