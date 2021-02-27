using System.Collections.Generic;

namespace Globe3DLight.Data
{
    public struct GroundObjectListData
    {
        public IDictionary<string, (double lon, double lat)> Positions;

        public double EarthRadius;
    }
}
