using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using GlmSharp;

namespace Globe3DLight.Geometry.Models
{
    public interface IMesh
    {
        IList<vec3> Vertices { get; set; }
        IList<vec3> Normals { get; set; }
        IList<vec2> TexCoords { get; set; }
        IList<vec3> Tangents { get; set; }
        IList<ushort> Indices { get; set; }
        //  IMaterial Material { get;set; }
        int MaterialIndex { get; set; }
    }

    public class Mesh : IMesh
    {
        public IList<vec3> Vertices { get; set; }
        public IList<vec3> Normals { get; set; }
        public IList<vec2> TexCoords { get; set; }
        public IList<vec3> Tangents { get; set; }
        public IList<ushort> Indices { get; set; }
        // public IMaterial Material { get; set; }
        public int MaterialIndex { get; set; }
    }
}
