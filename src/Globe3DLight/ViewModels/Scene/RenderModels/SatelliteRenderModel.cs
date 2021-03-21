using System;
using System.Collections.Generic;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public class SatelliteRenderModel : BaseRenderModel
    {
        public double Scale { get; set; }

        public IModel Model { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
