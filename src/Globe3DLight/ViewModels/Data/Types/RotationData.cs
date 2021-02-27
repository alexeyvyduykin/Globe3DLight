using System.Collections.Generic;

namespace Globe3DLight.Data
{
    public struct RotationData
    {
        //  public ContinuousEvents<RotationState> RotationEvents { get; set; }

        public List<RotationRecord> Rotations;

        public double TimeBegin;

        public double TimeEnd;
    }


    public struct RotationRecord
    {
        public double BeginTime { get; set; } // local(or satellite) time
        public double EndTime { get; set; } // local(or satellite) time
        public double Angle { get; set; } // deg
    }
}
