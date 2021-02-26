using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;
using A = Pfim;


namespace Globe3DLight.ImageLoader.Pfim
{
    internal class PfimDdsImageHeader : ObservableObject, IDdsImageHeader
    {
        private readonly A.DdsHeader _ddsImageHeader;
        private readonly IDdsPixelFormat _ddsPixelFormat;

        public PfimDdsImageHeader(A.DdsHeader ddsImageHeader)
        {
            _ddsImageHeader = ddsImageHeader;

            _ddsPixelFormat = new PfimDdsPixelFormat(ddsImageHeader.PixelFormat);
        }


     
        public uint Size => _ddsImageHeader.Size;
        public uint Height => _ddsImageHeader.Height;
        public uint Width => _ddsImageHeader.Width;
        public uint PitchOrLinearSize => _ddsImageHeader.PitchOrLinearSize;
        public uint Depth => _ddsImageHeader.Depth;
        public uint MipMapCount => _ddsImageHeader.MipMapCount;

        public uint[] Reserved1 => _ddsImageHeader.Reserved1; // [11]

        public uint Reserved2 => _ddsImageHeader.Reserved2;

        public uint Caps1 => _ddsImageHeader.Caps; // dwCaps1
        public uint Caps2 => _ddsImageHeader.Caps2; // dwCaps2
        public uint Caps3 => _ddsImageHeader.Caps3; // dwDDSX
        public uint Caps4 => _ddsImageHeader.Caps4; // dwReserved

        public IDdsPixelFormat PixelFormat => _ddsPixelFormat;



        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


}
