using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal static class ModelExtensions
    {
        public static RetranslatorData ToData(this Retranslator retranslator)
        {          
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

            return new RetranslatorData(retranslator.Name, records, begin, begin + duration, step);
        }

        public static GroundStationData ToData(this GroundStation groundStation)
        {
            return new GroundStationData(groundStation.Name, groundStation.Lon, groundStation.Lat, 0.0, 6371.0);
        }

        public static GroundObjectData ToData(this GroundObject groundObject)
        {
            return new GroundObjectData(groundObject.Name, groundObject.Lon, groundObject.Lat, 6371.0);
        }

        public static SatelliteData ToSatelliteData(this Satellite satellite)
        {
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

            return new SatelliteData(satellite.Name, records, begin, begin + duration, step);
        }
        
        public static OrbitData ToOrbitData(this Satellite satellite)
        {      
            var records = satellite.SatelliteOrbitPositions.OrderBy(s => s.SatelliteId).OrderBy(s => s.TrueAnomaly).Select(s =>
            new double[]
            {
                s.PositionX,
                s.PositionY,
                s.PositionZ,
                s.TrueAnomaly
            }).ToList();

            return new OrbitData(satellite.Name, "Orbit", records);
        }
        
        public static RotationData ToRotationData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;
         
            var rots = satellite.SatelliteRotations.OrderBy(s => s.Begin).Select(s => 
            new RotationRecord(s.Begin, s.Begin + s.Duration, s.ToAngle)).ToList();

            return new RotationData(satellite.Name, "Rotation", rots, begin, begin + duration);
        }

        public static SensorData ToSensorData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var shoots = satellite.SatelliteShootings.OrderBy(s => s.Begin).Select(s => 
            new ShootingRecord(s.Begin, s.Begin + s.Duration, s.Gam1, s.Gam2, s.Range1, s.Range2, s.GroundObject.Name)).ToList();

            return new SensorData(satellite.Name, "SAR", shoots, begin, begin + duration);
        }

        public static AntennaData ToAntennaData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var arr1 = satellite.SatelliteToGroundStationTransfers.Select(s => 
            new TranslationRecord(s.Begin, s.Begin + s.Duration, s.GroundStation.Name)).ToList();

            var arr2 = satellite.SatelliteToRetranslatorTransfers.Select(s => 
            new TranslationRecord(s.Begin, s.Begin + s.Duration, s.Retranslator.Name)).ToList();

            var arr = arr1.Union(arr2).OrderBy(s => s.BeginTime).ToList();

            return new AntennaData(satellite.Name, "TransmitAntenna", arr, begin, begin + duration);
        }

        public static SunData ToSunData(this InitialCondition initialCondition)
        {
            var begin = initialCondition.ModelingTimeBegin;
            var duration = initialCondition.ModelingTimeDuration;

            var pos0 = new GlmSharp.dvec3(
                initialCondition.SunPositionXbegin,
                initialCondition.SunPositionYbegin,
                initialCondition.SunPositionZbegin);

            var pos1 = new GlmSharp.dvec3(
                initialCondition.SunPositionXend,
                initialCondition.SunPositionYend,
                initialCondition.SunPositionZend);

            return new SunData("Sun", pos0, pos1, begin, begin + duration);
        }

        public static EarthData ToJ2000Data(this InitialCondition initialCondition)
        {
            return new EarthData("Earth", DateTime.FromOADate(initialCondition.JulianDateOnTheDay - 2415018.5), initialCondition.EarthAngleBegin);
        }
    }
}
