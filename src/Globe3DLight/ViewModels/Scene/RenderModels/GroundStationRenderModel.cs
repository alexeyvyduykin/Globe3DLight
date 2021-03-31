using System;
using System.Collections.Immutable;
using Globe3DLight.Models.Geometry;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class GroundStationRenderModel : BaseRenderModel
    {
        //public IAMesh Mesh { get; set; }
        public IMesh Mesh { get; set; }
        public double Scale { get; set; }
    }
}
