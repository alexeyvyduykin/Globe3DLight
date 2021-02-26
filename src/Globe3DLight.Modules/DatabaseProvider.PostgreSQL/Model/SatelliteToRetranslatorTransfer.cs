using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class SatelliteToRetranslatorTransfer
    {
        public int Id { get; set; }
        public double Begin { get; set; }
        public double Duration { get; set; }
        public int SatelliteId { get; set; }
        public int RetranslatorId { get; set; }

        public virtual Retranslator Retranslator { get; set; }
        public virtual Satellite Satellite { get; set; }
    }
}
