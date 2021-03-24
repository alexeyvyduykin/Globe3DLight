using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Models.Geometry
{
    public interface IAMaterial
    {
        public vec4 Ambient { get; set; }

        public vec4 Diffuse { get; set; }

        public vec4 Specular { get; set; }

        public vec4 Emission { get; set; }

        public float Shininess { get; set; }

        public ITexture MapDiffuse { get; set; }
    }

}
