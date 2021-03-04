using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Data;

namespace Globe3DLight.Editor
{
    public interface IContainerFactory
    {
        IProjectContainer GetProject();
        IProjectContainer GetDemo();
        Task<IProjectContainer> GetFromDatabase();
        Task<IProjectContainer> GetFromJson();
        Task SaveFromDatabaseToJson();
        IScenarioContainer GetScenario(string name);
        IProjectContainer GetEmptyProject();
    }
}
