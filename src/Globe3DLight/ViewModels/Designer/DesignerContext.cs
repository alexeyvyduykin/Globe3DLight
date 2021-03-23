﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using Globe3DLight.Editor;
using Globe3DLight.Entities;
using Globe3DLight.Scene;
using Globe3DLight.Time;


namespace Globe3DLight.Designer
{
    public class DesignerContext
    {
        public static ProjectEditor Editor { get; set; }

        public static ScenarioContainer Scenario { get; set; }

        public static ProjectContainer Project { get; set; }

        public static SliderTimePresenter SliderTimePresenter { get; set; }

        public static ICamera ArcballCamera { get; set; }

        public static Satellite Satellite { get; set; }

        public static SatelliteTask SatelliteTask { get; set; }

        public static Sensor Sensor { get; set; }

        public static Logical SatelliteNode { get; set; }

        public static Logical SensorNode { get; set; }

        public static Logical RotationNode { get; set; }

        public static Logical SunNode { get; set; }

        public static Logical J2000Node { get; set; }

        public static SatelliteData SatelliteData { get; set; }

        public static SensorData SensorData { get; set; }

        public static RotationData RotationData { get; set; }

        public static SunData SunData { get; set; }

        public static J2000Data J2000Data { get; set; }

        public static RetranslatorData RetranslatorData { get; set; }

        public static AntennaData AntennaData { get; set; }

        public static GroundStationData GroundStationData { get; set; }

        public static IState SatelliteAnimator { get; set; }

        public static IState SensorAnimator { get; set; }

        public static IState RotationAnimator { get; set; }

        public static IState SunAnimator { get; set; }

        public static IState J2000Animator { get; set; }

        public static IState RetranslatorAnimator { get; set; }

        public static IState AntennaAnimator { get; set; }

        public static IState GroundStationState { get; set; }

        public static void InitializeContext(IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<IFactory>();
            var objFactory = serviceProvider.GetService<IScenarioObjectFactory>();
            //var databaseFactory = serviceProvider.GetService<IDatabaseFactory>();

            var begin = DateTime.Now;
            var duration = TimeSpan.FromDays(1);

            // Editor

            Editor = serviceProvider.GetService<ProjectEditor>();

            SliderTimePresenter = factory.CreateSliderTimePresenter(begin, duration);// serviceProvider.GetService<ISceneTimer>();

            // New Project

            Editor.OnNewProject();


            // Project

            var containerFactory = serviceProvider.GetService<IContainerFactory>();
            var dataFactory = serviceProvider.GetService<IDataFactory>();

            Project = containerFactory.GetProject();

            Project.AddScenario(containerFactory.GetScenario("Scenario1", begin, duration));
            Project.AddScenario(containerFactory.GetScenario("Scenario2", begin, duration));
            Project.AddScenario(containerFactory.GetScenario("Scenario3", begin, duration));
            Project.CurrentScenario = Project.Scenarios.FirstOrDefault();


            var objBuilder = ImmutableArray.CreateBuilder<BaseEntity>();
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

            Project.CurrentScenario.Entities = objBuilder.ToImmutable();


            // Database

            SatelliteData = DataDesigner.SatelliteData;
            RotationData = DataDesigner.RotationData;
            SensorData = DataDesigner.SensorData;
            SunData = DataDesigner.SunData;
            J2000Data = new J2000Data { Epoch = DateTime.Now, AngleDeg = 120.0 };
            RetranslatorData = DataDesigner.RetranslatorData;
            AntennaData = DataDesigner.AntennaData;
            GroundStationData = new GroundStationData { Lon = 36.0, Lat = 42.17, Elevation = 0.135, EarthRadius = 6371.0 };

            // Data

            var satelliteAnimator = dataFactory.CreateSatelliteAnimator(SatelliteData);
            satelliteAnimator.Animate(0.0);
            SatelliteAnimator = satelliteAnimator;

            var rotationAnimator = dataFactory.CreateRotationAnimator(RotationData);
            rotationAnimator.Animate(1.0);
            RotationAnimator = rotationAnimator;

            var sensorAnimator = dataFactory.CreateSensorAnimator(SensorData);
            sensorAnimator.Animate(0.0);
            SensorAnimator = sensorAnimator;

            var sunAnimator = dataFactory.CreateSunAnimator(SunData);
            sunAnimator.Animate(0.0);
            SunAnimator = sunAnimator;

            var j2000Animator = dataFactory.CreateJ2000Animator(J2000Data);
            j2000Animator.Animate(0.0);
            J2000Animator = j2000Animator;

            var retranslatorAnimator = dataFactory.CreateRetranslatorAnimator(RetranslatorData);
            retranslatorAnimator.Animate(0.0);
            RetranslatorAnimator = retranslatorAnimator;

            var antennaAnimator = dataFactory.CreateAntennaAnimator(AntennaData);
            antennaAnimator.Animate(0.0);
            AntennaAnimator = antennaAnimator;

            var groundStationData = dataFactory.CreateGroundStationState(GroundStationData);
            GroundStationState = groundStationData;


            // Frames

            SatelliteNode = factory.CreateLogical("fr_orbit_satellite1", SatelliteAnimator);
            RotationNode = factory.CreateLogical("fr_rotation_satellite1", RotationAnimator);

            SatelliteNode.AddChild(RotationNode);

            SensorNode = factory.CreateLogical("fr_sensor1", SensorAnimator);

            SunNode = factory.CreateLogical("fr_sun", SunAnimator);

            // Scenario objects

            Satellite = objFactory.CreateSatellite("Satellite1", RotationNode);

            Sensor = objFactory.CreateSensor("Sensor1", null);

            Project.AddChildFrame(Project.CurrentScenario.LogicalTreeNodeRoot.FirstOrDefault(),
                factory.CreateLogical("Frame1", dataFactory.CreateFrameState()));



            //   Template = factory.CreateTemplateContainer();

            Scenario = factory.CreateScenarioContainer();

            var dt = DateTime.Now;
            var events = new List<BaseSatelliteEvent>();
            events.AddRange(objFactory.CreateRotationEvents(RotationData, dt));
            events.AddRange(objFactory.CreateObservationEvents(SensorData, dt));
            events.AddRange(objFactory.CreateTransmissionEvents(AntennaData, dt));
            SatelliteTask = objFactory.CreateSatelliteTask(Satellite, RotationData, SensorData, AntennaData, DateTime.Now);
            //SatelliteTask.Events = events.OrderBy(s => s.Begin).ToList();

            Scenario.AddSatelliteTask(SatelliteTask);

            // Scene

            ArcballCamera = objFactory.CreateArcballCamera(GlmSharp.dvec3.UnitZ);
        }
    }

    internal class DataDesigner
    {
        // x y z vx vy vz u   
        //public IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }

        public static SatelliteData SatelliteData =>
                new SatelliteData()
                {
                    Records = new List<double[]>()
                    {
                        new double[] { 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 0.0 },
                        new double[] { 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 1.0 },
                    },
                    TimeBegin = 0.0,
                    TimeEnd = 86400.0,
                    TimeStep = 60.0,
                };

        public static RetranslatorData RetranslatorData =>
                new RetranslatorData()
                {
                    Records = new List<double[]>()
                    {
                        new double[] { 12000.0, -11500.0, 11750.0, 0.0 },
                        new double[] { 13000.0, -12500.0, 12750.0, 3.0 },
                    },

                    TimeBegin = 0.0,
                    TimeEnd = 86400.0,
                    TimeStep = 60.0,
                };

        public static SensorData SensorData =>
        new SensorData()
        {
            Shootings = new List<ShootingRecord>()
            {
                new ShootingRecord()
                {
                    BeginTime = 0.0,
                    EndTime = 10.0,
                    Gam1 = 0.12,
                    Gam2 = 0.24,
                    Range1 = 550,
                    Range2 = 600,
                    TargetName = "GroundObject0043"
                } ,
                new ShootingRecord()
                {
                    BeginTime = 26.0,
                    EndTime = 35.0,
                    Gam1 = 0.12,
                    Gam2 = 0.24,
                    Range1 = 534,
                    Range2 = 567,
                    TargetName = "GroundObject0634"
                },
                new ShootingRecord()
                {
                    BeginTime = 56.0,
                    EndTime = 67.0,
                    Gam1 = -0.22,
                    Gam2 = -0.24,
                    Range1 = 554,
                    Range2 = 577,
                    TargetName = "GroundObject0234"
                },
            },
            TimeBegin = 0.0,
            TimeEnd = 86400.0,
        };

        public static AntennaData AntennaData =>
        new AntennaData()
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
            },

            TimeBegin = 0.0,
            TimeEnd = 86400.0,
        };

        public static RotationData RotationData =>
            new RotationData()
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
            },
                TimeBegin = 0.0,
                TimeEnd = 86400.0,
            };

        public static SunData SunData =>
        new SunData()
        {
            Position0 = new GlmSharp.dvec3(40000.0, -42000.0, 41500.0),

            Position1 = new GlmSharp.dvec3(38000.0, -44000.0, 37500.0),

            TimeBegin = 0.0,

            TimeEnd = 86400.0,
        };
    }
}