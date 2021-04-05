#nullable disable
using System.Collections.Generic;
using GlmSharp;

namespace Globe3DLight.ViewModels.Geometry
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
