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
        ISpacebox CreateSpacebox(string name, ILogical parent);

        IEarth CreateEarth(string name, ILogical parent);

        ISatellite CreateSatellite(string name, ILogical parent);

        ISun CreateSun(string name, ILogical parent);

        ISensor CreateSensor(string name, ILogical parent);

        IGroundStation CreateGroundStation(string name, ILogical parent);

        IScenarioObjectList CreateScenarioObjectList(string name, ILogicalCollection parent, IEnumerable<IScenarioObject> values);

        IGroundObject CreateGroundObject(string name, ILogical parent);

        IRetranslator CreateRetranslator(string name, ILogical parent);

        IAntenna CreateAntenna(string name, ILogical parent);

        IOrbit CreateOrbit(string name, ILogical parent);

        ISceneState CreateSceneState();

        ICamera CreateArcballCamera(dvec3 eye);

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

        public ISpacebox CreateSpacebox(string name, ILogical parent)
        {         
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Spacebox()
            {
                Name = name,        
                RenderModel = renderModelFactory.CreateSpacebox(1000000.0/*25000.0*/),//lib.Items.FirstOrDefault(),
                IsVisible = true,    
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,
            };

            return obj;
        }

        public IEarth CreateEarth(string name, ILogical parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
         
            var obj = new Earth()
            {
                Name = name,
                FrameRenderModel = renderModelFactory.CreateFrame(6371.0f * 1.3f),    
                RenderModel = renderModelFactory.CreateEarth(),         
                IsVisible = true,           
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,
            };

            return obj;
        }

        public ISatellite CreateSatellite(string name, ILogical parent)
        {       
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Satellite()
            {
                Name = name,
                IsVisible = true,            
                RenderModel = renderModelFactory.CreateSatellite(1), //0.009 //libRenderModel.Items.FirstOrDefault(),
                FrameRenderModel = renderModelFactory.CreateFrame(200.0f),                
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,          
            };

            return obj;
        }

        public ISun CreateSun(string name, ILogical parent)
        { 
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
                        
            var obj = new Sun()
            {
                Name = name,
                IsVisible = true,         
                RenderModel = renderModelFactory.CreateSun(),                
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,              
            };

            return obj;
        }

        public ISensor CreateSensor(string name, ILogical parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Sensor()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateSensor(),
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,
            };

            return obj;
        }

        public IAntenna CreateAntenna(string name, ILogical parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Antenna()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateAntenna(),//libRenderModel.Items.FirstOrDefault(),
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,
                Assets = ImmutableArray.Create<IScenarioObject>(),
                FrameRenderModel = renderModelFactory.CreateFrame(50.0f),
            };

            return obj;
        }

        public IOrbit CreateOrbit(string name, ILogical parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Orbit()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateOrbit(),
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,
            };

            return obj;
        }

        public IGroundStation CreateGroundStation(string name, ILogical parent)
        {      
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new GroundStation()
            {
                Name = name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateGroundStation(100.0),//libRenderModel.Items.FirstOrDefault(),                
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,  
            };

            return obj;
        }

        public IScenarioObjectList CreateScenarioObjectList(string name, ILogicalCollection parent, IEnumerable<IScenarioObject> values)
        {           
            var builder = ImmutableArray.CreateBuilder<IScenarioObject>();
            builder.AddRange(values);

            var obj = new ScenarioObjectList()
            { 
                Name = name,
                IsVisible = true,
                IsExpanded = false,
                LogicalCollection = parent,                        
                Values = builder.ToImmutable(),
            };

            return obj;
        }

        public IGroundObject CreateGroundObject(string name, ILogical parent)
        {        
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new GroundObject()
            {
                Name = name,
                IsVisible = true,           
                RenderModel = renderModelFactory.CreateGroundObject(),
                Children = ImmutableArray.Create<IScenarioObject>(),               
                Logical = parent,                
            };

            return obj;
        }

        public IRetranslator CreateRetranslator(string name, ILogical parent)
        {         
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Retranslator()
            {
                Name = name,
                IsVisible = true,            
                RenderModel = renderModelFactory.CreateRetranslator(1000), //0.009 //libRenderModel.Items.FirstOrDefault(),             
                Children = ImmutableArray.Create<IScenarioObject>(),
                Logical = parent,                  
            };

            return obj;
        }

        public ISceneState CreateSceneState()
        {
            var target = new RootFrame();
            var camera = CreateArcballCamera(new dvec3(0.0, 0.0, 20000.0));

            var cameraBehaviours = new Dictionary<Type, (dvec3 eye, Func<double, double> func)>();

            cameraBehaviours.Add(typeof(RootFrame), (new dvec3(0.0, 0.0, 20000.0), (x) => Math.Max(20.0, 0.025 * (x - 6400.0))));
            cameraBehaviours.Add(typeof(Earth), (new dvec3(0.0, 0.0, 20000.0), (x) => Math.Max(20.0, 0.025 * (x - 6400.0))));
            cameraBehaviours.Add(typeof(Satellite), (new dvec3(-200.0, 200.0, -200.0), (x) => Math.Max(5.0, 0.05 * (x - 100.0))));

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
