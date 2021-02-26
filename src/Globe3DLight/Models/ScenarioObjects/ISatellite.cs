using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;

namespace Globe3DLight.ScenarioObjects
{
    public interface ISatellite : IScenarioObject, ITargetable
    {
        ISatelliteRenderModel RenderModel { get; set; }
        IFrameRenderModel FrameRenderModel { get; set; }

        bool IsVisible { get; set; }

      //  IDataProvider OrbitalProvider { get; set; }

       // IDataProvider RotationProvider { get; set; }

        ILogicalTreeNode LogicalTreeNode { get; set; }
    }
}
