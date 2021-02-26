using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data.Database
{
    public interface ISensorDatabase : IDatabase
    {
        IList<ShootingRecord1> Shootings { get; set; } 
      //  ContinuousEvents<SensorState> Shootings { get; set; }
        double TimeBegin { get; set; }
        double TimeEnd { get; set; }
    //    double LifeTimeStep { get; set; }
    }

    public class SensorDatabase : ISensorDatabase
    {
        public IList<ShootingRecord1> Shootings { get; set; } = new List<ShootingRecord1>();
      //  public ContinuousEvents<SensorState> Shootings { get; set; }
        public double TimeBegin { get; set; }
        public double TimeEnd { get; set; }
      //  public double LifeTimeStep { get; set; }
    }

    public struct ShootingRecord1
    {
        public double BeginTime; // local(or satellite) time
        public double EndTime; // local(or satellite) time
        public double Gam1; // deg
        public double Gam2; // deg
        public double Range1;
        public double Range2;

        public string TargetName;
    }
}
