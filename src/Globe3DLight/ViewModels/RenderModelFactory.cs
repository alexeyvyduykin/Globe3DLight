using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.ViewModels.Geometry;
using Globe3DLight.ViewModels.Scene;
using Microsoft.Extensions.Configuration;

namespace Globe3DLight.ViewModels
{
    public interface IRenderModelFactory
    {
        Mesh CreateBillboard();

        Mesh CreateCube(double scale);

        Mesh CreateSolidSphere(double radius, int rings, int sectors);

        SpaceboxRenderModel CreateSpacebox();

        EarthRenderModel CreateEarth();

        RenderModel CreateSatellite();

        SunRenderModel CreateSun();

        SensorRenderModel CreateSensor();

        FrameRenderModel CreateFrame(float scale);

        RenderModel CreateGroundStation();

        GroundObjectRenderModel CreateGroundObject();

        RenderModel CreateRetranslator();

        AntennaRenderModel CreateAntenna();

        OrbitRenderModel CreateOrbit();
    }

    public class RenderModelFactory : IRenderModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RenderModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private Model LoadModelFromResources(string model, bool flipUVs)
        {
            var modelLoader = _serviceProvider.GetService<IModelLoader>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];

            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, model);

            return modelLoader.LoadModel(path, flipUVs);
        }

        public EarthRenderModel CreateEarthDefault()
        {
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();

            var pairs = ImmutableArray.Create<(string, string)>(
                ("PosX", "pos_x.dds"),
                ("NegZ", "neg_z.dds"),
                ("NegX", "neg_x.dds"),
                ("PosZ", "pos_z.dds"),
                ("PosY", "pos_y.dds"),
                ("NegY", "neg_y.dds"));

            var keys = ImmutableArray.Create<string>("EarthDiffuse", "EarthSpecular", "EarthNormal", "EarthNight");

            var model = LoadModelFromResources(@"models\TrueCubeSphere.obj", false);

            var obj = new EarthRenderModel()
            {
                Meshes = model.Meshes.ToImmutableArray(),
                DiffuseKeys = pairs.Select(s => keys[0] + s.Item1),
                SpecularKeys = pairs.Select(s => keys[1] + s.Item1),
                NormalKeys = pairs.Select(s => keys[2] + s.Item1),
                NightKeys = pairs.Select(s => keys[3] + s.Item1),
            };

            imageLibrary.AddKeys(pairs.Select(s => (keys[0] + s.Item1, @"resources\textures\earth\diffuseQubeMap\" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[1] + s.Item1, @"resources\textures\earth\specInvertQubeMap\" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[2] + s.Item1, @"resources\textures\earth\normalQubeMap\" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[3] + s.Item1, @"resources\textures\earth\nightQubeMap\" + s.Item2)).ToArray());

            return obj;
        }

        public EarthRenderModel CreateEarth()
        {
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];

            var pairs = ImmutableArray.Create<(string, string)>(
                ("PosX", "pos_x.dds"),
                ("NegZ", "neg_z.dds"),
                ("NegX", "neg_x.dds"),
                ("PosZ", "pos_z.dds"),
                ("PosY", "pos_y.dds"),
                ("NegY", "neg_y.dds"));

            var keys = ImmutableArray.Create<string>("EarthDiffuse", "EarthSpecular", "EarthNormal", "EarthNight");

            var model = LoadModelFromResources(@"models\TrueCubeSphere.obj", false);

            var obj = new EarthRenderModel()
            {
                Meshes = model.Meshes.ToImmutableArray(),
                DiffuseKeys = pairs.Select(s => keys[0] + s.Item1),
                SpecularKeys = pairs.Select(s => keys[1] + s.Item1),
                NormalKeys = pairs.Select(s => keys[2] + s.Item1),
                NightKeys = pairs.Select(s => keys[3] + s.Item1),
            };

            var path1 = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\EarthQubeMap\EarthDiffuseFake\");
            var path2 = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\EarthQubeMap\EarthSpecInvertQubeMap4096x4096, border=0\");
            var path3 = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\EarthQubeMap\EarthNormalQubeMap8192x8192, border=0\");
            var path4 = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\EarthQubeMap\EarthNightQubeMap2048x2048, border=0\");

            imageLibrary.AddKeys(pairs.Select(s => (keys[0] + s.Item1, Path.Combine(path1, s.Item2))).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[1] + s.Item1, Path.Combine(path2, s.Item2))).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[2] + s.Item1, Path.Combine(path3, s.Item2))).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[3] + s.Item1, Path.Combine(path4, s.Item2))).ToArray());

            return obj;
        }

        public SunRenderModel CreateSun()
        {
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            string key = "SunGlow";

            var resourcePath = configuration["ResourcePath"];

            var obj = new SunRenderModel()
            {
                Billboard = CreateBillboard(),
                SunGlowKey = key,
            };

            imageLibrary.AddKey(key, Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\Sun\starSpectrum.dds"));

            return obj;
        }

        public SpaceboxRenderModel CreateSpacebox()
        {        
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            string key = "SpaceboxCubemap";

            var resourcePath = configuration["ResourcePath"];

            var obj = new SpaceboxRenderModel()
            {
                Mesh = CreateCube(1000000.0f/*25000.0f*/),
                SpaceboxCubemapKey = key,
            };

            imageLibrary.AddKey(key, Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\Spacebox\Spacebox4096x4096Compressed.dds"));

            return obj;
        }

        public RenderModel CreateGroundStation()
        {           
            var obj = new RenderModel()
            {
                Frame = CreateFrame(200.0f),
                Model = LoadModelFromResources(@"models\tall_dish.obj", false),
                Scale = 180.0, //70.0
            };

            return obj;
        }

        public GroundObjectRenderModel CreateGroundObject()
        {
            var obj = new GroundObjectRenderModel()
            {

            };

            return obj;
        }

        public RenderModel CreateRetranslator()
        {         
            var obj = new RenderModel()
            {
                Frame = CreateFrame(6050.0f),
                Model = LoadModelFromResources(@"models\tdrs.obj", true),           
                Scale = 500,
            };

            return obj;
        }

        public SensorRenderModel CreateSatelliteSensor()
        {           
            var obj = new SensorRenderModel()
            {

            };

            return obj;
        }

        public FrameRenderModel CreateFrame(float scale)
        {           
            var obj = new FrameRenderModel()
            {
                Scale = scale,
            };

            return obj;
        }

        public RenderModel CreateSatellite()
        {    
            return new RenderModel()
            {
                Frame = CreateFrame(200.0f),
                Model = LoadModelFromResources(@"models\satellite_v1.obj", true),
                Scale = 1.0,                
            };
        }

        public SensorRenderModel CreateSensor()
        {
            var obj = new SensorRenderModel()
            {

            };

            return obj;
        }
        
        public AntennaRenderModel CreateAntenna()
        {
            var obj = new AntennaRenderModel()
            {
                AttachPosition = new dvec3(67.74, -12.22, -23.5),
            };

            return obj;
        }

        public OrbitRenderModel CreateOrbit()
        {
            var obj = new OrbitRenderModel()
            {

            };

            return obj;
        }

        public Mesh CreateBillboard()
        {
            return new Mesh()
            {
                Vertices = new vec3[4]
                {
                    new vec3(-1.0f, -1.0f, 0.0f),
                    new vec3(-1.0f, 1.0f, 0.0f),
                    new vec3(1.0f, 1.0f, 0.0f),
                    new vec3(1.0f, -1.0f, 0.0f)
                },
                Normals = new List<vec3>(),
                TexCoords = new List<vec2>(),
                Tangents = new List<vec3>(),
                Indices = new ushort[6] { 0, 1, 2, 0, 2, 3 },
                MaterialIndex = -1,
            };
        }

        public Mesh CreateCube(double scale)
        {
            return new Mesh()
            {
                Vertices = new vec3[8]
                {
                    new vec3(1.0f, -1.0f, -1.0f) * (float)scale,
                    new vec3(1.0f, -1.0f, 1.0f) * (float)scale,
                    new vec3(1.0f, 1.0f, 1.0f) * (float)scale,
                    new vec3(1.0f, 1.0f, -1.0f) * (float)scale,
                    new vec3(-1.0f, -1.0f, 1.0f) * (float)scale,
                    new vec3(-1.0f, -1.0f, -1.0f) * (float)scale,
                    new vec3(-1.0f, 1.0f, -1.0f) * (float)scale,
                    new vec3(-1.0f, 1.0f, 1.0f) * (float)scale
                },
                Normals = new List<vec3>(),
                TexCoords = new List<vec2>(),
                Tangents = new List<vec3>(),
                Indices = new ushort[36]
                {
                    0, 1, 2,  // x_pos
                    2, 3, 0,  //
                    4, 5, 6,  // x_neg
                    6, 7, 4,  //
                    6, 3, 2,  // y_pos
                    2, 7, 6,  //
                    0, 5, 4,  // y_neg
                    4, 1, 0,  //
                    1, 4, 7,  // z_pos
                    7, 2, 1,  //
                    5, 0, 3,  // z_neg
                    3, 6, 5   //
                },
                MaterialIndex = -1,
            };
        }

        public Mesh CreateSolidSphere(double radius, int rings, int sectors)
        {
            // FrontFaceDirection.Ccw
            var indices = new List<ushort>();
            var vertices = new List<vec3>();
            var texcoords = new List<vec2>();
            var normals = new List<vec3>();

            double R = 1.0 / (double)(rings - 1);
            double S = 1.0 / (double)(sectors - 1);

            for (int r = 0; r < rings; r++)
            {
                for (int s = 0; s < sectors; s++)
                {
                    double y = Math.Sin((-Math.PI / 2.0 + Math.PI * r * R));
                    double x = Math.Cos(2.0 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);
                    double z = Math.Sin(2.0 * Math.PI * s * S) * Math.Sin(Math.PI * r * R);

                    vertices.Add(new vec3((float)x * (float)radius, (float)y * (float)radius, (float)z * (float)radius));
                    texcoords.Add(new vec2((float)(s * S), (float)(r * R)));
                    normals.Add(new vec3((float)x, (float)y, (float)z));
                }
            }

            for (int r = 0; r < rings - 1; r++)
            {
                for (int s = 0; s < sectors - 1; s++)
                {
                    indices.Add((ushort)(r * sectors + s));
                    indices.Add((ushort)(r * sectors + (s + 1)));
                    indices.Add((ushort)((r + 1) * sectors + (s + 1)));

                    indices.Add((ushort)((r + 1) * sectors + (s + 1)));
                    indices.Add((ushort)((r + 1) * sectors + s));
                    indices.Add((ushort)(r * sectors + s));
                }
            }

            return new Mesh()
            {
                Vertices = vertices,
                Normals = normals,
                TexCoords = texcoords,
                Tangents = new List<vec3>(),
                Indices = indices,
                MaterialIndex = -1,
            };
        }
    }
}
