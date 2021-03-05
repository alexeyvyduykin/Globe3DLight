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

            var epoch = FromJulianDate(data.JulianDateOnTheDay);
            var begin = epoch.AddSeconds(data.ModelingTimeBegin);
            var duration = TimeSpan.FromSeconds(data.ModelingTimeDuration);

            var project = factory.CreateProjectContainer("Project1");
            var scenario1 = containerFactory.GetScenario("Scenario1", begin, duration);

            var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();

            var fr_earth = CreateEarthNode("fr_j2000", root, data.Earth);

            var fr_sun = CreateSunNode("fr_sun", root, data.Sun);

            var fr_gss = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.GroundStations.Count; i++)
            {
                fr_gss.Add(CreateGroundStationNode(string.Format("fr_gs{0:00}", i + 1), fr_earth, data.GroundStations[i]));
            }

            var fr_sats = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.SatellitePositions.Count; i++)
            {
                fr_sats.Add(CreateSatelliteNode(string.Format("fr_orbital_satellite{0}", i + 1), fr_earth, data.SatellitePositions[i]));
            }

            var fr_rotations = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.SatelliteRotations.Count; i++)
            {
                fr_rotations.Add(CreateRotationNode(string.Format("fr_rotation_satellite{0}", i + 1), fr_sats[i], data.SatelliteRotations[i]));
            }

            var fr_sensors = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.SatelliteShootings.Count; i++)
            {
                fr_sensors.Add(CreateSensorNode(string.Format("fr_shooting_sensor{0}", i + 1), fr_rotations[i], data.SatelliteShootings[i]));
            }

            var fr_antennas = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.SatelliteTransfers.Count; i++)
            {
                fr_antennas.Add(CreateAntennaNode(string.Format("fr_antenna{0}", i + 1), fr_rotations[i], data.SatelliteTransfers[i]));
            }

            var fr_orbits = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.SatelliteOrbits.Count; i++)
            {
                fr_orbits.Add(CreateOrbitNode(string.Format("fr_orbit{0}", i + 1), fr_rotations[i], data.SatelliteOrbits[i]));
            }

            var fr_retrs = new List<ILogicalTreeNode>();
            for (int i = 0; i < data.RetranslatorPositions.Count; i++)
            {
                fr_retrs.Add(CreateRetranslatorNode(string.Format("fr_retranslator{0}", i + 1), root, data.RetranslatorPositions[i]));
            }

            var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
            objBuilder.Add(objFactory.CreateSun("Sun", fr_sun));
            objBuilder.Add(objFactory.CreateEarth("Earth", fr_earth));

            var taskBuilder = ImmutableArray.CreateBuilder<ISatelliteTask>();

            for (int i = 0; i < fr_rotations.Count; i++)
            {
                var sat = objFactory.CreateSatellite(string.Format("Satellite{0}", i + 1), fr_rotations[i]);
                objBuilder.Add(sat);

                taskBuilder.Add(objFactory.CreateSatelliteTask(
                    sat,
                    data.SatelliteRotations[i],
                    data.SatelliteShootings[i],
                    data.SatelliteTransfers[i],
                    FromJulianDate(data.JulianDateOnTheDay)));
            }

            for (int i = 0; i < fr_sensors.Count; i++)
            {
                objBuilder.Add(objFactory.CreateSensor(string.Format("Sensor{0}", i + 1), fr_sensors[i]));
            }

            var gss = new List<IScenarioObject>();
            var rtrs = new List<IScenarioObject>();

            for (int i = 0; i < fr_gss.Count; i++)
            {
                gss.Add(objFactory.CreateGroundStation(string.Format("GroundStation{0:00}", i + 1), fr_gss[i], i));
            }

            for (int i = 0; i < fr_retrs.Count; i++)
            {
                rtrs.Add(objFactory.CreateRetranslator(string.Format("Retranslator{0}", i + 1), fr_retrs[i], i));
            }

            objBuilder.AddRange(gss);
            objBuilder.AddRange(rtrs);

            var assetsBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            assetsBuilder.AddRange(gss);
            assetsBuilder.AddRange(rtrs);


            for (int i = 0; i < fr_antennas.Count; i++)
            {
                var antenna = objFactory.CreateAntenna(string.Format("Antenna{0}", i + 1), fr_antennas[i]);
                antenna.Assets = assetsBuilder.ToImmutable();

                objBuilder.Add(antenna);
            }

            for (int i = 0; i < fr_orbits.Count; i++)
            {
                objBuilder.Add(objFactory.CreateOrbit(string.Format("Orbit{0}", i + 1), fr_orbits[i]));
            }

            scenario1.ScenarioObjects = objBuilder.ToImmutable();

            scenario1.SatelliteTasks = taskBuilder.ToImmutable();

            project.AddScenario(scenario1);

            project.SetCurrentScenario(scenario1);

            return project;
        }

        public IScenarioContainer GetScenario(string name, DateTime begin, TimeSpan duration)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var scenario = factory.CreateScenarioContainer(name);
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var sceneFactory = _serviceProvider.GetService<IScenarioObjectFactory>();

            var root = factory.CreateLogicalTreeNode("Root", dataFactory.CreateFrameState());

            //        root.Owner = scenario; ????????????????????????????????

            scenario.LogicalTreeNodeRoot = ImmutableArray.Create<ILogicalTreeNode>(root);
            scenario.CurrentLogicalTreeNode = scenario.LogicalTreeNodeRoot.FirstOrDefault();

            scenario.SceneState = sceneFactory.CreateSceneState();

            //scenario.SceneTimer = _serviceProvider.GetService<ISceneTimer>();

            scenario.TimePresenter = factory.CreateTimePresenter(begin, duration);

            return scenario;
        }

        private ILogicalTreeNode CreateSatelliteNode(ILogicalTreeNode parent, string path)
        {      
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();    

            var db1 = jsonDataProvider.CreateDataFromPath<SatelliteData>(path);
            var satelliteState = dataFactory.CreateSatelliteAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_satellite = factory.CreateLogicalTreeNode(name, satelliteState);
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }
        public ILogicalTreeNode CreateSatelliteNode(string name, ILogicalTreeNode parent, SatelliteData data)
        {          
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
   
            var satelliteState = dataFactory.CreateSatelliteAnimator(data);      
            var fr_satellite = factory.CreateLogicalTreeNode(name, satelliteState);
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }
        private ILogicalTreeNode CreateRotationNode(ILogicalTreeNode parent, string path)
        {            
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
         
            var db2 = jsonDataProvider.CreateDataFromPath<RotationData>(path);
            var rotationData = dataFactory.CreateRotationAnimator(db2);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_rotation = factory.CreateLogicalTreeNode(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;

        }
        public ILogicalTreeNode CreateRotationNode(string name, ILogicalTreeNode parent, RotationData data)
        {  
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
     
            var rotationData = dataFactory.CreateRotationAnimator(data); 
            var fr_rotation = factory.CreateLogicalTreeNode(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;
        }
        private ILogicalTreeNode CreateSunNode(ILogicalTreeNode parent, string path)
        {     
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();          
    
            var db = jsonDataProvider.CreateDataFromPath<SunData>(path);
            var sun_data = dataFactory.CreateSunAnimator(db);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_sun = factory.CreateLogicalTreeNode(name, sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;
            //  return objFactory.CreateSun(name, fr_sun);
        }
        public ILogicalTreeNode CreateSunNode(string name, ILogicalTreeNode parent, SunData data)
        {       
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var sun_data = dataFactory.CreateSunAnimator(data);      
            var fr_sun = factory.CreateLogicalTreeNode(name, sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;  
        }
        private ILogicalTreeNode CreateSensorNode(ILogicalTreeNode parent, string path)
        {     
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>(); 
    
            var db = jsonDataProvider.CreateDataFromPath<SensorData>(path);
            var sensor_data = dataFactory.CreateSensorAnimator(db);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_sensor = factory.CreateLogicalTreeNode(name, sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;
            // return objFactory.CreateSensor(name, fr_sensor);
        }
        public ILogicalTreeNode CreateSensorNode(string name, ILogicalTreeNode parent, SensorData data)
        {          
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var sensor_data = dataFactory.CreateSensorAnimator(data);         
            var fr_sensor = factory.CreateLogicalTreeNode(name, sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;        
        }
        private ILogicalTreeNode CreateRetranslatorNode(ILogicalTreeNode parent, string path)
        {        
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();       
  
            var db1 = jsonDataProvider.CreateDataFromPath<RetranslatorData>(path);
            var retranslatorData = dataFactory.CreateRetranslatorAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_retranslator = factory.CreateLogicalTreeNode(name, retranslatorData);
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }
        public ILogicalTreeNode CreateRetranslatorNode(string name, ILogicalTreeNode parent, RetranslatorData data)
        {           
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
        
            var retranslatorData = dataFactory.CreateRetranslatorAnimator(data);         
            var fr_retranslator = factory.CreateLogicalTreeNode(name, retranslatorData);
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }
        private ILogicalTreeNode CreateAntennaNode(ILogicalTreeNode parent, string path)
        {      
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();      

            //var p0LeftPos = new dvec3(67.74, -12.22, -23.5);
            //   var p0LeftPos = new dvec3(0.6774, -0.1222, -0.235);
  
            var db = jsonDataProvider.CreateDataFromPath<AntennaData>(path);
            var antenna_data = dataFactory.CreateAntennaAnimator(db/*, p0LeftPos*/);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_antenna = factory.CreateLogicalTreeNode(name, antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        public ILogicalTreeNode CreateAntennaNode(string name, ILogicalTreeNode parent, AntennaData data)
        {     
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
      
            var antenna_data = dataFactory.CreateAntennaAnimator(data);            
            var fr_antenna = factory.CreateLogicalTreeNode(name, antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        public ILogicalTreeNode CreateOrbitNode(string name, ILogicalTreeNode parent, OrbitData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var orbit_data = dataFactory.CreateOrbitState(data);
            var fr_orbit = factory.CreateLogicalTreeNode(name, orbit_data);
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }
        public ILogicalTreeNode CreateGroundStationNode(string name, ILogicalTreeNode parent, GroundStationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var groundStationData = dataFactory.CreateGroundStationState(data);
            var fr_groundStation = factory.CreateLogicalTreeNode(name, groundStationData);
            parent.AddChild(fr_groundStation);

            return fr_groundStation;       
        }
        public ILogicalTreeNode CreateEarthNode(string name, ILogicalTreeNode parent, J2000Data data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var earth_data = dataFactory.CreateJ2000Animator(data);
            var fr_earth = factory.CreateLogicalTreeNode(name, earth_data);
            parent.AddChild(fr_earth);
            return fr_earth;
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
            var fr_j2000 = factory.CreateLogicalTreeNode("fr_j2000", dataFactory.CreateJ2000Animator(begin, 0.0));
            root.AddChild(fr_j2000);

            var fr_sun = CreateSunNode(root, Path.Combine(path, @"data\fr_sun.json"));


            var fr_gs01 = factory.CreateLogicalTreeNode("fr_gs01", dataFactory.CreateGroundStationState(36.26, 54.97, 0.223, 6371.0));
            var fr_gs02 = factory.CreateLogicalTreeNode("fr_gs02", dataFactory.CreateGroundStationState(30.201389, 59.712777, 0.128, 6371.0));
            var fr_gs03 = factory.CreateLogicalTreeNode("fr_gs03", dataFactory.CreateGroundStationState(37.0532, 55.5856, 0.204, 6371.0));
            var fr_gs04 = factory.CreateLogicalTreeNode("fr_gs04", dataFactory.CreateGroundStationState(107.945, 51.87111, 0.621, 6371.0));
            var fr_gs05 = factory.CreateLogicalTreeNode("fr_gs05", dataFactory.CreateGroundStationState(37.9678, 55.9494, 0.157, 6371.0));
            var fr_gs06 = factory.CreateLogicalTreeNode("fr_gs06", dataFactory.CreateGroundStationState(131.7575, 44.0247, 0.080, 6371.0));
            var fr_gs07 = factory.CreateLogicalTreeNode("fr_gs07", dataFactory.CreateGroundStationState(136.7536, 50.6856, 0.222, 6371.0));
            var fr_gs08 = factory.CreateLogicalTreeNode("fr_gs08", dataFactory.CreateGroundStationState(38.39, 55.1536111, 0.189, 6371.0));
            var fr_gs09 = factory.CreateLogicalTreeNode("fr_gs09", dataFactory.CreateGroundStationState(107.934166, 51.864722, 0.633, 6371.0));

            fr_j2000.AddChild(fr_gs01);
            fr_j2000.AddChild(fr_gs02);
            fr_j2000.AddChild(fr_gs03);
            fr_j2000.AddChild(fr_gs04);
            fr_j2000.AddChild(fr_gs05);
            fr_j2000.AddChild(fr_gs06);
            fr_j2000.AddChild(fr_gs07);
            fr_j2000.AddChild(fr_gs08);
            fr_j2000.AddChild(fr_gs09);

            var fr_satellite1 = CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite1.json"));
            var fr_satellite2 = CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite2.json"));
            var fr_satellite3 = CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite3.json"));
            var fr_satellite4 = CreateSatelliteNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite4.json"));

            var fr_rotation1 = CreateRotationNode(fr_satellite1, Path.Combine(path, @"data\fr_rotation_satellite1.json"));
            var fr_rotation2 = CreateRotationNode(fr_satellite2, Path.Combine(path, @"data\fr_rotation_satellite2.json"));
            var fr_rotation3 = CreateRotationNode(fr_satellite3, Path.Combine(path, @"data\fr_rotation_satellite3.json"));
            var fr_rotation4 = CreateRotationNode(fr_satellite4, Path.Combine(path, @"data\fr_rotation_satellite4.json"));

            var fr_sensor1 = CreateSensorNode(fr_rotation1, Path.Combine(path, @"data\fr_shooting_sensor1.json"));
            var fr_sensor2 = CreateSensorNode(fr_rotation2, Path.Combine(path, @"data\fr_shooting_sensor2.json"));
            var fr_sensor3 = CreateSensorNode(fr_rotation3, Path.Combine(path, @"data\fr_shooting_sensor3.json"));
            var fr_sensor4 = CreateSensorNode(fr_rotation4, Path.Combine(path, @"data\fr_shooting_sensor4.json"));
            
            var fr_antenna1 = CreateAntennaNode(fr_rotation1, Path.Combine(path, @"data\fr_antenna1.json"));
            var fr_antenna2 = CreateAntennaNode(fr_rotation2, Path.Combine(path, @"data\fr_antenna2.json"));
            var fr_antenna3 = CreateAntennaNode(fr_rotation3, Path.Combine(path, @"data\fr_antenna3.json"));
            var fr_antenna4 = CreateAntennaNode(fr_rotation4, Path.Combine(path, @"data\fr_antenna4.json"));

            var fr_retr1 = CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator1.json"));
            var fr_retr2 = CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator2.json"));
            var fr_retr3 = CreateRetranslatorNode(root, Path.Combine(path, @"data\fr_retranslator3.json"));


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

            var gs1 = objFactory.CreateGroundStation("GroundStation01", fr_gs01, 0);
            var gs2 = objFactory.CreateGroundStation("GroundStation02", fr_gs02, 1);
            var gs3 = objFactory.CreateGroundStation("GroundStation03", fr_gs03, 2);
            var gs4 = objFactory.CreateGroundStation("GroundStation04", fr_gs04, 3);
            var gs5 = objFactory.CreateGroundStation("GroundStation05", fr_gs05, 4);
            var gs6 = objFactory.CreateGroundStation("GroundStation06", fr_gs06, 5);
            var gs7 = objFactory.CreateGroundStation("GroundStation07", fr_gs07, 6);
            var gs8 = objFactory.CreateGroundStation("GroundStation08", fr_gs08, 7);
            var gs9 = objFactory.CreateGroundStation("GroundStation09", fr_gs09, 8);

            var rtr1 = objFactory.CreateRetranslator("Retranslator1", fr_retr1, 0);
            var rtr2 = objFactory.CreateRetranslator("Retranslator2", fr_retr2, 1);
            var rtr3 = objFactory.CreateRetranslator("Retranslator3", fr_retr3, 2);


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
