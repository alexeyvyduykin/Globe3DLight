using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Image;
using A = Pfim;
using Globe3DLight.ViewModels;

namespace Globe3DLight.ImageLoader.Pfim
{
    internal class PfimDdsPixelFormat : ViewModelBase, IDdsPixelFormat
    {
        private readonly A.DdsPixelFormat _ddsPixelFormat;
        private readonly DdsPixelFormatFlags _flags;
        private readonly CompressionAlgorithm _fourCC;

        public PfimDdsPixelFormat(A.DdsPixelFormat ddsPixelFormat)
        {
            this._ddsPixelFormat = ddsPixelFormat;
            this._flags = ddsPixelFormat.PixelFormatFlags.Convert();
            this._fourCC = ddsPixelFormat.FourCC.Convert();
        }

        public uint Size => _ddsPixelFormat.Size;
        public DdsPixelFormatFlags Flags => _flags;
        public CompressionAlgorithm FourCC => _fourCC;
        public uint RGBBitCount => _ddsPixelFormat.RGBBitCount;
        public uint RBitMask => _ddsPixelFormat.RBitMask;
        public uint GBitMask => _ddsPixelFormat.GBitMask;
        public uint BBitMask => _ddsPixelFormat.BBitMask;
        public uint ABitMask => _ddsPixelFormat.ABitMask;
    }
}
