using GlmSharp;

namespace Globe3DLight.ViewModels.Geometry.Models
{
    public record Material
    {
        public vec4 Ambient { get; init; }

        public vec4 Diffuse { get; init; }

        public vec4 Specular { get; init; }

        public vec4 Emission { get; init; }

        public float Shininess { get; init; }

        public bool HasTextureDiffuse { get; init; }

        public string TextureDiffusePath { get; init; }

        public string TextureDiffuseKey { get; init; }

        //   public ITexture MapDiffuse { get; init; }
    }
}
