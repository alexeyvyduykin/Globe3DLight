using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public interface IOrbitRenderModel : IRenderModel
    {
        IList<dvec3> Vertices { get; set; }
    }
}
