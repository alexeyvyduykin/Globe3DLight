using System;
using System.Collections.Generic;
using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public class GroundStationRenderModel : BaseRenderModel
    {
        public IAMesh Mesh { get; set; }

        public double Scale { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
