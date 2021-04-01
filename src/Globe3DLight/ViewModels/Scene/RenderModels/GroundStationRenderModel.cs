using System;
using System.Collections.Immutable;
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class GroundStationRenderModel : BaseRenderModel
    {   
        public Mesh Mesh { get; set; }
        public double Scale { get; set; }
    }
}
