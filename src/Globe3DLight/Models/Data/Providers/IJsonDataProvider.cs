#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.Models.Data
{
    public interface IJsonDataProvider : IDataProvider
    {
        T? CreateDataFromJson<T>(string json);

        T? CreateDataFromPath<T>(string path);

        void Save(ScenarioData data);
    }

}
