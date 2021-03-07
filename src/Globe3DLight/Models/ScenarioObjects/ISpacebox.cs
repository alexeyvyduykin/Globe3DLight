using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.ScenarioObjects
{
    public interface ISpacebox : IScenarioObject
    {
        ISpaceboxRenderModel RenderModel { get; set; }

        ILogicalTreeNode LogicalTreeNode { get; set; }
    }
}
