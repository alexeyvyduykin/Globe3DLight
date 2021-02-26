using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight.Geometry
{
    public struct Vertex
    {
        public vec3 Position { get; set; }

        public vec3 Normal { get; set; }

        public vec2 TexCoords { get; set; }
    }
}
