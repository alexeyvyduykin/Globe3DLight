using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class Satellite
    {
        public Satellite()
        {
            SatelliteOrbitPositions = new HashSet<SatelliteOrbitPosition>();
            SatellitePositions = new HashSet<SatellitePosition>();
            SatelliteRotations = new HashSet<SatelliteRotation>();
            SatelliteShootings = new HashSet<SatelliteShooting>();
            SatelliteToGroundStationTransfers = new HashSet<SatelliteToGroundStationTransfer>();
            SatelliteToRetranslatorTransfers = new HashSet<SatelliteToRetranslatorTransfer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double JulianDateOnTheDay { get; set; }
        public double LifetimeBegin { get; set; }
        public double LifetimeDuration { get; set; }

        public virtual ICollection<SatelliteOrbitPosition> SatelliteOrbitPositions { get; set; }
        public virtual ICollection<SatellitePosition> SatellitePositions { get; set; }
        public virtual ICollection<SatelliteRotation> SatelliteRotations { get; set; }
        public virtual ICollection<SatelliteShooting> SatelliteShootings { get; set; }
        public virtual ICollection<SatelliteToGroundStationTransfer> SatelliteToGroundStationTransfers { get; set; }
        public virtual ICollection<SatelliteToRetranslatorTransfer> SatelliteToRetranslatorTransfers { get; set; }
    }
}
