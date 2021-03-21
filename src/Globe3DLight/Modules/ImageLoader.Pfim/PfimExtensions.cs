using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;
using A = Pfim;



namespace Globe3DLight.ImageLoader.Pfim
{
    internal static class PfimExtensions
    {
        public static DdsPixelFormatFlags Convert(this A.DdsPixelFormatFlags flags)
        {
            switch (flags)
            {
                case A.DdsPixelFormatFlags.AlphaPixels:
                    return DdsPixelFormatFlags.AlphaPixels;
                case A.DdsPixelFormatFlags.Alpha:
                    return DdsPixelFormatFlags.Alpha;
                case A.DdsPixelFormatFlags.Fourcc:
                    return DdsPixelFormatFlags.Fourcc;
                case A.DdsPixelFormatFlags.Rgb:
                    return DdsPixelFormatFlags.Rgb;
                case A.DdsPixelFormatFlags.Yuv:
                    return DdsPixelFormatFlags.Yuv;
                case A.DdsPixelFormatFlags.Luminance:
                    return DdsPixelFormatFlags.Luminance;
                default:
                    throw new Exception();
            }
        }


        public static CompressionAlgorithm Convert(this A.CompressionAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case A.CompressionAlgorithm.None:
                    return CompressionAlgorithm.None;
                case A.CompressionAlgorithm.D3DFMT_DXT1:
                    return CompressionAlgorithm.D3DFMT_DXT1;
                case A.CompressionAlgorithm.D3DFMT_DXT2:
                    return CompressionAlgorithm.D3DFMT_DXT2;
                case A.CompressionAlgorithm.D3DFMT_DXT3:
                    return CompressionAlgorithm.D3DFMT_DXT3;
                case A.CompressionAlgorithm.D3DFMT_DXT4:
                    return CompressionAlgorithm.D3DFMT_DXT4;
                case A.CompressionAlgorithm.D3DFMT_DXT5:
                    return CompressionAlgorithm.D3DFMT_DXT5;
                case A.CompressionAlgorithm.DX10:
                    return CompressionAlgorithm.DX10;
                case A.CompressionAlgorithm.ATI1:
                    return CompressionAlgorithm.ATI1;
                case A.CompressionAlgorithm.BC4U:
                    return CompressionAlgorithm.BC4U;
                case A.CompressionAlgorithm.BC4S:
                    return CompressionAlgorithm.BC4S;
                case A.CompressionAlgorithm.ATI2:
                    return CompressionAlgorithm.ATI2;
                case A.CompressionAlgorithm.BC5U:
                    return CompressionAlgorithm.BC5U;
                case A.CompressionAlgorithm.BC5S:
                    return CompressionAlgorithm.BC5S;
                default:
                    throw new Exception();
            }
        }
    }

}
