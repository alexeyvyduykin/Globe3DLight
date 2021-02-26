using System.Threading.Tasks;
using Globe3DLight.Containers;

namespace Globe3DLight.Editor
{
    public interface IContainerFactory
    {
        IProjectContainer GetProject();
        IProjectContainer GetDemo();
        Task<IProjectContainer> GetFromDatabase();
        IScenarioContainer GetScenario(string name);
        IProjectContainer GetEmptyProject();
    }
}
