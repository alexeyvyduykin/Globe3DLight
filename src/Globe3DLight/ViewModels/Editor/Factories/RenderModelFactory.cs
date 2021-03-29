using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Geometry.Models;
using System.Linq;
using System.Collections.Immutable;
using Globe3DLight.Models.Image;
using Microsoft.Extensions.Configuration;
using System.IO;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Editor
{
    public interface IRenderModelFactory
    {
        SpaceboxRenderModel CreateSpacebox(double scale);

        EarthRenderModel CreateEarth();

        SatelliteRenderModel CreateSatellite(double scale);

        SunRenderModel CreateSun();

        SensorRenderModel CreateSensor();

        FrameRenderModel CreateFrame(float scale);

        GroundStationRenderModel CreateGroundStation(double scale);

        GroundObjectRenderModel CreateGroundObject();

        RetranslatorRenderModel CreateRetranslator(double scale);

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

        public Globe3DLight.Models.Geometry.Models.IModel CreateCubeSphere()
        {
            var modelLoader = _serviceProvider.GetService<IModelLoader>();

            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];

            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"models\TrueCubeSphere.obj");

            return modelLoader.LoadModel(path, false);       
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

            var model = CreateCubeSphere();

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

            var model = CreateCubeSphere();

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

        public EarthRenderModel CreateEarthSimple()
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();

            var keys = ImmutableArray.Create<string>("EarthDiffuse", "EarthSpecular", "EarthNormal", "EarthNight");
            var model = CreateCubeSphere();

            var obj = new EarthRenderModel()
            {
              //  DiffuseImages = ImmutableArray.Create<IDdsImage>(),
              //  SpecularImages = ImmutableArray.Create<IDdsImage>(),
              //  NormalImages = ImmutableArray.Create<IDdsImage>(),
              //  NightImages = ImmutableArray.Create<IDdsImage>(),
                Meshes = model.Meshes.ToImmutableArray(),    //CreateCubeSphere().ToImmutableArray(),
                //   ImageLoader = imageLoader,

                //   DiffuseImagePaths = new List<string>() { "C:/data/textures/Earth2D/EarthDiffuseMap.dds" },
                //   SpecularImagePaths = new List<string>() { "C:/data/textures/Earth2D/earthSpecMap.dds" },
                //   NormalImagePaths = new List<string>() { "C:/data/textures/Earth2D/earthNormalMap.dds" },
                //   NightImagePaths = new List<string>() { "C:/data/textures/Earth2D/earthNightMap.dds" },
            };

            imageLibrary.AddKey(keys[0], "C:/data/textures/Earth2D/EarthDiffuseMap.dds");
            imageLibrary.AddKey(keys[1], "C:/data/textures/Earth2D/earthSpecMap.dds");
            imageLibrary.AddKey(keys[2], "C:/data/textures/Earth2D/earthNormalMap.dds");
            imageLibrary.AddKey(keys[3], "C:/data/textures/Earth2D/earthNightMap.dds");


            return obj;
        }

        public SunRenderModel CreateSun()
        {            
            var factory = _serviceProvider.GetService<IFactory>();
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            string key = "SunGlow";

            var resourcePath = configuration["ResourcePath"];

            var obj = new SunRenderModel()
            {                       
                Billboard = factory.CreateBillboard(),  
                SunGlowKey = key,             
            };

            imageLibrary.AddKey(key, Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\Sun\starSpectrum.dds"));

            return obj;
        }

        public SpaceboxRenderModel CreateSpacebox(double scale)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            string key = "SpaceboxCubemap";

            var resourcePath = configuration["ResourcePath"];

            var obj = new SpaceboxRenderModel()
            {                      
                Mesh = factory.CreateCube((float)scale), //factory.CreateCube(25000.0f),
                SpaceboxCubemapKey = key,
                Scale = scale,
            };

            imageLibrary.AddKey(key, Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"textures\Spacebox\Spacebox4096x4096Compressed.dds"));

            return obj;
        }

        public GroundStationRenderModel CreateGroundStation(double scale)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var mesh = factory.CreateSolidSphere(1.0f, 16, 16);

            var obj = new GroundStationRenderModel()
            {                
                Mesh = mesh,//factory.CreateSolidSphere(0.06f, 16, 16),
                Scale = scale,
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
        
        public RetranslatorRenderModel CreateRetranslator(double scale)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var mesh = factory.CreateSolidSphere(1.0f, 16, 16);

            var obj = new RetranslatorRenderModel()
            {
                Mesh = mesh,//factory.CreateSolidSphere(0.06f, 16, 16),
                Scale = scale,
            };
            return obj;
        }

        public SensorRenderModel CreateSatelliteSensor()
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var obj = new SensorRenderModel()
            {
               
            };

            return obj;
        }

        //public ISceneShape CreateSatelliteTranslation()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var d1evice = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new TranslationShape(d1evice)
        //    {

        //    };

        //    return obj;
        //}

        //public ISceneShape CreateSatelliteOrbit()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var d1evice = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new OrbitShape(d1evice)
        //    {

        //    };

        //    return obj;            
        //}

        public FrameRenderModel CreateFrame(float scale)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var obj = new FrameRenderModel()
            {
                Scale = scale,
            };

            return obj;
        }

        public SatelliteRenderModel CreateSatellite(double scale)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var modelLoader = _serviceProvider.GetService<IModelLoader>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];

            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath, @"models");
        
            var tempFile = Path.Combine(path, @"satellite_v3_temp.ase");
           
            string text = File.ReadAllText(Path.Combine(path, @"satellite_v3.ase"));
            text = text.Replace("\"Locator.bmp\"", "\"" + Path.Combine(path, "Locator.bmp") + "\"");
            text = text.Replace("\"MiniSolarPanel.bmp\"", "\"" + Path.Combine(path, "MiniSolarPanel.bmp") + "\"");
            text = text.Replace("\"SolarPanel.bmp\"", "\"" + Path.Combine(path, "SolarPanel.bmp") + "\"");
            File.WriteAllText(tempFile, text);

            var model = modelLoader.LoadModel(tempFile, true);

            var obj = new SatelliteRenderModel()
            {             
                Scale = scale,// 0.002,        
                Model = model,     
            };

            return obj;
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
    }
}
