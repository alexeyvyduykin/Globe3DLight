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
        private readonly IFileSystem _fileSystem;

        public JsonDataProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _jsonSerializer = serviceProvider.GetService<IJsonSerializer>();
            _fileSystem = _serviceProvider.GetService<IFileSystem>();
        }

        public T CreateDataFromJson<T>(string json)
        {
            try
            {
                return _jsonSerializer.Deserialize<T>(json);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public T CreateDataFromPath<T>(string path)
        {
            var json = _fileSystem.ReadUtf8Text(path);
            
            return CreateDataFromJson<T>(json);
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

}
