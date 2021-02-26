using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight.Data.Database
{

    public interface IJ2000Database : IDatabase
    {
        DateTime Epoch { get; set; }

        double AngleDeg { get; set; }
    }

    public class J2000Database : IJ2000Database
    {
        public DateTime Epoch { get; set; }

        public double AngleDeg { get; set; }
    }
}
