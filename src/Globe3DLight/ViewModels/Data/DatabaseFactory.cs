using System;
using System.Collections.Generic;
using GlmSharp;
using Globe3DLight.Data.Database;


namespace Globe3DLight.Data
{
    public interface IDatabaseFactory
    {
        IGroundStationDatabase CreateGroundStationDatabase(double lonDeg, double latDeg, double elevation);

        IJ2000Database CreateJ2000Database(DateTime epoch, double angleDeg);

        IOrbitDatabase CreateOrbitDatabase(double begin, double end, double step, List<double[]> records);
        //    IList<(double x, double y, double z, double vx, double vy, double vz, double u)> records);

        IRotationDatabase CreateRotationDatabase(double begin, double end, List<RotationRecord> rotations);

        ISensorDatabase CreateSensorDatabase(double begin, double end, List<ShootingRecord1> shootings);

        ISunDatabase CreateSunDatabase(double begin, double end, dvec3 p0, dvec3 p1);

        IAntennaDatabase CreateAntennaDatabase(double begin, double end, List<TranslationRecord> translations);
    }

    public class DatabaseFactory : IDatabaseFactory
    {


        public IGroundStationDatabase CreateGroundStationDatabase(double lonDeg, double latDeg, double elevation)
        {
            return new GroundStationDatabase()
            {
                Lon = lonDeg,
                Lat = latDeg,
                Elevation = elevation,
                EarthRadius = 6371.0,
            };
        }

        public IJ2000Database CreateJ2000Database(DateTime epoch, double angleDeg)
        {
            return new J2000Database()
            {
                Epoch = epoch,
                AngleDeg = angleDeg,
            };
        }

        public IOrbitDatabase CreateOrbitDatabase(double begin, double end, double step, List<double[]> records)
        //  IList<(double x, double y, double z, double vx, double vy, double vz, double u)> records)
        {
            return new OrbitDatabase()
            {
                TimeBegin = begin,
                TimeEnd = end,
                TimeStep = step,
                Records = records,
            };
        }

        public IRotationDatabase CreateRotationDatabase(double begin, double end, List<RotationRecord> rotations)
        {
            return new RotationDatabase()
            {
                Rotations = rotations,
                TimeBegin = begin,
                TimeEnd = end,
            };
        }

        public ISensorDatabase CreateSensorDatabase(double begin, double end, List<ShootingRecord1> shootings)
        {
            return new SensorDatabase()
            {
                TimeBegin = begin,
                TimeEnd = end,
                Shootings = shootings,
            };
        }

        public ISunDatabase CreateSunDatabase(double begin, double end, dvec3 p0, dvec3 p1)
        {
            return new SunDatabase()
            {
                TimeBegin = begin,
                TimeEnd = end,
                Position0 = p0,
                Position1 = p1,
            };
        }

        public IAntennaDatabase CreateAntennaDatabase(double begin, double end, List<TranslationRecord> translations)
        {
            return new AntennaDatabase()
            {
                TimeBegin = begin,
                TimeEnd = end,
                //TimeStep = step,                       
                Translations = translations,
            };
        }

    }
}
