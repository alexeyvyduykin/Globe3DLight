using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer
{
    public interface ITextureUnit : ICleanable
    {
        ITexture Texture { get; set; }

        ITextureSampler TextureSampler { get; }
    }

}
