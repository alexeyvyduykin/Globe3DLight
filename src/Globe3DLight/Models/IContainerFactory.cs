using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using System;

namespace Globe3DLight.Editor
{
    public interface IContainerFactory
    {
        ProjectContainer GetProject();
        ProjectContainer GetProject(ScenarioData data);
        ProjectContainer GetDemo();
        Task<ProjectContainer> GetFromDatabase();
        Task<ProjectContainer> GetFromJson();
        Task SaveFromDatabaseToJson();
        ScenarioContainer GetScenario(string name, DateTime begin, TimeSpan duration);
        ProjectContainer GetEmptyProject();
    }
}
