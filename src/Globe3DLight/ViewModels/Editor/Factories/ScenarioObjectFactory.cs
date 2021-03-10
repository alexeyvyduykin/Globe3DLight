using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Scene;
using System.Linq;
using System.Collections.Immutable;
using Globe3DLight.Containers;
using GlmSharp;
using System.Threading;
using Globe3DLight.Data;

namespace Globe3DLight.Editor
{
    public interface IScenarioObjectFactory
    {
        ISpacebox CreateSpacebox(string name, ILogicalTreeNode parent);

        IEarth CreateEarth(string name, ILogicalTreeNode parent);

        ISatellite CreateSatellite(string name, ILogicalTreeNode parent);

        ISun CreateSun(string name, ILogicalTreeNode parent);

        ISensor CreateSensor(string name, ILogicalTreeNode parent);

        IGroundStation CreateGroundStation(string name, ILogicalTreeNode parent);

        IScenarioObjectList CreateScenarioObjectList(string name, IEnumerable<IScenarioObject> values);

        IGroundObject CreateGroundObject(string name, ILogicalTreeNode parent);

        IRetranslator CreateRetranslator(string name, ILogicalTreeNode parent);

        IAntenna CreateAntenna(string name, ILogicalTreeNode parent);

        IOrbit CreateOrbit(string name, ILogicalTreeNode parent);

        ISceneState CreateSceneState();

        ICamera CreateArcballCamera(ITargetable target);

        ISatelliteTask CreateSatelliteTask(ISatellite satellite, RotationData rotationData, SensorData sensorData, AntennaData antennaData, DateTime epochOnDay);

        IEnumerable<ISatelliteEvent> CreateRotationEvents(RotationData data, DateTime epochOnDay);
        IEnumerable<ISatelliteEvent> CreateObservationEvents(SensorData data, DateTime epochOnDay);
        IEnumerable<ISatelliteEvent> CreateTransmissionEvents(AntennaData data, DateTime epochOnDay);
    }

    public class ScenarioObjectFactory : IScenarioObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScenarioObjectFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISpacebox CreateSpacebox(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

      //      var lib = factory.CreateLibrary<IRenderModel>("");
     //       var builder = lib.Items.ToBuilder();
     //       builder.Add(renderModelFactory.CreateSpacebox());
     //       lib.Items = builder.ToImmutable();

            var obj = new Spacebox()
            {
                Name = name,
            //    RenderModelLibrary = lib,
                RenderModel = renderModelFactory.CreateSpacebox(1000000.0/*25000.0*/),//lib.Items.FirstOrDefault(),
                IsVisible = true,    
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode =parent,// factory.CreateLogicalTreeNode(),
            };

            return obj;
        }

        public IEarth CreateEarth(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            //   var libRenderModel = factory.CreateLibrary<IRenderModel>("");
            //   var builder1 = libRenderModel.Items.ToBuilder();
            //   builder1.Add(renderModelFactory.CreateEarth());
            //   libRenderModel.Items = builder1.ToImmutable();

            var renderModel = renderModelFactory.CreateEarth();
        //    System.Threading.Thread thr = new System.Threading.Thread(renderModel.Load);
        //    thr.Start();

            var obj = new Earth()
            {
                Name = name,
                FrameRenderModel = renderModelFactory.CreateFrame(6371.0f * 1.3f),    
                RenderModel = renderModel,         
                IsVisible = true,           
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,//factory.CreateLogicalTreeNode(), 
            };

            return obj;

        }

        public ISatellite CreateSatellite(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            //var libRenderModel = factory.CreateLibrary<IRenderModel>("");
            //var builder1 = libRenderModel.Items.ToBuilder();
            //builder1.Add(renderModelFactory.CreateSatellite());
            //libRenderModel.Items = builder1.ToImmutable();

            var obj = new Globe3DLight.ScenarioObjects.Satellite()
            {
                Name = name,
                IsVisible = true,
              //  RenderModelLibrary = libRenderModel,
                RenderModel = renderModelFactory.CreateSatellite(1), //0.009 //libRenderModel.Items.FirstOrDefault(),
                FrameRenderModel = renderModelFactory.CreateFrame(200.0f),                
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode =parent,// factory.CreateLogicalTreeNode(),               
            };

            return obj;
        }

        public ISun CreateSun(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
            var imageLoader = _serviceProvider.GetService<IImageLoader>();

            //   var libRenderModel = factory.CreateLibrary<IRenderModel>("");
            //   var builder1 = libRenderModel.Items.ToBuilder();
            //    builder1.Add(renderModelFactory.CreateSun());
            //    libRenderModel.Items = builder1.ToImmutable();

            var renderModel = renderModelFactory.CreateSun();
            
          //  var thr = new Thread(() => renderModel.Load(imageLoader));
          //  thr.Start();
            
            var obj = new Sun()
            {
                Name = name,
                IsVisible = true,         
                RenderModel = renderModel,//libRenderModel.Items.FirstOrDefault(),
                
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode =parent,// factory.CreateLogicalTreeNode(),
                
            };

            return obj;
        }

        public ISensor CreateSensor(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();


            var obj = new Sensor()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateSensor(),//libRenderModel.Items.FirstOrDefault(),

                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,// factory.CreateLogicalTreeNode(),
            };

            return obj;
        }


        public IAntenna CreateAntenna(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();


            var obj = new Antenna()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateAntenna(),//libRenderModel.Items.FirstOrDefault(),

                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,// factory.CreateLogicalTreeNode(),
                Assets = ImmutableArray.Create<IScenarioObject>(),
                FrameRenderModel = renderModelFactory.CreateFrame(50.0f),
            };

            return obj;
        }

        public IOrbit CreateOrbit(string name, ILogicalTreeNode parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Orbit()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateOrbit(),
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,
            };

            return obj;
        }

        public IGroundStation CreateGroundStation(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();


            var obj = new GroundStation()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateGroundStation(100.0),//libRenderModel.Items.FirstOrDefault(),
                
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,// factory.CreateLogicalTreeNode(),
                //UniqueName = factory.CreateUniqueName(string.Format("GST{0:0000000}", Math.Abs(id))),
            };

            return obj;
        }

        public IScenarioObjectList CreateScenarioObjectList(string name, IEnumerable<IScenarioObject> values)
        {           
            //var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var builder = ImmutableArray.CreateBuilder<IScenarioObject>();
            builder.AddRange(values);

            var obj = new ScenarioObjectList()
            { 
                Name = name,
                IsVisible = true,
                IsExpanded = false,
                //RenderModel = renderModelFactory.CreateGroundStation(100.0),                
                Values = builder.ToImmutable(),
            };

            return obj;
        }

        public IGroundObject CreateGroundObject(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new GroundObject()
            {
                Name = name,
                IsVisible = true,           
                RenderModel = renderModelFactory.CreateGroundObject(),

                Children = ImmutableArray.Create<IScenarioObject>(),               
                LogicalTreeNode = parent,                
            };

            return obj;
        }

        public IRetranslator CreateRetranslator(string name, ILogicalTreeNode parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            //var libRenderModel = factory.CreateLibrary<IRenderModel>("");
            //var builder1 = libRenderModel.Items.ToBuilder();
            //builder1.Add(renderModelFactory.CreateSatellite());
            //libRenderModel.Items = builder1.ToImmutable();

            var obj = new Retranslator()
            {
                Name = name,
                IsVisible = true,
                //  RenderModelLibrary = libRenderModel,
                RenderModel = renderModelFactory.CreateRetranslator(1000), //0.009 //libRenderModel.Items.FirstOrDefault(),             
                Children = ImmutableArray.Create<IScenarioObject>(),
                LogicalTreeNode = parent,// factory.CreateLogicalTreeNode(),        
                //UniqueName = factory.CreateUniqueName(string.Format("RTR{0:0000000}", Math.Abs(id))),
            };

            return obj;
        }

        //public IEarthRenderModel CreateEarthDefault()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();

        //    var maps = ImmutableArray.Create<string>("pos_x.dds", "neg_z.dds", "neg_x.dds", "pos_z.dds", "pos_y.dds", "neg_y.dds");

        //    var obj = new Earth()
        //    {
        //        DiffuseMaps = ImmutableArray.Create<ITexture>(),
        //        SpecularMaps = ImmutableArray.Create<ITexture>(),
        //        NormalMaps = ImmutableArray.Create<ITexture>(),
        //        NightMaps = ImmutableArray.Create<ITexture>(),
        //        Meshes = CreateCubeSphere().ToImmutableArray(),
        //        Loader = ddsLoader,
        //        DiffuseMapFilenames = maps.Select(s => "resources/textures/earth/diffuseQubeMap/" + s).ToList(),
        //        SpecularMapFilenames = maps.Select(s => "resources/textures/earth/specInvertQubeMap/" + s).ToList(),
        //        NormalMapFilenames = maps.Select(s => "resources/textures/earth/normalQubeMap/" + s).ToList(),
        //        NightMapFilenames = maps.Select(s => "resources/textures/earth/nightQubeMap/" + s).ToList(),
        //        Children = ImmutableArray.Create<ISceneObject>(),
        //        ModelMatrix = dmat4.Translate(0.0, 0.0, 0.0),
        //    };

        //    return obj;
        //}

        //public IEarthRenderModel CreateEarthSimple()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();

        //    var obj = new Earth()
        //    {
        //        DiffuseMaps = ImmutableArray.Create<ITexture>(),
        //        SpecularMaps = ImmutableArray.Create<ITexture>(),
        //        NormalMaps = ImmutableArray.Create<ITexture>(),
        //        NightMaps = ImmutableArray.Create<ITexture>(),
        //        Meshes = CreateCubeSphere().ToImmutableArray(),
        //        Loader = ddsLoader,
        //        DiffuseMapFilenames = new List<string>() { "C:/data/textures/Earth2D/EarthDiffuseMap.dds" },
        //        SpecularMapFilenames = new List<string>() { "C:/data/textures/Earth2D/earthSpecMap.dds" },
        //        NormalMapFilenames = new List<string>() { "C:/data/textures/Earth2D/earthNormalMap.dds" },
        //        NightMapFilenames = new List<string>() { "C:/data/textures/Earth2D/earthNightMap.dds" },
        //        Children = ImmutableArray.Create<ISceneObject>(),
        //        ModelMatrix = dmat4.Translate(0.0, 0.0, 0.0),
        //    };

        //    return obj;
        //}

        //public IGroundStationRenderModel CreateGroundStation()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new GroundStation()
        //    {
        //        Mesh = factory.CreateSolidSphere(0.06f, 16, 16),
        //    };

        //    return obj;
        //}

        //public IRetranslatorRenderModel CreateRetranslator()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new Retranslator()
        //    {
        //        Mesh = factory.CreateSolidSphere(1.0f, 32, 32),
        //    };

        //    return obj;
        //}

        //public ISensorRenderModel CreateSatelliteSensor()
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new Sensor()
        //    {

        //    };

        //    return obj;
        //}

        //public IFrame CreateFrame(float scale)
        //{
        //    var ddsLoader = _serviceProvider.GetService<IDDSLoader>();
        //    var device = _serviceProvider.GetService<IDevice>();
        //    var factory = _serviceProvider.GetService<IFactory>();

        //    var obj = new Frame()
        //    {
        //        Scale = scale,
        //    };

        //    return obj;
        //}

        public ISceneState CreateSceneState()
        {
            var target = new RootFrame();
            var camera = CreateArcballCamera(target);
        
            return new SceneState()
            {
                DiffuseIntensity = 0.65f,
                SpecularIntensity = 0.25f,
                AmbientIntensity = 0.10f,
                Shininess = 12,
                HighResolutionSnapScale = 1,
                Camera = camera,
                Target = target,
                LightPosition = dvec4.Zero,
                //           LightPosition = new dvec4(-15.0, -53.0, -23, 1.0),
                //            WorldScale = 10.0 / 6371.0,
                FieldOfViewY = 70.0f * (float)Math.PI / 180.0f, //Math.PI / 6.0, //70.0;
                AspectRatio = 1,

                PerspectiveNearPlaneDistance = 10.5, // 0.5;
                PerspectiveFarPlaneDistance = 2500000.0,
            };
        }

        public ICamera CreateArcballCamera(ITargetable target)
        {
            if (target is ISatellite/* satellite*/)
            {                    
                return new ArcballCamera(new dvec3(-200.0, 200.0, -200.0), dvec3.Zero, dvec3.UnitY);
            }
            else if (target is IEarth/* earth*/)
            {
                return new ArcballCamera(new dvec3(0.0, 0.0, 20000.0), dvec3.Zero, dvec3.UnitY);
            }
            else
            {
                return new ArcballCamera(new dvec3(0.0, 0.0, 20000.0), dvec3.Zero, dvec3.UnitY);            
            }
        }


        public ISatelliteTask CreateSatelliteTask(ISatellite satellite, RotationData rotationData, SensorData sensorData, AntennaData antennaData, DateTime epochOnDay)
        {
            var name = satellite.Name;
        
            var events = new List<ISatelliteEvent>();

            events.AddRange(CreateRotationEvents(rotationData, epochOnDay));
            events.AddRange(CreateObservationEvents(sensorData, epochOnDay));
            events.AddRange(CreateTransmissionEvents(antennaData, epochOnDay));

            var sortEvents = events.OrderBy(s => s.Begin).ToList();

            return new SatelliteTask(sortEvents)
            {
                Name = name,
                HasRotations = true,
                HasObservations = true,
                HasTransmissions = true,
                SearchString = string.Empty,
                //Events = sortEvents,
                //SelectedEvent = sortEvents.FirstOrDefault(),
            };
        }

        public IEnumerable<ISatelliteEvent> CreateRotationEvents(RotationData data, DateTime epochOnDay)
        {
            var events = new List<ISatelliteEvent>();

            var dt = epochOnDay.AddSeconds(data.TimeBegin);
            foreach (var item in data.Rotations)
            {
                events.Add(new RotationEvent()
                {
                    Name = (item.Angle < 0) ? "Left" : "Right",
                    Begin = dt.AddSeconds(item.BeginTime),
                    Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                });
            }

            return events;
        }

        public IEnumerable<ISatelliteEvent> CreateObservationEvents(SensorData data, DateTime epochOnDay)
        {
            var events = new List<ISatelliteEvent>();
            var dt = epochOnDay.AddSeconds(data.TimeBegin);
            foreach (var item in data.Shootings)
            {
                events.Add(new ObservationEvent()
                {
                    Name = item.TargetName,
                    Begin = dt.AddSeconds(item.BeginTime),
                    Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                });
            }
            
            return events;
        }
        public IEnumerable<ISatelliteEvent> CreateTransmissionEvents(AntennaData data, DateTime epochOnDay)
        {
            var events = new List<ISatelliteEvent>(); 
            var dt = epochOnDay.AddSeconds(data.TimeBegin);
            foreach (var item in data.Translations)
            {
                events.Add(new TransmissionEvent()
                {
                    Name = item.Target,
                    Begin = dt.AddSeconds(item.BeginTime),
                    Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                });
            }
            return events;
        }
    }
}
