using System;
using System.Collections.Generic;
using Globe3DLight.Models.Geometry;
using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.ViewModels.Scene
{
    public class RetranslatorRenderModel : BaseRenderModel
    {
        public Model Model { get; set; }

        public double Scale { get; set; }
    }
}
