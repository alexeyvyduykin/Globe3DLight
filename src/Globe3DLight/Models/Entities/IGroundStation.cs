using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;

namespace Globe3DLight.Entities
{
    public interface IGroundStation : /*IEntity,*/ IDrawable, ITargetable
    {
        IGroundStationRenderModel RenderModel { get; set; }

        IFrameRenderModel FrameRenderModel { get; set; }

        Logical Logical { get; set; }
    }
}
