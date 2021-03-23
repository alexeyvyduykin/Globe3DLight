using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public class EarthRenderModel : BaseRenderModel
    {
        public ImmutableArray<IMesh> Meshes { get; set; }
        
        // (string pos_x, string neg_z, string neg_x, string pos_z, string pos_y, string neg_y)
        public IEnumerable<string> DiffuseKeys { get; set; }

        public IEnumerable<string> SpecularKeys { get; set; }

        public IEnumerable<string> NormalKeys { get; set; }

        public IEnumerable<string> NightKeys { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
