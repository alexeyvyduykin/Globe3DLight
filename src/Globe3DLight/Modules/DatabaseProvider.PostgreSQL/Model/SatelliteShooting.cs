using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class SatelliteShooting
    {
        public int Id { get; set; }
        public double Begin { get; set; }
        public double Duration { get; set; }
        public double Gam1 { get; set; }
        public double Gam2 { get; set; }
        public double Range1 { get; set; }
        public double Range2 { get; set; }
        public int SatelliteId { get; set; }
        public int GroundObjectId { get; set; }

        public virtual GroundObject GroundObject { get; set; }
        public virtual Satellite Satellite { get; set; }
    }
}
