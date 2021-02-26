using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;


namespace Globe3DLight.Renderer
{

    public interface ITextureSampler : IDisposable
    {
        void Bind(int textureUnitIndex);

        void UnBind(int textureUnitIndex);

        TextureMinFilter MinificationFilter { get; }

        TextureMagFilter MagnificationFilter { get; }

        TextureWrapMode WrapS { get; }

        TextureWrapMode WrapT { get; }

        float MaximumAnisotropic { get; }
    }

}
