#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Globe3DLight.Models;
using Globe3DLight.Models.Editor;
using Globe3DLight.Models.Scene;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Globe3DLight.ViewModels.Editor;
using Globe3DLight.ViewModels.Entities;
using Globe3DLight.ViewModels.Editors;

namespace Globe3DLight.ViewModels.Designer
{
    public class DesignerContext
    {
        public static ProjectEditorViewModel Editor { get; set; }

        public static ScenarioContainerViewModel Scenario { get; set; }

        public static ProjectContainerViewModel Project { get; set; }

        public static SceneTimerEditorViewModel SceneTimerEditor { get; set; }

        public static TaskListEditorViewModel TaskListEditor { get; set; }
        
        public static OutlinerEditorViewModel OutlinerEditor { get; set; }

        public static PropertiesEditorViewModel PropertiesEditor { get; set; }

        public static ICamera ArcballCamera { get; set; }

        public static Satellite Satellite { get; set; }

        public static SatelliteTask SatelliteTask { get; set; }

        public static Sensor Sensor { get; set; }

        //public static LogicalViewModel SatelliteNode { get; set; }

        //public static LogicalViewModel SensorNode { get; set; }

        public static BaseState RotationNode { get; set; }

        //public static LogicalViewModel SunNode { get; set; }

        //public static LogicalViewModel J2000Node { get; set; }

        public static SatelliteData SatelliteData { get; set; }

        public static SensorData SensorData { get; set; }

        public static RotationData RotationData { get; set; }

        public static SunData SunData { get; set; }

        public static EarthData EarthData { get; set; }

        public static RetranslatorData RetranslatorData { get; set; }

        public static AntennaData AntennaData { get; set; }

        public static GroundStationData GroundStationData { get; set; }

        public static FrameViewModel Frame { get; set; }

        public static void InitializeContext(IServiceProvider serviceProvider)
        {
            var factory = serviceProvider.GetService<IFactory>();

            var begin = DateTime.Now;
            var duration = TimeSpan.FromDays(1);

            // Editor

            Editor = serviceProvider.GetService<ProjectEditorViewModel>();

    

            // New Project

            Editor.OnNewProject();

            // Project

            var containerFactory = serviceProvider.GetService<IContainerFactory>();

            Project = containerFactory.GetProject();

            Project.AddScenario(factory.CreateScenarioContainer("Scenario1", begin, duration));
            Project.AddScenario(factory.CreateScenarioContainer("Scenario2", begin, duration));
            Project.AddScenario(factory.CreateScenarioContainer("Scenario3", begin, duration));
            Project.CurrentScenario = Project.Scenarios.FirstOrDefault();

            // Database

            SatelliteData = DataDesigner.SatelliteData;
            RotationData = DataDesigner.RotationData;
            SensorData = DataDesigner.SensorData;
            SunData = DataDesigner.SunData;
            EarthData = new EarthData("Earth", DateTime.Now, 120.0);
            RetranslatorData = DataDesigner.RetranslatorData;
            AntennaData = DataDesigner.AntennaData;
            GroundStationData = new GroundStationData("", 36.0, 42.17, 0.135, 6371.0);

            // Frames

            Frame = factory.CreateRootFrame();

            // Entities

            var objBuilder = ImmutableArray.CreateBuilder<BaseEntity>();
            objBuilder.Add(factory.CreateSpacebox(null));
            objBuilder.Add(factory.CreateSun(SunData, null));
            objBuilder.Add(factory.CreateEarth(EarthData, null));
            //objBuilder.Add(objFactory.CreateSatellite("Satellite1", null));
            //objBuilder.Add(objFactory.CreateSatellite("Satellite2", null));
            //objBuilder.Add(objFactory.CreateSatellite("Satellite3", null));
            //objBuilder.Add(objFactory.CreateSatellite("Satellite4", null));
            //objBuilder.Add(objFactory.CreateSensor("Sensor1", null));
            //objBuilder.Add(objFactory.CreateSensor("Sensor2", null));
            //objBuilder.Add(objFactory.CreateSensor("Sensor3", null));
            //objBuilder.Add(objFactory.CreateSensor("Sensor4", null));

            //Project.CurrentScenario.Entities = objBuilder.ToImmutable();

            // Frames and Data

            //var satelliteAnimator = dataFactory.CreateSatelliteAnimator("fr_orbit_satellite1", SatelliteData);
            //satelliteAnimator.Animate(0.0);
            //SatelliteNode = satelliteAnimator;

            //var rotationAnimator = dataFactory.CreateRotationAnimator("fr_rotation_satellite1", RotationData);
            //rotationAnimator.Animate(1.0);
            //RotationNode = rotationAnimator;

            //SatelliteNode.AddChild(RotationNode);

            //var sensorAnimator = dataFactory.CreateSensorAnimator("fr_sensor1", SensorData);
            //sensorAnimator.Animate(0.0);
            //SensorNode = sensorAnimator;

            //var sunAnimator = dataFactory.CreateSunAnimator("fr_sun", SunData);
            //sunAnimator.Animate(0.0);
            //SunNode = sunAnimator;

            //var j2000Animator = dataFactory.CreateJ2000Animator("fr_earth", J2000Data);
            //j2000Animator.Animate(0.0);
            //EarthNode = j2000Animator;

            //var retranslatorAnimator = dataFactory.CreateRetranslatorAnimator(RetranslatorData);
            //retranslatorAnimator.Animate(0.0);
            //RetranslatorAnimator = retranslatorAnimator;

            //var antennaAnimator = dataFactory.CreateAntennaAnimator(AntennaData);
            //antennaAnimator.Animate(0.0);
            //AntennaAnimator = antennaAnimator;

            //var groundStationData = dataFactory.CreateGroundStationState(GroundStationData);
            //GroundStationState = groundStationData;


            // Scenario objects

            //Satellite = objFactory.CreateSatellite("Satellite1", RotationNode);

            //Sensor = objFactory.CreateSensor("Sensor1", null);

            //Project.AddChildFrame(Project.CurrentScenario.FrameRoot.First().State, factory.CreateIdentityState());

            //   Template = factory.CreateTemplateContainer();

            Scenario = factory.CreateScenarioContainer("Scenario1", begin, duration);

            //var dt = DateTime.Now;
            //var events = new List<BaseSatelliteEvent>();
            //events.AddRange(objFactory.CreateRotationEvents(RotationData, dt));
            //events.AddRange(objFactory.CreateObservationEvents(SensorData, dt));
            //events.AddRange(objFactory.CreateTransmissionEvents(AntennaData, dt));
            //SatelliteTask = objFactory.CreateSatelliteTask(Satellite, RotationData, SensorData, AntennaData, DateTime.Now);
            //SatelliteTask.Events = events.OrderBy(s => s.Begin).ToList();

            //Scenario.AddSatelliteTask(SatelliteTask);

            // Scene

            ArcballCamera = factory.CreateArcballCamera(GlmSharp.dvec3.UnitZ);

            // Editors
            SceneTimerEditor = factory.CreateSceneTimerEditor(begin, duration);
            TaskListEditor = factory.CreateTaskListEditor(Scenario);
            OutlinerEditor = factory.CreateOutlinerEditor(Scenario);
            PropertiesEditor = factory.CreatePropertiesEditor(Scenario);
        }
    }

    internal class DataDesigner
    {
        // x y z vx vy vz u   
        //public IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }

        public static SatelliteData SatelliteData =>
            new SatelliteData("",
                    new List<double[]>()
                    {
                        new double[] { 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 0.0 },
                        new double[] { 7000.0, -6800.0, 6750.0, 100.0, -150.0, 80.0, 1.0 },
                    },
                    0.0, 86400.0, 60.0);

        public static RetranslatorData RetranslatorData =>
                new RetranslatorData("",
                    new List<double[]>()
                    {
                        new double[] { 12000.0, -11500.0, 11750.0, 0.0 },
                        new double[] { 13000.0, -12500.0, 12750.0, 3.0 },
                    },
                    0.0, 86400.0, 60.0);

        public static SensorData SensorData =>
        new SensorData("", "",
            new List<ShootingRecord>()
            {
                new ShootingRecord(0.0, 10.0, 0.12, 0.24, 550, 600, "GroundObject0043"),
                new ShootingRecord(26.0, 35.0, 0.12, 0.24, 534, 567, "GroundObject0634"),
                new ShootingRecord(56.0, 67.0, -0.22, -0.24, 554, 577, "GroundObject0234"),
            },
            0.0, 86400.0);

        public static AntennaData AntennaData =>
            new AntennaData("", "",
                new List<TranslationRecord>()
                {
                    new TranslationRecord(0.0, 10.0, "RTR0000001"),
                    new TranslationRecord(17.0, 23.0, "RTR0000002"),
                    new TranslationRecord(56.0, 60.0, "GST0000002")
                },
                0.0, 86400.0);

        public static RotationData RotationData =>
            new RotationData("", "",
                new List<RotationRecord>()
                {
                    new RotationRecord(0.0, 10.0, 25.0),
                    new RotationRecord(12.0, 15.0, -25.0),
                    new RotationRecord(22.0, 30.0, 25.0)
                },
                0.0, 86400.0);

        public static SunData SunData =>
            new SunData("Sun",
                new GlmSharp.dvec3(40000.0, -42000.0, 41500.0),
                new GlmSharp.dvec3(38000.0, -44000.0, 37500.0),
                0.0, 86400.0);
    }
}
