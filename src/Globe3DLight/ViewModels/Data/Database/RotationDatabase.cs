using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data.Database
{
    public interface IRotationDatabase : IDatabase
    {
      //  ContinuousEvents<RotationState> RotationEvents { get; set; }

        List<RotationRecord> Rotations { get; set; }

        double TimeBegin { get; set; }

        double TimeEnd { get; set; }
    }


    public class RotationDatabase : IRotationDatabase
    {
        //  public ContinuousEvents<RotationState> RotationEvents { get; set; }

        public List<RotationRecord> Rotations { get; set; } = new List<RotationRecord>();

        public double TimeBegin { get; set; }

        public double TimeEnd { get; set; }
    }


    public struct RotationRecord
    {
        public double BeginTime { get; set; } // local(or satellite) time
        public double EndTime { get; set; } // local(or satellite) time
        public double Angle { get; set; } // deg
    }
}
