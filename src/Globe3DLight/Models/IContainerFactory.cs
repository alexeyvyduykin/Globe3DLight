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
        ILogicalTreeNode CreateOrbitNode(string name, ILogicalTreeNode parent, OrbitData data);
        ILogicalTreeNode CreateRotationNode(string name, ILogicalTreeNode parent, RotationData data);
        ILogicalTreeNode CreateSunNode(string name, ILogicalTreeNode parent, SunData data);
        ILogicalTreeNode CreateSensorNode(string name, ILogicalTreeNode parent, SensorData data);
        ILogicalTreeNode CreateRetranslatorNode(string name, ILogicalTreeNode parent, RetranslatorData data);
        ILogicalTreeNode CreateAntennaNode(string name, ILogicalTreeNode parent, AntennaData data);
        ILogicalTreeNode CreateGroundStationNode(string name, ILogicalTreeNode parent, GroundStationData data);
        ILogicalTreeNode CreateEarthNode(string name, ILogicalTreeNode parent, J2000Data data);
    }
}
