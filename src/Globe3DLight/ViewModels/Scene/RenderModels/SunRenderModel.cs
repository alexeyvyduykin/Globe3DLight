#nullable disable
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class SunRenderModel : BaseRenderModel
    {
        public Mesh Billboard { get; set; }

        public string SunGlowKey { get; set; }
    }
}
