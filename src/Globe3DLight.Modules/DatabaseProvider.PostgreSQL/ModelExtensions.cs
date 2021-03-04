using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Data;

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal static class ModelExtensions
    {
        public static RetranslatorData ToData(this Retranslator retranslator)
        {
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

            return new RetranslatorData()
            {
                Records = records,
                TimeBegin = retranslator.LifetimeBegin,
                TimeEnd = retranslator.LifetimeDuration - retranslator.LifetimeBegin,
                TimeStep = step,
            };
        }

        public static GroundStationData ToData(this GroundStation groundStation)
        {
            return new GroundStationData()
            {
                Lon = groundStation.Lon,
                Lat = groundStation.Lat,
                Elevation = 0.0,
                EarthRadius = 6371.0,
            };
        }

        public static GroundObjectData ToData(this GroundObject groundObject)
        {
            return new GroundObjectData()
            {
                Lon = groundObject.Lon,
                Lat = groundObject.Lat,
                EarthRadius = 6371.0,
            };
        }

        public static OrbitData ToOrbitData(this Satellite satellite)
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

            return new OrbitData()
            {
                TimeBegin = begin,
                TimeEnd = begin + duration,
                TimeStep = step,
                Records = records,
            };
        }

        public static RotationData ToRotationData(this Satellite satellite)
        {
            return new RotationData()
            {
                TimeBegin = satellite.LifetimeBegin,
                TimeEnd = satellite.LifetimeBegin + satellite.LifetimeDuration,
                Rotations = satellite.SatelliteRotations.Select(s =>
                new RotationRecord()
                {
                    BeginTime = s.Begin,
                    EndTime = s.Begin + s.Duration,
                    Angle = s.ToAngle
                }).ToList(),
            };
        }

        public static SensorData ToSensorData(this Satellite satellite)
        {
            return new SensorData()
            {
                TimeBegin = satellite.LifetimeBegin,
                TimeEnd = satellite.LifetimeBegin + satellite.LifetimeDuration,
                Shootings = satellite.SatelliteShootings.Select(s => new ShootingRecord()
                {
                    BeginTime = s.Begin,
                    EndTime = s.Begin + s.Duration,
                    Gam1 = s.Gam1,
                    Gam2 = s.Gam2,
                    Range1 = s.Range1,
                    Range2 = s.Range2,
                    TargetName = s.GroundObject.Name,
                }).ToList(),
            };
        }

        public static AntennaData ToAntennaData(this Satellite satellite)
        {
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

            return new AntennaData()
            {
                TimeBegin = satellite.LifetimeBegin,
                TimeEnd = satellite.LifetimeBegin + satellite.LifetimeDuration,
                Translations = arr1.Union(arr2).ToList(),
            };
        }

        public static SunData ToSunData(this InitialCondition initialCondition)
        {
            var pos0 = new GlmSharp.dvec3(
                initialCondition.SunPositionXbegin,
                initialCondition.SunPositionYbegin,
                initialCondition.SunPositionZbegin);

            var pos1 = new GlmSharp.dvec3(
                initialCondition.SunPositionXend,
                initialCondition.SunPositionYend,
                initialCondition.SunPositionZend);

            var begin = initialCondition.ModelingTimeBegin;
            var duration = initialCondition.ModelingTimeDuration;

            return new SunData()
            {
                TimeBegin = begin,
                TimeEnd = begin + duration,
                Position0 = pos0,
                Position1 = pos1,
            };
        }

        public static J2000Data ToJ2000Data(this InitialCondition initialCondition)
        {
            return new J2000Data()
            {
                Epoch = DateTime.FromOADate(initialCondition.JulianDateOnTheDay - 2415018.5),
                AngleDeg = initialCondition.EarthAngleBegin,
            };
        }
    }
}
