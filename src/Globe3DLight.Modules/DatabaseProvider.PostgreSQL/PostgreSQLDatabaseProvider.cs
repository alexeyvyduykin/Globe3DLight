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
            this._serviceProvider = serviceProvider;         
        }

        public IProjectContainer LoadProject()
        {        
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("DatabaseProvider.PostgreSQL/postgresSettings.json");
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

        private double ToJulianDate(DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }

        private DateTime FromJulianDate(double jd)
        {
            var d = jd - 2415018.5;

            return DateTime.FromOADate(d);
        }

        private IProjectContainer GetProject(dbGlobe3DLightContext db)
        {
            var factory = _serviceProvider.GetService<IFactory>();
            var containerFactory = _serviceProvider.GetService<IContainerFactory>();
            var objFactory = _serviceProvider.GetService<IScenarioObjectFactory>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            //var databaseFactory = _serviceProvider.GetService<IDatabaseFactory>();

            var project = factory.CreateProjectContainer("Project1");

            var scenario1 = containerFactory.GetScenario("Scenario1");
            var root = scenario1.LogicalTreeNodeRoot.FirstOrDefault();

            var initialConditions = db.InitialConditions.FirstOrDefault();
            var groundStations = db.GroundStations.ToList();
            var retranslators = db.Retranslators.Include(s => s.RetranslatorPositions).ToList();
            var groundObjects = db.GroundObjects.ToList();
            db.SatellitePositions.Load();
            db.SatelliteRotations.Load();
            db.SatelliteShootings.Load();
            db.SatelliteToGroundStationTransfers.Load();
            db.SatelliteToRetranslatorTransfers.Load();     
            var satellites = db.Satellites.ToList();
           

            var epoch = FromJulianDate(initialConditions.JulianDateOnTheDay);
            var earthAngleDeg = initialConditions.EarthAngleBegin;


            var fr_j2000 = factory.CreateLogicalTreeNode("fr_j2000", dataFactory.CreateJ2000Animator(new J2000Data() { Epoch = epoch, AngleDeg = earthAngleDeg }));
            root.AddChild(fr_j2000);

            var fr_sun = CreateSunNode(root, initialConditions);

            var fr_gss = new List<ILogicalTreeNode>();

            for (int i = 0; i < groundStations.Count; i++)
            {
                var gs = groundStations[i];

                var fr_gs = factory.CreateLogicalTreeNode(string.Format("fr_gs{0:00}", i + 1), dataFactory.CreateGroundStationData(new GroundStationData() { Lon = gs.Lon, Lat = gs.Lat, Elevation = 0.0, EarthRadius = 6371.0 }));

                fr_gss.Add(fr_gs);

                fr_j2000.AddChild(fr_gs);
            }

            var gos = groundObjects.ToDictionary(s => s.Name, s => (s.Lon, s.Lat));

            var fr_gos = factory.CreateLogicalTreeNode("fr_gos", dataFactory.CreateGroundObjectListData(new GroundObjectListData() { Positions = gos, EarthRadius = 6371.0 }));
            fr_j2000.AddChild(fr_gos);

            var fr_orbits = new List<ILogicalTreeNode>();
            var fr_rotations = new List<ILogicalTreeNode>();
            var fr_sensors = new List<ILogicalTreeNode>();
            var fr_antennas = new List<ILogicalTreeNode>();

            for (int i = 0; i < satellites.Count; i++)
            {
                fr_orbits.Add(CreateOrbitNode(fr_j2000, satellites[i]));
                fr_rotations.Add(CreateRotationNode(fr_orbits[i], satellites[i]));
                fr_sensors.Add(CreateSensorNode(fr_rotations[i], satellites[i]));
                fr_antennas.Add(CreateAntennaNode(fr_rotations[i], satellites[i]));
            }

            var fr_retrs = new List<ILogicalTreeNode>();

            for (int i = 0; i < retranslators.Count; i++)
            {
                fr_retrs.Add(CreateRetranslatorNode(root, retranslators[i]));
            }

            var objBuilder = ImmutableArray.CreateBuilder<IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", root));
            objBuilder.Add(objFactory.CreateSun("Sun", fr_sun));
            objBuilder.Add(objFactory.CreateEarth("Earth", fr_j2000));

            for (int i = 0; i < satellites.Count; i++)
            {
                objBuilder.Add(objFactory.CreateSatellite(satellites[i].Name, fr_rotations[i]));
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

            scenario1.ScenarioObjects = objBuilder.ToImmutable();

            project.AddScenario(scenario1);
            //    project.AddScenario(scenario2);
            //    project.AddScenario(scenario3);
            project.SetCurrentScenario(scenario1);

            return project;
        }


        private ILogicalTreeNode CreateOrbitNode(ILogicalTreeNode parent, Satellite satellite/*, IList<SatellitePosition> positions*/)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = satellite.Name;
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var arr = satellite.SatellitePositions.OrderBy(s => s.PositionTime).Take(2).ToArray();
            var step = arr[1].PositionTime - arr[0].PositionTime;

            var records = satellite.SatellitePositions.OrderBy(s => s.PositionTime).Select(s =>
            new double[]
            {
                s.PositionX,
                s.PositionY,
                s.PositionZ,
                s.VelocityX,
                s.VelocityY,
                s.VelocityZ,
                s.TrueAnomaly
            }).ToList();

            var db = new OrbitData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            TimeStep = step,
            Records = records,
            };

            var orbitData = dataFactory.CreateOrbitAnimator(db);         
            var fr_orbit = factory.CreateLogicalTreeNode(string.Format("fr_orbital_{0}", name), orbitData);
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }

        private ILogicalTreeNode CreateRotationNode(ILogicalTreeNode parent, Satellite satellite/*, IList<SatelliteRotation> rotations*/)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;
            var name = satellite.Name;

            var rots = satellite.SatelliteRotations.OrderBy(s => s.Begin).Select(s => 
            new RotationRecord()
            { 
                BeginTime = s.Begin, 
                EndTime = s.Begin + s.Duration,
                Angle = s.ToAngle 
            }).ToList();

            var db = new RotationData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            Rotations = rots,
            };

            var rotationData = dataFactory.CreateRotationAnimator(db);
            var fr_rotation = factory.CreateLogicalTreeNode(string.Format("fr_rotation_{0}", name), rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;
        }
        private ILogicalTreeNode CreateSunNode(ILogicalTreeNode parent, InitialCondition initialCondition)
        {             
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var pos0 = new GlmSharp.dvec3(
            initialCondition.SunPositionXbegin,
            initialCondition.SunPositionYbegin,
            initialCondition.SunPositionZbegin
            );

            var pos1 = new GlmSharp.dvec3(
initialCondition.SunPositionXend,
initialCondition.SunPositionYend,
initialCondition.SunPositionZend
);

            var begin = initialCondition.ModelingTimeBegin;
            var duration = initialCondition.ModelingTimeDuration;

            var db = new SunData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            Position0 = pos0,
            Position1 = pos1,
            };

            var sun_data = dataFactory.CreateSunAnimator(db);        
            var fr_sun = factory.CreateLogicalTreeNode("fr_sun", sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;
            //  return objFactory.CreateSun(name, fr_sun);
        }

        private ILogicalTreeNode CreateSensorNode(ILogicalTreeNode parent, Satellite satellite/*, IList<SatelliteShooting> shootings*/)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = satellite.Name;
            var id = satellite.Id;

            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var shoots = satellite.SatelliteShootings.OrderBy(s => s.Begin).Select(s => new ShootingRecord1() 
            { 
            BeginTime = s.Begin,
            EndTime = s.Begin + s.Duration,
            Gam1 = s.Gam1,
            Gam2 = s.Gam2,
            Range1 = s.Range1,
            Range2 = s.Range2,
            TargetName = s.GroundObject.Name,
            }).ToList();

            var db = new SensorData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            Shootings = shoots,
            };
            
            var sensor_data = dataFactory.CreateSensorAnimator(db);
            var fr_sensor = factory.CreateLogicalTreeNode(string.Format("fr_shooting_sensor{0}", id), sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;
            // return objFactory.CreateSensor(name, fr_sensor);
        }

        private ILogicalTreeNode CreateRetranslatorNode(ILogicalTreeNode parent, Retranslator retranslator/*, IList<RetranslatorPosition> positions*/)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = retranslator.Name;
            var begin = retranslator.LifetimeBegin;
            var duration = retranslator.LifetimeDuration;

            var arr = retranslator.RetranslatorPositions.OrderBy(s => s.PositionTime).Take(2).ToArray();
            var step = arr[1].PositionTime - arr[0].PositionTime;

            var records = retranslator.RetranslatorPositions.OrderBy(s => s.PositionTime).Select(s => 
            new double[] 
            {
                s.PositionX, 
                s.PositionY, 
                s.PositionZ,
                s.TrueAnomaly 
            }).ToList();


            var db = new RetranslatorData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            TimeStep = step,
            Records = records,
            
            };

            var retranslatorData = dataFactory.CreateRetranslatorAnimator(db);    
            var fr_retranslator = factory.CreateLogicalTreeNode(string.Format("fr_{0}", name), retranslatorData);
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }

        private ILogicalTreeNode CreateAntennaNode(ILogicalTreeNode parent, Satellite satellite/*, 
            IList<SatelliteToGroundStationTransfer> toGS, IList<SatelliteToRetranslatorTransfer> toRtr*/)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            //var p0LeftPos = new dvec3(67.74, -12.22, -23.5);
            //   var p0LeftPos = new dvec3(0.6774, -0.1222, -0.235);

            var id = satellite.Id;

            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var arr1 = satellite.SatelliteToGroundStationTransfers.Select(s => new TranslationRecord()
            {
                BeginTime = s.Begin,
                EndTime = s.Begin + s.Duration,
                Target = string.Format("GST{0:0000000}", s.GroundStationId - 1),
            }).ToList();

            var arr2 = satellite.SatelliteToRetranslatorTransfers.Select(s => new TranslationRecord() 
            { 
            BeginTime = s.Begin,
            EndTime = s.Begin + s.Duration,
            Target = string.Format("RTR{0:0000000}", s.RetranslatorId - 1),
            }).ToList();

            var arr = arr1.Union(arr2).OrderBy(s => s.BeginTime).ToList();

            var db = new AntennaData() 
            { 
            TimeBegin = begin,
            TimeEnd = begin + duration,
            Translations = arr,
            };

            var antenna_data = dataFactory.CreateAntennaAnimator(db/*, p0LeftPos*/);
            var fr_antenna = factory.CreateLogicalTreeNode(string.Format("fr_antenna{0}", id), antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
    }
}
