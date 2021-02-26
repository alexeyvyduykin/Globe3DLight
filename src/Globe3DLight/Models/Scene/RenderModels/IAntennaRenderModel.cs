using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public interface IAntennaRenderModel : IRenderModel
    {
        dvec3 TargetPostion { get; set; }

        dvec3 AttachPosition { get; set; }
    }
}
