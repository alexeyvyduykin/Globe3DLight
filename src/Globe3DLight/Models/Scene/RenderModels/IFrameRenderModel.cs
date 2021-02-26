using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public interface IFrameRenderModel : IRenderModel//, IObservableObject
    {
     //   dvec3 Position { get; set; }

     //   dmat4 ModelMatrix { get; set; }

        float Scale { get; set; }
    }
}
