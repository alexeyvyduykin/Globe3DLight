using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.ViewModels.Scene;

namespace Globe3DLight.Models.Renderer
{
    public interface IGroundStationDrawNode : IDrawNode
    {
        GroundStationRenderModel GroundStation { get; set; }
    }
}
