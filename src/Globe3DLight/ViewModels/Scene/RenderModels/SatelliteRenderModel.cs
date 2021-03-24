using System;
using System.Collections.Generic;
using Globe3DLight.Models.Geometry.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class SatelliteRenderModel : BaseRenderModel
    {
        public double Scale { get; set; }

        public IModel Model { get; set; }
    }
}
