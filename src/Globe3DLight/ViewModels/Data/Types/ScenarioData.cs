using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data
{
    public struct ScenarioData
    {
        public double JulianDateOnTheDay;
        public double ModelingTimeBegin;
        public double ModelingTimeDuration;
        public SunData Sun;
        public J2000Data Earth;
        public IList<GroundObjectData> GroundObjects;
        public IList<GroundStationData> GroundStations;
        public IList<RetranslatorData> RetranslatorPositions;
        public IList<SatelliteData> SatellitePositions;
        public IList<OrbitData> SatelliteOrbits;
        public IList<RotationData> SatelliteRotations;
        public IList<SensorData> SatelliteShootings;
        public IList<AntennaData> SatelliteTransfers;
    }
}
