using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Data.Database;


namespace Globe3DLight.Data
{
    public interface IJsonDataProvider : IDataProvider
    {
        ISunDatabase CreateSunDatabase(string json);

        IJ2000Database CreateJ2000Database(string json);

        IOrbitDatabase CreateOrbitalDatabase(string json); 
        IRotationDatabase CreateRotationDatabase(string json);

        ISensorDatabase CreateSensorDatabase(string json);

        IRetranslatorDatabase CreateRetranslatorDatabase(string json);

        IAntennaDatabase CreateAntennaDatabase(string json);

    }

}
