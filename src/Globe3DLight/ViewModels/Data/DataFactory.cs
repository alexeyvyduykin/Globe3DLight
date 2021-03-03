using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using GlmSharp;
using System.Linq;

namespace Globe3DLight.Data
{
    public interface IDataFactory
    {
        IFrameState CreateFrameState();

        ISunState CreateSunAnimator(SunData data);
        ISunState CreateSunAnimator(dvec3 pos0, dvec3 pos1, double t0, double t1);

        IJ2000State CreateJ2000Animator(J2000Data data); 
        IJ2000State CreateJ2000Animator(DateTime epoch, double angleDeg);

        IOrbitState CreateOrbitAnimator(OrbitData data);
        IOrbitState CreateOrbitAnimator(IList<double[]> records, double t0, double t1, double tStep);

        IRotationState CreateRotationAnimator(RotationData data);
        IRotationState CreateRotationAnimator(IList<RotationRecord> rotations, double t0, double t1);

        ISensorState CreateSensorAnimator(SensorData data);
        ISensorState CreateSensorAnimator(IList<ShootingRecord> shootings, double t0, double t1);

        IAntennaState CreateAntennaAnimator(AntennaData data); 
        IAntennaState CreateAntennaAnimator(IList<TranslationRecord> translations, double t0, double t1);

        IGroundStationState CreateGroundStationState(GroundStationData data); 
        IGroundStationState CreateGroundStationState(double lon, double lat, double elevation, double earthRadius);

        IGroundObjectListState CreateGroundObjectListState(IDictionary<string, GroundObjectData> data);
        IGroundObjectListState CreateGroundObjectListState(IDictionary<string, (double lon, double lat, double earthRadius)> groundObjects);

        IGroundObjectState CreateGroundObjectState(GroundObjectData data); 
        IGroundObjectState CreateGroundObjectState(double lon, double lat, double earthRadius);

        IRetranslatorState CreateRetranslatorAnimator(RetranslatorData data); 
        IRetranslatorState CreateRetranslatorAnimator(IList<double[]> records, double t0, double t1, double tStep);
    }


    public class DataFactory : IDataFactory
    {
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

        public IOrbitState CreateOrbitAnimator(OrbitData data)
        {
            return new OrbitAnimator(data);
        }
        public IOrbitState CreateOrbitAnimator(IList<double[]> records, double t0, double t1, double tStep)
        {
            return new OrbitAnimator(new OrbitData()
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
                new Dictionary<string, IGroundObjectState>(groundObjects.Select(s => KeyValuePair.Create(s.Key, CreateGroundObjectState(s.Value.lon, s.Value.lat, s.Value.earthRadius)))));
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
    }
}
