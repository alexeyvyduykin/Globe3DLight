using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.ScenarioObjects;
using Globe3DLight.Time;
using Globe3DLight.Data;

namespace Globe3DLight.Containers
{
    public class InvalidateScenarioEventArgs : EventArgs { }

    public delegate void InvalidateScenarioEventHandler(object sender, InvalidateScenarioEventArgs e);

    public interface IScenarioContainer : IBaseContainer//, IGrid
    {
        event InvalidateScenarioEventHandler InvalidateScenarioHandler;

        void InvalidateScenario();

        double Width { get; set; }

        double Height { get; set; }

        bool IsExpanded { get; set; }

        ImmutableArray<ILogical> LogicalTreeNodeRoot { get; set; }

        ILogical CurrentLogicalTreeNode { get; set; }

        ImmutableArray<IScenarioObject> ScenarioObjects { get; set; }

        ImmutableArray<ISatelliteTask> SatelliteTasks { get; set; }

        IScenarioObject CurrentScenarioObject { get; set; }  

        ISceneState SceneState { get; set; }

        ITimePresenter TimePresenter { get; set; }

        IDataUpdater Updater { get; set; }

        void SetCameraTo(ITargetable target);

        void LogicalUpdate();
    }
}
