using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Scene;
using Globe3DLight.Renderer;
using Globe3DLight.Geometry;
using System.Linq;
using System.Collections.Immutable;
using Globe3DLight.Image;

namespace Globe3DLight.Editor
{
    public interface IRenderModelFactory
    {
        ISpaceboxRenderModel CreateSpacebox(double scale);

        IEarthRenderModel CreateEarth();

        ISatelliteRenderModel CreateSatellite(double scale);

        ISunRenderModel CreateSun();

        ISensorRenderModel CreateSensor();

        IFrameRenderModel CreateFrame(float scale);

        IGroundStationRenderModel CreateGroundStation(double scale);

        IGroundObjectListRenderModel CreateGroundObjectList();

        IRetranslatorRenderModel CreateRetranslator(double scale);

        IAntennaRenderModel CreateAntenna();
    }

    public class RenderModelFactory : IRenderModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RenderModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //public IEnumerable<IAMesh> CreateCubeSphere()
        //{
        //    var modelLoader = _serviceProvider.GetService<IModelLoader>();

        //    bool res = modelLoader.LoadFromFile(@"C:\resource\models\TrueCubeSphere.obj", false);
            
        //    return modelLoader.AMeshes.ToList();
        //}

        public Globe3DLight.Geometry.Models.IModel CreateCubeSphere()
        {
            var modelLoader = _serviceProvider.GetService<IModelLoader>();

            return modelLoader.LoadModel(@"C:\resource\models\TrueCubeSphere.obj", false);       
        }

        public IEarthRenderModel CreateEarthDefault()
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
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
                Meshes = model.Meshes.ToImmutableArray(),    //CreateCubeSphere().ToImmutableArray(),
               // ImageLoader = imageLoader,
                DiffuseKeys = pairs.Select(s => keys[0] + s.Item1),
                SpecularKeys = pairs.Select(s => keys[1] + s.Item1),
                NormalKeys = pairs.Select(s => keys[2] + s.Item1),
                NightKeys = pairs.Select(s => keys[3] + s.Item1),
                
            };


            imageLibrary.AddKeys(pairs.Select(s => (keys[0] + s.Item1, "resources/textures/earth/diffuseQubeMap/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[1] + s.Item1, "resources/textures/earth/specInvertQubeMap/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[2] + s.Item1, "resources/textures/earth/normalQubeMap/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[3] + s.Item1, "resources/textures/earth/nightQubeMap/" + s.Item2)).ToArray());


            return obj;
        }

        public IEarthRenderModel CreateEarth()
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
         //   var d1evice = _serviceProvider.GetService<IDevice>();
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
                Meshes = model.Meshes.ToImmutableArray(),    //CreateCubeSphere().ToImmutableArray(),
                DiffuseKeys = pairs.Select(s => keys[0] + s.Item1),
                SpecularKeys = pairs.Select(s => keys[1] + s.Item1),
                NormalKeys = pairs.Select(s => keys[2] + s.Item1),
                NightKeys = pairs.Select(s => keys[3] + s.Item1),
                //     ImageLoader = imageLoader,            
            };

            imageLibrary.AddKeys(pairs.Select(s => (keys[0] + s.Item1, "C:/data/textures/EarthQubeMap/EarthDiffuseFake/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[1] + s.Item1, "C:/data/textures/EarthQubeMap/EarthSpecInvertQubeMap4096x4096, border=0/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[2] + s.Item1, "C:/data/textures/EarthQubeMap/EarthNormalQubeMap8192x8192, border=0/" + s.Item2)).ToArray());
            imageLibrary.AddKeys(pairs.Select(s => (keys[3] + s.Item1, "C:/data/textures/EarthQubeMap/EarthNightQubeMap2048x2048, border=0/" + s.Item2)).ToArray());

            return obj;
        }

        public IEarthRenderModel CreateEarthSimple()
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

        public ISunRenderModel CreateSun()
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();           
            var factory = _serviceProvider.GetService<IFactory>();
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();

            string key = "SunGlow";

            var obj = new SunRenderModel()
            {             
               // SunGlowTexturePath = "C:/data/textures/Sun/starSpectrum.dds",
                Billboard = factory.CreateBillboard(),  
                SunGlowKey = key,
                
                //  IsLoading = false,
                //  ImageLoader = imageLoader,
                //  Loader = ddsLoader,                
            };

            imageLibrary.AddKey(key, "C:/data/textures/Sun/starSpectrum.dds");

            return obj;
        }

        public ISpaceboxRenderModel CreateSpacebox(double scale)
        {
           // var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
            var factory = _serviceProvider.GetService<IFactory>();
            var imageLibrary = _serviceProvider.GetService<IImageLibrary>();

            //int id = ddsLoader.LoadOGLSingleCubemap("C:/data/textures/Spacebox/Spacebox4096x4096Compressed.dds");
            //int id = ddsLoader.LoadOGLSingleCubemap("resources/textures/spacebox/Spacebox4096x4096Compressed.dds");

            string key = "SpaceboxCubemap";

     
            var obj = new SpaceboxRenderModel()
            {           
               // Loader = ddsLoader,
               // Filename = "C:/data/textures/Spacebox/Spacebox4096x4096Compressed.dds",
                //IdCubemapText = id,             
                Mesh =factory.CreateCube((float)scale), //factory.CreateCube(25000.0f),
                SpaceboxCubemapKey = key,
                Scale = scale,
            };

            imageLibrary.AddKey(key, "C:/data/textures/Spacebox/Spacebox4096x4096Compressed.dds");

            return obj;
        }

        public IGroundStationRenderModel CreateGroundStation(double scale)
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
         //   var d1evice = _serviceProvider.GetService<IDevice>();
            var factory = _serviceProvider.GetService<IFactory>();

            var mesh = factory.CreateSolidSphere(1.0f, 16, 16);

            var obj = new GroundStationRenderModel()
            {                
                Mesh = mesh,//factory.CreateSolidSphere(0.06f, 16, 16),
                Scale = scale,
            };

            return obj;
        }

        public IGroundObjectListRenderModel CreateGroundObjectList()
        {
            var obj = new GroundObjectListRenderModel()
            {

            };

            return obj;
        }
        public IRetranslatorRenderModel CreateRetranslator(double scale)
        {
         //   var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
            var factory = _serviceProvider.GetService<IFactory>();

            var mesh = factory.CreateSolidSphere(1.0f, 16, 16);

            var obj = new RetranslatorRenderModel()
            {
                Mesh = mesh,//factory.CreateSolidSphere(0.06f, 16, 16),
                Scale = scale,
            };
            return obj;
        }

        public ISensorRenderModel CreateSatelliteSensor()
        {
          //  var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
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

        public IFrameRenderModel CreateFrame(float scale)
        {
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
           // var d1evice = _serviceProvider.GetService<IDevice>();
            var factory = _serviceProvider.GetService<IFactory>();

            var obj = new FrameRenderModel()
            {
                Scale = scale,
            };

            return obj;
        }

        public ISatelliteRenderModel CreateSatellite(double scale)
        {
           // var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
          //  var d1evice = _serviceProvider.GetService<IDevice>();
            var factory = _serviceProvider.GetService<IFactory>();
            var modelLoader = _serviceProvider.GetService<IModelLoader>();

            var model = modelLoader.LoadModel(@"C:\resource\models\satellite_v3.ase", true);

            var obj = new SatelliteRenderModel()
            {             
                Scale = scale,// 0.002,        
             //   Meshes = modelLoader.Meshes.ToImmutableArray(),
             //   Loader = modelLoader,
                Model = model,
       //         Filename = @"C:\resource\models\satellite_v3.ase",       
            };

            return obj;
        }

        public ISensorRenderModel CreateSensor()
        {
            var obj = new SensorRenderModel()
            {

            };

            return obj;
        }
        public IAntennaRenderModel CreateAntenna()
        {                                
            var obj = new AntennaRenderModel()
            {
                AttachPosition = new dvec3(67.74, -12.22, -23.5),
            };

            return obj;
        }
    }



}
