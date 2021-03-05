using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;

namespace Globe3DLight.Scene
{
    public interface IOrbitRenderModel : IRenderModel
    {
        IList<dvec3> Vertices { get; set; }
    }
}
