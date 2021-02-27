using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public interface IJsonDataProvider : IDataProvider
    {
        SunData CreateSunDatabase(string json);

        J2000Data CreateJ2000Database(string json);

        OrbitData CreateOrbitalDatabase(string json); 
        RotationData CreateRotationDatabase(string json);

        SensorData CreateSensorDatabase(string json);

        RetranslatorData CreateRetranslatorDatabase(string json);

        AntennaData CreateAntennaDatabase(string json);

    }

}
