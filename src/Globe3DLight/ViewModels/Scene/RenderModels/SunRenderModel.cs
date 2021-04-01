using System;
using System.Collections.Generic;
using Globe3DLight.Models.Geometry;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class SunRenderModel : BaseRenderModel
    {
        //public IAMesh Billboard { get; set; }
        public Mesh Billboard { get; set; }
        public string SunGlowKey { get; set; }
    }
}
