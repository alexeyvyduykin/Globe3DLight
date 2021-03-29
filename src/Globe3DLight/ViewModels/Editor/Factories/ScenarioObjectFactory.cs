using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Scene;
using System.Linq;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Containers;
using GlmSharp;
using System.Threading;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.ViewModels.Editor
{
    public interface IScenarioObjectFactory
    {
        Spacebox CreateSpacebox(string name, BaseState parent);

        Earth CreateEarth(string name, BaseState parent);

        Satellite CreateSatellite(string name, BaseState parent);

        Sun CreateSun(string name, BaseState parent);

        Sensor CreateSensor(string name, BaseState parent);

        GroundStation CreateGroundStation(string name, LogicalViewModel parent);

        EntityList CreateEntityList(string name, LogicalCollectionViewModel parent, IEnumerable<BaseEntity> values);

        GroundObject CreateGroundObject(string name, LogicalViewModel parent);

        Retranslator CreateRetranslator(string name, BaseState parent);

        Antenna CreateAntenna(string name, LogicalViewModel parent);

        Orbit CreateOrbit(string name, BaseState parent);

        ISceneState CreateSceneState();

        ICamera CreateArcballCamera(dvec3 eye);

        SatelliteTask CreateSatelliteTask(Satellite satellite, RotationData rotationData, SensorData sensorData, AntennaData antennaData, DateTime epochOnDay);

        IEnumerable<BaseSatelliteEvent> CreateRotationEvents(RotationData data, DateTime epochOnDay);
        IEnumerable<BaseSatelliteEvent> CreateObservationEvents(SensorData data, DateTime epochOnDay);
        IEnumerable<BaseSatelliteEvent> CreateTransmissionEvents(AntennaData data, DateTime epochOnDay);
    }

    public class ScenarioObjectFactory : IScenarioObjectFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ScenarioObjectFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Spacebox CreateSpacebox(string name, BaseState parent)
        {         
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Spacebox()
            {
                Name = name,        
                RenderModel = renderModelFactory.CreateSpacebox(1000000.0/*25000.0*/),//lib.Items.FirstOrDefault(),
                IsVisible = true,    
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
            };

            return obj;
        }

        public Earth CreateEarth(string name, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
         
            var obj = new Earth()
            {
                Name = name,
                FrameRenderModel = renderModelFactory.CreateFrame(6371.0f * 1.3f),    
                RenderModel = renderModelFactory.CreateEarth(),         
                IsVisible = true,           
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
            };

            return obj;
        }

        public Satellite CreateSatellite(string name, BaseState parent)
        {       
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Satellite()
            {
                Name = name,
                IsVisible = true,            
                RenderModel = renderModelFactory.CreateSatellite(1), //0.009 //libRenderModel.Items.FirstOrDefault(),
                FrameRenderModel = renderModelFactory.CreateFrame(200.0f),                
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,          
            };

            return obj;
        }

        public Sun CreateSun(string name, BaseState parent)
        { 
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
                        
            var obj = new Sun()
            {
                Name = name,
                IsVisible = true,         
                RenderModel = renderModelFactory.CreateSun(),                
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,              
            };

            return obj;
        }

        public Sensor CreateSensor(string name, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Sensor()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateSensor(),
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
            };

            return obj;
        }

        public Antenna CreateAntenna(string name, LogicalViewModel parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Antenna()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateAntenna(),//libRenderModel.Items.FirstOrDefault(),
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
                Assets = ImmutableArray.Create<BaseEntity>(),
                FrameRenderModel = renderModelFactory.CreateFrame(50.0f),
            };

            return obj;
        }

        public Orbit CreateOrbit(string name, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Orbit()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateOrbit(),
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
            };

            return obj;
        }

        public GroundStation CreateGroundStation(string name, LogicalViewModel parent)
        {      
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new GroundStation()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateGroundStation(70.0),
                FrameRenderModel = renderModelFactory.CreateFrame(200.0f),
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,  
            };

            return obj;
        }

        public EntityList CreateEntityList(string name, LogicalCollectionViewModel parent, IEnumerable<BaseEntity> values)
        {           
            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(values);

            var obj = new EntityList()
            { 
                Name = name,
                IsVisible = true,
                IsExpanded = false,
                LogicalCollection = parent,                        
                Values = builder.ToImmutable(),
            };

            return obj;
        }

        public GroundObject CreateGroundObject(string name, LogicalViewModel parent)
        {        
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new GroundObject()
            {
                Name = name,
                IsVisible = true,           
                RenderModel = renderModelFactory.CreateGroundObject(),
                FrameRenderModel = renderModelFactory.CreateFrame(30.0f),
                Children = ImmutableArray.Create<BaseEntity>(),               
                Logical = parent,                
            };

            return obj;
        }

        public Retranslator CreateRetranslator(string name, BaseState parent)
        {         
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Retranslator()
            {
                Name = name,
                IsVisible = true,            
                RenderModel = renderModelFactory.CreateRetranslator(1000), //0.009 //libRenderModel.Items.FirstOrDefault(),             
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,                  
            };

            return obj;
        }

        public ISceneState CreateSceneState()
        {
            var target = new RootFrame();
            var camera = CreateArcballCamera(new dvec3(0.0, 0.0, 20000.0));

            var cameraBehaviours = new Dictionary<Type, (dvec3 eye, Func<double, double> func)>
            {
                { typeof(RootFrame), (new dvec3(0.0, 0.0, 20000.0), (x) => Math.Max(20.0, 0.025 * (x - 6400.0))) },
                { typeof(Earth), (new dvec3(0.0, 0.0, 20000.0), (x) => Math.Max(20.0, 0.025 * (x - 6400.0))) },
                { typeof(Satellite), (new dvec3(-200.0, 200.0, -200.0), (x) => Math.Max(5.0, 0.05 * (x - 100.0))) },
                { typeof(GroundStation), (new dvec3(0.0, 500.0, 0.0), (x) => Math.Max(5.0, 0.05 * (x - 100.0))) },
                { typeof(GroundObject), (new dvec3(0.0, 250.0, 0.0), (x) => Math.Max(5.0, 0.05 * (x - 5.0))) }
            };

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
                FieldOfViewY = 70.0f * (float)Math.PI / 180.0f, //Math.PI / 6.0, //70.0;
                AspectRatio = 1,
                CameraBehaviours = cameraBehaviours,
                PerspectiveNearPlaneDistance = 10.5, // 0.5;
                PerspectiveFarPlaneDistance = 2500000.0,
            };
        }

        public ICamera CreateArcballCamera(dvec3 eye)
        {                
            return new ArcballCamera(eye, dvec3.Zero, dvec3.UnitY);                        
        }

        public SatelliteTask CreateSatelliteTask(Satellite satellite, RotationData rotationData, SensorData sensorData, AntennaData antennaData, DateTime epochOnDay)
        {
            var name = satellite.Name;
        
            var events = new List<BaseSatelliteEvent>();

            events.AddRange(CreateRotationEvents(rotationData, epochOnDay));
            events.AddRange(CreateObservationEvents(sensorData, epochOnDay));
            events.AddRange(CreateTransmissionEvents(antennaData, epochOnDay));

            var sortEvents = events.OrderBy(s => s.Begin).ToList();

            return new SatelliteTask(sortEvents)
            {
                Name = name,
                Satellite = satellite,
                HasRotations = true,
                HasObservations = true,
                HasTransmissions = true,
                SearchString = string.Empty,
                //Events = sortEvents,
                //SelectedEvent = sortEvents.FirstOrDefault(),
            };
        }

        public IEnumerable<BaseSatelliteEvent> CreateRotationEvents(RotationData data, DateTime epochOnDay)
        {
            var events = new List<BaseSatelliteEvent>();

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

        public IEnumerable<BaseSatelliteEvent> CreateObservationEvents(SensorData data, DateTime epochOnDay)
        {
            var events = new List<BaseSatelliteEvent>();
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
       
        public IEnumerable<BaseSatelliteEvent> CreateTransmissionEvents(AntennaData data, DateTime epochOnDay)
        {
            var events = new List<BaseSatelliteEvent>(); 
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
