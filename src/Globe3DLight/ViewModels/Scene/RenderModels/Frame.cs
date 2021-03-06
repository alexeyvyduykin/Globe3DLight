using System;
using System.Collections.Generic;

namespace Globe3DLight.Scene
{
    public class FrameRenderModel : BaseRenderModel, IFrameRenderModel
    {
        public float Scale { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
