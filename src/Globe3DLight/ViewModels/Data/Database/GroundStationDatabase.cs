using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data.Database
{
    public interface IGroundStationDatabase : IDatabase
    {
        double Lon { get; set; }

        double Lat { get; set; }

        double Elevation { get; set; }

        double EarthRadius { get; set; }
    }

    public class GroundStationDatabase : IGroundStationDatabase
    {

        public double Lon { get; set; }

        public double Lat { get; set; }

        public double Elevation { get; set; }

        public double EarthRadius { get; set; }
    }
}
