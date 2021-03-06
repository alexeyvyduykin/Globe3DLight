using System;
using System.Collections.Generic;
using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public class SpaceboxRenderModel : BaseRenderModel, ISpaceboxRenderModel
    {
        public double Scale { get; set; }

        public IAMesh Mesh { get; set; }

        public string SpaceboxCubemapKey { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
