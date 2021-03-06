using System.Collections.Generic;
using System.Collections.Immutable;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public interface IEarthRenderModel : IRenderModel
    {
        ImmutableArray<IMesh> Meshes { get; set; }

        // (string pos_x, string neg_z, string neg_x, string pos_z, string pos_y, string neg_y)
        IEnumerable<string> DiffuseKeys { get; set; }
        IEnumerable<string> SpecularKeys { get; set; }
        IEnumerable<string> NormalKeys { get; set; }
        IEnumerable<string> NightKeys { get; set; }
    }
}
