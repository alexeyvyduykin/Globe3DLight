using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight;
using Globe3DLight.Image;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;


namespace Globe3DLight.ImageLoader.SOIL
{
    public class SOILImageLoader : IImageLoader
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


        private readonly IServiceProvider _serviceProvider;
        private readonly SOILFactory SOILFactory;


        public SOILImageLoader(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            SOILFactory = new SOILFactory();
        }

        public IDdsImage LoadDdsImageFromFile(string path)      
        {                                    
            var image = SOIL_direct_load_DDS(path);

            return image;
        }

        public IEnumerable<IDdsImage> LoadDdsImageFromFiles(IEnumerable<string> paths)
        {
            var list = new List<IDdsImage>();

            foreach (var path in paths)
            {
                var image = SOIL_direct_load_DDS(path);

                list.Add(image);
            }

            return list;
        }

        private IDdsImage SOIL_direct_load_DDS(string filename)
        {
            IDdsImage image = null;

            if (null == filename)
            {
                return null;
            }

            var buffer = File.ReadAllBytes(filename);
            var buffer_length = buffer.Length;

            if (null == buffer)
            {
                return null;
            }

            unsafe
            {
                /*	now try to do the loading	*/
                image = SOIL_direct_load_DDS_from_memory(buffer);
            }
            return image;
        }
        private bool HasFlag(uint a, uint b)
        {
            return (a & b) == b;
        }

        /*	for loading cube maps	*/
        private enum SOILCapability
        {
            Unknown = -1,
            None = 0,
            Present = 1
        }

        private bool DDSImage(byte[] ddsImage, ref DDS_header header)
        {
            if (ddsImage == null) return false;
            if (ddsImage.Length == 0) return false;

            using (MemoryStream stream = new MemoryStream(ddsImage.Length))
            {
                stream.Write(ddsImage, 0, ddsImage.Length);
                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(stream))
                {
                    //DDS_header header = new DDS_header();
                    return ReadHeader(reader, ref header);
                }
            }
        }
        private bool ReadHeader(BinaryReader reader, ref DDS_header header)
        {
            //byte[] signature = reader.ReadBytes(4);
            //if (!(signature[0] == 'D' && signature[1] == 'D' && signature[2] == 'S' && signature[3] == ' '))
            //    return false;

            header.dwMagic = reader.ReadUInt32();


            header.Size = reader.ReadUInt32();
            if (header.Size != 124)
                return false;

            //convert the data 
            header.dwFlags = reader.ReadUInt32();
            header.Height = reader.ReadUInt32();
            header.Width = reader.ReadUInt32();
            header.PitchOrLinearSize = reader.ReadUInt32();
            header.Depth = reader.ReadUInt32();
            header.MipMapCount = reader.ReadUInt32();
            //header.alphabitdepth = reader.ReadUInt32();

            unsafe
            {
                fixed (uint* pt = header.Reserved1)
                {
                    //header.dwReserved1 = new uint[11];// new uint[10];
                    for (int i = 0; i < 11; i++)
                    {
                        pt[i] = reader.ReadUInt32();
                        //header.dwReserved1[i] = reader.ReadUInt32();
                    }
                }
            }

            //pixelfromat 
            header.PixelFormat.Size = reader.ReadUInt32();
            header.PixelFormat.Flags = reader.ReadUInt32();
            header.PixelFormat.FourCC = reader.ReadUInt32();
            header.PixelFormat.RGBBitCount = reader.ReadUInt32();
            header.PixelFormat.RBitMask = reader.ReadUInt32();
            header.PixelFormat.GBitMask = reader.ReadUInt32();
            header.PixelFormat.BBitMask = reader.ReadUInt32();
            header.PixelFormat.AlphaBitMask = reader.ReadUInt32();


            //caps 
            header.Caps1 = reader.ReadUInt32();
            header.Caps2 = reader.ReadUInt32();
            header.Caps3 = reader.ReadUInt32();
            header.Caps4 = reader.ReadUInt32();

            header.Reserved2 = reader.ReadUInt32();// texturestage = reader.ReadUInt32();

            return true;
        }


        private IDdsImage SOIL_direct_load_DDS_from_memory(byte[] buffer)
        {           
            DDS_header header = new DDS_header();
                     
            byte[] memoryTarget = new byte[Marshal.SizeOf(typeof(DDS_header)) /*headerLength*/];

            Array.Copy(buffer, 0, memoryTarget, 0, Marshal.SizeOf(typeof(DDS_header))/*headerLength*/);

            if (this.DDSImage(memoryTarget, ref header) == false)
                return null;

            uint buffer_index = (uint)Marshal.SizeOf(typeof(DDS_header));// (uint)headerLength;// (uint)Marshal.SizeOf(header);
            var buffer_length = buffer.Length;


            /*	OK, validated the header, let's load the image data	*/
            var width = (int)header.Width;
            var height = (int)header.Height;
            var uncompressed = 1 - (int)(header.PixelFormat.Flags & DDPF_FOURCC) / DDPF_FOURCC;
            var cubemap = (int)(header.Caps2 & DDSCAPS2_CUBEMAP) / DDSCAPS2_CUBEMAP;


            byte[] data = new byte[buffer_length - buffer_index];

            Array.Copy(buffer, (int)buffer_index, data, 0, buffer_length - buffer_index);

            return new SOILDdsImage(width, height, data, header, uncompressed != 0);

         //   return new SOILDdsImage(width, height, buffer, header, uncompressed != 0);
        }
    }
}
