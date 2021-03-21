using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Scene;
using Globe3DLight.Entities;
using Globe3DLight.Time;
using Globe3DLight.Data;

namespace Globe3DLight.Containers
{
    public class InvalidateScenarioEventArgs : EventArgs { }

    public delegate void InvalidateScenarioEventHandler(object sender, InvalidateScenarioEventArgs e);

    //public interface IScenarioContainer : IBaseContainer//, IGrid
    //{
    //    event InvalidateScenarioEventHandler InvalidateScenarioHandler;

    //    void InvalidateScenario();

    //    double Width { get; set; }

    //    double Height { get; set; }

    //    bool IsExpanded { get; set; }

    //    ImmutableArray<ILogical> LogicalTreeNodeRoot { get; set; }

    //    ILogical CurrentLogicalTreeNode { get; set; }

    //    ImmutableArray<IEntity> Entities { get; set; }

    //    ImmutableArray<ISatelliteTask> Tasks { get; set; }

    //    ISatelliteTask CurrentTask { get; set; }

    //    IEntity CurrentEntity { get; set; }  

    //    ISceneState SceneState { get; set; }

    //    ITimePresenter TimePresenter { get; set; }

    //    IDataUpdater Updater { get; set; }

    //    void SetCameraTo(ITargetable target);

    //    void LogicalUpdate();
    //}
}
