using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data.Database
{

    public interface IOrbitDatabase : IDatabase
    {
        // x y z vx vy vz u   
        //IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }
        IList<double[]> Records { get; set; }
        double TimeBegin { get; set; }

        double TimeEnd { get; set; }

        double TimeStep { get; set; }
    }
    
    public class OrbitDatabase : IOrbitDatabase
    {
        // x y z vx vy vz u   
        //public IList<(double x, double y, double z, double vx, double vy, double vz, double u)> Records { get; set; }

        public IList<double[]> Records { get; set; } = new List<double[]>();
        public double TimeBegin { get; set; }

        public double TimeEnd { get; set; }

        public double TimeStep { get; set; }

    }
}
