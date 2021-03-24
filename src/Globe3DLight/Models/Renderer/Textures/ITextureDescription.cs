using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;



namespace Globe3DLight.Models.Renderer
{

    public interface ITextureDescription : IEquatable<ITextureDescription>
    {
        int Width { get; }

        int Height { get; }

        TextureFormat TextureFormat { get; }

        bool GenerateMipmaps { get; }

        bool ColorRenderable { get; }

        bool DepthRenderable { get; }

        bool DepthStencilRenderable { get; }

        int ApproximateSizeInBytes { get; }
    }

}
