using System.Collections.Generic;
using System;
using GlmSharp;

namespace Globe3DLight.ViewModels.Data
{
    public record ScenarioData(
        string Name,
        double JulianDateOnTheDay,
        double ModelingTimeBegin,
        double ModelingTimeDuration,
        SunData Sun,
        EarthData Earth,
        IList<GroundObjectData> GroundObjects,
        IList<GroundStationData> GroundStations,
        IList<RetranslatorData> RetranslatorPositions,
        IList<SatelliteData> SatellitePositions,
        IList<OrbitData> SatelliteOrbits,
        IList<RotationData> SatelliteRotations,
        IList<SensorData> SatelliteShootings,
        IList<AntennaData> SatelliteTransfers);

    public record AntennaData(string SatelliteName, string Name, IList<TranslationRecord> Translations, double TimeBegin, double TimeEnd);

    public record TranslationRecord(double BeginTime/*local(or satellite) time*/, double EndTime/*local(or satellite) time*/, string Target);

    public record GroundObjectData(string Name, double Lon, double Lat, double EarthRadius); 
    
    public record GroundObjectListData(IDictionary<string, GroundObjectData> GroundObjects);

    public record GroundStationData(string Name, double Lon, double Lat, double Elevation, double EarthRadius);

    public record EarthData(string Name, DateTime Epoch, double AngleDeg);

    public record OrbitData(string SatelliteName, string Name, IList<double[]> Records/*x y z u */);

    public record RetranslatorData(string Name, IList<double[]> Records/*x y z u*/, double TimeBegin, double TimeEnd, double TimeStep);

    public record RotationData(string SatelliteName, string Name, IList<RotationRecord> Rotations, double TimeBegin, double TimeEnd);

    public record RotationRecord(double BeginTime/*local(or satellite) time*/, double EndTime /*local(or satellite) time*/, double Angle /*deg*/);
    
    public record SatelliteData(string Name, IList<double[]> Records/*x y z vx vy vz u */, double TimeBegin, double TimeEnd, double TimeStep);

    public record SensorData(string SatelliteName, string Name, IList<ShootingRecord> Shootings, double TimeBegin, double TimeEnd);

    public record ShootingRecord(double BeginTime/*local(or satellite) time*/, double EndTime/*local(or satellite) time*/, double Gam1/*deg*/, double Gam2/*deg*/, double Range1, double Range2, string TargetName);

    public record SunData(string Name, dvec3 Position0, dvec3 Position1, double TimeBegin, double TimeEnd);    
}
