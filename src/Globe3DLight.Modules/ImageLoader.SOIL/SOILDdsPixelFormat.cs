using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;



namespace Globe3DLight.ImageLoader.SOIL
{
    internal class SOILDdsPixelFormat : ObservableObject, IDdsPixelFormat
    {
        #region Constants 

        // DDSStruct flags 
        public const int DDSD_CAPS = 0x00000001;
        public const int DDSD_HEIGHT = 0x00000002;
        public const int DDSD_WIDTH = 0x00000004;
        public const int DDSD_PITCH = 0x00000008;
        public const int DDSD_PIXELFORMAT = 0x00001000;
        public const int DDSD_MIPMAPCOUNT = 0x00020000;
        public const int DDSD_LINEARSIZE = 0x00080000;
        public const int DDSD_DEPTH = 0x00800000;

        // PixelFormat values 
        public const int DDPF_ALPHAPIXELS = 0x00000001;

        public const int DDPF_FOURCC = 0x00000004;
        public const int DDPF_RGB = 0x00000040;
        public const int DDPF_LUMINANCE = 0x00020000;

        // DDSCaps 
        public const int DDSCAPS_COMPLEX = 0x00000008;

        public const int DDSCAPS_TEXTURE = 0x00001000;
        public const int DDSCAPS_MIPMAP = 0x00400000;
        public const int DDSCAPS2_CUBEMAP = 0x00000200;
        public const int DDSCAPS2_CUBEMAP_POSITIVEX = 0x00000400;
        public const int DDSCAPS2_CUBEMAP_NEGATIVEX = 0x00000800;
        public const int DDSCAPS2_CUBEMAP_POSITIVEY = 0x00001000;
        public const int DDSCAPS2_CUBEMAP_NEGATIVEY = 0x00002000;
        public const int DDSCAPS2_CUBEMAP_POSITIVEZ = 0x00004000;
        public const int DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x00008000;
        public const int DDSCAPS2_VOLUME = 0x00200000;

        // FOURCC 
        public const uint FOURCC_DXT1 = 0x31545844;

        public const uint FOURCC_DXT2 = 0x32545844;
        public const uint FOURCC_DXT3 = 0x33545844;
        public const uint FOURCC_DXT4 = 0x34545844;
        public const uint FOURCC_DXT5 = 0x35545844;
        public const uint FOURCC_ATI1 = 0x31495441;
        public const uint FOURCC_ATI2 = 0x32495441;
        public const uint FOURCC_RXGB = 0x42475852;
        public const uint FOURCC_DOLLARNULL = 0x24;
        public const uint FOURCC_oNULL = 0x6f;
        public const uint FOURCC_pNULL = 0x70;
        public const uint FOURCC_qNULL = 0x71;
        public const uint FOURCC_rNULL = 0x72;
        public const uint FOURCC_sNULL = 0x73;
        public const uint FOURCC_tNULL = 0x74;

        #endregion Constants 

        private readonly DDS_header.sPixelFormatStruct _ddsPixelFormat;
        private readonly uint _flags;
        private readonly uint _fourCC;

        public SOILDdsPixelFormat(DDS_header.sPixelFormatStruct ddsPixelFormat)
        {
            this._ddsPixelFormat = ddsPixelFormat;
            this._flags = ddsPixelFormat.Flags;// PixelFormatFlags.Convert();
            this._fourCC = ddsPixelFormat.FourCC;//.Convert();
        }

        public uint Size => _ddsPixelFormat.Size;
        public DdsPixelFormatFlags Flags 
        {
            get
            {
                if ((_flags & DDPF_ALPHAPIXELS) != 0)
                {
                    return DdsPixelFormatFlags.AlphaPixels;
                }
                else if( (_flags & DDPF_FOURCC) != 0 )
                {
                    return DdsPixelFormatFlags.Fourcc;
                }
                else if ((_flags & DDPF_RGB) != 0)
                {
                    return DdsPixelFormatFlags.Rgb;
                }
                else if ((_flags & DDPF_LUMINANCE) != 0)
                {
                    return DdsPixelFormatFlags.Luminance;
                }
                else 
                {                         
                    throw new Exception();
                }
            }
        }
        public CompressionAlgorithm FourCC {
            get
            {
                if((_fourCC & FOURCC_DXT1) != 0)
                {
                    return CompressionAlgorithm.D3DFMT_DXT1;
                }
                else if ((_fourCC & FOURCC_DXT2) != 0)
                {
                    return CompressionAlgorithm.D3DFMT_DXT2;
                }
                else if ((_fourCC & FOURCC_DXT3) != 0)
                {
                    return CompressionAlgorithm.D3DFMT_DXT3;
                }
                else if ((_fourCC & FOURCC_DXT4) != 0)
                {
                    return CompressionAlgorithm.D3DFMT_DXT4;
                }
                else if ((_fourCC & FOURCC_DXT5) != 0)
                {
                    return CompressionAlgorithm.D3DFMT_DXT5;
                }

                else if ((_fourCC & FOURCC_ATI1) != 0)
                {
                    return CompressionAlgorithm.ATI1;
                }
                else if ((_fourCC & FOURCC_ATI2) != 0)
                {
                    return CompressionAlgorithm.ATI2;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public uint RGBBitCount => _ddsPixelFormat.RGBBitCount;
        public uint RBitMask => _ddsPixelFormat.RBitMask;
        public uint GBitMask => _ddsPixelFormat.GBitMask;
        public uint BBitMask => _ddsPixelFormat.BBitMask;
        public uint ABitMask => _ddsPixelFormat.AlphaBitMask;


        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
