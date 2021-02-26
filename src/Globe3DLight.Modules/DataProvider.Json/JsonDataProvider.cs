using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Data;
using Globe3DLight.Data.Database;
using GlmSharp;

namespace Globe3DLight.DataProvider.Json
{
    public class JsonDataProvider : ObservableObject, IDataProvider, IJsonDataProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IJsonSerializer _jsonSerializer;

        public JsonDataProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._jsonSerializer = serviceProvider.GetService<IJsonSerializer>();
        }

        public ISunDatabase CreateSunDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<ISunDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }

        }

        public IJ2000Database CreateJ2000Database(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<IJ2000Database>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public IOrbitDatabase CreateOrbitalDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<IOrbitDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public IRotationDatabase CreateRotationDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<IRotationDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public ISensorDatabase CreateSensorDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<ISensorDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public IRetranslatorDatabase CreateRetranslatorDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<IRetranslatorDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public IAntennaDatabase CreateAntennaDatabase(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<IAntennaDatabase>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }


        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

}
