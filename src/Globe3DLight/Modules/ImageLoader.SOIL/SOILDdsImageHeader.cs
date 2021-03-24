using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Image;
using System.Runtime.InteropServices;
using System.IO;
using Globe3DLight.ViewModels;

namespace Globe3DLight.ImageLoader.SOIL
{
    internal class SOILDdsImageHeader : ViewModelBase, IDdsImageHeader
    {
        private readonly DDS_header _ddsImageHeader;
        private readonly IDdsPixelFormat _ddsPixelFormat;

        public SOILDdsImageHeader(DDS_header ddsImageHeader)
        {
            _ddsImageHeader = ddsImageHeader;

            _ddsPixelFormat = new SOILDdsPixelFormat(ddsImageHeader.PixelFormat);
        }

        public uint Size => _ddsImageHeader.Size;
        public uint Height => _ddsImageHeader.Height;
        public uint Width => _ddsImageHeader.Width;
        public uint PitchOrLinearSize => _ddsImageHeader.PitchOrLinearSize;
        public uint Depth => _ddsImageHeader.Depth;
        public uint MipMapCount => _ddsImageHeader.MipMapCount;

        public uint[] Reserved1
        {
            get
            {
                var arr = new uint[11];

                unsafe
                {
                    for (int i = 0; i < 11; i++)
                    {
                        arr[i] = _ddsImageHeader.Reserved1[i];
                    }
                }
                return arr;
            }           
        }
   
        public uint Reserved2 => _ddsImageHeader.Reserved2;

        public uint Caps1 => _ddsImageHeader.Caps1; // dwCaps1
        public uint Caps2 => _ddsImageHeader.Caps2; // dwCaps2
        public uint Caps3 => _ddsImageHeader.Caps3; // dwDDSX
        public uint Caps4 => _ddsImageHeader.Caps4; // dwReserved

        public IDdsPixelFormat PixelFormat => _ddsPixelFormat;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal unsafe struct DDS_header 
    {
        public uint dwMagic;
        public uint Size;
        public uint dwFlags;
        public uint Height;
        public uint Width;
        public uint PitchOrLinearSize;
        public uint Depth;
        public uint MipMapCount;

        public fixed uint Reserved1[11];  //= new uint[11]; //[11]; 
   
        /*  DDPIXELFORMAT	*/    
        public struct sPixelFormatStruct
        {
            public uint Size;
            public uint Flags;
            public uint FourCC;
            public uint RGBBitCount;
            public uint RBitMask;
            public uint GBitMask;
            public uint BBitMask;
            public uint AlphaBitMask;
        }

        public sPixelFormatStruct PixelFormat;

        public uint Caps1;
        public uint Caps2;
        public uint Caps3;
        public uint Caps4;

        public uint Reserved2;
    }
}
