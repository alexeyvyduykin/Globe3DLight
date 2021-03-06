﻿using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public interface IRetranslatorRenderModel : IRenderModel
    {
        IAMesh Mesh { get; set; }

        double Scale { get; set; }
    }
}
