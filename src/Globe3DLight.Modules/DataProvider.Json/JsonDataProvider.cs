using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Data;
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

        public SunData CreateSunData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<SunData>(json);
            }
            catch (Exception)
            {
                return default;
            }

        }

        public J2000Data CreateJ2000Data(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<J2000Data>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public OrbitData CreateOrbitalData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<OrbitData>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public RotationData CreateRotationData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<RotationData>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public SensorData CreateSensorData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<SensorData>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public RetranslatorData CreateRetranslatorData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<RetranslatorData>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public AntennaData CreateAntennaData(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<AntennaData>(json);
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
