using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;
using GlmSharp;
using Extensions;
using System.Diagnostics;
using System.Drawing;
//using OpenTK.Graphics;
using Globe3DLight.Geometry;

namespace Globe3DLight.Renderer
{
    public interface IDevice
    {
        IVertexArray CreateVertexArray();
        //GameWindow CreateDefaultWindow();

        //GraphicsWindow CreateOpenTKWindow();

        bool AnisotropicFiltering { get; }

        IBuffer CreateVertexBuffer<T>(IList<T> values, BufferUsageHint usageHint) where T : struct;

        IBuffer CreateVertexBuffer(BufferUsageHint usageHint, int sizeInBytes);

        IIndexBuffer CreateIndexBuffer(BufferUsageHint usageHint, int sizeInBytes);

        IIndexBuffer CreateIndexBuffer<T>(IList<T> values, BufferUsageHint usageHint) where T : struct;

        IVertexArray CreateMeshBuffers111(IAMesh mesh, IEnumerable<IShaderVertexAttribute> shaderAttributes, BufferUsageHint usageHint);

        IVertexArray CreateVertexArray_NEW(IAMesh mesh, IEnumerable<IShaderVertexAttribute> shaderAttributes, BufferUsageHint usageHint);


        IShaderProgram CreateShaderProgram();

        IShaderProgram CreateShaderProgram(string vertexShaderSource, string fragmentShaderSource);

        IShaderProgram CreateShaderProgram(string vertexShaderSource, string geometryShaderSource, string fragmentShaderSource);

        ITexture CreateTexture2D(int name/*ITextureDescription description*/);

        ITexture CreateTexture2D111(int name, TextureWrapMode wrapMode);

        //ITexture CreateTexture2D(Bitmap bitmap, TextureFormat format, bool generateMipmaps);

        ITexture CreateTexture2DRectangle(int name/*ITextureDescription description*/);

        //ITexture CreateTexture2DRectangle(Bitmap bitmap, TextureFormat format);

        //ITexture CreateTexture2DFromBitmap(Bitmap bitmap, TextureFormat format, bool generateMipmaps, TextureTarget textureTarget);

        //WritePixelBuffer CreateWritePixelBuffer(PixelBufferHint usageHint, int sizeInBytes);

        ITextureSampler CreateTexture2DSampler(TextureMinFilter minificationFilter, TextureMagFilter magnificationFilter, TextureWrapMode wrapS, TextureWrapMode wrapT);

        ITextureSampler CreateTexture2DSampler(TextureMinFilter minificationFilter, TextureMagFilter magnificationFilter, TextureWrapMode wrapS, TextureWrapMode wrapT, float maximumAnistropy);
  
        int MaximumNumberOfVertexAttributes { get; }

        int NumberOfTextureUnits { get; }

        IVertexBufferAttributes CreateVertexBufferAttributes();

        ITextureSampler CreateTextureSamplerNearestClamp();
        ITextureSampler CreateTextureSamplerLinearClamp();
        ITextureSampler CreateTextureSamplerNearestRepeat();
        ITextureSampler CreateTextureSamplerLinearRepeat();
        ITextureSampler CreateTextureSamplerLinearClampToEdge();
        ITextureSampler CreateTextureSamplerLinearMipmapClampToEdge();
        IVertexBufferAttribute CreateVertexBufferAttribute(IBuffer vertexBuffer, VertexAttribPointerType type, int numberOfComponents);

        ITextureUnits CreateTextureUnits();
    }
}
