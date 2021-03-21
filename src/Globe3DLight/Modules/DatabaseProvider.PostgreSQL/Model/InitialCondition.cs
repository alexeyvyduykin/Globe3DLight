using System;
using System.Collections.Generic;

#nullable disable

namespace Globe3DLight.DatabaseProvider.PostgreSQL
{
    internal partial class InitialCondition
    {
        public int Id { get; set; }
        public double JulianDateOnTheDay { get; set; }
        public double ModelingTimeBegin { get; set; }
        public double ModelingTimeDuration { get; set; }
        public double EarthAngleBegin { get; set; }
        public double SunPositionXbegin { get; set; }
        public double SunPositionYbegin { get; set; }
        public double SunPositionZbegin { get; set; }
        public double SunPositionXend { get; set; }
        public double SunPositionYend { get; set; }
        public double SunPositionZend { get; set; }
    }
}
