﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Scene;


namespace Globe3DLight.ScenarioObjects
{
    public interface IGroundObjectList : IScenarioObject, IDrawable
    {
        IGroundObjectListRenderModel RenderModel { get; set; }

        ILogicalTreeNode LogicalTreeNode { get; set; }
    }
}
