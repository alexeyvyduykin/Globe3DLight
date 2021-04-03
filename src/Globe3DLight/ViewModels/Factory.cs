﻿using System;
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
using Globe3DLight.ViewModels.Geometry;
using Globe3DLight.ViewModels.Renderer;
using Globe3DLight.ViewModels.Scene;
using Globe3DLight.ViewModels.Time;

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

        public FrameState CreateFrameState(string name) => new FrameState()
        {
            Name = name,
            Children = ImmutableArray.Create<ViewModelBase>(),
        };

        public ProjectContainerViewModel CreateProjectContainer(string name = "Project")
        {
            return new ProjectContainerViewModel()
            {
                Name = name,
                Scenarios = ImmutableArray.Create<ScenarioContainerViewModel>()
            };
        }

        public ScenarioContainerViewModel CreateScenarioContainer(string name = "Scenario")
        {
            return new ScenarioContainerViewModel()
            {
                Name = name,
                LogicalRoot = ImmutableArray.Create<LogicalViewModel>(),
                Entities = ImmutableArray.Create<BaseEntity>(),
                Tasks = ImmutableArray.Create<SatelliteTask>(),
            };
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
                PerspectiveNearPlaneDistance = 80,// 10.5, // 0.5;
                PerspectiveFarPlaneDistance = 2500000.0,
            };
        }

        public ICamera CreateArcballCamera(dvec3 eye)
        {
            return new ArcballCamera(eye, dvec3.Zero, dvec3.UnitY);
        }

        public LogicalCollectionViewModel CreateLogicalCollection(string name)
        {
            var builder = ImmutableArray.CreateBuilder<LogicalViewModel>();

            return new LogicalCollectionViewModel()
            {
                Name = name,
                //State = states,
                Values = builder.ToImmutable(),
            };
        }

        private ITimer CreateAcceleratedTimer()
        {
            return new AcceleratedTimer();
        }

        public SliderTimePresenter CreateSliderTimePresenter(DateTime dateTime, TimeSpan timeSpan)
        {
            var timer = CreateAcceleratedTimer();

            return new SliderTimePresenter(timer, dateTime, timeSpan, 0, 1000);
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

        public Spacebox CreateSpacebox(BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var obj = new Spacebox()
            {
                Name = "Spacebox",
                RenderModel = renderModelFactory.CreateSpacebox(),
                IsVisible = true,
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = parent,
            };

            return obj;
        }

        public Sun CreateSun(SunData data, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var fr_sun = new SunAnimator(data)
            {
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                Children = ImmutableArray.Create<ViewModelBase>(),
            };

            parent.AddChild(fr_sun);

            var obj = new Sun()
            {
                Name = data.Name,
                IsVisible = true,
                RenderModel = renderModelFactory.CreateSun(),
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = fr_sun,
            };

            return obj;
        }

        public Earth CreateEarth(EarthData data, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var fr_earth = new EarthAnimator(data)
            {
                Name = string.Format("fr_{0}", data.Name.ToLower()),
                Children = ImmutableArray.Create<ViewModelBase>(),
            };

            parent.AddChild(fr_earth);

            var obj = new Earth()
            {
                Name = data.Name,
                FrameRenderModel = renderModelFactory.CreateFrame(6371.0f * 1.3f),
                RenderModel = renderModelFactory.CreateEarth(),
                IsVisible = true,
                Children = ImmutableArray.Create<BaseEntity>(),
                Logical = fr_earth,
            };

            return obj;
        }

        public EntityList CreateGroundObjects(ScenarioData data, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var renderModel = renderModelFactory.CreateGroundObject();
            var frameModel = renderModelFactory.CreateFrame(30.0f);

            var fr_go_collection = new LogicalCollectionViewModel()
            {
                Name = "fr_go_collection",
                Values = ImmutableArray.Create<LogicalViewModel>(),
            };

            parent.AddChild(fr_go_collection);

            var entities = new List<BaseEntity>();

            foreach (var item in data.GroundObjects)
            {
                var fr_groundObject = new GroundObjectState(item)
                {
                    Name = string.Format("fr_{0}", item.Name.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };

                fr_go_collection.AddValue(fr_groundObject);

                entities.Add(new GroundObject()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,
                    FrameRenderModel = frameModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_groundObject,
                });
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "GroundObjects",
                IsVisible = true,
                IsExpanded = false,
                LogicalCollection = fr_go_collection,
                Values = builder.ToImmutable(),
            };
        }

        public EntityList CreateGroundStations(ScenarioData data, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var renderModel = renderModelFactory.CreateGroundStation();
         
            var fr_gs_collection = new LogicalCollectionViewModel()
            {
                Name = "fr_gs_collection",
                Values = ImmutableArray.Create<LogicalViewModel>(),
            };

            parent.AddChild(fr_gs_collection);

            var entities = new List<BaseEntity>();

            foreach (var item in data.GroundStations)
            {
                var fr_groundStation = new GroundStationState(item)
                {
                    Name = string.Format("fr_{0}", item.Name.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };

                fr_gs_collection.AddValue(fr_groundStation);

                entities.Add(new GroundStation()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,            
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_groundStation,
                });
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "GroundStations",
                IsVisible = true,
                IsExpanded = false,
                LogicalCollection = fr_gs_collection,
                Values = builder.ToImmutable(),
            };
        }

        public EntityList CreateRetranslators(ScenarioData data, BaseState parent)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var renderModel = renderModelFactory.CreateRetranslator();

            var fr_rtr_collection = new LogicalCollectionViewModel()
            {
                Name = "fr_rtr_collection",
                Values = ImmutableArray.Create<LogicalViewModel>(),
            };

            parent.AddChild(fr_rtr_collection);

            var entities = new List<BaseEntity>();

            foreach (var item in data.RetranslatorPositions)
            {
                var fr_retranslator = new RetranslatorAnimator(item)
                {
                    Name = string.Format("fr_{0}", item.Name.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };

                fr_rtr_collection.AddValue(fr_retranslator);

                entities.Add(new Retranslator()
                {
                    Name = item.Name,
                    IsVisible = true,
                    RenderModel = renderModel,                 
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_retranslator,
                });
            }

            var builder = ImmutableArray.CreateBuilder<BaseEntity>();
            builder.AddRange(entities);

            return new EntityList()
            {
                Name = "Retranslators",
                IsVisible = true,
                IsExpanded = false,
                LogicalCollection = fr_rtr_collection,
                Values = builder.ToImmutable(),
            };
        }

        public IList<Satellite> CreateSatellites(ScenarioData data, BaseState parent, EntityList gss, EntityList rtrs)
        {
            var renderModelFactory = _serviceProvider.GetService<IRenderModelFactory>();

            var antennaModel = renderModelFactory.CreateAntenna();
            var antennaFrameModel = renderModelFactory.CreateFrame(50.0f);

            var sensorModel = renderModelFactory.CreateSensor();

            var satelliteModel = renderModelFactory.CreateSatellite();
        
            var list = new List<Satellite>();

            for (int i = 0; i < data.SatellitePositions.Count; i++)
            {
                var fr_satellite = new SatelliteAnimator(data.SatellitePositions[i])
                {
                    Name = string.Format("fr_{0}", data.SatellitePositions[i].Name.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };
                var fr_rotation = new RotationAnimator(data.SatelliteRotations[i])
                {
                    Name = string.Format("fr_{0}_{1}", data.SatelliteRotations[i].Name.ToLower(), data.SatelliteRotations[i].SatelliteName.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };
                var fr_sensor = new SensorAnimator(data.SatelliteShootings[i])
                {
                    Name = string.Format("fr_{0}_{1}", data.SatelliteShootings[i].Name.ToLower(), data.SatelliteShootings[i].SatelliteName.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };
                var fr_antenna = new AntennaAnimator(data.SatelliteTransfers[i])
                {
                    Name = string.Format("fr_{0}_{1}", data.SatelliteTransfers[i].Name.ToLower(), data.SatelliteTransfers[i].SatelliteName.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                    Assets = ImmutableArray.Create<BaseEntity>(),
                };
                var fr_orbit = new OrbitState(data.SatelliteOrbits[i])
                {
                    Name = string.Format("fr_{0}_{1}", data.SatelliteOrbits[i].Name.ToLower(), data.SatelliteOrbits[i].SatelliteName.ToLower()),
                    Children = ImmutableArray.Create<ViewModelBase>(),
                };

                parent.AddChild(fr_orbit);
                parent.AddChild(fr_satellite);
                fr_satellite.AddChild(fr_rotation);
                fr_satellite.AddChild(fr_sensor);
                fr_rotation.AddChild(fr_antenna);

                fr_antenna.AddAssets(gss.Values);
                fr_antenna.AddAssets(rtrs.Values);

                var satellite = new Satellite()
                {
                    Name = data.SatellitePositions[i].Name,
                    IsVisible = true,
                    RenderModel = satelliteModel,                 
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_rotation,
                };
                var sensor = new Sensor()
                {
                    Name = string.Format("Sensor{0}", i + 1),
                    IsVisible = true,
                    RenderModel = sensorModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_sensor,
                };
                var antenna = new Antenna()
                {
                    Name = string.Format("Antenna{0}", i + 1),
                    IsVisible = true,
                    RenderModel = antennaModel,
                    FrameRenderModel = antennaFrameModel,
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_antenna,
                };
                var orbit = new Orbit()
                {
                    Name = string.Format("Orbit{0}", i + 1),
                    IsVisible = true,
                    RenderModel = renderModelFactory.CreateOrbit(),
                    Children = ImmutableArray.Create<BaseEntity>(),
                    Logical = fr_orbit,
                };

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

                var sortEvents = events.OrderBy(s => s.Begin).ToList();

                var task = new SatelliteTask(sortEvents)
                {
                    Name = name,
                    Satellite = satellites[i],
                    HasRotations = true,
                    HasObservations = true,
                    HasTransmissions = true,
                    SearchString = string.Empty,
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
                        Begin = dt.AddSeconds(item.BeginTime),
                        Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
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
                        Begin = dt.AddSeconds(item.BeginTime),
                        Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
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
                        Begin = dt.AddSeconds(item.BeginTime),
                        Duration = TimeSpan.FromSeconds(item.EndTime - item.BeginTime),
                    });
                }
                return events;
            }
        }
    }
}
