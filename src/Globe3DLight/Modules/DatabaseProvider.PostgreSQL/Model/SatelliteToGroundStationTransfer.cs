using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class SatelliteToGroundStationTransfer
    {
        public int Id { get; set; }
        public double Begin { get; set; }
        public double Duration { get; set; }
        public int SatelliteId { get; set; }
        public int GroundStationId { get; set; }

        public virtual GroundStation GroundStation { get; set; }
        public virtual Satellite Satellite { get; set; }
    }
}
