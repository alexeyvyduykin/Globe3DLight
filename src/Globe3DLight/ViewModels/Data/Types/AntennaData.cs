using System.Collections.Generic;


namespace Globe3DLight.Data
{
    public struct AntennaData
    {
        public IList<TranslationRecord> Translations;

        public double TimeBegin;

        public double TimeEnd;
    }

    public struct TranslationRecord
    {
        public double BeginTime; // local(or satellite) time
        public double EndTime; // local(or satellite) time
        public string Target;
    }

}
