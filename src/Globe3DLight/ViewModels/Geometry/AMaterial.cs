using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public class AMaterial : IAMaterial
    {
        private vec4 _ambient;
        private vec4 _diffuse;
        private vec4 _specular;
        private vec4 _emission;
        private float _shininess;

        private ITexture _mapDiffuse;


        public vec4 Ambient
        {
            get => _ambient;
            set => _ambient = value;
        }
        public vec4 Diffuse
        {
            get => _diffuse;
            set => _diffuse = value;
        }
        public vec4 Specular
        {
            get => _specular;
            set => _specular = value;
        }
        public vec4 Emission
        {
            get => _emission;
            set => _emission = value;
        }
        public float Shininess
        {
            get => _shininess;
            set => _shininess = value;
        }

        public ITexture MapDiffuse
        {
            get => _mapDiffuse;
            set => _mapDiffuse = value;
        }
    }

}
