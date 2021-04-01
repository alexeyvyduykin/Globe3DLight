using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;
using GlmSharp;

namespace Globe3DLight.ViewModels.Geometry.Models
{
    public record Mesh
    {
        public IList<vec3> Vertices { get; init; }
        
        public IList<vec3> Normals { get; init; }
        
        public IList<vec2> TexCoords { get; init; }
        
        public IList<vec3> Tangents { get; init; }
        
        public IList<ushort> Indices { get; init; }

        // public Material Material { get; init; }

        public int MaterialIndex { get; init; }
    }
}
