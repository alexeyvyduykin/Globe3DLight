﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Scene;

namespace Globe3DLight.Renderer
{
    public interface IGroundObjectListDrawNode : IDrawNode
    {
        IGroundObjectListRenderModel GroundObjectList { get; set; }
    }
}
