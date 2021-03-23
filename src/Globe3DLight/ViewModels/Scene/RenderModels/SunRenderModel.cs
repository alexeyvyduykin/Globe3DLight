using System;
using System.Collections.Generic;
using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public class SunRenderModel : BaseRenderModel
    {
        public IAMesh Billboard { get; set; }

        public string SunGlowKey { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
