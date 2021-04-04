#nullable disable
using System;
using Globe3DLight.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Globe3DLight.Serializer.Newtonsoft
{
    public sealed class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly JsonSerializerSettings _settings;

        public NewtonsoftJsonSerializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new ProjectContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters =
                {
                    new KeyValuePairConverter(),
                }
            };
        }

        public string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public string SerializerWithSettings<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public T DeserializeWithSettings<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    }
}
