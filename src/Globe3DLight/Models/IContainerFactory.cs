#nullable enable
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Data;
using System;

namespace Globe3DLight.Models.Editor
{
    public interface IContainerFactory
    {
        ProjectContainerViewModel GetProject();

        ProjectContainerViewModel? GetProject(ScenarioData data);

        Task<ProjectContainerViewModel?> GetFromDatabase();

        Task<ProjectContainerViewModel?> GetFromJson();

        Task SaveFromDatabaseToJson();

        ProjectContainerViewModel GetEmptyProject();
    }
}
