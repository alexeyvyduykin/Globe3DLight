using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;

namespace Globe3DLight.Data
{
    public interface IDataProvider //: IObservableObject
    {
        Task<ProjectContainer> LoadProject();
        Task<ScenarioData> LoadData();
    }

}
