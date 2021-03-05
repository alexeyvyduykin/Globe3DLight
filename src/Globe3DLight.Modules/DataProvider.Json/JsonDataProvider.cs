﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Globe3DLight.Data;
using Microsoft.Extensions.Configuration;
using Globe3DLight.Containers;
using Globe3DLight.Editor;

namespace Globe3DLight.DataProvider.Json
{
    public class JsonDataProvider : ObservableObject, IJsonDataProvider
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

        public void Save(ScenarioData data)
        {
            var fileIO = _serviceProvider.GetService<IFileSystem>();

            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];
            var projectFilename = configuration["ProjectFilename"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath);

            var json = _jsonSerializer.Serialize<ScenarioData>(data);
         
            fileIO.WriteUtf8Text(Path.Combine(path, projectFilename), json);
        }

        public async Task<IProjectContainer> LoadProject()
        {         
            var data = await LoadData();

            return _serviceProvider.GetService<IContainerFactory>().GetProject(data);
        }

        public async Task<ScenarioData> LoadData()
        {
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var resourcePath = configuration["ResourcePath"];
            var projectFilename = configuration["ProjectFilename"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), resourcePath);

            return await Task.Run(() => CreateDataFromPath<ScenarioData>(Path.Combine(path, projectFilename)));
        }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

}
