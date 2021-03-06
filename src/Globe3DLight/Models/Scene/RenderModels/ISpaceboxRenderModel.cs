using Globe3DLight.Geometry;

namespace Globe3DLight.Scene
{
    public interface ISpaceboxRenderModel : IRenderModel
    {
        double Scale { get; set; }
        IAMesh Mesh { get; set; }
        string SpaceboxCubemapKey { get; set; }
    }
}
