using System.Collections.Generic;

namespace Globe3DLight.Data
{
    public struct SatelliteData
    {
        public string Name;

        // x y z vx vy vz u   
        //public IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }

        public IList<double[]> Records;

        public double TimeBegin;

        public double TimeEnd;

        public double TimeStep;

    }
}
