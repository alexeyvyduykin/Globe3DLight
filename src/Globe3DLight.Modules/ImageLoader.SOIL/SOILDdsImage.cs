using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;

namespace Globe3DLight.ImageLoader.SOIL
{
    internal class SOILDdsImage : ObservableObject, IDdsImage
    {
        //private readonly A.Dds _ddsImage;
        private readonly int _width;
        private readonly int _height;
        private readonly byte[] _data;
        private readonly DDS_header _header;
        private readonly bool _compressed;

        public SOILDdsImage(int width, int height, byte[] data, DDS_header header, bool compressed)
        {
            _width = width;
            _height = height;
            _data = data;
            _header = header;
            _compressed = compressed;


            //_ddsImage = ddsImage;
            //_ddsImageHeader = new SOILDdsImageHeader(ddsImage.Header);
        }

        public int Width => _width;
        public int Height => _height;

        public byte[] Data => _data;

        public bool Compressed => _compressed;

        public IDdsImageHeader Header => new SOILDdsImageHeader(_header);

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }
}
