﻿using System;
using Globe3DLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Globe3DLight.Serializer.Newtonsoft
{
    /// <summary>
    /// Json serializer.
    /// </summary>
    public sealed class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Specifies the settings on a <see cref="JsonSerializer"/> object.
        /// </summary>
        private static readonly JsonSerializerSettings Settings;

        /// <summary>
        /// Initializes static data.
        /// </summary>
        static NewtonsoftJsonSerializer()
        {
            Settings = new JsonSerializerSettings()
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
         //           new ArgbColorJsonConverter(),
         //           new FontStyleConverter(),
         //           new ShapeStateConverter()
                }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonSerializer"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public NewtonsoftJsonSerializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        string IJsonSerializer.Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }

        /// <inheritdoc/>
        T IJsonSerializer.Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }

        public string SerializerWithSettings<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }

        public T DeserializeWithSettings<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}
