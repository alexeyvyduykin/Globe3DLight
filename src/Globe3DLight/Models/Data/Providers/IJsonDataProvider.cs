using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Data
{
    public interface IJsonDataProvider : IDataProvider
    {
        T CreateDataFromJson<T>(string json);

        T CreateDataFromPath<T>(string path);
    }

}
