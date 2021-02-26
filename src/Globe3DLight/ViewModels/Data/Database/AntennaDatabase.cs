using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight.Data.Database
{
    public interface IAntennaDatabase : IDatabase
    {
        IList<TranslationRecord> Translations { get; set; }

        double TimeBegin { get; set; }

        double TimeEnd { get; set; }
    }

    public class AntennaDatabase : IAntennaDatabase
    {
        public IList<TranslationRecord> Translations { get; set; } = new List<TranslationRecord>();

        public double TimeBegin { get; set; }

        public double TimeEnd { get; set; }

    }

    public struct TranslationRecord
    {
        public double BeginTime; // local(or satellite) time
        public double EndTime; // local(or satellite) time
        public string Target; 
    }

}
