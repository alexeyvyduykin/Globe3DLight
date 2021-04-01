using System.Collections.Generic;

namespace Globe3DLight.ViewModels.Geometry.Models
{
    public record Model
    {
        public IList<Mesh> Meshes { get; init; }

        public IList<Material> Materials { get; init; }
    }
}
