using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Geometry.Models
{
    //public interface IMaterial
    //{ 
    //    public vec4 Ambient { get; set; }
        
    //    public vec4 Diffuse { get; set; }
        
    //    public vec4 Specular { get; set; }
        
    //    public vec4 Emission { get; set; }
        
    //    public float Shininess { get; set; }

    //    public ITexture MapDiffuse { get; set; }
    //}
    public interface IMaterial
    {
        vec4 Ambient { get; set; }
        vec4 Diffuse { get; set; }
        vec4 Specular { get; set; }
        vec4 Emission { get; set; }
        float Shininess { get; set; }
        bool HasTextureDiffuse { get; set; }
        string TextureDiffusePath { get; set; }
        string TextureDiffuseKey { get; set; }
        // ITexture MapDiffuse { get; set; }
    }
}
