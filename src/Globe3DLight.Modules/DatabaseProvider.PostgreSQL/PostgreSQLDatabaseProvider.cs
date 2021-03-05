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

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    public class PostgreSQLDatabaseProvider : IDatabaseProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public PostgreSQLDatabaseProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;         
        }

        public IProjectContainer LoadProject()
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

            IProjectContainer project = null;

            using (var db = new dbGlobe3DLightContext(options))
            {
                project = GetProject(db);
            }

            return project;
            //Scaffold-DbContext "Host=localhost;Port=5432;Database=dbGlobe3DLight;Username=postgres;Password=user"
            //Npgsql.EntityFrameworkCore.PostgreSQL
        }

        public ScenarioData LoadScenarioData()
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

            ScenarioData data;

            using (var db = new dbGlobe3DLightContext(options))
            {
                data = GetScenarioData(db);
            }

            return data;
        }

        private double ToJulianDate(DateTime date) => date.ToOADate() + 2415018.5;
        
        private DateTime FromJulianDate(double jd) => DateTime.FromOADate(jd - 2415018.5);
        
        private IProjectContainer GetProject(dbGlobe3DLightContext db)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = _serviceProvider.GetService<IContainerFactory>();
            var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
  
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
           
            var epoch = FromJulianDate(initialConditions.JulianDateOnTheDay);
            var earthAngleDeg = initialConditions.EarthAngleBegin;

            var begin = epoch.AddSeconds(initialConditions.ModelingTimeBegin);            
            var duration = TimeSpan.FromSeconds(initialConditions.ModelingTimeDuration);

            var project = factory.CreateProjectContainer("Project1");

            var scenario1 = containerFactory.GetScenario("Scenario1", begin, duration);

            var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();
         
            var fr_j2000 = factory.CreateLogicalTreeNode("fr_j2000", dataFactory.CreateJ2000Animator(epoch, earthAngleDeg));
            root.AddChild(fr_j2000);

            var fr_sun = containerFactory.CreateSunNode("fr_sun", root, initialConditions.ToSunData());

            var fr_gss = new List<ILogicalTreeNode>();

            for (int i = 0; i < groundStations.Count; i++)
            {
                var gs = groundStations[i];

                var fr_gs = factory.CreateLogicalTreeNode(string.Format("fr_gs{0:00}", i + 1), dataFactory.CreateGroundStationState(gs.Lon, gs.Lat, 0.0, 6371.0));

                fr_gss.Add(fr_gs);

                fr_j2000.AddChild(fr_gs);
            }

            var gos = groundObjects.ToDictionary(s => s.Name, s => (s.Lon, s.Lat, 6371.0));

            var fr_gos = factory.CreateLogicalTreeNode("fr_gos", dataFactory.CreateGroundObjectListState(gos));
            fr_j2000.AddChild(fr_gos);

            var fr_sats = new List<ILogicalTreeNode>();
            var fr_rotations = new List<ILogicalTreeNode>();
            var fr_sensors = new List<ILogicalTreeNode>();
            var fr_antennas = new List<ILogicalTreeNode>();
            var fr_orbits = new List<ILogicalTreeNode>();

            for (int i = 0; i < satellites.Count; i++)
            {
                fr_sats.Add(containerFactory.CreateSatelliteNode(string.Format("fr_orbital_{0}", satellites[i].Name), fr_j2000, satellites[i].ToSatelliteData()));
                fr_rotations.Add(containerFactory.CreateRotationNode(string.Format("fr_rotation_{0}", satellites[i].Name), fr_sats[i], satellites[i].ToRotationData()));
                fr_sensors.Add(containerFactory.CreateSensorNode(string.Format("fr_shooting_sensor{0}", satellites[i].Id), fr_rotations[i], satellites[i].ToSensorData()));
                fr_antennas.Add(containerFactory.CreateAntennaNode(string.Format("fr_antenna{0}", satellites[i].Id), fr_rotations[i], satellites[i].ToAntennaData()));
                fr_orbits.Add(containerFactory.CreateOrbitNode(string.Format("fr_orbit{0}", satellites[i].Id), fr_rotations[i], satellites[i].ToOrbitData()));
            }

            var fr_retrs = new List<ILogicalTreeNode>();

            for (int i = 0; i < retranslators.Count; i++)
            {
                fr_retrs.Add(containerFactory.CreateRetranslatorNode(string.Format("fr_{0}", retranslators[i].Name), root, retranslators[i].ToData()));
            }

            var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
            objBuilder.Add(objFactory.CreateSun("Sun", fr_sun));
            objBuilder.Add(objFactory.CreateEarth("Earth", fr_j2000));

            var taskBuilder = ImmutableArray.CreateBuilder<ISatelliteTask>();

            for (int i = 0; i < satellites.Count; i++)
            {
                var sat = objFactory.CreateSatellite(satellites[i].Name, fr_rotations[i]);
                objBuilder.Add(sat);

                taskBuilder.Add(objFactory.CreateSatelliteTask(
                    sat,
                    satellites[i].ToRotationData(),
                    satellites[i].ToSensorData(),
                    satellites[i].ToAntennaData(),
                    FromJulianDate(satellites[i].JulianDateOnTheDay)));
            }


            for (int i = 0; i < satellites.Count; i++)
            {
                objBuilder.Add(objFactory.CreateSensor(string.Format("Sensor{0}", satellites[i].Id), fr_sensors[i]));
            }

            var assetsBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();

            var rtrs = new List<IScenarioObject>();
            for (int i = 0; i < retranslators.Count; i++)
            {
                var rtr = objFactory.CreateRetranslator(string.Format("Retranslator{0}", retranslators[i].Id), fr_retrs[i], i);
                rtrs.Add(rtr);
            }

            var gss = new List<IScenarioObject>();
            for (int i = 0; i < groundStations.Count; i++)
            {
                var gs = objFactory.CreateGroundStation(groundStations[i].Name, fr_gss[i], i);
                gss.Add(gs);
            }


            objBuilder.Add(objFactory.CreateGroundObjectList("GroundObjectList", fr_gos));


            objBuilder.AddRange(gss);
            objBuilder.AddRange(rtrs);

            assetsBuilder.AddRange(gss);
            assetsBuilder.AddRange(rtrs);

            for (int i = 0; i < satellites.Count; i++)
            {
                var antenna = objFactory.CreateAntenna(string.Format("Antenna{0}", satellites[i].Id), fr_antennas[i]);
                antenna.Assets = assetsBuilder.ToImmutable();
                objBuilder.Add(antenna);
            }

            for (int i = 0; i < satellites.Count; i++)
            {
                objBuilder.Add(objFactory.CreateOrbit(string.Format("Orbit{0}", satellites[i].Id), fr_orbits[i]));
            }

            scenario1.ScenarioObjects = objBuilder.ToImmutable();
            scenario1.SatelliteTasks = taskBuilder.ToImmutable();

            project.AddScenario(scenario1);
            project.SetCurrentScenario(scenario1);
            return project;
        }
                            
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

    }
}
