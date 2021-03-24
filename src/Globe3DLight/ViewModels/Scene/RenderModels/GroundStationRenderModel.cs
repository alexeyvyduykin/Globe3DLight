using System;
using System.Collections.Generic;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class GroundStationRenderModel : BaseRenderModel
    {
        public IAMesh Mesh { get; set; }

        public double Scale { get; set; }
    }
}
