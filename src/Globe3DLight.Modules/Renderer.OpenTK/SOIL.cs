using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.IO;
using TeximpNet.Unmanaged;

namespace Globe3DLight.Renderer.OpenTK
{

    [Flags]
    public enum SOILFlags
    {
        PowerOfTwo = 1,
        Mipmaps = 2,
        TextureRepeats = 4,
        MultiplyAlpha = 8,
        InvertY = 16,
        CompressToDxt = 32,
        DdsLoadDirect = 64,
        NtscSafeRgb = 128,
        CoCgY = 256,
        TextureRectangle = 512
    }

    /*	for loading cube maps	*/
    public enum SOILCapability
    {
        Unknown = -1,
        None = 0,
        Present = 1
    }

   // 128 

   // /**	A bunch of DirectDraw Surface structures and flags **/
   //// [StructLayout(LayoutKind.Sequential, Pack = 1)]
   // [StructLayout(LayoutKind.Explicit)]
   // public struct DDS_header
   // {
   //     [FieldOffset(0)] public uint dwMagic;
   //     [FieldOffset(4)] public uint dwSize;
   //     [FieldOffset(8)] public uint dwFlags;
   //     [FieldOffset(12)] public uint dwHeight;
   //     [FieldOffset(16)] public uint dwWidth;
   //     [FieldOffset(20)] public uint dwPitchOrLinearSize;
   //     [FieldOffset(24)] public uint dwDepth;
   //     [FieldOffset(28)] public uint dwMipMapCount;
   //     [FieldOffset(32)] public uint[] dwReserved1;// = new uint[11]; //[11]; 

   //     /*  DDPIXELFORMAT	*/
   //     public struct sPixelFormatStruct
   //     {
   //         public uint dwSize;
   //         public uint dwFlags;
   //         public uint dwFourCC;
   //         public uint dwRGBBitCount;
   //         public uint dwRBitMask;
   //         public uint dwGBitMask;
   //         public uint dwBBitMask;
   //         public uint dwAlphaBitMask;
   //     }

   //     [FieldOffset(76)] public sPixelFormatStruct sPixelFormat;

   //     /*  DDCAPS2	*/
   //     public struct sCapsStruct
   //     {
   //         public uint dwCaps1;
   //         public uint dwCaps2;
   //         public uint dwDDSX;
   //         public uint dwReserved;
   //     }

   //     [FieldOffset(108)] public sCapsStruct sCaps;

   //     [FieldOffset(124)] public uint dwReserved2;
   // }

    /**	A bunch of DirectDraw Surface structures and flags **/
    // [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct DDS_header
    {
        public uint dwMagic;
        public uint dwSize;
        public uint dwFlags;
        public uint dwHeight;
        public uint dwWidth;
        public uint dwPitchOrLinearSize;
        public uint dwDepth;
        public uint dwMipMapCount;
        public fixed uint dwReserved1[11];// = new uint[11]; //[11]; 

        /*  DDPIXELFORMAT	*/
        public struct sPixelFormatStruct
        {
            public uint dwSize;
            public uint dwFlags;
            public uint dwFourCC;
            public uint dwRGBBitCount;
            public uint dwRBitMask;
            public uint dwGBitMask;
            public uint dwBBitMask;
            public uint dwAlphaBitMask;
        }

        public sPixelFormatStruct sPixelFormat;

        /*  DDCAPS2	*/
        public struct sCapsStruct
        {
            public uint dwCaps1;
            public uint dwCaps2;
            public uint dwDDSX;
            public uint dwReserved;
        }

        public sCapsStruct sCaps;

        public uint dwReserved2;
    }


    // unsigned char == byte

    //if(c) == if(c != 0) 
    //if(!c) == if(c == 0)

    public class SOIL
    {
        //[DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        //public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);


        public static byte[] RawSerialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawdata = new byte[rawsize];
            Marshal.Copy(buffer, rawdata, 0, rawsize);
            Marshal.FreeHGlobal(buffer);
            return rawdata;
        }

        // private int headerLength = 128;

            // из-за ошибок при .NET Core проверка не выполняется, а сразу задаются значения,
            // которые подходили для версии .NET Framework

        static SOILCapability has_NPOT_capability = SOILCapability.Present;// SOILCapability.Unknown;
        static SOILCapability has_tex_rectangle_capability = SOILCapability.Present;// SOILCapability.Unknown;
        static SOILCapability has_DXT_capability = SOILCapability.Present;// SOILCapability.Unknown;
        static SOILCapability has_cubemap_capability = SOILCapability.Present;// SOILCapability.Unknown;

        static string failure_reason;

        string stbi_failure_reason()
        {
            return failure_reason;
        }

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

        public static bool HasFlag(uint a, uint b)
        {
            return (a & b) == b;
        }

        private static A.PixelFormat ToFormat(A.PixelInternalFormat InternalFormat)
        {
            switch (InternalFormat)
            {
                case A.PixelInternalFormat.DepthComponent:
                    return A.PixelFormat.DepthComponent;
                case A.PixelInternalFormat.Alpha:
                    return A.PixelFormat.Alpha;
                case A.PixelInternalFormat.Rgb:
                    return A.PixelFormat.Rgb;
                case A.PixelInternalFormat.Rgba:
                    return A.PixelFormat.Rgba;
                case A.PixelInternalFormat.Luminance:
                    return A.PixelFormat.Luminance;
                case A.PixelInternalFormat.LuminanceAlpha:
                    return A.PixelFormat.LuminanceAlpha;
                //case PixelInternalFormat.R3G3B2:
                //case PixelInternalFormat.Alpha4:
                //case PixelInternalFormat.Alpha8:
                //case PixelInternalFormat.Alpha12:
                //case PixelInternalFormat.Alpha16:
                //case PixelInternalFormat.Luminance4:
                //case PixelInternalFormat.Luminance8:
                //case PixelInternalFormat.Luminance12:
                //case PixelInternalFormat.Luminance16:
                //case PixelInternalFormat.Luminance4Alpha4:
                //case PixelInternalFormat.Luminance6Alpha2:
                //case PixelInternalFormat.Luminance8Alpha8:
                //case PixelInternalFormat.Luminance12Alpha4:
                //case PixelInternalFormat.Luminance12Alpha12:
                //case PixelInternalFormat.Luminance16Alpha16:
                //case PixelInternalFormat.Intensity:
                //case PixelInternalFormat.Intensity4:
                //case PixelInternalFormat.Intensity8:
                //case PixelInternalFormat.Intensity12:
                //case PixelInternalFormat.Intensity16:
                //case PixelInternalFormat.Rgb2Ext:
                //case PixelInternalFormat.Rgb4:
                //case PixelInternalFormat.Rgb5:
                //case PixelInternalFormat.Rgb8:
                //case PixelInternalFormat.Rgb10:
                //case PixelInternalFormat.Rgb12:
                //case PixelInternalFormat.Rgb16:
                //case PixelInternalFormat.Rgba2:
                //case PixelInternalFormat.Rgba4:
                //case PixelInternalFormat.Rgb5A1:
                //case PixelInternalFormat.Rgba8:
                //case PixelInternalFormat.Rgb10A2:
                //case PixelInternalFormat.Rgba12:
                //case PixelInternalFormat.Rgba16:
                default:
                    return 0;
            }
        }

        /*	error reporting	*/
        private string result_string_pointer = "SOIL initialized";

        private void check_for_GL_errors(string message)
        {

        }

        SOILCapability query_NPOT_capability()
        {
            /*	check for the capability	*/
            if (has_NPOT_capability == SOILCapability.Unknown)
            {
                /*	we haven't yet checked for the capability, do so	*/
                if ((-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_ARB_texture_non_power_of_two" ) ))		                
                {
                    /*	not there, flag the failure	*/
                    has_NPOT_capability = SOILCapability.None;
                } 
                else{
                    /*	it's there!	*/
                    has_NPOT_capability = SOILCapability.Present;
                }
            }

            /*	let the user know if we can do non-power-of-two textures or not	*/
            return has_NPOT_capability;
        }

        SOILCapability query_tex_rectangle_capability()
        {
            /*	check for the capability	*/
            if (has_tex_rectangle_capability == SOILCapability.Unknown)
            {
                /*	we haven't yet checked for the capability, do so	*/
                if ((-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_ARB_texture_rectangle"))
                    &&
                    (-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_EXT_texture_rectangle"))
                    &&
                    (-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_NV_texture_rectangle")))
                {
                    /*	not there, flag the failure	*/
                    has_tex_rectangle_capability = SOILCapability.None;
                }
                else
                {
                    /*	it's there!	*/
                    has_tex_rectangle_capability = SOILCapability.Present;
                }
            }
            /*	let the user know if we can do texture rectangles or not	*/
            return has_tex_rectangle_capability;
        }

        SOILCapability query_cubemap_capability()
        {
            /*	check for the capability	*/
            if (has_cubemap_capability == SOILCapability.Unknown)
            {
                /*	we haven't yet checked for the capability, do so	*/
                if ((-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_ARB_texture_cube_map"))
                    &&
                    (-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_EXT_texture_cube_map")))
                {
                    /*	not there, flag the failure	*/
                    has_cubemap_capability = SOILCapability.None;
                }
                else
                {
                    /*	it's there!	*/
                    has_cubemap_capability = SOILCapability.Present;
                }
            }

            /*	let the user know if we can do cubemaps or not	*/
            return has_cubemap_capability;
        }

        SOILCapability query_DXT_capability()
        {
            /*	check for the capability	*/
            if (has_DXT_capability == SOILCapability.Unknown)
            {
                /*	we haven't yet checked for the capability, do so	*/
                if (-1 == A.GL.GetString(A.StringName.Extensions).IndexOf("GL_EXT_texture_compression_s3tc") )
                {
                    /*	not there, flag the failure	*/
                    has_DXT_capability = SOILCapability.None;
                }
                else
                {
                        /*	all's well!	*/
                        has_DXT_capability = SOILCapability.Present;
                }
            }
            /*	let the user know if we can do DXT or not	*/

            return has_DXT_capability;
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


            header.dwSize = reader.ReadUInt32();
            if (header.dwSize != 124)
                return false;

            //convert the data 
            header.dwFlags = reader.ReadUInt32();
            header.dwHeight = reader.ReadUInt32();
            header.dwWidth = reader.ReadUInt32();
            header.dwPitchOrLinearSize = reader.ReadUInt32();
            header.dwDepth = reader.ReadUInt32();
            header.dwMipMapCount = reader.ReadUInt32();
            //header.alphabitdepth = reader.ReadUInt32();

            unsafe
            {
                fixed(uint* pt = header.dwReserved1)
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
            header.sPixelFormat.dwSize = reader.ReadUInt32();
            header.sPixelFormat.dwFlags = reader.ReadUInt32();
            header.sPixelFormat.dwFourCC = reader.ReadUInt32();
            header.sPixelFormat.dwRGBBitCount = reader.ReadUInt32();
            header.sPixelFormat.dwRBitMask = reader.ReadUInt32();
            header.sPixelFormat.dwGBitMask = reader.ReadUInt32();
            header.sPixelFormat.dwBBitMask = reader.ReadUInt32();
            header.sPixelFormat.dwAlphaBitMask = reader.ReadUInt32();


            //caps 
            header.sCaps.dwCaps1 = reader.ReadUInt32();
            header.sCaps.dwCaps2 = reader.ReadUInt32();
            header.sCaps.dwDDSX = reader.ReadUInt32();
            header.sCaps.dwReserved = reader.ReadUInt32();

            header.dwReserved2 = reader.ReadUInt32();// texturestage = reader.ReadUInt32();

            return true;
        }

        private int SOIL_direct_load_DDS(string filename, uint reuse_texture_ID, SOILFlags flags, int loading_as_cubemap)
        {
            byte[] buffer;
            int buffer_length;
            int tex_ID = 0;

            /*	error checks	*/
            if (null == filename)
            {
                result_string_pointer = "null filename";
                return 0;
            }

            buffer = File.ReadAllBytes(filename);
            buffer_length = buffer.Length;

            if (null == buffer)
            {
                result_string_pointer = "malloc failed";
                return 0;
            }

            unsafe
            {
                // fixed (byte* pt = buffer)
                // {
                /*	now try to do the loading	*/
                tex_ID = SOIL_direct_load_DDS_from_memory(buffer, buffer_length, reuse_texture_ID, flags, loading_as_cubemap);
                //SOIL_free_image_data(buffer);
                // }
            }
            return tex_ID;
        }

        public int SOIL_direct_load_DDS_from_memory(byte[] buffer, int buffer_length, uint reuse_texture_ID, SOILFlags flags, int loading_as_cubemap)
        {
            /*	variables	*/
            //int lenHeader = 128;// Marshal.SizeOf(typeof(DDS_header));
            DDS_header header = new DDS_header();
            uint buffer_index = 0;
            int tex_ID = 0;
            /*	file reading variables	*/
            A.PixelInternalFormat S3TC_type = 0;
            A.PixelFormat S3TC_type_ = 0;
            byte[] DDS_data;
            uint DDS_main_size;
            uint DDS_full_size;
            int width, height;
            int mipmaps, cubemap, uncompressed, block_size = 16;
            uint flag;
            A.TextureTarget cf_target, ogl_target_start, ogl_target_end;
            A.TextureTarget opengl_texture_type;
            int i;
            /*	1st off, does the filename even exist?	*/
            if (null == buffer)
            {
                /*	we can't do it!	*/
                result_string_pointer = "NULL buffer";
                return 0;
            }
            if (buffer_length < Marshal.SizeOf(typeof(DDS_header)))
            {
                /*	we can't do it!	*/
                result_string_pointer = "DDS file was too small to contain the DDS header";
                return 0;
            }
            /*	try reading in the header	*/
            //memcpy((void*)(&header), (const void*)buffer, sizeof(DDS_header) );
            //Marshal.StructureToPtr(header, buffer, false);


            //    public static void MarshalUnmananagedArray2Struct<T>(IntPtr unmanagedArray, int length, out T[] mangagedArray)
            //{
            //    var size = Marshal.SizeOf(typeof(T));
            //    mangagedArray = new T[length];

            //    for (int i = 0; i < length; i++)
            //    {
            //        IntPtr ins = new IntPtr(unmanagedArray.ToInt64() + i * size);
            //        mangagedArray[i] = Marshal.PtrToStructure<T>(ins);
            //    }
            //}



            //IntPtr memoryTarget = Marshal.AllocHGlobal(len);
            byte[] memoryTarget = new byte[Marshal.SizeOf(typeof(DDS_header)) /*headerLength*/];

            Array.Copy(buffer, 0, memoryTarget, 0, Marshal.SizeOf(typeof(DDS_header))/*headerLength*/);

            // CopyMemory(memoryTarget, buffer, (uint)len);

            //header = Marshal.PtrToStructure<DDS_header>(memoryTarget);

            if (this.DDSImage(memoryTarget, ref header) == false)
                return 0;

            buffer_index = (uint)Marshal.SizeOf(typeof(DDS_header));// (uint)headerLength;// (uint)Marshal.SizeOf(header);

            /*	guilty until proven innocent	*/
            result_string_pointer = "Failed to read a known DDS header";
            /*	validate the header (warning, "goto"'s ahead, shield your eyes!!)	*/
            flag = ('D' << 0) | ('D' << 8) | ('S' << 16) | (' ' << 24);
            if (header.dwMagic != flag) { goto quick_exit; }
            if (header.dwSize != 124) { goto quick_exit; }
            /*	I need all of these	*/
            flag = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT;
            if ((header.dwFlags & flag) != flag) { goto quick_exit; }
            /*	According to the MSDN spec, the dwFlags should contain
                DDSD_LINEARSIZE if it's compressed, or DDSD_PITCH if
                uncompressed.  Some DDS writers do not conform to the
                spec, so I need to make my reader more tolerant	*/
            /*	I need one of these	*/
            flag = DDPF_FOURCC | DDPF_RGB;
            if ((header.sPixelFormat.dwFlags & flag) == 0) { goto quick_exit; }
            if (header.sPixelFormat.dwSize != 32) { goto quick_exit; }
            if ((header.sCaps.dwCaps1 & DDSCAPS_TEXTURE) == 0) { goto quick_exit; }
            /*	make sure it is a type we can upload	*/
            if (((header.sPixelFormat.dwFlags & DDPF_FOURCC) != 0) &&
                !(
                (header.sPixelFormat.dwFourCC == (('D' << 0) | ('X' << 8) | ('T' << 16) | ('1' << 24))) ||
                (header.sPixelFormat.dwFourCC == (('D' << 0) | ('X' << 8) | ('T' << 16) | ('3' << 24))) ||
                (header.sPixelFormat.dwFourCC == (('D' << 0) | ('X' << 8) | ('T' << 16) | ('5' << 24)))
                ))
            {
                goto quick_exit;
            }
            /*	OK, validated the header, let's load the image data	*/
            result_string_pointer = "DDS header loaded and validated";
            width = (int)header.dwWidth;
            height = (int)header.dwHeight;
            uncompressed = 1 - (int)(header.sPixelFormat.dwFlags & DDPF_FOURCC) / DDPF_FOURCC;
            cubemap = (int)(header.sCaps.dwCaps2 & DDSCAPS2_CUBEMAP) / DDSCAPS2_CUBEMAP;

            if (uncompressed != 0)
            {
                S3TC_type = A.PixelInternalFormat.Rgb;
                S3TC_type_ = A.PixelFormat.Rgb;
                block_size = 3;
                if ((header.sPixelFormat.dwFlags & DDPF_ALPHAPIXELS) != 0)
                {
                    S3TC_type = A.PixelInternalFormat.Rgba;
                    S3TC_type_ = A.PixelFormat.Rgba;
                    block_size = 4;
                }
                DDS_main_size = (uint)(width * height * block_size);
            }
            else
            {
                /*	can we even handle direct uploading to OpenGL DXT compressed images?	*/
                if (false/*query_DXT_capability() != SOILCapability.Present*/)// SOIL_CAPABILITY_PRESENT)
                {
                    /*	we can't do it!	*/
                    result_string_pointer = "Direct upload of S3TC images not supported by the OpenGL driver";
                    return 0;
                }
                /*	well, we know it is DXT1/3/5, because we checked above	*/
                switch ((header.sPixelFormat.dwFourCC >> 24) - '0')
                {
                    case 1:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;// SOIL_RGBA_S3TC_DXT1;
                        block_size = 8;
                        break;
                    case 3:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;// SOIL_RGBA_S3TC_DXT3;
                        block_size = 16;
                        break;
                    case 5:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;// SOIL_RGBA_S3TC_DXT5;
                        block_size = 16;
                        break;
                }
                DDS_main_size = (uint)(((width + 3) >> 2) * ((height + 3) >> 2) * block_size);
            }
            if (cubemap != 0)
            {
                /* does the user want a cubemap?	*/
                if (loading_as_cubemap == 0)
                {
                    /*	we can't do it!	*/
                    result_string_pointer = "DDS image was a cubemap";
                    return 0;
                }
                /*	can we even handle cubemaps with the OpenGL driver?	*/
                if (query_cubemap_capability() != SOILCapability.Present)
                {
                    /*	we can't do it!	*/
                    result_string_pointer = "Direct upload of cubemap images not supported by the OpenGL driver";
                    return 0;
                }
                ogl_target_start = A.TextureTarget.TextureCubeMapPositiveX;
                ogl_target_end = A.TextureTarget.TextureCubeMapNegativeZ;
                opengl_texture_type = A.TextureTarget.TextureCubeMap;
            }
            else
            {
                /* does the user want a non-cubemap?	*/
                if (loading_as_cubemap != 0)
                {
                    /*	we can't do it!	*/
                    result_string_pointer = "DDS image was not a cubemap";
                    return 0;
                }
                ogl_target_start = A.TextureTarget.Texture2D;
                ogl_target_end = A.TextureTarget.Texture2D;
                opengl_texture_type = A.TextureTarget.Texture2D;
            }

            //if (((header.sCaps.dwCaps1 & DDSCAPS_MIPMAP) != 0) && (header.dwMipMapCount > 1))
            if (HasFlag(header.sCaps.dwCaps1, DDSCAPS_MIPMAP) && (header.dwMipMapCount > 1))
            {
                int shift_offset;
                mipmaps = (int)(header.dwMipMapCount - 1);
                DDS_full_size = DDS_main_size;
                if (uncompressed != 0)
                {
                    /*	uncompressed DDS, simple MIPmap size calculation	*/
                    shift_offset = 0;
                }
                else
                {
                    /*	compressed DDS, MIPmap size calculation is block based	*/
                    shift_offset = 2;
                }
                for (i = 1; i <= mipmaps; ++i)
                {
                    int w, h;
                    w = width >> (shift_offset + i);
                    h = height >> (shift_offset + i);
                    if (w < 1)
                    {
                        w = 1;
                    }
                    if (h < 1)
                    {
                        h = 1;
                    }
                    DDS_full_size += (uint)(w * h * block_size);
                }
            }
            else
            {
                mipmaps = 0;
                DDS_full_size = DDS_main_size;
            }
            DDS_data = new byte[DDS_full_size];// (unsigned char*)malloc(DDS_full_size);
                                               /*	got the image data RAM, create or use an existing OpenGL texture handle	*/
            tex_ID = (int)reuse_texture_ID;
            if (tex_ID == 0)
            {
                tex_ID = A.GL.GenTexture();
            }
            /*  bind an OpenGL texture ID	*/
            A.GL.BindTexture(opengl_texture_type, tex_ID);
            /*	do this for each face of the cubemap!	*/
            for (cf_target = ogl_target_start; cf_target <= ogl_target_end; ++cf_target)
            {

                if (buffer_index + DDS_full_size <= (uint)buffer_length)
                {

                    uint byte_offset = DDS_main_size;

                    //memcpy((void*)DDS_data, (const void*)(&buffer[buffer_index]), DDS_full_size );
                    //void* memcpy (void* destination, const void* source, size_t num );

                    // void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length);


                    Array.Copy(buffer, (int)buffer_index, DDS_data, 0, (int)DDS_full_size);

                    // Marshal.Copy(DDS_data, (int)buffer_index, buffer, (int)DDS_full_size);


                    buffer_index += DDS_full_size;
                    /*	upload the main chunk	*/

                    if (uncompressed != 0)
                    {
                        /*	and remember, DXT uncompressed uses BGR(A),
                            so swap to RGB(A) for ALL MIPmap levels	*/
                        for (i = 0; i < (int)DDS_full_size; i += block_size)
                        {
                            byte temp = DDS_data[i];
                            DDS_data[i] = DDS_data[i + 2];
                            DDS_data[i + 2] = temp;
                        }
                        A.GL.TexImage2D(
                            cf_target, 0,
                            S3TC_type, width, height, 0,
                            S3TC_type_,
                            A.PixelType.UnsignedByte, DDS_data);
                    }
                    else
                    {
                        //soilGlCompressedTexImage2D(cf_target, 0, S3TC_type, width, height, 0, DDS_main_size, DDS_data);

                        A.GL.CompressedTexImage2D(
                            cf_target, 0,
                            S3TC_type, width, height, 0,
                            (int)DDS_main_size, DDS_data);

                    }
                    /*	upload the mipmaps, if we have them	*/
                    for (i = 1; i <= mipmaps; ++i)
                    {
                        int w, h, mip_size;
                        w = width >> i;
                        h = height >> i;
                        if (w < 1)
                        {
                            w = 1;
                        }
                        if (h < 1)
                        {
                            h = 1;
                        }
                        unsafe
                        {
                            fixed (byte* source = DDS_data)
                            {
                                byte* pt = source + byte_offset;

                                /*	upload this mipmap	*/
                                if (uncompressed != 0)
                                {
                                    mip_size = w * h * block_size;
                                    A.GL.TexImage2D(
                                        cf_target, i,
                                        S3TC_type, w, h, 0,
                                        S3TC_type_, A.PixelType.UnsignedByte, (IntPtr)pt);
                                }
                                else
                                {
                                    mip_size = ((w + 3) / 4) * ((h + 3) / 4) * block_size;
                                    //soilGlCompressedTexImage2D(cf_target, i, S3TC_type, w, h, 0, mip_size, &DDS_data[byte_offset]);
                                    A.GL.CompressedTexImage2D(cf_target, i,
                                        S3TC_type, w, h, 0,
                                        mip_size, (IntPtr)pt);
                                }
                            }
                        }
                        /*	and move to the next mipmap	*/
                        byte_offset += (uint)mip_size;
                    }
                    /*	it worked!	*/
                    result_string_pointer = "DDS file loaded";
                }
                else
                {
                    A.GL.DeleteTexture(tex_ID);
                    tex_ID = 0;
                    cf_target = ogl_target_end + 1;
                    result_string_pointer = "DDS file was too small for expected image data";
                }
            }/* end reading each face */

            //SOIL_free_image_data(DDS_data);

            if (tex_ID != 0)
            {
                /*	did I have MIPmaps?	*/
                if (mipmaps > 0)
                {
                    /*	instruct OpenGL to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                }
                else
                {
                    /*	instruct OpenGL _NOT_ to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                }
                /*	does the user want clamping, or wrapping?	*/
                if ((flags & SOILFlags.TextureRepeats) != 0)
                {
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                }
                else
                {
                    /*	unsigned int clamp_mode = SOIL_CLAMP_TO_EDGE;	*/
                    int clamp_mode = (int)TextureWrapMode.Clamp;
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, clamp_mode);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, clamp_mode);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, clamp_mode);
                }
            }

            quick_exit:
            /*	report success or failure	*/

            return tex_ID;
        }

        public int SOIL_load_OGL_texture(string filename, int force_channels, uint reuse_texture_ID, SOILFlags flags)
        {
            /*	variables	*/
            byte[] img;
            int width, height, channels;
            int tex_id;
            /*	does the user want direct uploading of the image as a DDS file?	*/
            if ((flags & SOILFlags.DdsLoadDirect) != 0)
            {
                /*	1st try direct loading of the image as a DDS file
                    note: direct uploading will only load what is in the
                    DDS file, no MIPmaps will be generated, the image will
                    not be flipped, etc.	*/
                tex_id = SOIL_direct_load_DDS(filename, reuse_texture_ID, flags, 0);
                if (tex_id != 0)
                {
                    /*	hey, it worked!!	*/
                    return tex_id;
                }
            }
            /*	try to load the image	*/
            img = SOIL_load_image(filename, out width, out height, out channels, force_channels);
            /*	channels holds the original number of channels, which may have been forced	*/
            if ((force_channels >= 1) && (force_channels <= 4))
            {
                channels = force_channels;
            }
            if (null == img)
            {
                /*	image loading failed	*/
                result_string_pointer = stbi_failure_reason();
                return 0;
            }
            /*	OK, make it a texture!	*/
            unsafe
            {
              //  byte[] destinationArray = new byte[width * height * channels];
               // IntPtr pt = Marshal.AllocHGlobal(width * height * channels);

               // Marshal.Copy(img, 0, pt, img.Length);


              //  Array.Copy(img, 0, destinationArray, 0, width * height * channels);

                //fixed (byte* pt = img)
                //{
                tex_id = SOIL_internal_create_OGL_texture(img, width, height, channels, reuse_texture_ID, flags, A.TextureTarget.Texture2D, A.TextureTarget.Texture2D, (int)A.GetPName.MaxTextureSize);

                /*	and nuke the image data	*/
                //!!!           SOIL_free_image_data(img);
                // }

              //  Marshal.FreeHGlobal(pt);
            }
            /*	and return the handle, such as it is	*/
            return tex_id;
        }

        public int SOIL_load_OGL_cubemap(
            string x_pos_file,
            string x_neg_file,
            string y_pos_file,
            string y_neg_file,
            string z_pos_file,
            string z_neg_file,
            int force_channels,
            uint reuse_texture_ID,
            SOILFlags flags)
        {
            /*	variables	*/
            byte[] img;
            int width, height, channels;
            int tex_id;
            /*	error checking	*/
            if ((x_pos_file == null) ||
                (x_neg_file == null) ||
                (y_pos_file == null) ||
                (y_neg_file == null) ||
                (z_pos_file == null) ||
                (z_neg_file == null))
            {
                result_string_pointer = "Invalid cube map files list";
                return 0;
            }
            /*	capability checking	*/
            if (query_cubemap_capability() != SOILCapability.Present)
            {
                result_string_pointer = "No cube map capability present";
                return 0;
            }
            /*	1st face: try to load the image	*/
            img = SOIL_load_image(x_pos_file, out width, out height, out channels, force_channels);
            /*	channels holds the original number of channels, which may have been forced	*/
            if ((force_channels >= 1) && (force_channels <= 4))
            {
                channels = force_channels;
            }
            if (null == img)
            {
                /*	image loading failed	*/
                result_string_pointer = stbi_failure_reason();
                return 0;
            }
            /*	upload the texture, and create a texture ID if necessary	*/
            tex_id = SOIL_internal_create_OGL_texture(
                    img, width, height, channels,
                    reuse_texture_ID, flags,
                   A.TextureTarget.TextureCubeMap,
                   A.TextureTarget.TextureCubeMapPositiveX,
                    (int)A.GetPName.MaxCubeMapTextureSize);
            /*	and nuke the image data	*/
            //SOIL_free_image_data(img);
            Array.Clear(img, 0, img.Length);
            /*	continue?	*/
            if (tex_id != 0)
            {
                /*	1st face: try to load the image	*/
                img = SOIL_load_image(x_neg_file, out width, out height, out channels, force_channels);
                /*	channels holds the original number of channels, which may have been forced	*/
                if ((force_channels >= 1) && (force_channels <= 4))
                {
                    channels = force_channels;
                }
                if (null == img)
                {
                    /*	image loading failed	*/
                    result_string_pointer = stbi_failure_reason();
                    return 0;
                }
                /*	upload the texture, but reuse the assigned texture ID	*/
                tex_id = SOIL_internal_create_OGL_texture(
                        img, width, height, channels,
                        (uint)tex_id, flags,
                       A.TextureTarget.TextureCubeMap,
                       A.TextureTarget.TextureCubeMapNegativeX,
                        (int)A.GetPName.MaxCubeMapTextureSize);

                /*	and nuke the image data	*/
                //SOIL_free_image_data(img );
                Array.Clear(img, 0, img.Length);
            }
            /*	continue?	*/
            if (tex_id != 0)
            {
                /*	1st face: try to load the image	*/
                img = SOIL_load_image(y_pos_file, out width, out height, out channels, force_channels);
                /*	channels holds the original number of channels, which may have been forced	*/
                if ((force_channels >= 1) && (force_channels <= 4))
                {
                    channels = force_channels;
                }
                if (null == img)
                {
                    /*	image loading failed	*/
                    result_string_pointer = stbi_failure_reason();
                    return 0;
                }
                /*	upload the texture, but reuse the assigned texture ID	*/
                tex_id = SOIL_internal_create_OGL_texture(
                        img, width, height, channels,
                        (uint)tex_id, flags,
                       A.TextureTarget.TextureCubeMap,
                        A.TextureTarget.TextureCubeMapPositiveY,
                        (int)A.GetPName.MaxCubeMapTextureSize);
                /*	and nuke the image data	*/
                //SOIL_free_image_data(img);
                Array.Clear(img, 0, img.Length);
            }
            /*	continue?	*/
            if (tex_id != 0)
            {
                /*	1st face: try to load the image	*/
                img = SOIL_load_image(y_neg_file, out width, out height, out channels, force_channels);
                /*	channels holds the original number of channels, which may have been forced	*/
                if ((force_channels >= 1) && (force_channels <= 4))
                {
                    channels = force_channels;
                }
                if (null == img)
                {
                    /*	image loading failed	*/
                    result_string_pointer = stbi_failure_reason();
                    return 0;
                }
                /*	upload the texture, but reuse the assigned texture ID	*/
                tex_id = SOIL_internal_create_OGL_texture(                        
                        img, width, height, channels,
                        (uint)tex_id, flags,
                       A.TextureTarget.TextureCubeMap,
                       A.TextureTarget.TextureCubeMapNegativeY,
                        (int)A.GetPName.MaxCubeMapTextureSize);
                /*	and nuke the image data	*/
                //SOIL_free_image_data(img);
                Array.Clear(img, 0, img.Length);
            }
            /*	continue?	*/
            if (tex_id != 0)
            {
                /*	1st face: try to load the image	*/
                img = SOIL_load_image(z_pos_file, out width, out height, out channels, force_channels);
                /*	channels holds the original number of channels, which may have been forced	*/
                if ((force_channels >= 1) && (force_channels <= 4))
                {
                    channels = force_channels;
                }
                if (null == img)
                {
                    /*	image loading failed	*/
                    result_string_pointer = stbi_failure_reason();
                    return 0;
                }
                /*	upload the texture, but reuse the assigned texture ID	*/
                tex_id = SOIL_internal_create_OGL_texture(
                        img, width, height, channels,
                        (uint)tex_id, flags,
                        A.TextureTarget.TextureCubeMap,
                       A.TextureTarget.TextureCubeMapPositiveZ,
                        (int)A.GetPName.MaxCubeMapTextureSize);
                /*	and nuke the image data	*/
                //SOIL_free_image_data(img);
                Array.Clear(img, 0, img.Length);
            }
            /*	continue?	*/
            if (tex_id != 0)
            {
                /*	1st face: try to load the image	*/
                img = SOIL_load_image(z_neg_file, out width, out height, out channels, force_channels);
                /*	channels holds the original number of channels, which may have been forced	*/
                if ((force_channels >= 1) && (force_channels <= 4))
                {
                    channels = force_channels;
                }
                if (null == img)
                {
                    /*	image loading failed	*/
                    result_string_pointer = stbi_failure_reason();
                    return 0;
                }
                /*	upload the texture, but reuse the assigned texture ID	*/
                tex_id = SOIL_internal_create_OGL_texture(
                        img, width, height, channels,
                        (uint)tex_id, flags,
                       A.TextureTarget.TextureCubeMap,
                        A.TextureTarget.TextureCubeMapNegativeZ,
                        (int)A.GetPName.MaxCubeMapTextureSize);
                /*	and nuke the image data	*/
                //SOIL_free_image_data(img);
                Array.Clear(img, 0, img.Length);
            }
            /*	and return the handle, such as it is	*/
            return tex_id;
        }


        public int SOIL_load_OGL_single_cubemap(string filename, uint reuse_texture_ID, SOILFlags flags)
        {
            /*	variables	*/
            int tex_id = 0;
            /*	error checking	*/
            if (filename == null)
            {
                result_string_pointer = "Invalid single cube map file name";
                return 0;
            }
            /*	does the user want direct uploading of the image as a DDS file?	*/
            if ((flags & SOILFlags.DdsLoadDirect) != 0)
            {
                /*	1st try direct loading of the image as a DDS file
                    note: direct uploading will only load what is in the
                    DDS file, no MIPmaps will be generated, the image will
                    not be flipped, etc.	*/
                tex_id = SOIL_direct_load_DDS(filename, reuse_texture_ID, flags, 1);
                if (tex_id != 0)
                {
                    /*	hey, it worked!!	*/
                    return tex_id;
                }
            }

            return tex_id;
        }


        public byte[] SOIL_load_image(string filename, out int width, out int height, out int channels, int force_channels)
        {

            IntPtr bmp = FreeImageLibrary.Instance.LoadFromFile(filename, 0);

            width = FreeImageLibrary.Instance.GetWidth(bmp);
            height = FreeImageLibrary.Instance.GetHeight(bmp);


            TeximpNet.ImageColorType asddf = FreeImageLibrary.Instance.GetImageColorType(bmp);

            if (filename.Contains(".dds") == true)
                channels = 4;
            else
                channels = 3;

            //int gfgf = sizeof(byte);

            IntPtr pt = FreeImageLibrary.Instance.GetData(bmp);

            //byte[] result = stbi_load(filename, out width, out height, out channels, force_channels);
            if (pt == null)
            {
                result_string_pointer = stbi_failure_reason();
            }
            else
            {
                result_string_pointer = "Image loaded";
            }

            byte[] result = new byte[width * height * channels];
            Marshal.Copy(pt, result, 0, width * height * channels);



            return result;
        }

        public int SOIL_internal_create_OGL_texture(byte[] img, int width, int height, int channels, uint reuse_texture_ID, SOILFlags flags, A.TextureTarget opengl_texture_type, A.TextureTarget opengl_texture_target, int texture_check_size_enum)
        {
            /*	variables	*/
            //byte[] img;
            int tex_id;
            A.PixelInternalFormat internal_texture_format = 0;
            A.PixelFormat original_texture_format = 0;
            SOILCapability DXT_mode = SOILCapability.Unknown;
            int max_supported_size;
            /*	If the user wants to use the texture rectangle I kill a few flags	*/
            if ((flags & SOILFlags.TextureRectangle) != 0)
            {
                /*	well, the user asked for it, can we do that?	*/
                if (query_tex_rectangle_capability() == SOILCapability.Present)// SOIL_CAPABILITY_PRESENT)
                {
                    /*	only allow this if the user in _NOT_ trying to do a cubemap!	*/
                    if (opengl_texture_type == A.TextureTarget.Texture2D)
                    {
                        /*	clean out the flags that cannot be used with texture rectangles	*/
                        flags &= ~(SOILFlags.PowerOfTwo | SOILFlags.Mipmaps | SOILFlags.TextureRepeats);
                        /*	and change my target	*/
                        opengl_texture_target = A.TextureTarget.TextureRectangleArb;// SOIL_TEXTURE_RECTANGLE_ARB;
                        opengl_texture_type = A.TextureTarget.TextureRectangleArb;// SOIL_TEXTURE_RECTANGLE_ARB;
                    }
                    else
                    {
                        /*	not allowed for any other uses (yes, I'm looking at you, cubemaps!)	*/
                        flags &= ~SOILFlags.TextureRectangle;// SOIL_FLAG_TEXTURE_RECTANGLE;
                    }

                }
                else
                {
                    /*	can't do it, and that is a breakable offense (uv coords use pixels instead of [0,1]!)	*/
                    result_string_pointer = "Texture Rectangle extension unsupported";
                    return 0;
                }
            }
            /*	create a copy the image data	*/
          //  img = new byte[width * height * channels];//(unsigned char*)malloc(width * height * channels);

            //void * memcpy ( void * destination, const void * source, size_t num );
            //memcpy(img, data, width * height * channels);
          //  Marshal.Copy(data, img, 0, width * height * channels);



            /*	does the user want me to invert the image?	*/
            if ((flags & SOILFlags.InvertY) != 0)
            {
                int i, j;
                for (j = 0; j * 2 < height; ++j)
                {
                    int index1 = j * width * channels;
                    int index2 = (height - 1 - j) * width * channels;
                    for (i = width * channels; i > 0; --i)
                    {
                        byte temp = img[index1];
                        img[index1] = img[index2];
                        img[index2] = temp;
                        ++index1;
                        ++index2;
                    }
                }
            }
            
            /*	does the user want me to scale the colors into the NTSC safe RGB range?	*/
            if ((flags & SOILFlags.NtscSafeRgb) != 0)
            {
                scale_image_RGB_to_NTSC_safe(img, width, height, channels);
            }
            /*	does the user want me to convert from straight to pre-multiplied alpha?
                (and do we even _have_ alpha?)	*/
            if ((flags & SOILFlags.MultiplyAlpha) != 0)
            {
                int i;
                switch (channels)
                {
                    case 2:
                        for (i = 0; i < 2 * width * height; i += 2)
                        {
                            img[i] = (byte)((img[i] * img[i + 1] + 128) >> 8);
                        }
                        break;
                    case 4:
                        for (i = 0; i < 4 * width * height; i += 4)
                        {
                            img[i + 0] = (byte)((img[i + 0] * img[i + 3] + 128) >> 8);
                            img[i + 1] = (byte)((img[i + 1] * img[i + 3] + 128) >> 8);
                            img[i + 2] = (byte)((img[i + 2] * img[i + 3] + 128) >> 8);
                        }
                        break;
                    default:
                        /*	no other number of channels contains alpha data	*/
                        break;
                }
            }
            /*	if the user can't support NPOT textures, make sure we force the POT option	*/
            if ((query_NPOT_capability() == SOILCapability.None) && !((flags & SOILFlags.TextureRectangle) != 0))
            {
                /*	add in the POT flag */
                flags |= SOILFlags.PowerOfTwo;
            }
            /*	how large of a texture can this OpenGL implementation handle?	*/
            /*	texture_check_size_enum will be GL_MAX_TEXTURE_SIZE or SOIL_MAX_CUBE_MAP_TEXTURE_SIZE	*/

            //GL.GetIntegerv(texture_check_size_enum, &max_supported_size);
            A.GL.GetInteger(A.GetPName.MaxTextureSize, out max_supported_size);

            /*	do I need to make it a power of 2?	*/
            if (
                ((flags & SOILFlags.PowerOfTwo) != 0) || /*	user asked for it	*/
                ((flags & SOILFlags.Mipmaps) != 0) ||      /*	need it for the MIP-maps	*/
                (width > max_supported_size) ||     /*	it's too big, (make sure it's	*/
                (height > max_supported_size))      /*	2^n for later down-sampling)	*/
            {
                int new_width = 1;
                int new_height = 1;
                while (new_width < width)
                {
                    new_width *= 2;
                }
                while (new_height < height)
                {
                    new_height *= 2;
                }
                /*	still?	*/
                if ((new_width != width) || (new_height != height))
                {
                    /*	yep, resize	*/
                    byte[] resampled = new byte[channels * new_width * new_height];// (unsigned char*)malloc(channels * new_width * new_height);

                    up_scale_image(
                            img, width, height, channels,
                            resampled, new_width, new_height);
                    /*	OJO	this is for debug only!	*/
                    /*
                    SOIL_save_image( "\\showme.bmp", SOIL_SAVE_TYPE_BMP,
                                    new_width, new_height, channels,
                                    resampled );
                    */
                    /*	nuke the old guy, then point it at the new guy	*/
                    //SOIL_free_image_data(img);
                    img = resampled;
                    width = new_width;
                    height = new_height;
                }
            }
            /*	now, if it is too large...	*/
            if ((width > max_supported_size) || (height > max_supported_size))
            {
                /*	I've already made it a power of two, so simply use the MIPmapping
                    code to reduce its size to the allowable maximum.	*/
                byte[] resampled;
                int reduce_block_x = 1, reduce_block_y = 1;
                int new_width, new_height;
                if (width > max_supported_size)
                {
                    reduce_block_x = width / max_supported_size;
                }
                if (height > max_supported_size)
                {
                    reduce_block_y = height / max_supported_size;
                }
                new_width = width / reduce_block_x;
                new_height = height / reduce_block_y;
                resampled = new byte[channels * new_width * new_height]; //(unsigned char*)malloc(channels * new_width * new_height);
                /*	perform the actual reduction	*/
                mipmap_image(img, width, height, channels, resampled, reduce_block_x, reduce_block_y);
                /*	nuke the old guy, then point it at the new guy	*/
                //SOIL_free_image_data(img);
                img = resampled;
                width = new_width;
                height = new_height;
            }
            /*	does the user want us to use YCoCg color space?	*/
            if ((flags & SOILFlags.CoCgY) != 0)
            {
                /*	this will only work with RGB and RGBA images */
                convert_RGB_to_YCoCg(img, width, height, channels);
                /*
                save_image_as_DDS( "CoCg_Y.dds", width, height, channels, img );
                */
            }
            /*	create the OpenGL texture ID handle
                (note: allowing a forced texture ID lets me reload a texture)	*/
            tex_id = (int)reuse_texture_ID;
            if (tex_id == 0)
            {
                tex_id = A.GL.GenTexture();
            }

            check_for_GL_errors("glGenTextures");
            /* Note: sometimes glGenTextures fails (usually no OpenGL context)	*/
            if (tex_id != 0)
            {
                /*	and what type am I using as the internal texture format?	*/
                switch (channels)
                {
                    case 1:
                        original_texture_format = A.PixelFormat.Luminance;
                        internal_texture_format = A.PixelInternalFormat.Luminance;
                        break;
                    case 2:
                        original_texture_format = A.PixelFormat.LuminanceAlpha;
                        internal_texture_format = A.PixelInternalFormat.LuminanceAlpha;
                        break;
                    case 3:
                        original_texture_format = A.PixelFormat.Bgr;// PixelFormat.Rgb;
                        internal_texture_format = A.PixelInternalFormat.Rgb;
                        break;
                    case 4:
                        original_texture_format = A.PixelFormat.Bgra;// PixelFormat.Rgba;
                        internal_texture_format = A.PixelInternalFormat.Rgba;
                        break;
                }
                //internal_texture_format = original_texture_format;
                /*	does the user want me to, and can I, save as DXT?	*/
                if ((flags & SOILFlags.CompressToDxt) != 0)
                {
                    DXT_mode = query_DXT_capability();
                    if (DXT_mode == SOILCapability.Present)// SOIL_CAPABILITY_PRESENT)
                    {
                        /*	I can use DXT, whether I compress it or OpenGL does	*/
                        if ((channels & 1) == 1)
                        {
                            /*	1 or 3 channels = DXT1	*/
                            internal_texture_format = A.PixelInternalFormat.CompressedRgbS3tcDxt1Ext;// SOIL_RGB_S3TC_DXT1;
                        }
                        else
                        {
                            /*	2 or 4 channels = DXT5	*/
                            internal_texture_format = A.PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;// SOIL_RGBA_S3TC_DXT5;
                        }
                    }
                }
                /*  bind an OpenGL texture ID	*/
                A.GL.BindTexture(opengl_texture_type, tex_id);

                check_for_GL_errors("glBindTexture");
                /*  upload the main image	*/
                if (DXT_mode == SOILCapability.Present)
                {
                    /*	user wants me to do the DXT conversion!	*/
                    int DDS_size;
                    byte[] DDS_data;
                    if ((channels & 1) == 1)
                    {
                        /*	RGB, use DXT1	*/
                        DDS_data = convert_image_to_DXT1(img, width, height, channels, out DDS_size);
                    }
                    else
                    {
                        /*	RGBA, use DXT5	*/
                        DDS_data = convert_image_to_DXT5(img, width, height, channels, out DDS_size);
                    }
                    if (DDS_data != null)
                    {
                        //soilGlCompressedTexImage2D(opengl_texture_target, 0, internal_texture_format, width, height, 0, DDS_size, DDS_data);

                        A.GL.CompressedTexImage2D(
                            opengl_texture_target, 0,
                            internal_texture_format, width, height, 0,
                            DDS_size, DDS_data);

                        check_for_GL_errors("glCompressedTexImage2D");

                        //SOIL_free_image_data(DDS_data);
                        /*	printf( "Internal DXT compressor\n" );	*/
                    }
                    else
                    {
                        /*	my compression failed, try the OpenGL driver's version	*/
                        A.GL.TexImage2D(
                            opengl_texture_target, 0,
                            internal_texture_format, width, height, 0,
                            original_texture_format, A.PixelType.UnsignedByte, img);

                        check_for_GL_errors("glTexImage2D");
                        /*	printf( "OpenGL DXT compressor\n" );	*/
                    }
                }
                else
                {
                    /*	user want OpenGL to do all the work!	*/
                    A.GL.TexImage2D(
                        opengl_texture_target, 0,
                        internal_texture_format, width, height, 0,
                        original_texture_format, A.PixelType.UnsignedByte, img);

                    check_for_GL_errors("glTexImage2D");
                    /*printf( "OpenGL DXT compressor\n" );	*/
                }
                /*	are any MIPmaps desired?	*/
                if ((flags & SOILFlags.Mipmaps) != 0)
                {
                    int MIPlevel = 1;
                    int MIPwidth = (width + 1) / 2;
                    int MIPheight = (height + 1) / 2;
                    byte[] resampled = new byte[channels * MIPwidth * MIPheight]; //(unsigned char*)malloc(channels * MIPwidth * MIPheight);
                    while (((1 << MIPlevel) <= width) || ((1 << MIPlevel) <= height))
                    {
                        /*	do this MIPmap level	*/
                        mipmap_image(
                                img, width, height, channels,
                                resampled,
                                (1 << MIPlevel), (1 << MIPlevel));
                        /*  upload the MIPmaps	*/
                        if (DXT_mode == SOILCapability.Present)
                        {
                            /*	user wants me to do the DXT conversion!	*/
                            int DDS_size;
                            byte[] DDS_data;// = null;
                            if ((channels & 1) == 1)
                            {
                                /*	RGB, use DXT1	*/
                                DDS_data = convert_image_to_DXT1(
                                        resampled, MIPwidth, MIPheight, channels, out DDS_size);
                            }
                            else
                            {
                                /*	RGBA, use DXT5	*/
                                DDS_data = convert_image_to_DXT5(
                                        resampled, MIPwidth, MIPheight, channels, out DDS_size);
                            }
                            if (DDS_data != null)
                            {
                                A.GL.CompressedTexImage2D(
                                    opengl_texture_target, MIPlevel,
                                    internal_texture_format, MIPwidth, MIPheight, 0,
                                    DDS_size, DDS_data);

                                check_for_GL_errors("glCompressedTexImage2D");

                                //SOIL_free_image_data(DDS_data);
                            }
                            else
                            {
                                /*	my compression failed, try the OpenGL driver's version	*/
                                A.GL.TexImage2D(
                                    opengl_texture_target, MIPlevel,
                                    internal_texture_format, MIPwidth, MIPheight, 0,
                                    original_texture_format, A.PixelType.UnsignedByte, resampled);

                                check_for_GL_errors("glTexImage2D");
                            }
                        }
                        else
                        {
                            /*	user want OpenGL to do all the work!	*/
                            A.GL.TexImage2D(
                                opengl_texture_target, MIPlevel,
                                internal_texture_format, MIPwidth, MIPheight, 0,
                                original_texture_format, A.PixelType.UnsignedByte, resampled);

                            check_for_GL_errors("glTexImage2D");
                        }
                        /*	prep for the next level	*/
                        ++MIPlevel;
                        MIPwidth = (MIPwidth + 1) / 2;
                        MIPheight = (MIPheight + 1) / 2;
                    }

                    //SOIL_free_image_data(resampled);
                    /*	instruct OpenGL to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

                    check_for_GL_errors("GL_TEXTURE_MIN/MAG_FILTER");
                }
                else
                {
                    /*	instruct OpenGL _NOT_ to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

                    check_for_GL_errors("GL_TEXTURE_MIN/MAG_FILTER");
                }
                /*	does the user want clamping, or wrapping?	*/
                if ((flags & SOILFlags.TextureRepeats) != 0)
                {

                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                    if (opengl_texture_type == A.TextureTarget.TextureCubeMap)
                    {
                        /*	SOIL_TEXTURE_WRAP_R is invalid if cubemaps aren't supported	*/
                        A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                    }

                    check_for_GL_errors("GL_TEXTURE_WRAP_*");
                }
                else
                {
                    /*	unsigned int clamp_mode = SOIL_CLAMP_TO_EDGE;	*/
                    uint clamp_mode = (uint)TextureWrapMode.Clamp;

                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, clamp_mode);

                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, clamp_mode);
                    if (opengl_texture_type == A.TextureTarget.TextureCubeMap) 
                    {
                        /*	SOIL_TEXTURE_WRAP_R is invalid if cubemaps aren't supported	*/
                        A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, clamp_mode);
                    }

                    check_for_GL_errors("GL_TEXTURE_WRAP_*");
                }
                /*	done	*/
                result_string_pointer = "Image loaded as an OpenGL texture";
            }
            else
            {
                /*	failed	*/
                result_string_pointer = "Failed to generate an OpenGL texture name; missing OpenGL context?";
            }

            //SOIL_free_image_data(img);
            return tex_id;
        }


        int scale_image_RGB_to_NTSC_safe(byte[] orig, int width, int height, int channels)
        {
            const float scale_lo = 16.0f - 0.499f;
            const float scale_hi = 235.0f + 0.499f;
            int i, j;
            int nc = channels;
            byte[] scale_LUT = new byte[256];
            /*	error check	*/
            if ((width < 1) || (height < 1) ||
                (channels < 1) || (orig == null))
            {
                /*	nothing to do	*/
                return 0;
            }
            /*	set up the scaling Look Up Table	*/
            for (i = 0; i < 256; ++i)
            {
                scale_LUT[i] = (byte)((scale_hi - scale_lo) * i / 255.0f + scale_lo);
            }
            /*	for channels = 2 or 4, ignore the alpha component	*/
            nc -= 1 - (channels & 1);
            /*	OK, go through the image and scale any non-alpha components	*/
            for (i = 0; i < width * height * channels; i += channels)
            {
                for (j = 0; j < nc; ++j)
                {
                    orig[i + j] = scale_LUT[orig[i + j]];
                }
            }
            return 1;
        }
        int up_scale_image(byte[] orig, int width, int height, int channels, byte[] resampled, int resampled_width, int resampled_height)
        {

            float dx, dy;
            int x, y, c;

            /* error(s) check	*/
            if ((width < 1) || (height < 1) ||
                    (resampled_width < 2) || (resampled_height < 2) ||
                    (channels < 1) ||
                    (null == orig) || (null == resampled))
            {
                /*	signify badness	*/
                return 0;
            }
            /*
                for each given pixel in the new map, find the exact location
                from the original map which would contribute to this guy
            */
            dx = (width - 1.0f) / (resampled_width - 1.0f);
            dy = (height - 1.0f) / (resampled_height - 1.0f);
            for (y = 0; y < resampled_height; ++y)
            {
                /* find the base y index and fractional offset from that	*/
                float sampley = y * dy;
                int inty = (int)sampley;
                /*	if( inty < 0 ) { inty = 0; } else	*/
                if (inty > height - 2) { inty = height - 2; }
                sampley -= inty;
                for (x = 0; x < resampled_width; ++x)
                {
                    float samplex = x * dx;
                    int intx = (int)samplex;
                    int base_index;
                    /* find the base x index and fractional offset from that	*/
                    /*	if( intx < 0 ) { intx = 0; } else	*/
                    if (intx > width - 2) { intx = width - 2; }
                    samplex -= intx;
                    /*	base index into the original image	*/
                    base_index = (inty * width + intx) * channels;
                    for (c = 0; c < channels; ++c)
                    {
                        /*	do the sampling	*/
                        float value = 0.5f;
                        value += orig[base_index]

                                                    * (1.0f - samplex) * (1.0f - sampley);
                        value += orig[base_index + channels]

                                    * (samplex) * (1.0f - sampley);
                        value += orig[base_index + width * channels]

                                    * (1.0f - samplex) * (sampley);
                        value += orig[base_index + width * channels + channels]

                                    * (samplex) * (sampley);
                        /*	move to the next channel	*/
                        ++base_index;
                        /*	save the new value	*/
                        resampled[y * resampled_width * channels + x * channels + c] =
                                (byte)(value);
                    }
                }
            }
            /*	done	*/
            return 1;
        }
        int mipmap_image(byte[] orig, int width, int height, int channels, byte[] resampled, int block_size_x, int block_size_y)
        {

            int mip_width, mip_height;
            int i, j, c;

            /*	error check	*/
            if ((width < 1) || (height < 1) ||
                (channels < 1) || (orig == null) ||
                (resampled == null) ||
                (block_size_x < 1) || (block_size_y < 1))
            {
                /*	nothing to do	*/
                return 0;
            }
            mip_width = width / block_size_x;
            mip_height = height / block_size_y;
            if (mip_width < 1)
            {
                mip_width = 1;
            }
            if (mip_height < 1)
            {
                mip_height = 1;
            }
            for (j = 0; j < mip_height; ++j)
            {
                for (i = 0; i < mip_width; ++i)
                {
                    for (c = 0; c < channels; ++c)
                    {
                        int index = (j * block_size_y) * width * channels + (i * block_size_x) * channels + c;
                        int sum_value;
                        int u, v;
                        int u_block = block_size_x;
                        int v_block = block_size_y;
                        int block_area;
                        /*	do a bit of checking so we don't over-run the boundaries
                            (necessary for non-square textures!)	*/
                        if (block_size_x * (i + 1) > width)
                        {
                            u_block = width - i * block_size_y;
                        }
                        if (block_size_y * (j + 1) > height)
                        {
                            v_block = height - j * block_size_y;
                        }
                        block_area = u_block * v_block;
                        /*	for this pixel, see what the average
                            of all the values in the block are.
                            note: start the sum at the rounding value, not at 0	*/
                        sum_value = block_area >> 1;
                        for (v = 0; v < v_block; ++v)
                            for (u = 0; u < u_block; ++u)
                            {
                                sum_value += orig[index + v * width * channels + u * channels];
                            }
                        resampled[j * mip_width * channels + i * channels + c] = (byte)(sum_value / block_area);
                    }
                }
            }
            return 1;
        }
        byte clamp_byte(int x)
        {
            return (byte)((x) < 0 ? (0) : ((x) > 255 ? 255 : (x)));
        }
        int convert_RGB_to_YCoCg(byte[] orig, int width, int height, int channels)
        {
            int i;
            /*	error check	*/
            if ((width < 1) || (height < 1) ||
                (channels < 3) || (channels > 4) ||
                (orig == null))
            {
                /*	nothing to do	*/
                return -1;
            }
            /*	do the conversion	*/
            if (channels == 3)
            {
                for (i = 0; i < width * height * 3; i += 3)
                {
                    int r = orig[i + 0];
                    int g = (orig[i + 1] + 1) >> 1;
                    int b = orig[i + 2];
                    int tmp = (2 + r + b) >> 2;
                    /*	Co	*/
                    orig[i + 0] = clamp_byte(128 + ((r - b + 1) >> 1));
                    /*	Y	*/
                    orig[i + 1] = clamp_byte(g + tmp);
                    /*	Cg	*/
                    orig[i + 2] = clamp_byte(128 + g - tmp);
                }
            }
            else
            {
                for (i = 0; i < width * height * 4; i += 4)
                {
                    int r = orig[i + 0];
                    int g = (orig[i + 1] + 1) >> 1;
                    int b = orig[i + 2];
                    byte a = orig[i + 3];
                    int tmp = (2 + r + b) >> 2;
                    /*	Co	*/
                    orig[i + 0] = clamp_byte(128 + ((r - b + 1) >> 1));
                    /*	Cg	*/
                    orig[i + 1] = clamp_byte(128 + g - tmp);
                    /*	Alpha	*/
                    orig[i + 2] = a;
                    /*	Y	*/
                    orig[i + 3] = clamp_byte(g + tmp);
                }
            }
            /*	done	*/
            return 0;
        }
        byte[] convert_image_to_DXT1(byte[] uncompressed, int width, int height, int channels, out int out_size)
        {

            byte[] compressed;
            int i, j, x, y;
            byte[] ublock = new byte[16 * 3];
            byte[] cblock = new byte[8];
            int index = 0, chan_step = 1;
            int block_count = 0;
            /*	error check	*/
            out_size = 0;

            if ((width < 1) || (height < 1) ||
        (null == uncompressed) ||
        (channels < 1) || (channels > 4))
            {
                return null;
            }
            /*	for channels == 1 or 2, I do not step forward for R,G,B values	*/
            if (channels < 3)
            {
                chan_step = 0;
            }
            /*	get the RAM for the compressed image
                (8 bytes per 4x4 pixel block)	*/
            out_size = ((width + 3) >> 2) * ((height + 3) >> 2) * 8;
            //compressed = (unsigned char*)malloc(*out_size);
            compressed = new byte[out_size];

            /*	go through each block	*/
            for (j = 0; j < height; j += 4)
            {
                for (i = 0; i < width; i += 4)
                {
                    /*	copy this block into a new one	*/
                    int idx = 0;
                    int mx = 4, my = 4;
                    if (j + 4 >= height)
                    {
                        my = height - j;
                    }
                    if (i + 4 >= width)
                    {
                        mx = width - i;
                    }
                    for (y = 0; y < my; ++y)
                    {
                        for (x = 0; x < mx; ++x)
                        {
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels];
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels + chan_step];
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels + chan_step + chan_step];
                        }
                        for (x = mx; x < 4; ++x)
                        {
                            ublock[idx++] = ublock[0];
                            ublock[idx++] = ublock[1];
                            ublock[idx++] = ublock[2];
                        }
                    }
                    for (y = my; y < 4; ++y)
                    {
                        for (x = 0; x < 4; ++x)
                        {
                            ublock[idx++] = ublock[0];
                            ublock[idx++] = ublock[1];
                            ublock[idx++] = ublock[2];
                        }
                    }
                    /*	compress the block	*/
                    ++block_count;

                    compress_DDS_color_block(3, ublock, cblock);
                    /*	copy the data from the block into the main block	*/
                    for (x = 0; x < 8; ++x)
                    {
                        compressed[index++] = cblock[x];
                    }
                }
            }
            return compressed;
        }
        byte[] convert_image_to_DXT5(byte[] uncompressed, int width, int height, int channels, out int out_size)
        {
            byte[] compressed;
            int i, j, x, y;
            byte[] ublock = new byte[16 * 4];
            byte[] cblock = new byte[8];
            int index = 0, chan_step = 1;
            int block_count = 0, has_alpha;
            /*	error check	*/

            out_size = 0;
            if ((width < 1) || (height < 1) ||
                (null == uncompressed) ||
                (channels < 1) || (channels > 4))
            {
                return null;
            }
            /*	for channels == 1 or 2, I do not step forward for R,G,B vales	*/
            if (channels < 3)
            {
                chan_step = 0;
            }
            /*	# channels = 1 or 3 have no alpha, 2 & 4 do have alpha	*/
            has_alpha = 1 - (channels & 1);
            /*	get the RAM for the compressed image
                (16 bytes per 4x4 pixel block)	*/
            out_size = ((width + 3) >> 2) * ((height + 3) >> 2) * 16;
            //compressed = (unsigned char*)malloc(*out_size);
            compressed = new byte[out_size];

            /*	go through each block	*/
            for (j = 0; j < height; j += 4)
            {
                for (i = 0; i < width; i += 4)
                {
                    /*	local variables, and my block counter	*/
                    int idx = 0;
                    int mx = 4, my = 4;
                    if (j + 4 >= height)
                    {
                        my = height - j;
                    }
                    if (i + 4 >= width)
                    {
                        mx = width - i;
                    }
                    for (y = 0; y < my; ++y)
                    {
                        for (x = 0; x < mx; ++x)
                        {
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels];
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels + chan_step];
                            ublock[idx++] = uncompressed[(j + y) * width * channels + (i + x) * channels + chan_step + chan_step];
                            ublock[idx++] =
                                (byte)(has_alpha * uncompressed[(j + y) * width * channels + (i + x) * channels + channels - 1]
                                + (1 - has_alpha) * 255);
                        }
                        for (x = mx; x < 4; ++x)
                        {
                            ublock[idx++] = ublock[0];
                            ublock[idx++] = ublock[1];
                            ublock[idx++] = ublock[2];
                            ublock[idx++] = ublock[3];
                        }
                    }
                    for (y = my; y < 4; ++y)
                    {
                        for (x = 0; x < 4; ++x)
                        {
                            ublock[idx++] = ublock[0];
                            ublock[idx++] = ublock[1];
                            ublock[idx++] = ublock[2];
                            ublock[idx++] = ublock[3];
                        }
                    }
                    /*	now compress the alpha block	*/
                    compress_DDS_alpha_block(ublock, cblock);
                    /*	copy the data from the compressed alpha block into the main buffer	*/
                    for (x = 0; x < 8; ++x)
                    {
                        compressed[index++] = cblock[x];
                    }
                    /*	then compress the color block	*/
                    ++block_count;
                    compress_DDS_color_block(4, ublock, cblock);
                    /*	copy the data from the compressed color block into the main buffer	*/
                    for (x = 0; x < 8; ++x)
                    {
                        compressed[index++] = cblock[x];
                    }
                }
            }
            return compressed;
        }
        void compute_color_line_STDEV(byte[] uncompressed, int channels, float[] point, float[] direction)
        {

            const float inv_16 = 1.0f / 16.0f;
            int i;
            float sum_r = 0.0f, sum_g = 0.0f, sum_b = 0.0f;
            float sum_rr = 0.0f, sum_gg = 0.0f, sum_bb = 0.0f;
            float sum_rg = 0.0f, sum_rb = 0.0f, sum_gb = 0.0f;
            /*	calculate all data needed for the covariance matrix
                ( to compare with _rygdxt code)	*/
            for (i = 0; i < 16 * channels; i += channels)
            {
                sum_r += uncompressed[i + 0];
                sum_rr += uncompressed[i + 0] * uncompressed[i + 0];
                sum_g += uncompressed[i + 1];
                sum_gg += uncompressed[i + 1] * uncompressed[i + 1];
                sum_b += uncompressed[i + 2];
                sum_bb += uncompressed[i + 2] * uncompressed[i + 2];
                sum_rg += uncompressed[i + 0] * uncompressed[i + 1];
                sum_rb += uncompressed[i + 0] * uncompressed[i + 2];
                sum_gb += uncompressed[i + 1] * uncompressed[i + 2];
            }
            /*	convert the sums to averages	*/
            sum_r *= inv_16;
            sum_g *= inv_16;
            sum_b *= inv_16;
            /*	and convert the squares to the squares of the value - avg_value	*/
            sum_rr -= 16.0f * sum_r * sum_r;
            sum_gg -= 16.0f * sum_g * sum_g;
            sum_bb -= 16.0f * sum_b * sum_b;
            sum_rg -= 16.0f * sum_r * sum_g;
            sum_rb -= 16.0f * sum_r * sum_b;
            sum_gb -= 16.0f * sum_g * sum_b;
            /*	the point on the color line is the average	*/
            point[0] = sum_r;
            point[1] = sum_g;
            point[2] = sum_b;
#if USE_COV_MAT
	/*
		The following idea was from ryg.
		(https://mollyrocket.com/forums/viewtopic.php?t=392)
		The method worked great (less RMSE than mine) most of
		the time, but had some issues handling some simple
		boundary cases, like full green next to full red,
		which would generate a covariance matrix like this:

		| 1  -1  0 |
		| -1  1  0 |
		| 0   0  0 |

		For a given starting vector, the power method can
		generate all zeros!  So no starting with {1,1,1}
		as I was doing!  This kind of error is still a
		slight posibillity, but will be very rare.
	*/
	/*	use the covariance matrix directly
		(1st iteration, don't use all 1.0 values!)	*/
	sum_r = 1.0f;
	sum_g = 2.718281828f;
	sum_b = 3.141592654f;
	direction[0] = sum_r*sum_rr + sum_g*sum_rg + sum_b*sum_rb;
	direction[1] = sum_r*sum_rg + sum_g*sum_gg + sum_b*sum_gb;
	direction[2] = sum_r*sum_rb + sum_g*sum_gb + sum_b*sum_bb;
	/*	2nd iteration, use results from the 1st guy	*/
	sum_r = direction[0];
	sum_g = direction[1];
	sum_b = direction[2];
	direction[0] = sum_r*sum_rr + sum_g*sum_rg + sum_b*sum_rb;
	direction[1] = sum_r*sum_rg + sum_g*sum_gg + sum_b*sum_gb;
	direction[2] = sum_r*sum_rb + sum_g*sum_gb + sum_b*sum_bb;
	/*	3rd iteration, use results from the 2nd guy	*/
	sum_r = direction[0];
	sum_g = direction[1];
	sum_b = direction[2];
	direction[0] = sum_r*sum_rr + sum_g*sum_rg + sum_b*sum_rb;
	direction[1] = sum_r*sum_rg + sum_g*sum_gg + sum_b*sum_gb;
	direction[2] = sum_r*sum_rb + sum_g*sum_gb + sum_b*sum_bb;
#else
            /*	use my standard deviation method
                (very robust, a tiny bit slower and less accurate)	*/
            direction[0] = (float)Math.Sqrt(sum_rr);
            direction[1] = (float)Math.Sqrt(sum_gg);
            direction[2] = (float)Math.Sqrt(sum_bb);
            /*	which has a greater component	*/
            if (sum_gg > sum_rr)
            {
                /*	green has greater component, so base the other signs off of green	*/
                if (sum_rg < 0.0f)
                {
                    direction[0] = -direction[0];
                }
                if (sum_gb < 0.0f)
                {
                    direction[2] = -direction[2];
                }
            }
            else
            {
                /*	red has a greater component	*/
                if (sum_rg < 0.0f)
                {
                    direction[1] = -direction[1];
                }
                if (sum_rb < 0.0f)
                {
                    direction[2] = -direction[2];
                }
            }
#endif
        }
        int convert_bit_range(int c, int from_bits, int to_bits)
        {
            int b = (1 << (from_bits - 1)) + c * ((1 << to_bits) - 1);
            return (b + (b >> from_bits)) >> from_bits;
        }
        int rgb_to_565(int r, int g, int b)
        {
            return
                (convert_bit_range(r, 8, 5) << 11) |
                (convert_bit_range(g, 8, 6) << 05) |
                (convert_bit_range(b, 8, 5) << 00);
        }
        void rgb_888_from_565(uint c, ref int r, ref int g, ref int b)
        {
            r = convert_bit_range((int)(c >> 11) & 31, 5, 8);
            g = convert_bit_range((int)(c >> 05) & 63, 6, 8);
            b = convert_bit_range((int)(c >> 00) & 31, 5, 8);
        }
        void LSE_master_colors_max_min(ref int cmax, ref int cmin, int channels, byte[] uncompressed)
        {

            int i, j;
            /*	the master colors	*/
            int[] c0 = new int[3];
            int[] c1 = new int[3];
            /*	used for fitting the line	*/
            float[] sum_x = new float[] { 0.0f, 0.0f, 0.0f };
            float[] sum_x2 = new float[] { 0.0f, 0.0f, 0.0f };
            float dot_max = 1.0f, dot_min = -1.0f;
            float vec_len2 = 0.0f;
            float dot;
            /*	error check	*/
            if ((channels < 3) || (channels > 4))
            {
                return;
            }

            compute_color_line_STDEV(uncompressed, channels, sum_x, sum_x2);
            vec_len2 = 1.0f / (0.00001f +
                sum_x2[0] * sum_x2[0] + sum_x2[1] * sum_x2[1] + sum_x2[2] * sum_x2[2]);
            /*	finding the max and min vector values	*/
            dot_max =
                    (
                        sum_x2[0] * uncompressed[0] +
                        sum_x2[1] * uncompressed[1] +
                        sum_x2[2] * uncompressed[2]
                    );
            dot_min = dot_max;
            for (i = 1; i < 16; ++i)
            {
                dot =
                    (
                        sum_x2[0] * uncompressed[i * channels + 0] +
                        sum_x2[1] * uncompressed[i * channels + 1] +
                        sum_x2[2] * uncompressed[i * channels + 2]
                    );
                if (dot < dot_min)
                {
                    dot_min = dot;
                }
                else if (dot > dot_max)
                {
                    dot_max = dot;
                }
            }
            /*	and the offset (from the average location)	*/
            dot = sum_x2[0] * sum_x[0] + sum_x2[1] * sum_x[1] + sum_x2[2] * sum_x[2];
            dot_min -= dot;
            dot_max -= dot;
            /*	post multiply by the scaling factor	*/
            dot_min *= vec_len2;
            dot_max *= vec_len2;
            /*	OK, build the master colors	*/
            for (i = 0; i < 3; ++i)
            {
                /*	color 0	*/
                c0[i] = (int)(0.5f + sum_x[i] + dot_max * sum_x2[i]);
                if (c0[i] < 0)
                {
                    c0[i] = 0;
                }
                else if (c0[i] > 255)
                {
                    c0[i] = 255;
                }
                /*	color 1	*/
                c1[i] = (int)(0.5f + sum_x[i] + dot_min * sum_x2[i]);
                if (c1[i] < 0)
                {
                    c1[i] = 0;
                }
                else if (c1[i] > 255)
                {
                    c1[i] = 255;
                }
            }
            /*	down_sample (with rounding?)	*/
            i = rgb_to_565(c0[0], c0[1], c0[2]);
            j = rgb_to_565(c1[0], c1[1], c1[2]);
            if (i > j)
            {
                cmax = i;
                cmin = j;
            }
            else
            {
                cmax = j;
                cmin = i;
            }
        }
        void compress_DDS_color_block(int channels, byte[] uncompressed, byte[] compressed)
        {
            /*	variables	*/
            int i;
            int next_bit;
            int enc_c0 = 0, enc_c1 = 0;
            int[] c0 = new int[4];
            int[] c1 = new int[4];
            float[] color_line = new float[] { 0.0f, 0.0f, 0.0f, 0.0f };
            float vec_len2 = 0.0f, dot_offset = 0.0f;
            /*	stupid order	*/
            int[] swizzle4 = new int[] { 0, 2, 3, 1 };
            /*	get the master colors	*/
            LSE_master_colors_max_min(ref enc_c0, ref enc_c1, channels, uncompressed);
            /*	store the 565 color 0 and color 1	*/
            compressed[0] = (byte)((enc_c0 >> 0) & 255);
            compressed[1] = (byte)((enc_c0 >> 8) & 255);
            compressed[2] = (byte)((enc_c1 >> 0) & 255);
            compressed[3] = (byte)((enc_c1 >> 8) & 255);
            /*	zero out the compressed data	*/
            compressed[4] = 0;
            compressed[5] = 0;
            compressed[6] = 0;
            compressed[7] = 0;
            /*	reconstitute the master color vectors	*/
            rgb_888_from_565((uint)enc_c0, ref c0[0], ref c0[1], ref c0[2]);

            rgb_888_from_565((uint)enc_c1, ref c1[0], ref c1[1], ref c1[2]);
            /*	the new vector	*/
            vec_len2 = 0.0f;
            for (i = 0; i < 3; ++i)
            {
                color_line[i] = (float)(c1[i] - c0[i]);
                vec_len2 += color_line[i] * color_line[i];
            }
            if (vec_len2 > 0.0f)
            {
                vec_len2 = 1.0f / vec_len2;
            }
            /*	pre-proform the scaling	*/
            color_line[0] *= vec_len2;
            color_line[1] *= vec_len2;
            color_line[2] *= vec_len2;
            /*	compute the offset (constant) portion of the dot product	*/
            dot_offset = color_line[0] * c0[0] + color_line[1] * c0[1] + color_line[2] * c0[2];
            /*	store the rest of the bits	*/
            next_bit = 8 * 4;
            for (i = 0; i < 16; ++i)
            {
                /*	find the dot product of this color, to place it on the line
                    (should be [-1,1])	*/
                int next_value = 0;
                float dot_product =
                    color_line[0] * uncompressed[i * channels + 0] +
                    color_line[1] * uncompressed[i * channels + 1] +
                    color_line[2] * uncompressed[i * channels + 2] -
                    dot_offset;
                /*	map to [0,3]	*/
                next_value = (int)(dot_product * 3.0f + 0.5f);
                if (next_value > 3)
                {
                    next_value = 3;
                }
                else if (next_value < 0)
                {
                    next_value = 0;
                }
                /*	OK, store this value	*/
                compressed[next_bit >> 3] |= (byte)(swizzle4[next_value] << (next_bit & 7));
                next_bit += 2;
            }
            /*	done compressing to DXT1	*/
        }
        void compress_DDS_alpha_block(byte[] uncompressed, byte[] compressed)
        {
            /*	variables	*/
            int i;
            int next_bit;
            int a0, a1;
            float scale_me;
            /*	stupid order	*/
            int[] swizzle8 = new int[] { 1, 7, 6, 5, 4, 3, 2, 0 };
            /*	get the alpha limits (a0 > a1)	*/
            a0 = a1 = uncompressed[3];
            for (i = 4 + 3; i < 16 * 4; i += 4)
            {
                if (uncompressed[i] > a0)
                {
                    a0 = uncompressed[i];
                }
                else if (uncompressed[i] < a1)
                {
                    a1 = uncompressed[i];
                }
            }
            /*	store those limits, and zero the rest of the compressed dataset	*/
            compressed[0] = (byte)a0;
            compressed[1] = (byte)a1;
            /*	zero out the compressed data	*/
            compressed[2] = 0;
            compressed[3] = 0;
            compressed[4] = 0;
            compressed[5] = 0;
            compressed[6] = 0;
            compressed[7] = 0;
            /*	store the all of the alpha values	*/
            next_bit = 8 * 2;
            scale_me = 7.9999f / (a0 - a1);
            for (i = 3; i < 16 * 4; i += 4)
            {
                /*	convert this alpha value to a 3 bit number	*/
                int svalue;
                int value = (int)((uncompressed[i] - a1) * scale_me);
                svalue = swizzle8[value & 7];
                /*	OK, store this value, start with the 1st byte	*/
                compressed[next_bit >> 3] |= (byte)(svalue << (next_bit & 7));
                if ((next_bit & 7) > 5)
                {
                    /*	spans 2 bytes, fill in the start of the 2nd byte	*/
                    compressed[1 + (next_bit >> 3)] |= (byte)(svalue >> (8 - (next_bit & 7)));
                }
                next_bit += 3;
            }
            /*	done compressing to DXT1	*/
        }

    }

}