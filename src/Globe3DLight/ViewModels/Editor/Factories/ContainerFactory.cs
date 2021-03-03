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

        IProjectContainer IContainerFactory.GetProject()
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
            var scenario = containerFactory.GetScenario("Scenario1");

            var scenarioBuilder = project.Scenarios.ToBuilder();
            scenarioBuilder.Add(scenario);
            project.Scenarios = scenarioBuilder.ToImmutable();

            // project.Selected = scenario.Pages.FirstOrDefault();

            return project;
        }

        IScenarioContainer IContainerFactory.GetScenario(string name)
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

            scenario.TimePresenter = factory.CreateTimePresenter();

            return scenario;
        }

        private ILogicalTreeNode CreateOrbitNode(ILogicalTreeNode parent, string path)
        {      
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();    

            var db1 = jsonDataProvider.CreateDataFromPath<OrbitData>(path);
            var orbitData = dataFactory.CreateOrbitAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_orbit = factory.CreateLogicalTreeNode(name, orbitData);
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }

        private ILogicalTreeNode CreateRotationNode(ILogicalTreeNode parent, string path)
        {            
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();
         
            var db2 = jsonDataProvider.CreateDataFromPath<RotationData>(path);
            var rotationData = dataFactory.CreateRotationAnimator(db2);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_rotation = factory.CreateLogicalTreeNode(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;

        }
        private ILogicalTreeNode CreateSunNode(ILogicalTreeNode parent, string path)
        {     
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
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

        private ILogicalTreeNode CreateSensorNode(ILogicalTreeNode parent, string path)
        {     
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
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

        private ILogicalTreeNode CreateRetranslatorNode(ILogicalTreeNode parent, string path)
        {        
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();       
  
            var db1 = jsonDataProvider.CreateDataFromPath<RetranslatorData>(path);
            var retranslatorData = dataFactory.CreateRetranslatorAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_retranslator = factory.CreateLogicalTreeNode(name, retranslatorData);
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }

        private ILogicalTreeNode CreateAntennaNode(ILogicalTreeNode parent, string path)
        {      
            var jsonDataProvider = (IJsonDataProvider)_serviceProvider.GetService<IDataProvider>();
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


        public IProjectContainer GetDemo()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;                   
            var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();         
            var dataFactory = _serviceProvider.GetService<IDataFactory>();        
            var dataProvider = _serviceProvider.GetService<IDataProvider>();
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath);

            var project = factory.CreateProjectContainer("Project1");
            var scenario1 = containerFactory.GetScenario("Scenario1");

            if (dataProvider is IJsonDataProvider jsonDataProvider)
            {
                //  --- Creating new json-files ---
          //   var json1 = fileSystem.ReadUtf8Text(@"C:\resource\globe3d\data\Antenna4\fr_antenna4.json");
          //   var db2_ = jsonSerializer.Deserialize<Globe3DLight.Data.Database.AntennaDatabase>(json1);
          //   var str_ = jsonSerializer.Serialize<Globe3DLight.Data.Database.IAntennaDatabase>(db2_);
          //   fileSystem.WriteUtf8Text(@"C:\resource\globe3d\data\Antenna4\fr_antenna4_true.json", str_);

            }

            var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();
            var fr_j2000 = factory.CreateLogicalTreeNode("fr_j2000", dataFactory.CreateJ2000Animator(DateTime.Now, 0.0));
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

            var fr_orbit1 = CreateOrbitNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite1.json"));
            var fr_orbit2 = CreateOrbitNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite2.json"));
            var fr_orbit3 = CreateOrbitNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite3.json"));
            var fr_orbit4 = CreateOrbitNode(fr_j2000, Path.Combine(path, @"data\fr_orbital_satellite4.json"));

            var fr_rotation1 = CreateRotationNode(fr_orbit1, Path.Combine(path, @"data\fr_rotation_satellite1.json"));
            var fr_rotation2 = CreateRotationNode(fr_orbit2, Path.Combine(path, @"data\fr_rotation_satellite2.json"));
            var fr_rotation3 = CreateRotationNode(fr_orbit3, Path.Combine(path, @"data\fr_rotation_satellite3.json"));
            var fr_rotation4 = CreateRotationNode(fr_orbit4, Path.Combine(path, @"data\fr_rotation_satellite4.json"));

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
            var databaseProvider = _serviceProvider.GetService<IDatabaseProvider>();
    
            try
            {
                return await Task.Run(() => databaseProvider.LoadProject());

                //var project = databaseProvider.LoadProject(); 
                //return project;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }             
        }
        public IProjectContainer GetEmptyProject()
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = this as IContainerFactory;                 

            var project = factory.CreateProjectContainer("Project1");

            var scenario1 = containerFactory.GetScenario("Scenario1");

            project.AddScenario(scenario1);
            project.SetCurrentScenario(scenario1);

            return project;
        }


    }
}
