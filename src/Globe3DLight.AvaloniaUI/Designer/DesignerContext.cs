using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Data.Database;
using Globe3DLight.Editor;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Scene;
using Globe3DLight.Time;


namespace Globe3DLight.AvaloniaUI.Designer
{
    public class DesignerContext
    {
        public static IProjectEditor Editor { get; set; }

        public static IScenarioContainer Scenario { get; set; }

        public static IProjectContainer Project { get; set; }

        public static ITimePresenter TimePresenter { get; set; }

        public static ICamera ArcballCamera { get; set; }

        public static ISatellite Satellite { get; set; }

        public static ISatelliteTask SatelliteTask { get; set; }
        public static ISensor Sensor { get; set; }


        public static ILogicalTreeNode OrbitNode { get; set; }
        public static ILogicalTreeNode SensorNode { get; set; }
        public static ILogicalTreeNode RotationNode { get; set; }
        public static ILogicalTreeNode SunNode { get; set; }
        public static ILogicalTreeNode J2000Node { get; set; }

        public static IOrbitDatabase OrbitDatabase { get; set; }
        public static ISensorDatabase SensorDatabase { get; set; }
        public static IRotationDatabase RotationDatabase { get; set; }
        public static ISunDatabase SunDatabase { get; set; }
        public static IJ2000Database J2000Database { get; set; }
        public static IRetranslatorDatabase RetranslatorDatabase { get; set; }
        public static IAntennaDatabase AntennaDatabase { get; set; }
        public static IGroundStationDatabase GroundStationDatabase { get; set; }


        public static IData OrbitAnimator { get; set; }
        public static IData SensorAnimator { get; set; }
        public static IData RotationAnimator { get; set; }
        public static IData SunAnimator { get; set; }
        public static IData J2000Animator { get; set; }
        public static IData RetranslatorAnimator { get; set; }
        public static IData AntennaAnimator { get; set; }
        public static IData GroundStationData { get; set; }

        public static void InitializeContext(IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<IFactory>();
            var objFactory = serviceProvider.GetService<IScenarioObjectFactory>();
            var databaseFactory = serviceProvider.GetService<IDatabaseFactory>();

            // Editor

            Editor = serviceProvider.GetService<IProjectEditor>();

            TimePresenter = factory.CreateTimePresenter();// serviceProvider.GetService<ISceneTimer>();

            // New Project

            Editor.OnNewProject();


            // Project

            var containerFactory = serviceProvider.GetService<IContainerFactory>();
            var dataFactory = serviceProvider.GetService<IDataFactory>();

            Project = containerFactory.GetProject();

            Project.AddScenario(containerFactory.GetScenario("Scenario1"));
            Project.AddScenario(containerFactory.GetScenario("Scenario2"));
            Project.AddScenario(containerFactory.GetScenario("Scenario3"));
            Project.CurrentScenario = Project.Scenarios.FirstOrDefault();


            var objBuilder = ImmutableArray.CreateBuilder<ScenarioObjects.IScenarioObject>();
            objBuilder.Add(objFactory.CreateSpacebox("Spacebox", null));
            objBuilder.Add(objFactory.CreateSun("Sun", null));
            objBuilder.Add(objFactory.CreateEarth("Earth", null));
            objBuilder.Add(objFactory.CreateSatellite("Satellite1", null));
            objBuilder.Add(objFactory.CreateSatellite("Satellite2", null));
            objBuilder.Add(objFactory.CreateSatellite("Satellite3", null));
            objBuilder.Add(objFactory.CreateSatellite("Satellite4", null));
            objBuilder.Add(objFactory.CreateSensor("Sensor1", null));
            objBuilder.Add(objFactory.CreateSensor("Sensor2", null));
            objBuilder.Add(objFactory.CreateSensor("Sensor3", null));
            objBuilder.Add(objFactory.CreateSensor("Sensor4", null));

            Project.CurrentScenario.ScenarioObjects = objBuilder.ToImmutable();


            // Database

            OrbitDatabase = new DesignerOrbitDatabase();
            RotationDatabase = new DesignRotationDatabase();
            SensorDatabase = new DesignerSensorDatabase();
            SunDatabase = new DesignSunDatabse();
            J2000Database = new J2000Database() { Epoch = DateTime.Now, AngleDeg = 120.0 };
            RetranslatorDatabase = new DesignerRetranslatorDatabase();
            AntennaDatabase = new DesignerAntennaDatabase();
            GroundStationDatabase = new GroundStationDatabase() { Lon = 36.0, Lat = 42.17, Elevation = 0.135, EarthRadius = 6371.0 };

            // Data

            var orbitAnimator = dataFactory.CreateOrbitAnimator(OrbitDatabase);
            orbitAnimator.Animate(0.0);
            OrbitAnimator = orbitAnimator;

            var rotationAnimator = dataFactory.CreateRotationAnimator(RotationDatabase);
            rotationAnimator.Animate(1.0);
            RotationAnimator = rotationAnimator;

            var sensorAnimator = dataFactory.CreateSensorAnimator(SensorDatabase);
            sensorAnimator.Animate(0.0);
            SensorAnimator = sensorAnimator;

            var sunAnimator = dataFactory.CreateSunAnimator(SunDatabase);
            sunAnimator.Animate(0.0);
            SunAnimator = sunAnimator;

            var j2000Animator = dataFactory.CreateJ2000Animator(J2000Database);
            j2000Animator.Animate(0.0);
            J2000Animator = j2000Animator;

            var retranslatorAnimator = dataFactory.CreateRetranslatorAnimator(RetranslatorDatabase);
            retranslatorAnimator.Animate(0.0);
            RetranslatorAnimator = retranslatorAnimator;

            var antennaAnimator = dataFactory.CreateAntennaAnimator(AntennaDatabase);
            antennaAnimator.Animate(0.0);
            AntennaAnimator = antennaAnimator;

            var groundStationData = dataFactory.CreateGroundStationData(GroundStationDatabase);
            GroundStationData = groundStationData;


            // Frames

            OrbitNode = factory.CreateLogicalTreeNode("fr_orbit_satellite1", OrbitAnimator);
            RotationNode = factory.CreateLogicalTreeNode("fr_rotation_satellite1", RotationAnimator);

            OrbitNode.AddChild(RotationNode);

            SensorNode = factory.CreateLogicalTreeNode("fr_sensor1", SensorAnimator);

            SunNode = factory.CreateLogicalTreeNode("fr_sun", SunAnimator);

            // Scenario objects

            Satellite = objFactory.CreateSatellite("Satellite1", RotationNode);

            Sensor = objFactory.CreateSensor("Sensor1", null);

            Project.AddChildFrame(Project.CurrentScenario.LogicalTreeNodeRoot.FirstOrDefault(),
                factory.CreateLogicalTreeNode("Frame1", dataFactory.CreateFrameData()));



            //   Template = factory.CreateTemplateContainer();

            Scenario = factory.CreateScenarioContainer();

            var dt = DateTime.Now;
            var events = new List<ISatelliteEvent>();
            events.AddRange(objFactory.CreateRotationEvents(RotationDatabase, dt));
            events.AddRange(objFactory.CreateObservationEvents(SensorDatabase, dt));
            events.AddRange(objFactory.CreateTransmissionEvents(AntennaDatabase, dt));
            SatelliteTask = objFactory.CreateSatelliteTask(Satellite, DateTime.Now);
            SatelliteTask.Events = events.OrderBy(s => s.Begin).ToList();

            Scenario.SatelliteTasks = ImmutableArray.Create(SatelliteTask);



            // Scene

            ArcballCamera = objFactory.CreateArcballCamera(Satellite);
        }
    }

    internal class DesignerOrbitDatabase : OrbitDatabase
    {
        // x y z vx vy vz u   
        //public IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }

        public DesignerOrbitDatabase()
        {
            Records = new List<double[]>()
            {
                new double[]{ 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 0.0 },
                new double[]{ 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 1.0 },
            };
            TimeBegin = 0.0;
            TimeEnd = 86400.0;
            TimeStep = 60.0;
        }


    }

    internal class DesignerRetranslatorDatabase : RetranslatorDatabase
    {
        public DesignerRetranslatorDatabase()
        {

            Records = new List<double[]>()
            {
            new double[]{ 12000.0, -11500.0, 11750.0, 0.0 },
            new double[]{ 13000.0, -12500.0, 12750.0, 3.0 },
            };

            TimeBegin = 0.0;
            TimeEnd = 86400.0;
            TimeStep = 60.0;
        }
    }

    internal class DesignerSensorDatabase : SensorDatabase
    {
        public DesignerSensorDatabase()
        {
            Shootings = new List<ShootingRecord1>()
            {
                new ShootingRecord1()
                {
                    BeginTime = 0.0,
                    EndTime = 10.0,
                    Gam1 = 0.12,
                    Gam2 = 0.24,
                    Range1 = 550,
                    Range2 = 600,
                    TargetName = "GroundObject0043"
                } ,
                new ShootingRecord1()
                {
                    BeginTime = 26.0,
                    EndTime = 35.0,
                    Gam1 = 0.12,
                    Gam2 = 0.24,
                    Range1 = 534,
                    Range2 = 567,
                    TargetName = "GroundObject0634"
                },
                new ShootingRecord1()
                {
                    BeginTime = 56.0,
                    EndTime = 67.0,
                    Gam1 = -0.22,
                    Gam2 = -0.24,
                    Range1 = 554,
                    Range2 = 577,
                    TargetName = "GroundObject0234"
                },
            };
            TimeBegin = 0.0;
            TimeEnd = 86400.0;
        }
    }

    internal class DesignerAntennaDatabase : AntennaDatabase
    {
        public DesignerAntennaDatabase()
        {
            Translations = new List<TranslationRecord>()
            {
                new TranslationRecord()
                {
                    BeginTime = 0.0,
                    EndTime = 10.0,
                    Target = "RTR0000001",
                },
                new TranslationRecord()
                {
                    BeginTime = 17.0,
                    EndTime = 23.0,
                    Target = "RTR0000002",
                },
                new TranslationRecord()
                {
                    BeginTime = 56.0,
                    EndTime = 60.0,
                    Target = "GST0000002",
                },
            };

            TimeBegin = 0.0;
            TimeEnd = 86400.0;
        }
    }

    internal class DesignRotationDatabase : RotationDatabase
    {
        public DesignRotationDatabase()
        {
            Rotations = new List<RotationRecord>()
            {
                new RotationRecord()
                {
                    BeginTime = 0.0,
                    EndTime = 10.0,
                    Angle = 25.0
                },
                new RotationRecord()
                {
                    BeginTime = 12.0,
                    EndTime = 15.0,
                    Angle = -25.0
                },

                new RotationRecord()
                {
                    BeginTime = 22.0,
                    EndTime = 30.0,
                    Angle = 25.0
                },
            };
            TimeBegin = 0.0;
            TimeEnd = 86400.0;
        }
    }

    internal class DesignSunDatabse : SunDatabase
    {
        public DesignSunDatabse()
        {
            Position0 = new GlmSharp.dvec3(40000.0, -42000.0, 41500.0);

            Position1 = new GlmSharp.dvec3(38000.0, -44000.0, 37500.0);

            TimeBegin = 0.0;

            TimeEnd = 86400.0;
        }
    }
}
