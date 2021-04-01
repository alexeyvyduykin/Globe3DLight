using Globe3DLight.Models.Geometry.Models;

namespace Globe3DLight.Models
{
    public interface IModelLoader
    {
        IModel LoadModel(string path);
    }
}
