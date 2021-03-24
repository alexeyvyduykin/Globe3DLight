using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Models.Renderer
{
    public interface IOrbitDrawNode : IDrawNode
    {
        OrbitRenderModel Orbit { get; }
    }
}
