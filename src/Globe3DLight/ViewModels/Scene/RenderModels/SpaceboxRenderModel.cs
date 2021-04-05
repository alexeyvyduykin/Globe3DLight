#nullable disable
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class SpaceboxRenderModel : BaseRenderModel
    {
        public Mesh Mesh { get; set; }
        public string SpaceboxCubemapKey { get; set; }
    }
}
