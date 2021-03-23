using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class SatelliteOrbitPosition
    {
        public int Id { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }
        public double TrueAnomaly { get; set; }
        public int SatelliteId { get; set; }
        public virtual Satellite Satellite { get; set; }
    }
}
