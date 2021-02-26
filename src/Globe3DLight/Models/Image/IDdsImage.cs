using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Image
{
    public interface IDdsImage : IImage
    {

        IDdsImageHeader Header { get; }

        int Width { get; }
        int Height { get; }

        byte[] Data { get; }

        bool Compressed { get; }
    
    }

    public interface IDdsImageHeader
    {
        uint Size { get; }
        uint Height { get; }
        uint Width { get; }
        uint PitchOrLinearSize { get; }
        uint Depth { get; }
        uint MipMapCount { get; }

        uint[] Reserved1 { get; } // [11]

        uint Reserved2 { get; }

        uint Caps1 { get; } // dwCaps1
        uint Caps2 { get; } // dwCaps2
        uint Caps3 { get; } // dwDDSX
        uint Caps4 { get; } // dwReserved

        IDdsPixelFormat PixelFormat { get; }
    }

    [Flags]
    public enum DdsPixelFormatFlags : uint
    {
        AlphaPixels = 1,
        Alpha = 2,
        Fourcc = 4,
        Rgb = 64,
        Yuv = 512,
        Luminance = 131072
    }
    public enum CompressionAlgorithm : uint
    {
        None = 0,
        DX10 = 808540228,
        ATI1 = 826889281,
        D3DFMT_DXT1 = 827611204,
        ATI2 = 843666497,
        D3DFMT_DXT2 = 844388420,
        D3DFMT_DXT3 = 861165636,
        D3DFMT_DXT4 = 877942852,
        D3DFMT_DXT5 = 894720068,
        BC4S = 1395934018,
        BC5S = 1395999554,
        BC4U = 1429488450,
        BC5U = 1429553986
    }

    public interface IDdsPixelFormat
    {
        uint Size { get; }
        DdsPixelFormatFlags Flags { get; }
        CompressionAlgorithm FourCC { get; }
        uint RGBBitCount { get; }
        uint RBitMask { get; }
        uint GBitMask { get; }
        uint BBitMask { get; }
        uint ABitMask { get; }
    }


}
