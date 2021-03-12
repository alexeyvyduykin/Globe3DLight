using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.SceneTimer;
using Globe3DLight.Data;
using System.IO;
using GlmSharp;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Globe3DLight.Editor
{
    public class ContainerFactory : IContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IProjectContainer GetProject()
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

        public IProjectContainer GetProject(ScenarioData data)
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

            var root = scenario.LogicalTreeNodeRoot.FirstOrDefault();

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

            var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
            objBuilder.Add(objFactory.CreateSun(fr_sun.Name, fr_sun.Node));
            objBuilder.Add(objFactory.CreateEarth(fr_earth.Name, fr_earth.Node));

            var taskBuilder = ImmutableArray.CreateBuilder<ISatelliteTask>();
        
            var satellites = fr_rotations.Select(s => objFactory.CreateSatellite(s.Key, s.Value)).ToList();

            for (int i = 0; i < satellites.Count; i++)
            {
                taskBuilder.Add(objFactory.CreateSatelliteTask(
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

            var assetsBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            assetsBuilder.AddRange(gss);
            assetsBuilder.AddRange(rtrs);

            for (int i = 0; i < fr_antennas.Count; i++)
            {
                var antenna = objFactory.CreateAntenna(string.Format("Antenna{0}", i + 1), fr_antennas[satellites[i].Name]);
                antenna.Assets = assetsBuilder.ToImmutable();

                satellites[i].AddChild(antenna);
            }

            for (int i = 0; i < fr_orbits.Count; i++)
            {
                satellites[i].AddChild(objFactory.CreateOrbit(string.Format("Orbit{0}", i + 1), fr_orbits[satellites[i].Name]));
            }
            
            objBuilder.Add(objFactory.CreateScenarioObjectList("GroundObjects", fr_go_collection, gos));
            objBuilder.Add(objFactory.CreateScenarioObjectList("GroundStations", fr_gs_collection, gss));
            objBuilder.Add(objFactory.CreateScenarioObjectList("Retranslators", fr_rtr_collection, rtrs));

            objBuilder.AddRange(satellites);

            scenario.ScenarioObjects = objBuilder.ToImmutable();

            scenario.SatelliteTasks = taskBuilder.ToImmutable();

            project.AddScenario(scenario);

            project.SetCurrentScenario(scenario);

            return project;
        }

        public IScenarioContainer GetScenario(string name, DateTime begin, TimeSpan duration)
        {
            var factory = _serviceProvider.GetService<IFactory>();          
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var scenarioObjectFactory = _serviceProvider.GetService<IScenarioObjectFactory>();

            var scenario = factory.CreateScenarioContainer(name);
            var root = factory.CreateLogical("Root", dataFactory.CreateFrameState());
              
            //        root.Owner = scenario; ????????????????????????????????

            scenario.LogicalTreeNodeRoot = ImmutableArray.Create<ILogical>(root);
            scenario.CurrentLogicalTreeNode = scenario.LogicalTreeNodeRoot.FirstOrDefault();
            scenario.SceneState = scenarioObjectFactory.CreateSceneState();  
            scenario.TimePresenter = factory.CreateTimePresenter(begin, duration);

            return scenario;
        }

        public IProjectContainer GetDemo()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;                   
            var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();         
            var dataFactory = _serviceProvider.GetService<IDataFactory>();                  
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath);

            var begin = DateTime.Now;
            var duration = TimeSpan.FromDays(1);

            var project = factory.CreateProjectContainer("Project1");
            var scenario1 = containerFactory.GetScenario("Scenario1", begin, duration);

            var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();
            var fr_j2000 = factory.CreateLogical("fr_j2000", dataFactory.CreateJ2000Animator(begin, 0.0));
            root.AddChild(fr_j2000);

            var fr_sun = dataFactory.CreateSunNode(root, Path.Combine(path, @"data\fr_sun.json"));


            var fr_gs01 = factory.CreateLogical("fr_gs01", dataFactory.CreateGroundStationState(36.26, 54.97, 0.223, 6371.0));
            var fr_gs02 = factory.CreateLogical("fr_gs02", dataFactory.CreateGroundStationState(30.201389, 59.712777, 0.128, 6371.0));
            var fr_gs03 = factory.CreateLogical("fr_gs03", dataFactory.CreateGroundStationState(37.0532, 55.5856, 0.204, 6371.0));
            var fr_gs04 = factory.CreateLogical("fr_gs04", dataFactory.CreateGroundStationState(107.945, 51.87111, 0.621, 6371.0));
            var fr_gs05 = factory.CreateLogical("fr_gs05", dataFactory.CreateGroundStationState(37.9678, 55.9494, 0.157, 6371.0));
            var fr_gs06 = factory.CreateLogical("fr_gs06", dataFactory.CreateGroundStationState(131.7575, 44.0247, 0.080, 6371.0));
            var fr_gs07 = factory.CreateLogical("fr_gs07", dataFactory.CreateGroundStationState(136.7536, 50.6856, 0.222, 6371.0));
            var fr_gs08 = factory.CreateLogical("fr_gs08", dataFactory.CreateGroundStationState(38.39, 55.1536111, 0.189, 6371.0));
            var fr_gs09 = factory.CreateLogical("fr_gs09", dataFactory.CreateGroundStationState(107.934166, 51.864722, 0.633, 6371.0));

            fr_j2000.AddChild(fr_gs01);
            fr_j2000.AddChild(fr_gs02);
            fr_j2000.AddChild(fr_gs03);
            fr_j2000.AddChild(fr_gs04);
            fr_j2000.AddChild(fr_gs05);
            fr_j2000.AddChild(fr_gs06);
            fr_j2000.AddChild(fr_gs07);
            fr_j2000.AddChild(fr_gs08);
            fr_j2000.AddChild(fr_gs09);

            var fr_satellite1 = dataFactory.CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite1.json"));
            var fr_satellite2 = dataFactory.CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite2.json"));
            var fr_satellite3 = dataFactory.CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite3.json"));
            var fr_satellite4 = dataFactory.CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite4.json"));

            var fr_rotation1 = dataFactory.CreateRotationNode(fr_satellite1, Path.Combine(path, @"data\fr_rotation_satellite1.json"));
            var fr_rotation2 = dataFactory.CreateRotationNode(fr_satellite2, Path.Combine(path, @"data\fr_rotation_satellite2.json"));
            var fr_rotation3 = dataFactory.CreateRotationNode(fr_satellite3, Path.Combine(path, @"data\fr_rotation_satellite3.json"));
            var fr_rotation4 = dataFactory.CreateRotationNode(fr_satellite4, Path.Combine(path, @"data\fr_rotation_satellite4.json"));

            var fr_sensor1 = dataFactory.CreateSensorNode(fr_rotation1, Path.Combine(path, @"data\fr_shooting_sensor1.json"));
            var fr_sensor2 = dataFactory.CreateSensorNode(fr_rotation2, Path.Combine(path, @"data\fr_shooting_sensor2.json"));
            var fr_sensor3 = dataFactory.CreateSensorNode(fr_rotation3, Path.Combine(path, @"data\fr_shooting_sensor3.json"));
            var fr_sensor4 = dataFactory.CreateSensorNode(fr_rotation4, Path.Combine(path, @"data\fr_shooting_sensor4.json"));
            
            var fr_antenna1 = dataFactory.CreateAntennaNode(fr_rotation1, Path.Combine(path, @"data\fr_antenna1.json"));
            var fr_antenna2 = dataFactory.CreateAntennaNode(fr_rotation2, Path.Combine(path, @"data\fr_antenna2.json"));
            var fr_antenna3 = dataFactory.CreateAntennaNode(fr_rotation3, Path.Combine(path, @"data\fr_antenna3.json"));
            var fr_antenna4 = dataFactory.CreateAntennaNode(fr_rotation4, Path.Combine(path, @"data\fr_antenna4.json"));

            var fr_retr1 = dataFactory.CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator1.json"));
            var fr_retr2 = dataFactory.CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator2.json"));
            var fr_retr3 = dataFactory.CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator3.json"));


            var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
            objBuilder.Add(objFactory.CreateSun("Sun", fr_sun));
            objBuilder.Add(objFactory.CreateEarth("Earth", fr_j2000));
            objBuilder.Add(objFactory.CreateSatellite("Satellite1", fr_rotation1));            
            objBuilder.Add(objFactory.CreateSatellite("Satellite2", fr_rotation2));
            objBuilder.Add(objFactory.CreateSatellite("Satellite3", fr_rotation3));
            objBuilder.Add(objFactory.CreateSatellite("Satellite4", fr_rotation4));
            objBuilder.Add(objFactory.CreateSensor("Sensor1", fr_sensor1));
            objBuilder.Add(objFactory.CreateSensor("Sensor2", fr_sensor2));
            objBuilder.Add(objFactory.CreateSensor("Sensor3", fr_sensor3));
            objBuilder.Add(objFactory.CreateSensor("Sensor4", fr_sensor4));


            var assetsBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();

            var gs1 = objFactory.CreateGroundStation("GroundStation01", fr_gs01/*, 0*/);
            var gs2 = objFactory.CreateGroundStation("GroundStation02", fr_gs02/*, 1*/);
            var gs3 = objFactory.CreateGroundStation("GroundStation03", fr_gs03/*, 2*/);
            var gs4 = objFactory.CreateGroundStation("GroundStation04", fr_gs04/*, 3*/);
            var gs5 = objFactory.CreateGroundStation("GroundStation05", fr_gs05/*, 4*/);
            var gs6 = objFactory.CreateGroundStation("GroundStation06", fr_gs06/*, 5*/);
            var gs7 = objFactory.CreateGroundStation("GroundStation07", fr_gs07/*, 6*/);
            var gs8 = objFactory.CreateGroundStation("GroundStation08", fr_gs08/*, 7*/);
            var gs9 = objFactory.CreateGroundStation("GroundStation09", fr_gs09/*, 8*/);

            var rtr1 = objFactory.CreateRetranslator("Retranslator1", fr_retr1/*, 0*/);
            var rtr2 = objFactory.CreateRetranslator("Retranslator2", fr_retr2/*, 1*/);
            var rtr3 = objFactory.CreateRetranslator("Retranslator3", fr_retr3/*, 2*/);


            var gss = new IScenarioObject[] { gs1, gs2, gs3, gs4, gs5, gs6, gs7, gs8, gs9 };
            var rtrs = new IScenarioObject[] { rtr1, rtr2, rtr3 };

            objBuilder.AddRange(gss);
            objBuilder.AddRange(rtrs);

            assetsBuilder.AddRange(gss);            
            assetsBuilder.AddRange(rtrs);


            var antenna1 = objFactory.CreateAntenna("Antenna1", fr_antenna1);          
            antenna1.Assets = assetsBuilder.ToImmutable();

            var antenna2 = objFactory.CreateAntenna("Antenna2", fr_antenna2);
            antenna2.Assets = assetsBuilder.ToImmutable();

            var antenna3 = objFactory.CreateAntenna("Antenna3", fr_antenna3);
            antenna3.Assets = assetsBuilder.ToImmutable();

            var antenna4 = objFactory.CreateAntenna("Antenna4", fr_antenna4);
            antenna4.Assets = assetsBuilder.ToImmutable();



            objBuilder.Add(antenna1);
            objBuilder.Add(antenna2);
            objBuilder.Add(antenna3);
            objBuilder.Add(antenna4);

            scenario1.ScenarioObjects = objBuilder.ToImmutable();


            project.AddScenario(scenario1);
        //    project.AddScenario(scenario2);
        //    project.AddScenario(scenario3);
            project.SetCurrentScenario(scenario1);

            return project;
        }

        public async Task<IProjectContainer> GetFromDatabase()
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

        public async Task<IProjectContainer> GetFromJson()
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
        
        public IProjectContainer GetEmptyProject()
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
