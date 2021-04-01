using Globe3DLight.ViewModels.Geometry.Models;

namespace Globe3DLight.Models
{
    public interface IModelLoader
    {
        Model LoadModel(string path);
    }
}
