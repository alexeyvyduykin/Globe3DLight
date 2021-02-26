using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Geometry
{
    public interface ITexture
    {
        int Id { get; set; }
        string Type { get; set; }
        string Path { get; set; }
    }
}
