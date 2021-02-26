using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class GroundStation
    {
        public GroundStation()
        {
            SatelliteToGroundStationTransfers = new HashSet<SatelliteToGroundStationTransfer>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }

        public virtual ICollection<SatelliteToGroundStationTransfer> SatelliteToGroundStationTransfers { get; set; }
    }
}
