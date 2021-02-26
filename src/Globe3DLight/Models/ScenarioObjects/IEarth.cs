using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;

namespace Globe3DLight.ScenarioObjects
{
    public interface IEarth : IScenarioObject, ITargetable
    { 
        IEarthRenderModel RenderModel { get; set; }

        IFrameRenderModel FrameRenderModel { get; set; }


        bool IsVisible { get; set; }
      
    //    IDataProvider Provider { get; set; }

        ILogicalTreeNode LogicalTreeNode { get; set; }
    }
}
