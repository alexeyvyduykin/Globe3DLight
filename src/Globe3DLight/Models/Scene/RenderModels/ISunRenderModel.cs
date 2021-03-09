using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public interface ISunRenderModel : IRenderModel
    {
        IAMesh Billboard { get; set; }

        string SunGlowKey { get; set; }
    }
}
