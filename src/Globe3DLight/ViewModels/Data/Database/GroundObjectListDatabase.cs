using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Data.Database
{
    public interface IGroundObjectListDatabase : IDatabase
    {
        IDictionary<string, (double lon, double lat)> Positions { get; set; }
     
        double EarthRadius { get; set; }
    }

    public class GroundObjectListDatabase : IGroundObjectListDatabase
    {
        public IDictionary<string, (double lon, double lat)> Positions { get; set; }

        public double EarthRadius { get; set; }
    }
}
