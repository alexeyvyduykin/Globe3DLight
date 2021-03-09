using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public interface ISatelliteRenderModel : IRenderModel
    {
        IModel Model { get; set; }

        double Scale { get; set; }
    }
}
