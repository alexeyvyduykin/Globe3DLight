using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using GlmSharp;
using System.Linq;
using Globe3DLight.ViewModels.Containers;
using System.IO;
using Globe3DLight.Models.Data;
using Globe3DLight.Models;

namespace Globe3DLight.ViewModels.Data
{
    public interface IDataFactory
    {
        FrameState CreateFrameState();

        SunAnimator CreateSunAnimator(SunData data);

        SunAnimator CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1);

        EarthAnimator CreateJ2000Animator(EarthData data);

        EarthAnimator CreateJ2000Animator(DateTime epoch, double angleDeg);

        SatelliteAnimator CreateSatelliteAnimator(SatelliteData data);

        SatelliteAnimator CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep);

        RotationAnimator CreateRotationAnimator(RotationData data);

        RotationAnimator CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1);

        SensorAnimator CreateSensorAnimator(SensorData data);
        
        SensorAnimator CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1);

        AntennaAnimator CreateAntennaAnimator(AntennaData data);
       
        AntennaAnimator CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1);

        OrbitState CreateOrbitState(OrbitData data);
        
        OrbitState CreateOrbitState(IList<double[]> records);
      
        GroundStationState CreateGroundStationState(GroundStationData data); 
        
        GroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius);

        GroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data);
        
        GroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects);

        GroundObjectState CreateGroundObjectState(GroundObjectData data); 
        
        GroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius);

        RetranslatorAnimator CreateRetranslatorAnimator(RetranslatorData data);
        
        RetranslatorAnimator CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep);

        LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, SatelliteData data);

        LogicalViewModel CreateRotationNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateRotationNode(LogicalViewModel parent, RotationData data);

        LogicalViewModel CreateSunNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateSunNode(LogicalViewModel parent, SunData data);

        LogicalViewModel CreateSensorNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateSensorNode(LogicalViewModel parent, SensorData data);

        LogicalViewModel CreateRetranslatorNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateRetranslatorNode(ViewModelBase parent, RetranslatorData data);

        LogicalViewModel CreateAntennaNode(LogicalViewModel parent, string path);

        LogicalViewModel CreateAntennaNode(LogicalViewModel parent, AntennaData data);

        LogicalViewModel CreateOrbitNode(LogicalViewModel parent, OrbitData data);

        LogicalViewModel CreateGroundStationNode(ViewModelBase parent, GroundStationData data);
        
        LogicalViewModel CreateGroundObjectNode(ViewModelBase parent, GroundObjectData data);

        LogicalViewModel CreateEarthNode(LogicalViewModel parent, EarthData data);

        LogicalCollectionViewModel CreateCollectionNode(string name, LogicalViewModel parent);
    }


    public class DataFactory : IDataFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public FrameState CreateFrameState() => new FrameState();
        
        public SunAnimator CreateSunAnimator(SunData data) => new SunAnimator(data);
                
        public SunAnimator CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1) => new SunAnimator(new SunData(nameof(SunData), pos0, pos1, t0, t1));
        
        public EarthAnimator CreateJ2000Animator(EarthData data) => new EarthAnimator(data);

        public EarthAnimator CreateJ2000Animator(DateTime epoch, double angleDeg) => 
            new EarthAnimator(new EarthData(nameof(EarthData), epoch, angleDeg));
        
        public SatelliteAnimator CreateSatelliteAnimator(SatelliteData data) => new SatelliteAnimator(data);
                
        public SatelliteAnimator CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep) => 
            new SatelliteAnimator(new SatelliteData(nameof(SatelliteData), records, t0, t1, tStep));
        
        public RotationAnimator CreateRotationAnimator(RotationData data) => new RotationAnimator(data);
                
        public RotationAnimator CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1) => 
            new RotationAnimator(new RotationData("", nameof(RotationData), rotations, t0, t1));
        
        public SensorAnimator CreateSensorAnimator(SensorData data) => new SensorAnimator(data);
                
        public SensorAnimator CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1) => 
            new SensorAnimator(new SensorData("", nameof(SensorData), shootings, t0, t1));
        
        public AntennaAnimator CreateAntennaAnimator(AntennaData data) => new AntennaAnimator(data);
                
        public AntennaAnimator CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1) => 
            new AntennaAnimator(new AntennaData("", nameof(AntennaData), translations, t0, t1));
        
        public OrbitState CreateOrbitState(OrbitData data) => new OrbitState(data);
                
        public OrbitState CreateOrbitState(IList<double[]> records) => new OrbitState(new OrbitData("", nameof(OrbitData), records));
                
        public GroundStationState CreateGroundStationState(GroundStationData data) => new GroundStationState(data);
                
        public GroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius) => 
            new GroundStationState(new GroundStationData(nameof(GroundStationData), lon, lat, elevation, earthRadius));
        
        public GroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data) => 
            new GroundObjectListState(
                new Dictionary<string, GroundObjectState>(data.Select(s => KeyValuePair.Create(s.Key, CreateGroundObjectState(s.Value)))));
                
        public GroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects) => 
            new GroundObjectListState(
                groundObjects.ToDictionary(s => s.Key, s => CreateGroundObjectState(s.Value.lon, s.Value.lat, s.Value.earthRadius)));
        
        public GroundObjectState CreateGroundObjectState(GroundObjectData data) => new GroundObjectState(data);
                
        public GroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius) => 
            new GroundObjectState(new GroundObjectData(nameof(GroundObjectData), lon, lat, earthRadius));
        
        public RetranslatorAnimator CreateRetranslatorAnimator(RetranslatorData data) => new RetranslatorAnimator(data);
                
        public RetranslatorAnimator CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep) => 
            new RetranslatorAnimator(new RetranslatorData(nameof(RetranslatorData), records, t0, t1, tStep));

        public LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var db1 = jsonDataProvider.CreateDataFromPath<SatelliteData>(path);
            var satelliteState = dataFactory.CreateSatelliteAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_satellite = factory.CreateLogical(name, satelliteState);
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }
        
        public LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, SatelliteData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var satelliteState = dataFactory.CreateSatelliteAnimator(data);
            var fr_satellite = factory.CreateLogical(name, satelliteState);
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }

        public LogicalViewModel CreateRotationNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var db2 = jsonDataProvider.CreateDataFromPath<RotationData>(path);
            var rotationData = dataFactory.CreateRotationAnimator(db2);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_rotation = factory.CreateLogical(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;

        }
        
        public LogicalViewModel CreateRotationNode(LogicalViewModel parent, RotationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var rotationData = dataFactory.CreateRotationAnimator(data);
            var fr_rotation = factory.CreateLogical(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;
        }

        public LogicalViewModel CreateSunNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var db = jsonDataProvider.CreateDataFromPath<SunData>(path);
            var sun_data = dataFactory.CreateSunAnimator(db);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_sun = factory.CreateLogical(name, sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;
            //  return objFactory.CreateSun(name, fr_sun);
        }
        
        public LogicalViewModel CreateSunNode(LogicalViewModel parent, SunData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var sun_data = dataFactory.CreateSunAnimator(data);
            var fr_sun = factory.CreateLogical(name, sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;
        }
        
        public LogicalViewModel CreateSensorNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var db = jsonDataProvider.CreateDataFromPath<SensorData>(path);
            var sensor_data = dataFactory.CreateSensorAnimator(db);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_sensor = factory.CreateLogical(name, sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;
            // return objFactory.CreateSensor(name, fr_sensor);
        }
        
        public LogicalViewModel CreateSensorNode(LogicalViewModel parent, SensorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var sensor_data = dataFactory.CreateSensorAnimator(data);
            var fr_sensor = factory.CreateLogical(name, sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;
        }
        
        public LogicalViewModel CreateRetranslatorNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var db1 = jsonDataProvider.CreateDataFromPath<RetranslatorData>(path);
            var retranslatorData = dataFactory.CreateRetranslatorAnimator(db1);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_retranslator = factory.CreateLogical(name, retranslatorData);
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }
        
        public LogicalViewModel CreateRetranslatorNode(ViewModelBase parent, RetranslatorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var retranslatorData = dataFactory.CreateRetranslatorAnimator(data);
            var fr_retranslator = factory.CreateLogical(name, retranslatorData);
          
            if (parent is LogicalViewModel logical)
            {
                logical.AddChild(fr_retranslator);
            }
            else if (parent is LogicalCollectionViewModel collection)
            {
                collection.AddValue(fr_retranslator);
            }

            return fr_retranslator;
        }

        public LogicalViewModel CreateAntennaNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            //var p0LeftPos = new dvec3(67.74, -12.22, -23.5);
            //   var p0LeftPos = new dvec3(0.6774, -0.1222, -0.235);

            var db = jsonDataProvider.CreateDataFromPath<AntennaData>(path);
            var antenna_data = dataFactory.CreateAntennaAnimator(db/*, p0LeftPos*/);
            var name = Path.GetFileNameWithoutExtension(path);

            var fr_antenna = factory.CreateLogical(name, antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        
        public LogicalViewModel CreateAntennaNode(LogicalViewModel parent, AntennaData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var antenna_data = dataFactory.CreateAntennaAnimator(data);
            var fr_antenna = factory.CreateLogical(name, antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        
        public LogicalViewModel CreateOrbitNode(LogicalViewModel parent, OrbitData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var orbit_data = dataFactory.CreateOrbitState(data);
            var fr_orbit = factory.CreateLogical(name, orbit_data);
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }
        
        public LogicalViewModel CreateGroundStationNode(ViewModelBase parent, GroundStationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var groundStationData = dataFactory.CreateGroundStationState(data);
            var fr_groundStation = factory.CreateLogical(name, groundStationData);
        
            if (parent is LogicalViewModel logical)
            {
                logical.AddChild(fr_groundStation);
            }
            else if (parent is LogicalCollectionViewModel collection)
            {
                collection.AddValue(fr_groundStation);
            }

            return fr_groundStation;
        }

        public LogicalViewModel CreateGroundObjectNode(ViewModelBase parent, GroundObjectData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var groundObjectState = dataFactory.CreateGroundObjectState(data);
            var fr_groundObject = factory.CreateLogical(name, groundObjectState);

            if (parent is LogicalViewModel logical)
            {
                logical.AddChild(fr_groundObject);
            }
            else if(parent is LogicalCollectionViewModel collection)
            {
                collection.AddValue(fr_groundObject);
            }

            return fr_groundObject;
        }

        public LogicalViewModel CreateEarthNode(LogicalViewModel parent, EarthData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var earth_data = dataFactory.CreateJ2000Animator(data);
            var fr_earth = factory.CreateLogical(name, earth_data);
            parent.AddChild(fr_earth);
            return fr_earth;
        }

        public LogicalCollectionViewModel CreateCollectionNode(string name, LogicalViewModel parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var fr_collection = factory.CreateLogicalCollection(name);
            parent.AddChild(fr_collection);

            return fr_collection;
        }
    }
}
