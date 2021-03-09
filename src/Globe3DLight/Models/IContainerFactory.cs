using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using System;

namespace Globe3DLight.Editor
{
    public interface IContainerFactory
    {
        IProjectContainer GetProject();
        IProjectContainer GetProject(ScenarioData data);
        IProjectContainer GetDemo();
        Task<IProjectContainer> GetFromDatabase();
        Task<IProjectContainer> GetFromJson();
        Task SaveFromDatabaseToJson();
        IScenarioContainer GetScenario(string name, DateTime begin, TimeSpan duration);
        IProjectContainer GetEmptyProject();
    }
}
