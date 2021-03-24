using System;
using System.Collections.Generic;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class SpaceboxRenderModel : BaseRenderModel
    {
        public double Scale { get; set; }

        public IAMesh Mesh { get; set; }

        public string SpaceboxCubemapKey { get; set; }
    }
}
