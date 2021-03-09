using Globe3DLight.Data;

namespace Globe3DLight.Scene
{
    public interface ISensorRenderModel : IRenderModel
    {
        IScan Scan { get; set; }
        IShoot Shoot { get; set; }
    }
}
