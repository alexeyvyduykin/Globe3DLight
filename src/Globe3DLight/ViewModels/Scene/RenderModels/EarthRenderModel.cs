using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Geometry;
using System.Collections.Generic;
using Globe3DLight.Image;
using System.Linq;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public interface IEarthRenderModel : IRenderModel
    {
        ImmutableArray<IMesh> Meshes { get; set; }

       // IImageLoader ImageLoader { get; set; }

        // (string pos_x, string neg_z, string neg_x, string pos_z, string pos_y, string neg_y)
        IEnumerable<string> DiffuseKeys { get; set; }
        IEnumerable<string> SpecularKeys { get; set; }
        IEnumerable<string> NormalKeys { get; set; }
        IEnumerable<string> NightKeys { get; set; }
    }

    public class EarthRenderModel : BaseRenderModel, IEarthRenderModel
    {      
       // public IImageLoader ImageLoader { get; set; }
        public ImmutableArray<IMesh> Meshes { get; set; }

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
