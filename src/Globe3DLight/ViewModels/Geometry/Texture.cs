using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public class Texture : ITexture
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}
