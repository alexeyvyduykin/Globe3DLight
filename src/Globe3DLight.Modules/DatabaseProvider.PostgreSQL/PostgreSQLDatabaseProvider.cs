using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Editor;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    public class PostgreSQLDatabaseProvider : ObservableObject, IDatabaseProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public PostgreSQLDatabaseProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;         
        }

        public async Task<ScenarioData> LoadData() => await Task.Run(() => LoadScenarioDataFromDatabase());
        
        public async Task<IProjectContainer> LoadProject() => await Task.Run(() => LoadProjectFromDatabase());

        public async Task Save()
        {
            var data = await LoadData();

            _serviceProvider.GetService<IJsonDataProvider>().Save(data);
        }

        private IProjectContainer LoadProjectFromDatabase()
        {        
            IProjectContainer project = null;

            using (var db = new dbGlobe3DLightContext(GetOptions()))
            {
                var scenarioData = GetScenarioData(db);
                //project = GetProject(db);
                project = _serviceProvider.GetService<IContainerFactory>().GetProject(scenarioData);
            }

            return project;

        }

        private ScenarioData LoadScenarioDataFromDatabase()
        {
            ScenarioData data;

            using (var db = new dbGlobe3DLightContext(GetOptions()))
            {
                data = GetScenarioData(db);
            }

            return data;
        }

        private DbContextOptions<dbGlobe3DLightContext> GetOptions()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");
            var major = int.Parse(config["PostgresVersionMajor"]);
            var minor = int.Parse(config["PostgresVersionMinor"]);

            var optionsBuilder = new DbContextOptionsBuilder<dbGlobe3DLightContext>();
            var options = optionsBuilder.UseNpgsql(connectionString, options => options.SetPostgresVersion(new Version(major, minor))).Options;

            return options;
            //Scaffold-DbContext "Host=localhost;Port=5432;Database=dbGlobe3DLight;Username=postgres;Password=user"
            //Npgsql.EntityFrameworkCore.PostgreSQL
        }

        private double ToJulianDate(DateTime date) => date.ToOADate() + 2415018.5;
        
        private DateTime FromJulianDate(double jd) => DateTime.FromOADate(jd - 2415018.5);
        
        //private IProjectContainer GetProject(dbGlobe3DLightContext db)
        //{
        //    var factory = _serviceProvider.GetService<IFactory>();
        //    var containerFactory = _serviceProvider.GetService<IContainerFactory>();
        //    var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();
        //    var dataFactory = _serviceProvider.GetService<IDataFactory>();
  
        //    var initialConditions = db.InitialConditions.FirstOrDefault();
        //    var groundStations = db.GroundStations.ToList();
        //    var retranslators = db.Retranslators.Include(s => s.RetranslatorPositions).ToList();
        //    var groundObjects = db.GroundObjects.ToList();
        //    db.SatelliteOrbitPositions.Load();
        //    db.SatellitePositions.Load();
        //    db.SatelliteRotations.Load();
        //    db.SatelliteShootings.Load();
        //    db.SatelliteToGroundStationTransfers.Load();
        //    db.SatelliteToRetranslatorTransfers.Load();     
        //    var satellites = db.Satellites.ToList();
           
        //    var epoch = FromJulianDate(initialConditions.JulianDateOnTheDay);
        //    var earthAngleDeg = initialConditions.EarthAngleBegin;

        //    var begin = epoch.AddSeconds(initialConditions.ModelingTimeBegin);            
        //    var duration = TimeSpan.FromSeconds(initialConditions.ModelingTimeDuration);

        //    var project = factory.CreateProjectContainer("Project1");

        //    var scenario1 = containerFactory.GetScenario("Scenario1", begin, duration);

        //    var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();
         
        //    var fr_j2000 = factory.CreateLogicalTreeNode("fr_j2000", dataFactory.CreateJ2000Animator(epoch, earthAngleDeg));
        //    root.AddChild(fr_j2000);

        //    var fr_sun = dataFactory.CreateSunNode(root, initialConditions.ToSunData());

        //    var fr_gss = new List<ILogicalTreeNode>();

        //    for (int i = 0; i < groundStations.Count; i++)
        //    {
        //        var gs = groundStations[i];

        //        var fr_gs = factory.CreateLogicalTreeNode(string.Format("fr_gs{0:00}", i + 1), dataFactory.CreateGroundStationState(gs.Lon, gs.Lat, 0.0, 6371.0));

        //        fr_gss.Add(fr_gs);

        //        fr_j2000.AddChild(fr_gs);
        //    }

        //    var gos = groundObjects.ToDictionary(s => s.Name, s => (s.Lon, s.Lat, 6371.0));

        //    var fr_gos = factory.CreateLogicalTreeNode("fr_gos", dataFactory.CreateGroundObjectListState(gos));
        //    fr_j2000.AddChild(fr_gos);

        //    var fr_sats = new List<ILogicalTreeNode>();
        //    var fr_rotations = new List<ILogicalTreeNode>();
        //    var fr_sensors = new List<ILogicalTreeNode>();
        //    var fr_antennas = new List<ILogicalTreeNode>();
        //    var fr_orbits = new List<ILogicalTreeNode>();

        //    for (int i = 0; i < satellites.Count; i++)
        //    {
        //        fr_sats.Add(dataFactory.CreateSatelliteNode(fr_j2000, satellites[i].ToSatelliteData()));
        //        fr_rotations.Add(dataFactory.CreateRotationNode(fr_sats[i], satellites[i].ToRotationData()));
        //        fr_sensors.Add(dataFactory.CreateSensorNode(fr_rotations[i], satellites[i].ToSensorData()));
        //        fr_antennas.Add(dataFactory.CreateAntennaNode(fr_rotations[i], satellites[i].ToAntennaData()));
        //        fr_orbits.Add(dataFactory.CreateOrbitNode(fr_rotations[i], satellites[i].ToOrbitData()));
        //    }

        //    var fr_retrs = new List<ILogicalTreeNode>();

        //    for (int i = 0; i < retranslators.Count; i++)
        //    {
        //        fr_retrs.Add(dataFactory.CreateRetranslatorNode(root, retranslators[i].ToData()));
        //    }

        //    var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
        //    objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
        //    objBuilder.Add(objFactory.CreateSun("Sun", fr_sun));
        //    objBuilder.Add(objFactory.CreateEarth("Earth", fr_j2000));

        //    var taskBuilder = ImmutableArray.CreateBuilder<ISatelliteTask>();

        //    var satelliteList = new List<ISatellite>();

        //    for (int i = 0; i < satellites.Count; i++)
        //    {
        //        var sat = objFactory.CreateSatellite(satellites[i].Name, fr_rotations[i]);
        //        satelliteList.Add(sat);

        //        taskBuilder.Add(objFactory.CreateSatelliteTask(
        //            sat,
        //            satellites[i].ToRotationData(),
        //            satellites[i].ToSensorData(),
        //            satellites[i].ToAntennaData(),
        //            FromJulianDate(satellites[i].JulianDateOnTheDay)));
        //    }


        //    for (int i = 0; i < satellites.Count; i++)
        //    {
        //        satelliteList[i].AddChild(objFactory.CreateSensor(string.Format("Sensor{0}", satellites[i].Id), fr_sensors[i]));
        //    }

        //    var assetsBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();

        //    var rtrs = new List<IScenarioObject>();
        //    for (int i = 0; i < retranslators.Count; i++)
        //    {
        //        var rtr = objFactory.CreateRetranslator(retranslators[i].Name, fr_retrs[i], i);
        //        rtrs.Add(rtr);
        //    }

        //    var gss = new List<IScenarioObject>();
        //    for (int i = 0; i < groundStations.Count; i++)
        //    {
        //        var gs = objFactory.CreateGroundStation(groundStations[i].Name, fr_gss[i], i);
        //        gss.Add(gs);
        //    }


        //    objBuilder.Add(objFactory.CreateGroundObjectList("GroundObjectList", fr_gos));


        //    objBuilder.AddRange(gss);
        //    objBuilder.AddRange(rtrs);

        //    assetsBuilder.AddRange(gss);
        //    assetsBuilder.AddRange(rtrs);

        //    for (int i = 0; i < satellites.Count; i++)
        //    {
        //        var antenna = objFactory.CreateAntenna(string.Format("Antenna{0}", satellites[i].Id), fr_antennas[i]);
        //        antenna.Assets = assetsBuilder.ToImmutable();
        //        satelliteList[i].AddChild(antenna);
        //    }

        //    for (int i = 0; i < satellites.Count; i++)
        //    {
        //        satelliteList[i].AddChild(objFactory.CreateOrbit(string.Format("Orbit{0}", satellites[i].Id), fr_orbits[i]));
        //    }

        //    objBuilder.AddRange(satelliteList);

        //    scenario1.ScenarioObjects = objBuilder.ToImmutable();
        //    scenario1.SatelliteTasks = taskBuilder.ToImmutable();

        //    project.AddScenario(scenario1);
        //    project.SetCurrentScenario(scenario1);
        //    return project;
        //}
                            
        private ScenarioData GetScenarioData(dbGlobe3DLightContext db)
        {                  
            var initialConditions = db.InitialConditions.FirstOrDefault();
            var groundStations = db.GroundStations.ToList();
            var retranslators = db.Retranslators.Include(s => s.RetranslatorPositions).ToList();
            var groundObjects = db.GroundObjects.ToList();
            db.SatelliteOrbitPositions.Load();
            db.SatellitePositions.Load();
            db.SatelliteRotations.Load();
            db.SatelliteShootings.Load();
            db.SatelliteToGroundStationTransfers.Load();
            db.SatelliteToRetranslatorTransfers.Load();
            var satellites = db.Satellites.ToList();

            var epoch = initialConditions.JulianDateOnTheDay;
            var begin = initialConditions.ModelingTimeBegin;
            var duration = initialConditions.ModelingTimeDuration;

            return new ScenarioData()
            {
                Name = "Scenario1",
                JulianDateOnTheDay = epoch,
                ModelingTimeBegin = begin,
                ModelingTimeDuration = duration,
                Sun = initialConditions.ToSunData(),
                Earth = initialConditions.ToJ2000Data(),
                GroundObjects = groundObjects.OrderBy(s => s.Id).Select(s => s.ToData()).ToList(),
                GroundStations = groundStations.OrderBy(s => s.Id).Select(s => s.ToData()).ToList(),
                RetranslatorPositions = retranslators.OrderBy(s => s.Id).Select(s => s.ToData()).ToList(),
                SatellitePositions = satellites.OrderBy(s => s.Id).Select(s => s.ToSatelliteData()).ToList(),
                SatelliteOrbits = satellites.OrderBy(s => s.Id).Select(s => s.ToOrbitData()).ToList(),
                SatelliteRotations = satellites.OrderBy(s => s.Id).Select(s => s.ToRotationData()).ToList(),
                SatelliteShootings = satellites.OrderBy(s => s.Id).Select(s => s.ToSensorData()).ToList(),
                SatelliteTransfers = satellites.OrderBy(s => s.Id).Select(s => s.ToAntennaData()).ToList(),
            };
        }
        
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }        
    }
}
