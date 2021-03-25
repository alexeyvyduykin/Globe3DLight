using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.Models.Scene;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.SceneTimer;
using Globe3DLight.ViewModels.Data;
using System.IO;
using GlmSharp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Globe3DLight.Models.Editor;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;

namespace Globe3DLight.ViewModels.Editor
{
    public class ContainerFactory : IContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ProjectContainerViewModel GetProject()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;
            var project = factory.CreateProjectContainer("Project1");

            // Templates
            //   var templateBuilder = project.Templates.ToBuilder();
            //   templateBuilder.Add(CreateDefaultTemplate(this, project, "Default"));
            //   project.Templates = templateBuilder.ToImmutable();

            //   project.SetCurrentTemplate(project.Templates.FirstOrDefault(t => t.Name == "Default"));

            // Documents and Pages      
            var scenario = containerFactory.GetScenario("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

            var scenarioBuilder = project.Scenarios.ToBuilder();
            scenarioBuilder.Add(scenario);
            project.Scenarios = scenarioBuilder.ToImmutable();

            // project.Selected = scenario.Pages.FirstOrDefault();

            return project;
        }

        public ProjectContainerViewModel GetProject(ScenarioData data)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;
            var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();

            var epoch = FromJulianDate(data.JulianDateOnTheDay);
            var begin = epoch.AddSeconds(data.ModelingTimeBegin);
            var duration = TimeSpan.FromSeconds(data.ModelingTimeDuration);

            var project = factory.CreateProjectContainer("Project1");
            var scenario = containerFactory.GetScenario(data.Name, begin, duration);

            project.AddScenario(scenario);
            project.SetCurrentScenario(scenario);

            var root = scenario.LogicalRoot.FirstOrDefault();

            var fr_earth = (Name: data.Earth.Name, Node: dataFactory.CreateEarthNode(root, data.Earth));
            var fr_sun = (Name: data.Sun.Name, Node: dataFactory.CreateSunNode(root, data.Sun));
            var fr_gs_collection = dataFactory.CreateCollectionNode("fr_gs_collection1", fr_earth.Node);
            var fr_go_collection = dataFactory.CreateCollectionNode("fr_go_collection1", fr_earth.Node);
            var fr_rtr_collection = dataFactory.CreateCollectionNode("fr_rtr_collection1", root);
            var fr_gss = data.GroundStations.ToDictionary(s => s.Name, s => dataFactory.CreateGroundStationNode(fr_gs_collection, s));
            var fr_gos = data.GroundObjects.ToDictionary(s => s.Name, s => dataFactory.CreateGroundObjectNode(fr_go_collection, s));
            var fr_sats = data.SatellitePositions.ToDictionary(s => s.Name, s => dataFactory.CreateSatelliteNode(fr_earth.Node, s));
            var fr_rotations = data.SatelliteRotations.ToDictionary(s => s.SatelliteName, s => dataFactory.CreateRotationNode(fr_sats[s.SatelliteName], s));
            var fr_sensors = data.SatelliteShootings.ToDictionary(s => s.SatelliteName, s => dataFactory.CreateSensorNode(fr_rotations[s.SatelliteName], s));
            var fr_antennas = data.SatelliteTransfers.ToDictionary(s => s.SatelliteName, s => dataFactory.CreateAntennaNode(fr_rotations[s.SatelliteName], s));            
            var fr_orbits = data.SatelliteOrbits.ToDictionary(s => s.SatelliteName, s => dataFactory.CreateOrbitNode(fr_rotations[s.SatelliteName], s));            
            var fr_retrs = data.RetranslatorPositions.ToDictionary(s => s.Name, s => dataFactory.CreateRetranslatorNode(fr_rtr_collection, s));
        
            project.AddEntity(objFactory.CreateSpacebox("Spacebox", root));
            project.AddEntity(objFactory.CreateSun(fr_sun.Name, fr_sun.Node));
            project.AddEntity(objFactory.CreateEarth(fr_earth.Name, fr_earth.Node));
            
            var satellites = fr_rotations.Select(s => objFactory.CreateSatellite(s.Key, s.Value)).ToList();

            for (int i = 0; i < satellites.Count; i++)
            {
                scenario.AddSatelliteTask(objFactory.CreateSatelliteTask(
                    satellites[i],
                    data.SatelliteRotations[i],
                    data.SatelliteShootings[i],
                    data.SatelliteTransfers[i],
                    FromJulianDate(data.JulianDateOnTheDay)));
            }

            for (int i = 0; i < fr_sensors.Count; i++)
            {
                satellites[i].AddChild(objFactory.CreateSensor(string.Format("Sensor{0}", i + 1), fr_sensors[satellites[i].Name]));
            }

            var gss = fr_gss.Select(s => objFactory.CreateGroundStation(s.Key, s.Value));
            var gos = fr_gos.Select(s => objFactory.CreateGroundObject(s.Key, s.Value));
            var rtrs = fr_retrs.Select(s => objFactory.CreateRetranslator(s.Key, s.Value));

            for (int i = 0; i < fr_antennas.Count; i++)
            {
                var antenna = objFactory.CreateAntenna(string.Format("Antenna{0}", i + 1), fr_antennas[satellites[i].Name]);
                antenna.AddAssets(gss);
                antenna.AddAssets(rtrs);

                satellites[i].AddChild(antenna);
            }

            for (int i = 0; i < fr_orbits.Count; i++)
            {
                satellites[i].AddChild(objFactory.CreateOrbit(string.Format("Orbit{0}", i + 1), fr_orbits[satellites[i].Name]));
            }

            project.AddEntity(objFactory.CreateEntityList("GroundObjects", fr_go_collection, gos));
            project.AddEntity(objFactory.CreateEntityList("GroundStations", fr_gs_collection, gss));
            project.AddEntity(objFactory.CreateEntityList("Retranslators", fr_rtr_collection, rtrs));

            project.AddEntities(satellites);

            return project;
        }

        public ScenarioContainerViewModel GetScenario(string name, DateTime begin, TimeSpan duration)
        {
            var factory = _serviceProvider.GetService<IFactory>();          
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var scenarioObjectFactory = _serviceProvider.GetService<IScenarioObjectFactory>();

            var scenario = factory.CreateScenarioContainer(name);
            var root = dataFactory.CreateFrameState("Root");
              
            //        root.Owner = scenario; ????????????????????????????????

            scenario.LogicalRoot = ImmutableArray.Create<LogicalViewModel>(root);
            scenario.CurrentLogical = scenario.LogicalRoot.FirstOrDefault();
            scenario.SceneState = scenarioObjectFactory.CreateSceneState();  
            scenario.TimePresenter = factory.CreateSliderTimePresenter(begin, duration);
            //scenario.Tasks = ImmutableArray.Create<ISatelliteTask>();
            scenario.Updater = factory.CreateDataUpdater();

            return scenario;
        }

        public async Task<ProjectContainerViewModel> GetFromDatabase()
        {    
            try
            {
                return await _serviceProvider.GetService<IDatabaseProvider>().LoadProject();
            }
            catch (Exception)
            {
                throw new Exception();
            }             
        }

        public async Task<ProjectContainerViewModel> GetFromJson()
        {
            try
            {
                return await _serviceProvider.GetService<IJsonDataProvider>().LoadProject();            
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task SaveFromDatabaseToJson()
        {            
            try
            {
                await _serviceProvider.GetService<IDatabaseProvider>().Save();           
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private DateTime FromJulianDate(double jd) => DateTime.FromOADate(jd - 2415018.5);
        
        public ProjectContainerViewModel GetEmptyProject()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;                 

            var project = factory.CreateProjectContainer("Project1");

            var scenario1 = containerFactory.GetScenario("Scenario1", DateTime.Now, TimeSpan.FromDays(1));

            project.AddScenario(scenario1);
            project.SetCurrentScenario(scenario1);

            return project;
        }
    }
}
