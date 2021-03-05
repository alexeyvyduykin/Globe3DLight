using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Data;
using System;

namespace Globe3DLight.Editor
{
    public interface IContainerFactory
    {
        IProjectContainer GetProject();
        IProjectContainer GetDemo();
        Task<IProjectContainer> GetFromDatabase();
        Task<IProjectContainer> GetFromJson();
        Task SaveFromDatabaseToJson();
        IScenarioContainer GetScenario(string name, DateTime begin, TimeSpan duration);
        IProjectContainer GetEmptyProject();
        ILogicalTreeNode CreateSatelliteNode(string name, ILogicalTreeNode parent, SatelliteData data);
        ILogicalTreeNode CreateRotationNode(string name, ILogicalTreeNode parent, RotationData data);
        ILogicalTreeNode CreateSunNode(string name, ILogicalTreeNode parent, SunData data);
        ILogicalTreeNode CreateSensorNode(string name, ILogicalTreeNode parent, SensorData data);
        ILogicalTreeNode CreateRetranslatorNode(string name, ILogicalTreeNode parent, RetranslatorData data);
        ILogicalTreeNode CreateAntennaNode(string name, ILogicalTreeNode parent, AntennaData data);
        ILogicalTreeNode CreateOrbitNode(string name, ILogicalTreeNode parent, OrbitData data);
        ILogicalTreeNode CreateGroundStationNode(string name, ILogicalTreeNode parent, GroundStationData data);
        ILogicalTreeNode CreateEarthNode(string name, ILogicalTreeNode parent, J2000Data data);
    }
}
