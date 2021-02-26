using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight.Data.Database
{


    public interface ISunDatabase : IDatabase
    {
        dvec3 Position0 { get; set; }

        dvec3 Position1 { get; set; }

        double TimeBegin { get; set; }

        double TimeEnd { get; set; }
    }



    public class SunDatabase : ISunDatabase
    {      
        public dvec3 Position0 { get; set; }

        public dvec3 Position1 { get; set; }

        public double TimeBegin { get; set; }

        public double TimeEnd { get; set; }
    }

}
