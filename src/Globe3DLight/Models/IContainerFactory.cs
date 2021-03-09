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
        ILogicalTreeNode CreateSatelliteNode(ILogicalTreeNode parent, SatelliteData data);
        ILogicalTreeNode CreateRotationNode(ILogicalTreeNode parent, RotationData data);
        ILogicalTreeNode CreateSunNode(ILogicalTreeNode parent, SunData data);
        ILogicalTreeNode CreateSensorNode(ILogicalTreeNode parent, SensorData data);
        ILogicalTreeNode CreateRetranslatorNode(ILogicalTreeNode parent, RetranslatorData data);
        ILogicalTreeNode CreateAntennaNode(ILogicalTreeNode parent, AntennaData data);
        ILogicalTreeNode CreateOrbitNode(ILogicalTreeNode parent, OrbitData data);
        ILogicalTreeNode CreateGroundStationNode(ILogicalTreeNode parent, GroundStationData data);
        ILogicalTreeNode CreateEarthNode(ILogicalTreeNode parent, J2000Data data);
    }
}
