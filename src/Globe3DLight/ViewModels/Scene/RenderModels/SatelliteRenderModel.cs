using System;
using System.Collections.Generic;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class SatelliteRenderModel : BaseRenderModel
    {
        public double Scale { get; set; }

        public Model Model { get; set; }
    }
}
