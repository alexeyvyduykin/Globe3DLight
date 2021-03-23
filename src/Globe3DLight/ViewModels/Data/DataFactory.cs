using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using GlmSharp;
using System.Linq;
using Globe3DLight.Containers;
using System.IO;

namespace Globe3DLight.Data
{
    public interface IDataFactory
    {
        IFrameState CreateFrameState();

        ISunState CreateSunAnimator(SunData data);
        ISunState CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1);

        IJ2000State CreateJ2000Animator(J2000Data data); 
        IJ2000State CreateJ2000Animator(DateTime epoch, double angleDeg);

        ISatelliteState CreateSatelliteAnimator(SatelliteData data);
        ISatelliteState CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep);

        IRotationState CreateRotationAnimator(RotationData data);
        IRotationState CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1);

        ISensorState CreateSensorAnimator(SensorData data);
        ISensorState CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1);

        IAntennaState CreateAntennaAnimator(AntennaData data); 
        IAntennaState CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1);

        IOrbitState CreateOrbitState(OrbitData data);
        IOrbitState CreateOrbitState(IList<double[]> records);
      
        IGroundStationState CreateGroundStationState(GroundStationData data); 
        IGroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius);

        IGroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data);
        IGroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects);

        IGroundObjectState CreateGroundObjectState(GroundObjectData data); 
        IGroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius);

        IRetranslatorState CreateRetranslatorAnimator(RetranslatorData data); 
        IRetranslatorState CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep);

        Logical CreateSatelliteNode(Logical parent, string path);

        Logical CreateSatelliteNode(Logical parent, SatelliteData data);

        Logical CreateRotationNode(Logical parent, string path);

        Logical CreateRotationNode(Logical parent, RotationData data);

        Logical CreateSunNode(Logical parent, string path);

        Logical CreateSunNode(Logical parent, SunData data);

        Logical CreateSensorNode(Logical parent, string path);

        Logical CreateSensorNode(Logical parent, SensorData data);

        Logical CreateRetranslatorNode(Logical parent, string path);

        Logical CreateRetranslatorNode(ObservableObject parent, RetranslatorData data);

        Logical CreateAntennaNode(Logical parent, string path);

        Logical CreateAntennaNode(Logical parent, AntennaData data);

        Logical CreateOrbitNode(Logical parent, OrbitData data);

        Logical CreateGroundStationNode(ObservableObject parent, GroundStationData data);
        
        Logical CreateGroundObjectNode(ObservableObject parent, GroundObjectData data);

        Logical CreateEarthNode(Logical parent, J2000Data data);

        LogicalCollection CreateCollectionNode(string name, Logical parent);
    }


    public class DataFactory : IDataFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IFrameState CreateFrameState()
        {
            return new FrameState();
        }

        public ISunState CreateSunAnimator(SunData data)
        {
            return new SunAnimator(data);
        }
        
        public ISunState CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1)
        {
            return new SunAnimator(new SunData()
            {
                Position0 = pos0,
                Position1 = pos1,
                TimeBegin = t0,
                TimeEnd = t1,
            });
        }

        public IJ2000State CreateJ2000Animator(J2000Data data)
        {
            return new J2000Animator(data);
        }
        
        public IJ2000State CreateJ2000Animator(DateTime epoch, double angleDeg)
        {
            return new J2000Animator(new J2000Data()
            {
                Epoch = epoch,
                AngleDeg = angleDeg,
            });
        }

        public ISatelliteState CreateSatelliteAnimator(SatelliteData data)
        {
            return new SatelliteAnimator(data);
        }
        
        public ISatelliteState CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep)
        {
            return new SatelliteAnimator(new SatelliteData()
            {
                Records = records,
                TimeBegin = t0,
                TimeEnd = t1,
                TimeStep = tStep,
            });
        }

        public IRotationState CreateRotationAnimator(RotationData data)
        {
            return new RotationAnimator(data);
        }
        
        public IRotationState CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1)
        {
            return new RotationAnimator(new RotationData()
            {
                Rotations = rotations,
                TimeBegin = t0,
                TimeEnd = t1,
            });
        }

        public ISensorState CreateSensorAnimator(SensorData data)
        {
            return new SensorAnimator(data);
        }
        
        public ISensorState CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1)
        {
            return new SensorAnimator(new SensorData()
            {
                Shootings = shootings,
                TimeBegin = t0,
                TimeEnd = t1,
            });
        }

        public IAntennaState CreateAntennaAnimator(AntennaData data)
        {
            return new AntennaAnimator(data);
        }
        
        public IAntennaState CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1)
        {
            return new AntennaAnimator(new AntennaData()
            {
                Translations = translations,
                TimeBegin = t0,
                TimeEnd = t1,
            });
        }

        public IOrbitState CreateOrbitState(OrbitData data)
        {
            return new OrbitState(data);
        }
        
        public IOrbitState CreateOrbitState(IList<double[]> records)
        {
            return new OrbitState(new OrbitData()
            {
                Records = records,
            });
        }
        
        public IGroundStationState CreateGroundStationState(GroundStationData data)
        {
            return new GroundStationState(data);
        }
        
        public IGroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius)
        {
            return new GroundStationState(new GroundStationData()
            {
                Lon = lon,
                Lat = lat,
                Elevation = elevation,
                EarthRadius = earthRadius,
            });
        }

        public IGroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data)
        {
            return new GroundObjectListState(
                new Dictionary<string, IGroundObjectState>(data.Select(s => KeyValuePair.Create(s.Key, CreateGroundObjectState(s.Value)))));
        }
        
        public IGroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects)
        {
            return new GroundObjectListState(
                groundObjects.ToDictionary(s => s.Key, s => CreateGroundObjectState(s.Value.lon, s.Value.lat, s.Value.earthRadius)));
        }

        public IGroundObjectState CreateGroundObjectState(GroundObjectData data)
        {
            return new GroundObjectState(data);
        }
        
        public IGroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius)
        {
            return new GroundObjectState(new GroundObjectData()
            {
                Lon = lon,
                Lat = lat,
                EarthRadius = earthRadius,
            });
        }

        public IRetranslatorState CreateRetranslatorAnimator(RetranslatorData data)
        {
            return new RetranslatorAnimator(data);
        }
        
        public IRetranslatorState CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep)
        {
            return new RetranslatorAnimator(new RetranslatorData()
            {
                Records = records,
                TimeBegin = t0,
                TimeEnd = t1,
                TimeStep = tStep,
            });
        }

        public Logical CreateSatelliteNode(Logical parent, string path)
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
        
        public Logical CreateSatelliteNode(Logical parent, SatelliteData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var satelliteState = dataFactory.CreateSatelliteAnimator(data);
            var fr_satellite = factory.CreateLogical(name, satelliteState);
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }

        public Logical CreateRotationNode(Logical parent, string path)
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
        
        public Logical CreateRotationNode(Logical parent, RotationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var rotationData = dataFactory.CreateRotationAnimator(data);
            var fr_rotation = factory.CreateLogical(name, rotationData);

            parent.AddChild(fr_rotation);

            return fr_rotation;
        }

        public Logical CreateSunNode(Logical parent, string path)
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
        
        public Logical CreateSunNode(Logical parent, SunData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var sun_data = dataFactory.CreateSunAnimator(data);
            var fr_sun = factory.CreateLogical(name, sun_data);
            parent.AddChild(fr_sun);
            return fr_sun;
        }
        
        public Logical CreateSensorNode(Logical parent, string path)
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
        
        public Logical CreateSensorNode(Logical parent, SensorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var sensor_data = dataFactory.CreateSensorAnimator(data);
            var fr_sensor = factory.CreateLogical(name, sensor_data);
            parent.AddChild(fr_sensor);

            return fr_sensor;
        }
        
        public Logical CreateRetranslatorNode(Logical parent, string path)
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
        
        public Logical CreateRetranslatorNode(ObservableObject parent, RetranslatorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var retranslatorData = dataFactory.CreateRetranslatorAnimator(data);
            var fr_retranslator = factory.CreateLogical(name, retranslatorData);
          
            if (parent is Logical logical)
            {
                logical.AddChild(fr_retranslator);
            }
            else if (parent is LogicalCollection collection)
            {
                collection.AddValue(fr_retranslator);
            }

            return fr_retranslator;
        }

        public Logical CreateAntennaNode(Logical parent, string path)
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
        
        public Logical CreateAntennaNode(Logical parent, AntennaData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var antenna_data = dataFactory.CreateAntennaAnimator(data);
            var fr_antenna = factory.CreateLogical(name, antenna_data);
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        
        public Logical CreateOrbitNode(Logical parent, OrbitData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var orbit_data = dataFactory.CreateOrbitState(data);
            var fr_orbit = factory.CreateLogical(name, orbit_data);
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }
        
        public Logical CreateGroundStationNode(ObservableObject parent, GroundStationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var groundStationData = dataFactory.CreateGroundStationState(data);
            var fr_groundStation = factory.CreateLogical(name, groundStationData);
        
            if (parent is Logical logical)
            {
                logical.AddChild(fr_groundStation);
            }
            else if (parent is LogicalCollection collection)
            {
                collection.AddValue(fr_groundStation);
            }

            return fr_groundStation;
        }

        public Logical CreateGroundObjectNode(ObservableObject parent, GroundObjectData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var groundObjectState = dataFactory.CreateGroundObjectState(data);
            var fr_groundObject = factory.CreateLogical(name, groundObjectState);

            if (parent is Logical logical)
            {
                logical.AddChild(fr_groundObject);
            }
            else if(parent is LogicalCollection collection)
            {
                collection.AddValue(fr_groundObject);
            }

            return fr_groundObject;
        }

        public Logical CreateEarthNode(Logical parent, J2000Data data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
            var factory = _serviceProvider.GetService<IFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var earth_data = dataFactory.CreateJ2000Animator(data);
            var fr_earth = factory.CreateLogical(name, earth_data);
            parent.AddChild(fr_earth);
            return fr_earth;
        }

        public LogicalCollection CreateCollectionNode(string name, Logical parent)
        {
            var factory = _serviceProvider.GetService<IFactory>();

            var fr_collection = factory.CreateLogicalCollection(name);
            parent.AddChild(fr_collection);

            return fr_collection;
        }
    }
}
