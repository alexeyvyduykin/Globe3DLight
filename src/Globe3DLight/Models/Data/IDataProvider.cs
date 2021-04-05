#nullable enable
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;

namespace Globe3DLight.Models.Data
{
    public interface IDataProvider
    {
        Task<ProjectContainerViewModel?> LoadProject();
        Task<ScenarioData?> LoadData();
    }
}
