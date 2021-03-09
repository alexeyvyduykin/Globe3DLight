using System.Collections.Generic;

namespace Globe3DLight.Data
{
    public struct RetranslatorData
    {
        public string Name;

        // x y z u   
        //public IList<(double x, double y, double z, double u)> Records { get; set; }

        public IList<double[]> Records;

        public double TimeBegin;

        public double TimeEnd;

        public double TimeStep;
    }
}
