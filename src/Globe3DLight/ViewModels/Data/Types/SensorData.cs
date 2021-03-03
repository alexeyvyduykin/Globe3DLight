using System.Collections.Generic;

namespace Globe3DLight.Data
{
    public struct SensorData
    {
        public IList<ShootingRecord> Shootings;

        public double TimeBegin;
        public double TimeEnd;
    }

    public struct ShootingRecord
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
