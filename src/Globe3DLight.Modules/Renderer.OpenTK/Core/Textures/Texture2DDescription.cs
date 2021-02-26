using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal enum TextureFormat
    {
        RedGreenBlue8,
        RedGreenBlue16,
        RedGreenBlueAlpha8,
        RedGreenBlue10A2,
        RedGreenBlueAlpha16,
        Depth16,
        Depth24,
        Red8,
        Red16,
        RedGreen8,
        RedGreen16,
        Red16f,
        Red32f,
        RedGreen16f,
        RedGreen32f,
        Red8i,
        Red8ui,
        Red16i,
        Red16ui,
        Red32i,
        Red32ui,
        RedGreen8i,
        RedGreen8ui,
        RedGreen16i,
        RedGreen16ui,
        RedGreen32i,
        RedGreen32ui,
        RedGreenBlueAlpha32f,
        RedGreenBlue32f,
        RedGreenBlueAlpha16f,
        RedGreenBlue16f,
        Depth24Stencil8,
        Red11fGreen11fBlue10f,
        RedGreenBlue9E5,
        SRedGreenBlue8,
        SRedGreenBlue8Alpha8,
        Depth32f,
        Depth32fStencil8,
        RedGreenBlueAlpha32ui,
        RedGreenBlue32ui,
        RedGreenBlueAlpha16ui,
        RedGreenBlue16ui,
        RedGreenBlueAlpha8ui,
        RedGreenBlue8ui,
        RedGreenBlueAlpha32i,
        RedGreenBlue32i,
        RedGreenBlueAlpha16i,
        RedGreenBlue16i,
        RedGreenBlueAlpha8i,
        RedGreenBlue8i
    }

    //public struct Texture2DDescription : IEquatable<Texture2DDescription>
    //{
    //    public Texture2DDescription(int width, int height, TextureFormat format)
    //        : this(width, height, format, false)
    //    {         
    //    }

    //    public Texture2DDescription(
    //        int width,
    //        int height,
    //        TextureFormat format,
    //        bool generateMipmaps)
    //    {
    //        this.width = width;
    //        this.height = height;
    //        this.format = format;
    //        this.generateMipmaps = generateMipmaps;
    //    }

    //    public int Width
    //    {
    //        get { return width; }
    //    }

    //    public int Height
    //    {
    //        get { return height; }
    //    }

    //    public TextureFormat TextureFormat
    //    {
    //        get { return format; }
    //    }

    //    public PixelInternalFormat TextureToPixelInternalFormat()
    //    {
    //        switch (format)
    //        {
    //            case TextureFormat.RedGreenBlue8:
    //                return PixelInternalFormat.Rgb8;
    //            case TextureFormat.RedGreenBlue16:
    //                return PixelInternalFormat.Rgb16;
    //            case TextureFormat.RedGreenBlueAlpha8:
    //                return PixelInternalFormat.Rgba8;
    //            case TextureFormat.RedGreenBlue10A2:
    //                return PixelInternalFormat.Rgb10A2;
    //            case TextureFormat.RedGreenBlueAlpha16:
    //                return PixelInternalFormat.Rgba16;
    //            case TextureFormat.Depth16:
    //                return PixelInternalFormat.DepthComponent16;
    //            case TextureFormat.Depth24:
    //                return PixelInternalFormat.DepthComponent24;
    //            case TextureFormat.Red8:
    //                return PixelInternalFormat.R8;
    //            case TextureFormat.Red16:
    //                return PixelInternalFormat.R16;
    //            case TextureFormat.RedGreen8:
    //                return PixelInternalFormat.Rg8;
    //            case TextureFormat.RedGreen16:
    //                return PixelInternalFormat.Rg16;
    //            case TextureFormat.Red16f:
    //                return PixelInternalFormat.R16f;
    //            case TextureFormat.Red32f:
    //                return PixelInternalFormat.R32f;
    //            case TextureFormat.RedGreen16f:
    //                return PixelInternalFormat.Rg16f;
    //            case TextureFormat.RedGreen32f:
    //                return PixelInternalFormat.Rg32f;
    //            case TextureFormat.Red8i:
    //                return PixelInternalFormat.R8i;
    //            case TextureFormat.Red8ui:
    //                return PixelInternalFormat.R8ui;
    //            case TextureFormat.Red16i:
    //                return PixelInternalFormat.R16i;
    //            case TextureFormat.Red16ui:
    //                return PixelInternalFormat.R16ui;
    //            case TextureFormat.Red32i:
    //                return PixelInternalFormat.R32i;
    //            case TextureFormat.Red32ui:
    //                return PixelInternalFormat.R32ui;
    //            case TextureFormat.RedGreen8i:
    //                return PixelInternalFormat.Rg8i;
    //            case TextureFormat.RedGreen8ui:
    //                return PixelInternalFormat.Rg8ui;
    //            case TextureFormat.RedGreen16i:
    //                return PixelInternalFormat.Rg16i;
    //            case TextureFormat.RedGreen16ui:
    //                return PixelInternalFormat.Rg16ui;
    //            case TextureFormat.RedGreen32i:
    //                return PixelInternalFormat.Rg32i;
    //            case TextureFormat.RedGreen32ui:
    //                return PixelInternalFormat.Rg32ui;
    //            case TextureFormat.RedGreenBlueAlpha32f:
    //                return PixelInternalFormat.Rgba32f;
    //            case TextureFormat.RedGreenBlue32f:
    //                return PixelInternalFormat.Rgb32f;
    //            case TextureFormat.RedGreenBlueAlpha16f:
    //                return PixelInternalFormat.Rgba16f;
    //            case TextureFormat.RedGreenBlue16f:
    //                return PixelInternalFormat.Rgb16f;
    //            case TextureFormat.Depth24Stencil8:
    //                return PixelInternalFormat.Depth24Stencil8;
    //            case TextureFormat.Red11fGreen11fBlue10f:
    //                return PixelInternalFormat.R11fG11fB10f;
    //            case TextureFormat.RedGreenBlue9E5:
    //                return PixelInternalFormat.Rgb9E5;
    //            case TextureFormat.SRedGreenBlue8:
    //                return PixelInternalFormat.Srgb8;
    //            case TextureFormat.SRedGreenBlue8Alpha8:
    //                return PixelInternalFormat.Srgb8Alpha8;
    //            case TextureFormat.Depth32f:
    //                return PixelInternalFormat.DepthComponent32f;
    //            case TextureFormat.Depth32fStencil8:
    //                return PixelInternalFormat.Depth32fStencil8;
    //            case TextureFormat.RedGreenBlueAlpha32ui:
    //                return PixelInternalFormat.Rgba32ui;
    //            case TextureFormat.RedGreenBlue32ui:
    //                return PixelInternalFormat.Rgb32ui;
    //            case TextureFormat.RedGreenBlueAlpha16ui:
    //                return PixelInternalFormat.Rgba16ui;
    //            case TextureFormat.RedGreenBlue16ui:
    //                return PixelInternalFormat.Rgb16ui;
    //            case TextureFormat.RedGreenBlueAlpha8ui:
    //                return PixelInternalFormat.Rgba8ui;
    //            case TextureFormat.RedGreenBlue8ui:
    //                return PixelInternalFormat.Rgb8ui;
    //            case TextureFormat.RedGreenBlueAlpha32i:
    //                return PixelInternalFormat.Rgba32i;
    //            case TextureFormat.RedGreenBlue32i:
    //                return PixelInternalFormat.Rgb32i;
    //            case TextureFormat.RedGreenBlueAlpha16i:
    //                return PixelInternalFormat.Rgba16i;
    //            case TextureFormat.RedGreenBlue16i:
    //                return PixelInternalFormat.Rgb16i;
    //            case TextureFormat.RedGreenBlueAlpha8i:
    //                return PixelInternalFormat.Rgba8i;
    //            case TextureFormat.RedGreenBlue8i:
    //                return PixelInternalFormat.Rgb8i;
    //        }

    //        throw new ArgumentException("format");
    //    }

    //    public PixelFormat TextureToPixelFormat()
    //    {
    //        // TODO:  Not tested exhaustively
    //        switch (format)
    //        {
    //            case TextureFormat.RedGreenBlue8:
    //            case TextureFormat.RedGreenBlue16:
    //                return PixelFormat.Rgb;
    //            case TextureFormat.RedGreenBlueAlpha8:
    //            case TextureFormat.RedGreenBlue10A2:
    //            case TextureFormat.RedGreenBlueAlpha16:
    //                return PixelFormat.Rgba;
    //            case TextureFormat.Depth16:
    //            case TextureFormat.Depth24:
    //                return PixelFormat.DepthComponent;
    //            case TextureFormat.Red8:
    //            case TextureFormat.Red16:
    //                return PixelFormat.Red;
    //            case TextureFormat.RedGreen8:
    //            case TextureFormat.RedGreen16:
    //                return PixelFormat.Rg;
    //            case TextureFormat.Red16f:
    //            case TextureFormat.Red32f:
    //                return PixelFormat.Red;
    //            case TextureFormat.RedGreen16f:
    //            case TextureFormat.RedGreen32f:
    //                return PixelFormat.Rg;
    //            case TextureFormat.Red8i:
    //            case TextureFormat.Red8ui:
    //            case TextureFormat.Red16i:
    //            case TextureFormat.Red16ui:
    //            case TextureFormat.Red32i:
    //            case TextureFormat.Red32ui:
    //                return PixelFormat.RedInteger;
    //            case TextureFormat.RedGreen8i:
    //            case TextureFormat.RedGreen8ui:
    //            case TextureFormat.RedGreen16i:
    //            case TextureFormat.RedGreen16ui:
    //            case TextureFormat.RedGreen32i:
    //            case TextureFormat.RedGreen32ui:
    //                return PixelFormat.RgInteger;
    //            case TextureFormat.RedGreenBlueAlpha32f:
    //                return PixelFormat.Rgba;
    //            case TextureFormat.RedGreenBlue32f:
    //                return PixelFormat.Rgb;
    //            case TextureFormat.RedGreenBlueAlpha16f:
    //                return PixelFormat.Rgba;
    //            case TextureFormat.RedGreenBlue16f:
    //                return PixelFormat.Rgb;
    //            case TextureFormat.Depth24Stencil8:
    //                return PixelFormat.DepthStencil;
    //            case TextureFormat.Red11fGreen11fBlue10f:
    //            case TextureFormat.RedGreenBlue9E5:
    //                return PixelFormat.Rgb;
    //            case TextureFormat.SRedGreenBlue8:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.SRedGreenBlue8Alpha8:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.Depth32f:
    //                return PixelFormat.DepthComponent;
    //            case TextureFormat.Depth32fStencil8:
    //                return PixelFormat.DepthStencil;
    //            case TextureFormat.RedGreenBlueAlpha32ui:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue32ui:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.RedGreenBlueAlpha16ui:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue16ui:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.RedGreenBlueAlpha8ui:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue8ui:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.RedGreenBlueAlpha32i:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue32i:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.RedGreenBlueAlpha16i:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue16i:
    //                return PixelFormat.RgbInteger;
    //            case TextureFormat.RedGreenBlueAlpha8i:
    //                return PixelFormat.RgbaInteger;
    //            case TextureFormat.RedGreenBlue8i:
    //                return PixelFormat.RgbInteger;
    //        }

    //        throw new ArgumentException("textureFormat");
    //    }

    //    public PixelType TextureToPixelType()
    //    {
    //        // TODO:  Not tested exhaustively
    //        switch (format)
    //        {
    //            case TextureFormat.RedGreenBlue8:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.RedGreenBlue16:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.RedGreenBlueAlpha8:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.RedGreenBlue10A2:
    //                return PixelType.UnsignedInt1010102;
    //            case TextureFormat.RedGreenBlueAlpha16:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.Depth16:
    //                return PixelType.HalfFloat;
    //            case TextureFormat.Depth24:
    //                return PixelType.Float;
    //            case TextureFormat.Red8:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.Red16:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.RedGreen8:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.RedGreen16:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.Red16f:
    //                return PixelType.HalfFloat;
    //            case TextureFormat.Red32f:
    //                return PixelType.Float;
    //            case TextureFormat.RedGreen16f:
    //                return PixelType.HalfFloat;
    //            case TextureFormat.RedGreen32f:
    //                return PixelType.Float;
    //            case TextureFormat.Red8i:
    //                return PixelType.Byte;
    //            case TextureFormat.Red8ui:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.Red16i:
    //                return PixelType.Short;
    //            case TextureFormat.Red16ui:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.Red32i:
    //                return PixelType.Int;
    //            case TextureFormat.Red32ui:
    //                return PixelType.UnsignedInt;
    //            case TextureFormat.RedGreen8i:
    //                return PixelType.Byte;
    //            case TextureFormat.RedGreen8ui:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.RedGreen16i:
    //                return PixelType.Short;
    //            case TextureFormat.RedGreen16ui:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.RedGreen32i:
    //                return PixelType.Int;
    //            case TextureFormat.RedGreen32ui:
    //                return PixelType.UnsignedInt;
    //            case TextureFormat.RedGreenBlueAlpha32f:
    //                return PixelType.Float;
    //            case TextureFormat.RedGreenBlue32f:
    //                return PixelType.Float;
    //            case TextureFormat.RedGreenBlueAlpha16f:
    //                return PixelType.HalfFloat;
    //            case TextureFormat.RedGreenBlue16f:
    //                return PixelType.HalfFloat;
    //            case TextureFormat.Depth24Stencil8:
    //                return PixelType.UnsignedInt248;
    //            case TextureFormat.Red11fGreen11fBlue10f:
    //                return PixelType.Float;
    //            case TextureFormat.RedGreenBlue9E5:
    //                return PixelType.Float;
    //            case TextureFormat.SRedGreenBlue8:
    //            case TextureFormat.SRedGreenBlue8Alpha8:
    //                return PixelType.Byte;
    //            case TextureFormat.Depth32f:
    //            case TextureFormat.Depth32fStencil8:
    //                return PixelType.Float;
    //            case TextureFormat.RedGreenBlueAlpha32ui:
    //            case TextureFormat.RedGreenBlue32ui:
    //                return PixelType.UnsignedInt;
    //            case TextureFormat.RedGreenBlueAlpha16ui:
    //            case TextureFormat.RedGreenBlue16ui:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.RedGreenBlueAlpha8ui:
    //            case TextureFormat.RedGreenBlue8ui:
    //                return PixelType.UnsignedByte;
    //            case TextureFormat.RedGreenBlueAlpha32i:
    //            case TextureFormat.RedGreenBlue32i:
    //                return PixelType.UnsignedInt;
    //            case TextureFormat.RedGreenBlueAlpha16i:
    //            case TextureFormat.RedGreenBlue16i:
    //                return PixelType.UnsignedShort;
    //            case TextureFormat.RedGreenBlueAlpha8i:
    //            case TextureFormat.RedGreenBlue8i:
    //                return PixelType.UnsignedByte;
    //        }

    //        throw new ArgumentException("textureFormat");
    //    }

    //    public bool GenerateMipmaps
    //    {
    //        get { return generateMipmaps; }
    //    }

    //    public bool ColorRenderable
    //    {
    //        get
    //        {
    //            return !DepthRenderable && !DepthStencilRenderable;
    //        }
    //    }

    //    public bool DepthRenderable
    //    {
    //        get
    //        {
    //            return
    //                format == TextureFormat.Depth16 ||
    //                format == TextureFormat.Depth24 ||
    //                format == TextureFormat.Depth32f ||
    //                format == TextureFormat.Depth24Stencil8 ||
    //                format == TextureFormat.Depth32fStencil8;
    //        }
    //    }

    //    public bool DepthStencilRenderable
    //    {
    //        get
    //        {
    //            return
    //                format == TextureFormat.Depth24Stencil8 ||
    //                format == TextureFormat.Depth32fStencil8;
    //        }
    //    }

    //    /// <summary>
    //    /// This is approximate because we don't know exactly how the driver stores the texture.
    //    /// </summary>
    //    public int ApproximateSizeInBytes
    //    {
    //        get
    //        {
    //            return width * height * SizeInBytes(format);
    //        }
    //    }

    //    private static int SizeInBytes(TextureFormat format)
    //    {
    //        switch (format)
    //        {
    //            case TextureFormat.RedGreenBlue8:
    //                return 3;
    //            case TextureFormat.RedGreenBlue16:
    //                return 6;
    //            case TextureFormat.RedGreenBlueAlpha8:
    //                return 4;
    //            case TextureFormat.RedGreenBlue10A2:
    //                return 4;
    //            case TextureFormat.RedGreenBlueAlpha16:
    //                return 8;
    //            case TextureFormat.Depth16:
    //                return 2;
    //            case TextureFormat.Depth24:
    //                return 3;
    //            case TextureFormat.Red8:
    //                return 1;
    //            case TextureFormat.Red16:
    //                return 2;
    //            case TextureFormat.RedGreen8:
    //                return 2;
    //            case TextureFormat.RedGreen16:
    //                return 4;
    //            case TextureFormat.Red16f:
    //                return 2;
    //            case TextureFormat.Red32f:
    //                return 4;
    //            case TextureFormat.RedGreen16f:
    //                return 4;
    //            case TextureFormat.RedGreen32f:
    //                return 8;
    //            case TextureFormat.Red8i:
    //                return 1;
    //            case TextureFormat.Red8ui:
    //                return 1;
    //            case TextureFormat.Red16i:
    //                return 2;
    //            case TextureFormat.Red16ui:
    //                return 2;
    //            case TextureFormat.Red32i:
    //                return 4;
    //            case TextureFormat.Red32ui:
    //                return 4;
    //            case TextureFormat.RedGreen8i:
    //                return 2;
    //            case TextureFormat.RedGreen8ui:
    //                return 2;
    //            case TextureFormat.RedGreen16i:
    //                return 4;
    //            case TextureFormat.RedGreen16ui:
    //                return 4;
    //            case TextureFormat.RedGreen32i:
    //                return 8;
    //            case TextureFormat.RedGreen32ui:
    //                return 8;
    //            case TextureFormat.RedGreenBlueAlpha32f:
    //                return 16;
    //            case TextureFormat.RedGreenBlue32f:
    //                return 12;
    //            case TextureFormat.RedGreenBlueAlpha16f:
    //                return 8;
    //            case TextureFormat.RedGreenBlue16f:
    //                return 6;
    //            case TextureFormat.Depth24Stencil8:
    //                return 4;
    //            case TextureFormat.Red11fGreen11fBlue10f:
    //                return 4;
    //            case TextureFormat.RedGreenBlue9E5:
    //                return 4;
    //            case TextureFormat.SRedGreenBlue8:
    //                return 3;
    //            case TextureFormat.SRedGreenBlue8Alpha8:
    //                return 4;
    //            case TextureFormat.Depth32f:
    //                return 4;
    //            case TextureFormat.Depth32fStencil8:
    //                return 5;
    //            case TextureFormat.RedGreenBlueAlpha32ui:
    //                return 16;
    //            case TextureFormat.RedGreenBlue32ui:
    //                return 12;
    //            case TextureFormat.RedGreenBlueAlpha16ui:
    //                return 8;
    //            case TextureFormat.RedGreenBlue16ui:
    //                return 6;
    //            case TextureFormat.RedGreenBlueAlpha8ui:
    //                return 4;
    //            case TextureFormat.RedGreenBlue8ui:
    //                return 3;
    //            case TextureFormat.RedGreenBlueAlpha32i:
    //                return 16;
    //            case TextureFormat.RedGreenBlue32i:
    //                return 12;
    //            case TextureFormat.RedGreenBlueAlpha16i:
    //                return 8;
    //            case TextureFormat.RedGreenBlue16i:
    //                return 6;
    //            case TextureFormat.RedGreenBlueAlpha8i:
    //                return 4;
    //            case TextureFormat.RedGreenBlue8i:
    //                return 3;
    //        }

    //        throw new ArgumentException("format");
    //    }

    //    public static bool operator ==(Texture2DDescription left, Texture2DDescription right)
    //    {
    //        return left.Equals(right);
    //    }

    //    public static bool operator !=(Texture2DDescription left, Texture2DDescription right)
    //    {
    //        return !left.Equals(right);
    //    }

    //    public override string ToString()
    //    {
    //        return string.Format(CultureInfo.CurrentCulture, "Width: {0} Height: {1} Format: {2} GenerateMipmaps: {3}",
    //            width, height, format, generateMipmaps);
    //    }

    //    public override int GetHashCode()
    //    {
    //        return
    //            width.GetHashCode() ^
    //            height.GetHashCode() ^
    //            format.GetHashCode() ^
    //            generateMipmaps.GetHashCode();
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (!(obj is Texture2DDescription))
    //            return false;

    //        return this.Equals((Texture2DDescription)obj);
    //    }

    //    #region IEquatable<Texture2DDescription> Members

    //    public bool Equals(Texture2DDescription other)
    //    {
    //        return
    //            (width == other.width) &&
    //            (height == other.height) &&
    //            (format == other.format) &&
    //            (generateMipmaps == other.generateMipmaps);
    //    }

    //    #endregion

    //    private readonly int width;
    //    private readonly int height;
    //    private readonly TextureFormat format;
    //    private readonly bool generateMipmaps;
    //}

}
