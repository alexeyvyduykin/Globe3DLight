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
        FrameState CreateFrameState(string name);

        SunAnimator CreateSunAnimator(string name, SunData data);

        SunAnimator CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1);

        EarthAnimator CreateEarthAnimator(string name, EarthData data);

        EarthAnimator CreateEarthAnimator(DateTime epoch, double angleDeg);

        SatelliteAnimator CreateSatelliteAnimator(string name, SatelliteData data);

        SatelliteAnimator CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep);

        RotationAnimator CreateRotationAnimator(string name, RotationData data);

        RotationAnimator CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1);

        SensorAnimator CreateSensorAnimator(string name, SensorData data);
        
        SensorAnimator CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1);

        AntennaAnimator CreateAntennaAnimator(string name, AntennaData data);
       
        AntennaAnimator CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1);

        OrbitState CreateOrbitState(string name, OrbitData data);
        
        OrbitState CreateOrbitState(IList<double[]> records);
      
        GroundStationState CreateGroundStationState(string name, GroundStationData data); 
        
        GroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius);

        GroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data);
        
        GroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects);

        GroundObjectState CreateGroundObjectState(string name, GroundObjectData data); 
        
        GroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius);

        RetranslatorAnimator CreateRetranslatorAnimator(string name, RetranslatorData data);
        
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

        BaseState CreateOrbitNode(LogicalViewModel parent, OrbitData data);

        LogicalViewModel CreateGroundStationNode(ViewModelBase parent, GroundStationData data);
        
        LogicalViewModel CreateGroundObjectNode(ViewModelBase parent, GroundObjectData data);

        BaseState CreateEarthNode(LogicalViewModel parent, EarthData data);

        LogicalCollectionViewModel CreateCollectionNode(string name, LogicalViewModel parent);
    }


    public class DataFactory : IDataFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public FrameState CreateFrameState(string name) => 
            new FrameState() 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
        
        public SunAnimator CreateSunAnimator(string name, SunData data) => 
            new SunAnimator(data)
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };

        public SunAnimator CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1) => 
            new SunAnimator(new SunData(nameof(SunData), pos0, pos1, t0, t1));
        
        public EarthAnimator CreateEarthAnimator(string name, EarthData data) => 
            new EarthAnimator(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),               
            };

        public EarthAnimator CreateEarthAnimator(DateTime epoch, double angleDeg) => 
            new EarthAnimator(new EarthData(nameof(EarthData), epoch, angleDeg));
        
        public SatelliteAnimator CreateSatelliteAnimator(string name, SatelliteData data) => 
            new SatelliteAnimator(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public SatelliteAnimator CreateSatelliteAnimator(IList<double[]> records, double t0, double t1, double tStep) => 
            new SatelliteAnimator(new SatelliteData(nameof(SatelliteData), records, t0, t1, tStep));
        
        public RotationAnimator CreateRotationAnimator(string name, RotationData data) => 
            new RotationAnimator(data)
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public RotationAnimator CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1) => 
            new RotationAnimator(new RotationData("", nameof(RotationData), rotations, t0, t1));
        
        public SensorAnimator CreateSensorAnimator(string name, SensorData data) => 
            new SensorAnimator(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public SensorAnimator CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1) => 
            new SensorAnimator(new SensorData("", nameof(SensorData), shootings, t0, t1));
        
        public AntennaAnimator CreateAntennaAnimator(string name, AntennaData data) => 
            new AntennaAnimator(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public AntennaAnimator CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1) => 
            new AntennaAnimator(new AntennaData("", nameof(AntennaData), translations, t0, t1));
        
        public OrbitState CreateOrbitState(string name, OrbitData data) => 
            new OrbitState(data)
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public OrbitState CreateOrbitState(IList<double[]> records) => new OrbitState(new OrbitData("", nameof(OrbitData), records));
                
        public GroundStationState CreateGroundStationState(string name, GroundStationData data) => 
            new GroundStationState(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public GroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius) => 
            new GroundStationState(new GroundStationData(nameof(GroundStationData), lon, lat, elevation, earthRadius));
        
        public GroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data) => 
            new GroundObjectListState(
                new Dictionary<string, GroundObjectState>(data.Select(s => KeyValuePair.Create(s.Key, CreateGroundObjectState(s.Key, s.Value)))));
                
        public GroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects) => 
            new GroundObjectListState(
                groundObjects.ToDictionary(s => s.Key, s => CreateGroundObjectState(s.Value.lon, s.Value.lat, s.Value.earthRadius)));
        
        public GroundObjectState CreateGroundObjectState(string name, GroundObjectData data) => 
            new GroundObjectState(data)
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public GroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius) => 
            new GroundObjectState(new GroundObjectData(nameof(GroundObjectData), lon, lat, earthRadius));
        
        public RetranslatorAnimator CreateRetranslatorAnimator(string name, RetranslatorData data) => 
            new RetranslatorAnimator(data) 
            {
                Name = name,
                Children = ImmutableArray.Create<ViewModelBase>(),
            };
                
        public RetranslatorAnimator CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep) => 
            new RetranslatorAnimator(new RetranslatorData(nameof(RetranslatorData), records, t0, t1, tStep));

        public LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();
           
            var db1 = jsonDataProvider.CreateDataFromPath<SatelliteData>(path);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_satellite = dataFactory.CreateSatelliteAnimator(name, db1);   
            
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }
        
        public LogicalViewModel CreateSatelliteNode(LogicalViewModel parent, SatelliteData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();          

            var name = string.Format("fr_{0}", data.Name.ToLower());
            var fr_satellite = dataFactory.CreateSatelliteAnimator(name, data);     
            
            parent.AddChild(fr_satellite);

            return fr_satellite;
        }

        public LogicalViewModel CreateRotationNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();          

            var db2 = jsonDataProvider.CreateDataFromPath<RotationData>(path);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_rotation = dataFactory.CreateRotationAnimator(name, db2);
            
            parent.AddChild(fr_rotation);

            return fr_rotation;

        }
        
        public LogicalViewModel CreateRotationNode(LogicalViewModel parent, RotationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();         

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var fr_rotation = dataFactory.CreateRotationAnimator(name, data);           

            parent.AddChild(fr_rotation);

            return fr_rotation;
        }

        public LogicalViewModel CreateSunNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();  

            var db = jsonDataProvider.CreateDataFromPath<SunData>(path);
            var name = Path.GetFileNameWithoutExtension(path);            
            var fr_sun = dataFactory.CreateSunAnimator(name, db);
               
            parent.AddChild(fr_sun);

            return fr_sun;        
        }
        
        public LogicalViewModel CreateSunNode(LogicalViewModel parent, SunData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();

            var name = string.Format("fr_{0}", data.Name.ToLower());
            var fr_sun = dataFactory.CreateSunAnimator(name, data);
            
            parent.AddChild(fr_sun);

            return fr_sun;
        }
        
        public LogicalViewModel CreateSensorNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();            
           
            var name = Path.GetFileNameWithoutExtension(path);
            var db = jsonDataProvider.CreateDataFromPath<SensorData>(path);
            var fr_sensor = dataFactory.CreateSensorAnimator(name, db);
                      
            parent.AddChild(fr_sensor);

            return fr_sensor;      
        }
        
        public LogicalViewModel CreateSensorNode(LogicalViewModel parent, SensorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();       

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());
            var fr_sensor = dataFactory.CreateSensorAnimator(name, data);
    
            parent.AddChild(fr_sensor);

            return fr_sensor;
        }
        
        public LogicalViewModel CreateRetranslatorNode(LogicalViewModel parent, string path)
        {
            var jsonDataProvider = _serviceProvider.GetService<IJsonDataProvider>();
            var dataFactory = _serviceProvider.GetService<IDataFactory>();       

            var db1 = jsonDataProvider.CreateDataFromPath<RetranslatorData>(path);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_retranslator = dataFactory.CreateRetranslatorAnimator(name, db1);
                       
            parent.AddChild(fr_retranslator);

            return fr_retranslator;
        }
        
        public LogicalViewModel CreateRetranslatorNode(ViewModelBase parent, RetranslatorData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();           

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var fr_retranslator = dataFactory.CreateRetranslatorAnimator(name, data);
                     
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

            //var p0LeftPos = new dvec3(67.74, -12.22, -23.5);
            //   var p0LeftPos = new dvec3(0.6774, -0.1222, -0.235);

            var db = jsonDataProvider.CreateDataFromPath<AntennaData>(path);
            var name = Path.GetFileNameWithoutExtension(path);
            var fr_antenna = dataFactory.CreateAntennaAnimator(name, db);
            
            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        
        public LogicalViewModel CreateAntennaNode(LogicalViewModel parent, AntennaData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();           

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var fr_antenna = dataFactory.CreateAntennaAnimator(name, data);

            parent.AddChild(fr_antenna);

            return fr_antenna;
        }
        
        public BaseState CreateOrbitNode(LogicalViewModel parent, OrbitData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();

            var name = string.Format("fr_{0}_{1}", data.Name.ToLower(), data.SatelliteName.ToLower());

            var fr_orbit = dataFactory.CreateOrbitState(name, data);
            
            parent.AddChild(fr_orbit);

            return fr_orbit;
        }
        
        public LogicalViewModel CreateGroundStationNode(ViewModelBase parent, GroundStationData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();       

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var fr_groundStation = dataFactory.CreateGroundStationState(name, data);           
        
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

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var fr_groundObject = dataFactory.CreateGroundObjectState(name, data);
       
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

        public BaseState CreateEarthNode(LogicalViewModel parent, EarthData data)
        {
            var dataFactory = _serviceProvider.GetService<IDataFactory>();         

            var name = string.Format("fr_{0}", data.Name.ToLower());

            var fr_earth = dataFactory.CreateEarthAnimator(name, data);
            
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
