﻿#nullable enable
using System;
using System.IO;
using System.Threading.Tasks;
using Globe3DLight.Models;
using Globe3DLight.Models.Data;
using Globe3DLight.Models.Editor;
using Globe3DLight.ViewModels;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using Microsoft.Extensions.Configuration;

namespace Globe3DLight.DataProvider.Json
{
    public class JsonDataProvider : ViewModelBase, IJsonDataProvider
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

        public T? CreateDataFromJson<T>(string json)
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

        public T? CreateDataFromPath<T>(string path)
        {
            var json = _fileSystem.ReadUtf8Text(path);

            return CreateDataFromJson<T>(json);
        }

        public void Save(ScenarioData data)
        {
            var fileIO = _serviceProvider.GetService<IFileSystem>();

            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var dataPath = configuration["DataPath"];
            var projectFilename = configuration["ProjectFilename"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), dataPath);

            var json = _jsonSerializer.Serialize<ScenarioData>(data);

            fileIO.WriteUtf8Text(Path.Combine(path, projectFilename), json);
        }

        public async Task<ProjectContainerViewModel?> LoadProject()
        {
            var data = await LoadData();
            if (data is not null)
            {
                return _serviceProvider.GetService<IContainerFactory>().GetProject(data);
            }

            return default;
        }

        public async Task<ScenarioData?> LoadData()
        {
            var configuration = _serviceProvider.GetService<IConfigurationRoot>();

            var dataPath = configuration["DataPath"];
            var projectFilename = configuration["ProjectFilename"];
            var path = Path.Combine(Directory.GetCurrentDirectory(), dataPath);

            return await Task.Run(() => CreateDataFromPath<ScenarioData>(Path.Combine(path, projectFilename)));
        }
    }
}
