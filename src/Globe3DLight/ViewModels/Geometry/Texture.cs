using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Geometry
{
    public class Texture : ITexture
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}
