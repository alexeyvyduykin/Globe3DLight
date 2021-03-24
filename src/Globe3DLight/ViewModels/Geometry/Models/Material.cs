using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Geometry.Models;

namespace Globe3DLight.ViewModels.Geometry.Models
{
    //public class Material : IMaterial
    //{
    //    private vec4 _ambient;
    //    private vec4 _diffuse;
    //    private vec4 _specular;
    //    private vec4 _emission;
    //    private float _shininess;

    //    private ITexture _mapDiffuse;


    //    public vec4 Ambient 
    //    {
    //        get => _ambient;
    //        set => _ambient = value;
    //    }
    //    public vec4 Diffuse
    //    {
    //        get => _diffuse;
    //        set => _diffuse = value;
    //    }
    //    public vec4 Specular
    //    {
    //        get => _emission;
    //        set => _emission = value;
    //    }
    //    public vec4 Emission
    //    {
    //        get => _ambient;
    //        set => _ambient = value;
    //    }
    //    public float Shininess
    //    {
    //        get => _shininess;
    //        set => _shininess = value;
    //    }

    //    public ITexture MapDiffuse
    //    {
    //        get => _mapDiffuse;
    //        set => _mapDiffuse = value;
    //    }
    //}
    public struct Material : IMaterial
    {
        public vec4 Ambient { get; set; }
        public vec4 Diffuse { get; set; }
        public vec4 Specular { get; set; }
        public vec4 Emission { get; set; }
        public float Shininess { get; set; }

        public bool HasTextureDiffuse { get; set; }
        public string TextureDiffusePath { get; set; }

        public string TextureDiffuseKey { get; set; }
        //   public ITexture MapDiffuse { get; set; }
    }
}
