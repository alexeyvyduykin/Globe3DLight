using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public interface IGroundStationRenderModel : IRenderModel
    {
        IAMesh Mesh { get; set; }
        double Scale { get; set; }
    }

}
