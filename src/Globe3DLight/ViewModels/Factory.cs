#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using GlmSharp;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Models.Scene;
using Globe3DLight.Timer;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Renderer;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.ViewModels.Editors;

namespace Globe3DLight.ViewModels
{
    public class Factory : IFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public Factory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public LibraryViewModel<T> CreateLibrary<T>(string name)
        {
            return new LibraryViewModel<T>()
            {
                Name = name,
                Items = ImmutableArray.Create<T>(),
                Selected = default
            };
        }

        public LibraryViewModel<T> CreateLibrary<T>(string name, IEnumerable<T> items)
        {
            return new LibraryViewModel<T>()
            {
                Name = name,
                Items = ImmutableArray.CreateRange<T>(items),
                Selected = items.FirstOrDefault()
            };
        }

        public ICache<TKey, TValue> CreateCache<TKey, TValue>(Action<TValue> dispose = null)
        {
            return new Cache<TKey, TValue>(dispose);
        }

        public IdentityState CreateIdentityState() => new IdentityState();

        public ProjectContainerViewModel CreateProjectContainer(string name = "Project")
        {
            return new ProjectContainerViewModel()
            {
                Name = name,
                Scenarios = ImmutableArray.Create<ScenarioContainerViewModel>()
            };
        }

        public ScenarioContainerViewModel CreateScenarioContainer(string name, DateTime begin, TimeSpan duration)
        {    
            var scenario = new ScenarioContainerViewModel()
            {
                Name = name,       
                IsExpanded = true,                                  
                SceneState = CreateSceneState(),
                Updater = CreateDataUpdater(),
                SceneTimerEditor = CreateSceneTimerEditor(begin, duration),
                CurrentScenarioMode = ScenarioMode.Visual,            
            };

            scenario.TaskListEditor = CreateTaskListEditor(scenario);
            scenario.OutlinerEditor = CreateOutlinerEditor(scenario);
            scenario.PropertiesEditor = CreatePropertiesEditor(scenario);
      
            return scenario;
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
                PerspectiveNearPlaneDistance = 85,// 10.5, // 0.5;
                PerspectiveFarPlaneDistance = 2500000.0,                
            };
        }

        public ICamera CreateArcballCamera(dvec3 eye)
        {
            return new ArcballCamera(eye, dvec3.Zero, dvec3.UnitY);
        }

        //public LogicalCollectionViewModel CreateLogicalCollection(string name)
        //{
        //    var builder = ImmutableArray.CreateBuilder<LogicalViewModel>();

        //    return new LogicalCollectionViewModel()
        //    {
        //        Name = name,
        //        //State = states,
        //        Values = builder.ToImmutable(),
        //    };
        //}

        private ITimer CreateAcceleratedTimer()
        {
            return new AcceleratedTimer();
        }

        public SceneTimerEditorViewModel CreateSceneTimerEditor(DateTime dateTime, TimeSpan timeSpan)
        {
            var timer = CreateAcceleratedTimer();

            return new SceneTimerEditorViewModel(timer, dateTime, timeSpan);
        }

        public TaskListEditorViewModel CreateTaskListEditor(ScenarioContainerViewModel scenario)
        {
            return new TaskListEditorViewModel(scenario);
        }

        public OutlinerEditorViewModel CreateOutlinerEditor(ScenarioContainerViewModel scenario)
        {
            var frame = CreateRootFrame();

            frame.Owner = scenario;

            var editor = new OutlinerEditorViewModel(scenario) 
            {        
                SelectedMode = DisplayMode.Visual,
                FrameRoot = ImmutableArray.Create<FrameViewModel>(frame),            
                CurrentFrame = frame,
                Entities = ImmutableArray.Create<BaseEntity>(),
            };

            return editor;
        }

        public PropertiesEditorViewModel CreatePropertiesEditor(ScenarioContainerViewModel scenario)
        {
            var editor = new PropertiesEditorViewModel(scenario);

            return editor;
        }

        public IDataUpdater CreateDataUpdater()
        {
            return new DataUpdater();
        }

        public void SaveProjectContainer(ProjectContainerViewModel project, string path, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //if (project is IImageCache imageCache)
            {
                using var stream = fileIO.Create(path);
                SaveProjectContainer(project/*, imageCache*/, stream, fileIO, serializer);
            }
        }

        public ProjectContainerViewModel OpenProjectContainer(string path, IFileSystem fileIO, IJsonSerializer serializer)
        {
            using var stream = fileIO.Open(path);
            return OpenProjectContainer(stream, fileIO, serializer);
        }

        public ProjectContainerViewModel OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            //var projectEntry = archive.Entries.FirstOrDefault(e => e.FullName == "Project.json");
            var project = ReadProjectContainer(stream, fileIO, serializer);
            return project;
        }

        //public IProjectContainer OpenProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        //    var projectEntry = archive.Entries.FirstOrDefault(e => e.FullName == "Project.json");
        //    var project = ReadProjectContainer(projectEntry, fileIO, serializer);
        //    return project;
        //}

        public void SaveProjectContainer(ProjectContainerViewModel project, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            //var projectEntry = archive.CreateEntry("Project.json");
            WriteProjectContainer(project, stream, fileIO, serializer);
        }
        //public void SaveProjectContainer(IProjectContainer project/*, IImageCache imageCache*/, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        //    var projectEntry = archive.CreateEntry("Project.json");
        //    WriteProjectContainer(project, projectEntry, fileIO, serializer);
        //}
        private ProjectContainerViewModel ReadProjectContainer(Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            return serializer.Deserialize<ProjectContainerViewModel>(fileIO.ReadUtf8Text(stream));
        }
        //private IProjectContainer ReadProjectContainer(ZipArchiveEntry projectEntry, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var entryStream = projectEntry.Open();
        //    return serializer.Deserialize<ProjectContainer>(fileIO.ReadUtf8Text(entryStream));
        //}

        private void WriteProjectContainer(ProjectContainerViewModel project, Stream stream, IFileSystem fileIO, IJsonSerializer serializer)
        {
            //using var jsonStream = projectEntry.Open();
            fileIO.WriteUtf8Text(stream, serializer.Serialize(project));
        }
        //private void WriteProjectContainer(IProjectContainer project, ZipArchiveEntry projectEntry, IFileSystem fileIO, IJsonSerializer serializer)
        //{
        //    using var jsonStream = projectEntry.Open();
        //    fileIO.WriteUtf8Text(jsonStream, serializer.Serialize(project));
        //}
        private DateTime FromJulianDate(double jd) => DateTime.FromOADate(jd - 2415018.5);


        public FrameViewModel CreateRootFrame()
        {
            var frame = new FrameViewModel()
            {
                Parent = null,
                Children = ImmutableArray.Create<FrameViewModel>(),                
                Name = "fr_root",
                IsExpanded = true,
                IsVisible = false,
                State = CreateIdentityState(),                               
                RenderModel = null,
            };

            return frame;
        }

        public FrameViewModel CreateFrame(string name, FrameViewModel parent)
        {
            var frame = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = name,
                IsExpanded = false,
                IsVisible = false,
                State = CreateIdentityState(),
                RenderModel = null,
            };

            parent.AddChild(frame);

            return frame;
        }

        public FrameViewModel CreateCollectionFrame(string name, FrameViewModel parent)
        {
            var fr_collection = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = name,
                IsExpanded = false,
                IsVisible = true,
                State = null,
                RenderModel = null,
            };

            parent.AddChild(fr_collection);

            return fr_collection;
        }

        public FrameViewModel CreateSunFrame(SunData data, FrameViewModel parent)
        {    
            var frame = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                IsExpanded = false,
                IsVisible = false,
                State = new SunAnimator(data),
                RenderModel = null,
            };

            parent.AddChild(frame);

            return frame;
        }

        public FrameViewModel CreateEarthFrame(EarthData data, FrameViewModel parent, float scale)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
         
            var frame = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                IsExpanded = true,
                IsVisible = true,
                State = new EarthAnimator(data),
                RenderModel = renderModelFactory.CreateFrame(scale),
            };

            parent.AddChild(frame);

            return frame;
        }

        public FrameViewModel CreateGroundObjectFrame(GroundObjectData data, FrameViewModel parent, FrameRenderModel model)
        {          
            var fr_groundObject = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                IsExpanded = false,
                IsVisible = true,
                State = new GroundObjectState(data),
                RenderModel = model,
            };

            parent.AddChild(fr_groundObject);

            return fr_groundObject;
        }

        public FrameViewModel CreateGroundStationFrame(GroundStationData data, FrameViewModel parent, FrameRenderModel model)
        {       
            var fr_groundStation = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                IsExpanded = false,
                IsVisible = true,
                State = new GroundStationState(data),
                RenderModel = model,
            };

            parent.AddChild(fr_groundStation);

            return fr_groundStation;
        }

        public FrameViewModel CreateRetranslatorFrame(RetranslatorData data, FrameViewModel parent, FrameRenderModel model)
        {           
            var fr_retranslator = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                IsExpanded = false,
                IsVisible = true,
                State = new RetranslatorAnimator(data),
                RenderModel = model,
            };

            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }

        public (FrameViewModel fr_sat, FrameViewModel fr_rot, FrameViewModel fr_sen, FrameViewModel fr_ant, FrameViewModel fr_orb)
            CreateSatellitesFrame(SatelliteData satelliteData, RotationData rotationData, SensorData sensorData,
            AntennaData antennaData, OrbitData orbitData, FrameViewModel parent, 
            FrameRenderModel satelliteModel, FrameRenderModel antennaModel, EntityList gss, EntityList rtrs)
        {
            var fr_satellite = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}", satelliteData.Name.ToLower()),
                IsExpanded = true,
                IsVisible = false,
                State = new SatelliteAnimator(satelliteData),
                RenderModel = null,
            };

            var fr_rotation = new FrameViewModel()
            {
                Parent = fr_satellite,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}_{1}", rotationData.Name.ToLower(), rotationData.SatelliteName.ToLower()),
                IsExpanded = false,
                IsVisible = true,
                State = new RotationAnimator(rotationData),
                RenderModel = satelliteModel,
            };

            var fr_sensor = new FrameViewModel()
            {
                Parent = fr_satellite,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}_{1}", sensorData.Name.ToLower(), sensorData.SatelliteName.ToLower()),
                IsExpanded = false,
                IsVisible = false,
                State = new SensorAnimator(sensorData),
                RenderModel = null,
            };

            var fr_antenna = new FrameViewModel()
            {
                Parent = fr_rotation,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}_{1}", antennaData.Name.ToLower(), antennaData.SatelliteName.ToLower()),
                IsExpanded = false,
                IsVisible = true,
                State = new AntennaAnimator(antennaData)
                {                   
                    Assets = ImmutableArray.Create<BaseEntity>(),
                    AttachPosition = new dvec3(67.74, -12.22, -23.5),
                },
                RenderModel = antennaModel,
            };

            var fr_orbit = new FrameViewModel()
            {
                Parent = parent,
                Children = ImmutableArray.Create<FrameViewModel>(),
                Name = string.Format("fr_{0}_{1}", orbitData.Name.ToLower(), orbitData.SatelliteName.ToLower()),
                IsExpanded = false,
                IsVisible = false,
                State = new OrbitState(orbitData),
                RenderModel = null,
            };

            parent.AddChild(fr_orbit);
            parent.AddChild(fr_satellite);
            fr_satellite.AddChild(fr_rotation);
            fr_satellite.AddChild(fr_sensor);
            fr_rotation.AddChild(fr_antenna);

            ((AntennaAnimator)fr_antenna.State).AddAssets(gss.Values);
            ((AntennaAnimator)fr_antenna.State).AddAssets(rtrs.Values);

            return (fr_satellite, fr_rotation, fr_sensor, fr_antenna, fr_orbit);
        }

        //------------------------------------------------------------------------------------

        public Spacebox CreateSpacebox(FrameViewModel parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var fr_spacebox = CreateFrame("fr_spacebox", parent);

            var obj = new Spacebox()
            {
                Name = "Spacebox",
                RenderModel = renderModelFactory.CreateSpacebox(),
                IsVisible = true,
                Children = ImmutableArray.Create<BaseEntity>(),            
                Frame = fr_spacebox,
            };

            fr_spacebox.State.Parent = parent.State;

            fr_spacebox.Owner = obj;

            return obj;
        }

        public Sun CreateSun(SunData data, FrameViewModel parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var fr_sun = CreateSunFrame(data, parent);

            var obj = new Sun()
            {
                Name = data.Name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateSun(),
                Children = ImmutableArray.Create<BaseEntity>(),
                Frame = fr_sun,
            };
            
            fr_sun.State.Parent = parent.State;
            fr_sun.Owner = obj;

            return obj;
        }

        public Earth CreateEarth(EarthData data, FrameViewModel parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var fr_earth = CreateEarthFrame(data, parent, 6371.0f * 1.3f);
       
            var obj = new Earth()
            {
                Name = data.Name,            
                RenderModel = renderModelFactory.CreateEarth(),
                IsVisible = true,
                Children = ImmutableArray.Create<BaseEntity>(),
                Frame = fr_earth,
            };

            fr_earth.State.Parent = parent.State;
            fr_earth.Owner = obj;

            return obj;
        }

        public EntityList CreateGroundObjects(ScenarioData data, FrameViewModel parent)
        {       
            var renderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateGroundObject();
            var frameRenderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateFrame(30.0f);

            var entities = new List<BaseEntity>();

            var fr_go_collection = CreateCollectionFrame("fr_go_collection", parent);

            foreach (var item in data.GroundObjects)
            {
                var fr_go = CreateGroundObjectFrame(item, fr_go_collection, frameRenderModel);

                var go = new GroundObject()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,                  
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_go,
                };

                fr_go.State.Parent = parent.State;
                fr_go.Owner = go;

                entities.Add(go);
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "GroundObjects",
                IsVisible = true,
                IsExpanded = false,              
                Values = builder.ToImmutable(),
            };
        }

        public EntityList CreateGroundStations(ScenarioData data, FrameViewModel parent)
        {          
            var renderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateGroundStation(180.0);
            var frameRenderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateFrame(200.0f);

            var fr_gs_collection = CreateCollectionFrame("fr_gs_collection", parent);

            var entities = new List<BaseEntity>();

            foreach (var item in data.GroundStations)
            {
                var fr_gs = CreateGroundStationFrame(item, fr_gs_collection, frameRenderModel);
              
                var gs = new GroundStation()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_gs,
                };
                
                fr_gs.State.Parent = parent.State;
                fr_gs.Owner = gs;

                entities.Add(gs);
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "GroundStations",
                IsVisible = true,
                IsExpanded = false,
                //LogicalCollection = fr_gs_collection,
                Values = builder.ToImmutable(),
            };
        }

        public EntityList CreateRetranslators(ScenarioData data, FrameViewModel parent)
        { 
            var renderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateRetranslator(500.0);
            var frameRenderModel = _serviceProvider.GetService<IRenderModelFactory>().CreateFrame(6050.0f);

            var fr_rtr_collection = CreateCollectionFrame("fr_rtr_collection", parent);

            var entities = new List<BaseEntity>();

            foreach (var item in data.RetranslatorPositions)
            {
                var fr_rtr = CreateRetranslatorFrame(item, fr_rtr_collection, frameRenderModel);

                var rtr = new Retranslator()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_rtr,
                };
                
                fr_rtr.State.Parent = parent.State;
                fr_rtr.Owner = rtr;

                entities.Add(rtr);
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "Retranslators",
                IsVisible = true,
                IsExpanded = false,
                //LogicalCollection = fr_rtr_collection,
                Values = builder.ToImmutable(),
            };
        }

        public IList<Satellite> CreateSatellites(ScenarioData data, FrameViewModel parent, EntityList gss, EntityList rtrs)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();
            var antennaModel = renderModelFactory.CreateAntenna();
            var antennaFrameModel = renderModelFactory.CreateFrame(50.0f);
            var satelliteFrameModel = renderModelFactory.CreateFrame(200.0f);
            var sensorModel = renderModelFactory.CreateSensor();
            var satelliteModel = renderModelFactory.CreateSatellite(1.0);

            var list = new List<Satellite>();

            for (int i = 0; i < data.SatellitePositions.Count; i++)
            {
                var (fr_sat, fr_rot, fr_sen, fr_ant, fr_orb) = CreateSatellitesFrame(
                    data.SatellitePositions[i], data.SatelliteRotations[i], 
                    data.SatelliteShootings[i], data.SatelliteTransfers[i], data.SatelliteOrbits[i],
                    parent, satelliteFrameModel, antennaFrameModel, gss, rtrs);
                                        
                var satellite = new Satellite()
                {
                    Name = data.SatellitePositions[i].Name,
                    IsVisible = true,
                    RenderModel = satelliteModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_rot,               
                };
                var sensor = new Sensor()
                {
                    Name = string.Format("Sensor{0}", i + 1),
                    IsVisible = true,
                    RenderModel = sensorModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_sen,
                };
                var antenna = new Antenna()
                {
                    Name = string.Format("Antenna{0}", i + 1),
                    IsVisible = true,
                    RenderModel = antennaModel,                 
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_ant,
                };
                var orbit = new Orbit()
                {
                    Name = string.Format("Orbit{0}", i + 1),
                    IsVisible = true,
                    RenderModel = renderModelFactory.CreateOrbit(),
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Frame = fr_orb,
                };

                fr_sat.State.Parent = parent.State;
                fr_rot.State.Parent = fr_sat.State;
                fr_sen.State.Parent = fr_sat.State;
                fr_ant.State.Parent = fr_rot.State;
                fr_orb.State.Parent = parent.State;

                fr_rot.Owner = satellite;
                fr_sen.Owner = sensor;
                fr_ant.Owner = antenna;
                fr_orb.Owner = orbit;

                satellite.AddChild(sensor);
                satellite.AddChild(antenna);
                satellite.AddChild(orbit);

                list.Add(satellite);
            }

            return list;
        }

        public IList<SatelliteTask> CreateSatelliteTasks(IList<Satellite> satellites, ScenarioData data)
        {
            var list = new List<SatelliteTask>();

            var epochOnDay = FromJulianDate(data.JulianDateOnTheDay);

            for (int i = 0; i < satellites.Count; i++)
            {
                var name = satellites[i].Name;

                var events = new List<BaseSatelliteEvent>();

                events.AddRange(CreateRotationEvents(data.SatelliteRotations[i], epochOnDay));
                events.AddRange(CreateObservationEvents(data.SatelliteShootings[i], epochOnDay));
                events.AddRange(CreateTransmissionEvents(data.SatelliteTransfers[i], epochOnDay));

                var sortEvents = events.OrderBy(s => s.BeginTime/*Begin*/).ToList();

                var task = new SatelliteTask(satellites[i], sortEvents)
                {
                    Name = name,
                    //HasRotations = true,
                    //HasObservations = true,
                    //HasTransmissions = true,
                    //SearchString = string.Empty,
                };

                list.Add(task);
            }

            return list;

            IEnumerable<BaseSatelliteEvent> CreateRotationEvents(RotationData data, DateTime epochOnDay)
            {
                var events = new List<BaseSatelliteEvent>();

                var dt = epochOnDay.AddSeconds(data.TimeBegin);
                foreach (var item in data.Rotations)
                {
                    events.Add(new RotationEvent()
                    {
                        Name = (item.Angle < 0) ? "Left" : "Right",
                        Epoch = dt,
                        BeginTime = item.BeginTime,
                        EndTime = item.EndTime,
                        //Begin = dt.AddSeconds(item.BeginTime),
                        //Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                    });
                }

                return events;
            }

            IEnumerable<BaseSatelliteEvent> CreateObservationEvents(SensorData data, DateTime epochOnDay)
            {
                var events = new List<BaseSatelliteEvent>();
                var dt = epochOnDay.AddSeconds(data.TimeBegin);
                foreach (var item in data.Shootings)
                {
                    events.Add(new ObservationEvent()
                    {
                        Name = item.TargetName,
                        Epoch = dt,
                        BeginTime = item.BeginTime,
                        EndTime = item.EndTime,
                        //Begin = dt.AddSeconds(item.BeginTime),
                        //Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                    });
                }

                return events;
            }

            IEnumerable<BaseSatelliteEvent> CreateTransmissionEvents(AntennaData data, DateTime epochOnDay)
            {
                var events = new List<BaseSatelliteEvent>();
                var dt = epochOnDay.AddSeconds(data.TimeBegin);
                foreach (var item in data.Translations)
                {
                    events.Add(new TransmissionEvent()
                    {
                        Name = item.Target,
                        Epoch = dt,
                        BeginTime = item.BeginTime,
                        EndTime = item.EndTime,
                        //Begin = dt.AddSeconds(item.BeginTime),
                        //Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                    });
                }
                return events;
            }
        }

        public GroundObjectList CreateGroundObjectList(EntityList gos)
        {
            return new GroundObjectList(gos);
        }
    }
}
