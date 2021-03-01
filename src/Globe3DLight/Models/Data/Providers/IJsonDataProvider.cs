using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public interface IJsonDataProvider : IDataProvider
    {
        SunData CreateSunData(string json);

        J2000Data CreateJ2000Data(string json);

        OrbitData CreateOrbitalData(string json); 
        RotationData CreateRotationData(string json);

        SensorData CreateSensorData(string json);

        RetranslatorData CreateRetranslatorData(string json);

        AntennaData CreateAntennaData(string json);

    }

}
