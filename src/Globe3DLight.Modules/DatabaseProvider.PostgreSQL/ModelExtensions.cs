﻿using System;
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

            return new RetranslatorData()
            {
                Name = retranslator.Name,
                Records = records,
                TimeBegin = begin,
                TimeEnd = begin + duration,
                TimeStep = step,
            };
        }

        public static GroundStationData ToData(this GroundStation groundStation)
        {
            return new GroundStationData()
            {
                Name = groundStation.Name,
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
                Name = groundObject.Name,
                Lon = groundObject.Lon,
                Lat = groundObject.Lat,
                EarthRadius = 6371.0,
            };
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

            return new SatelliteData()
            {
                Name = satellite.Name,
                TimeBegin = begin,
                TimeEnd = begin + duration,
                TimeStep = step,
                Records = records,
            };
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

            return new OrbitData()
            {
                SatelliteName = satellite.Name,
                Name = "Orbit",
                Records = records,
            };
        }
        
        public static RotationData ToRotationData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;
         
            var rots = satellite.SatelliteRotations.OrderBy(s => s.Begin).Select(s =>
new RotationRecord()
{
    BeginTime = s.Begin,
    EndTime = s.Begin + s.Duration,
    Angle = s.ToAngle
}).ToList();

            return new RotationData()
            {
                SatelliteName = satellite.Name,
                Name = "Rotation",
                TimeBegin = begin,
                TimeEnd = begin + duration,
                Rotations = rots,
            };
        }

        public static SensorData ToSensorData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var shoots = satellite.SatelliteShootings.OrderBy(s => s.Begin).Select(s => new ShootingRecord()
            {
                BeginTime = s.Begin,
                EndTime = s.Begin + s.Duration,
                Gam1 = s.Gam1,
                Gam2 = s.Gam2,
                Range1 = s.Range1,
                Range2 = s.Range2,
                TargetName = s.GroundObject.Name,
            }).ToList();

            return new SensorData()
            {
                SatelliteName = satellite.Name,
                Name = "SAR",
                TimeBegin = begin,
                TimeEnd = begin + duration,
                Shootings = shoots,
            };
        }

        public static AntennaData ToAntennaData(this Satellite satellite)
        {
            var begin = satellite.LifetimeBegin;
            var duration = satellite.LifetimeDuration;

            var arr1 = satellite.SatelliteToGroundStationTransfers.Select(s => new TranslationRecord()
            {
                BeginTime = s.Begin,
                EndTime = s.Begin + s.Duration,
                Target = s.GroundStation.Name,
            }).ToList();

            var arr2 = satellite.SatelliteToRetranslatorTransfers.Select(s => new TranslationRecord()
            {
                BeginTime = s.Begin,
                EndTime = s.Begin + s.Duration,
                Target = s.Retranslator.Name,
            }).ToList();

            var arr = arr1.Union(arr2).OrderBy(s => s.BeginTime).ToList();

            return new AntennaData()
            {
                SatelliteName = satellite.Name,
                Name = "TransmitAntenna",
                TimeBegin = begin,
                TimeEnd = begin + duration,
                Translations = arr,
            };
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

            return new SunData()
            {
                Name = "Sun",
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
                Name = "Earth",
                Epoch = DateTime.FromOADate(initialCondition.JulianDateOnTheDay - 2415018.5),
                AngleDeg = initialCondition.EarthAngleBegin,
            };
        }
    }
}
