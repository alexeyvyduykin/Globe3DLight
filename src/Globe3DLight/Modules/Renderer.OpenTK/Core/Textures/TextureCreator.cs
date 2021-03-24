using System;
using System.Collections.Generic;
using System.Text;
using A = OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;
using System.IO;
using Globe3DLight.Models.Image;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class TextureCreator
    {
        public const int DDSCAPS2_CUBEMAP = 0x00000200;
        public const int DDSCAPS_MIPMAP = 0x00400000;

        private bool HasFlag(uint a, uint b)
        {
            return (a & b) == b;
        }


        public int Create1(IDdsImage image1, uint reuse_texture_ID, int loading_as_cubemap)
        {
            IDdsImage DdsImage = image1;
            var Header = DdsImage.Header;
   
            A.PixelInternalFormat S3TC_type = 0;
            A.PixelFormat S3TC_type_ = 0;
            byte[] DDS_data;
            uint DDS_main_size;
            uint DDS_full_size;         
            int mipmaps, block_size = 16;
            A.TextureTarget ogl_target_start = A.TextureTarget.Texture2D, ogl_target_end = A.TextureTarget.Texture2D;
            A.TextureTarget opengl_texture_type = A.TextureTarget.Texture2D;
       
            var width = image1.Width;
            var height = image1.Height;
            var compressed = image1.Compressed;// 1 - (int)(header.sPixelFormat.dwFlags & DDPF_FOURCC) / DDPF_FOURCC;
            var cubemap = (int)(Header.Caps2 & DDSCAPS2_CUBEMAP) / DDSCAPS2_CUBEMAP;
        
            if (compressed == true)
            {
                S3TC_type = A.PixelInternalFormat.Rgb;
                S3TC_type_ = A.PixelFormat.Rgb;
                block_size = 3;

                if (Header.PixelFormat.Flags == DdsPixelFormatFlags.AlphaPixels)
                {
                    S3TC_type = A.PixelInternalFormat.Rgba;
                    S3TC_type_ = A.PixelFormat.Rgba;
                    block_size = 4;
                }
                DDS_main_size = (uint)(width * height * block_size);
            }
            else
            {
                /*	well, we know it is DXT1/3/5, because we checked above	*/
                switch (Header.PixelFormat.FourCC)
                {
                    case CompressionAlgorithm.D3DFMT_DXT1:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;// SOIL_RGBA_S3TC_DXT1;
                        block_size = 8;
                        break;
                    case CompressionAlgorithm.D3DFMT_DXT3:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;// SOIL_RGBA_S3TC_DXT3;
                        block_size = 16;
                        break;
                    case CompressionAlgorithm.D3DFMT_DXT5:
                        S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;// SOIL_RGBA_S3TC_DXT5;
                        block_size = 16;
                        break;
                    default:
                        throw new Exception();
                }
                DDS_main_size = (uint)(((width + 3) >> 2) * ((height + 3) >> 2) * block_size);
            }
            if (cubemap != 0)
            {
                /* does the user want a cubemap?	*/
                if (loading_as_cubemap == 0)
                {
                    /*	we can't do it!	*/
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
                    return 0;
                }
                ogl_target_start = A.TextureTarget.Texture2D;
                ogl_target_end = A.TextureTarget.Texture2D;
                opengl_texture_type = A.TextureTarget.Texture2D;
            }

            if (HasFlag(Header.Caps1, DDSCAPS_MIPMAP) && (Header.MipMapCount > 1))
            {
                int shift_offset;
                mipmaps = (int)(Header.MipMapCount - 1);
                DDS_full_size = DDS_main_size;
                if (compressed == true)
                {
                    /*	uncompressed DDS, simple MIPmap size calculation	*/
                    shift_offset = 0;
                }
                else
                {
                    /*	compressed DDS, MIPmap size calculation is block based	*/
                    shift_offset = 2;
                }
                for (int i = 1; i <= mipmaps; ++i)
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
            var tex_ID = (int)reuse_texture_ID;
            if (tex_ID == 0)
            {
                tex_ID = A.GL.GenTexture();
            }
            /*  bind an OpenGL texture ID	*/
            A.GL.BindTexture(opengl_texture_type, tex_ID);
            /*	do this for each face of the cubemap!	*/

            uint buffer_index = 0;

            for (var cf_target = ogl_target_start; cf_target <= ogl_target_end; ++cf_target)
            {
                if (buffer_index + DDS_full_size <= (uint)image1.Data.Length/* DataLen*//* buffer_length*/)
                {
                    uint byte_offset = DDS_main_size;

                    Array.Copy(image1.Data, (int)buffer_index, DDS_data, 0, (int)DDS_full_size);

                    buffer_index += DDS_full_size;
                    /*	upload the main chunk	*/

                    if (compressed == true)
                    {
                        /*	and remember, DXT uncompressed uses BGR(A),
                            so swap to RGB(A) for ALL MIPmap levels	*/
                        for (int i = 0; i < (int)DDS_full_size; i += block_size)
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
                        A.GL.CompressedTexImage2D(
                            cf_target, 0,
                            S3TC_type, width, height, 0,
                            (int)DDS_main_size, DDS_data);

                    }
                    /*	upload the mipmaps, if we have them	*/
                    for (int i = 1; i <= mipmaps; ++i)
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
                                if (compressed == true)
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
                }
                else
                {
                    A.GL.DeleteTexture(tex_ID);
                    tex_ID = 0;
                    cf_target = ogl_target_end + 1;
                }
            }/* end reading each face */
     
            if (tex_ID != 0)
            {
                /*	did I have MIPmaps?	*/
                if (mipmaps > 0)
                {
                    /*	instruct OpenGL to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)A.TextureMagFilter.Linear);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)A.TextureMinFilter.LinearMipmapLinear);
                }
                else
                {
                    /*	instruct OpenGL _NOT_ to use the MIPmaps	*/
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMagFilter, (int)A.TextureMagFilter.Linear);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureMinFilter, (int)A.TextureMinFilter.Linear);
                }
                /*	does the user want clamping, or wrapping?	*/
                //if (false/*(flags & SOILFlags.TextureRepeats) != 0*/)
                //{
                //    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.Repeat);
                //    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.Repeat);
                //    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, (int)A.TextureWrapMode.Repeat);
                //}
                //else
                //{
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapS, (int)A.TextureWrapMode.Clamp);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapT, (int)A.TextureWrapMode.Clamp);
                    A.GL.TexParameter(opengl_texture_type, A.TextureParameterName.TextureWrapR, (int)A.TextureWrapMode.Clamp);
                //}
            }

            return tex_ID;
        }


        public int Create(IDdsImage image1, uint reuse_texture_ID, int loading_as_cubemap)
        {
            var header = image1.Header;
            int tex_ID = 0;
            A.PixelInternalFormat S3TC_type = 0;
            byte[] DDS_data;
            uint DDS_main_size;
            uint DDS_full_size;
            int width, height;
            int mipmaps, cubemap, block_size = 16;
            //            int uncompressed = 0;
            //uint flag;
            A.TextureTarget ogl_target_start, ogl_target_end;
            A.TextureTarget opengl_texture_type;
            int i;

            uint buffer_index = 0;// (uint)Marshal.SizeOf(typeof(DDS_header));// (uint)headerLength;// (uint)Marshal.SizeOf(header);

            /*	OK, validated the header, let's load the image data	*/
            width = (int)header.Width;
            height = (int)header.Height;
            //   uncompressed = 1 - (int)(header.sPixelFormat.dwFlags & DDPF_FOURCC) / DDPF_FOURCC;
            cubemap = (int)(header.Caps2 & DDSCAPS2_CUBEMAP) / DDSCAPS2_CUBEMAP;

            var buffer = image1.Data;
            var buffer_length = image1.Data.Length;


            //-----------------------------------
            /*	well, we know it is DXT1/3/5, because we checked above	*/
            switch (header.PixelFormat.FourCC)
            {
                case CompressionAlgorithm.D3DFMT_DXT1:
                    S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;// SOIL_RGBA_S3TC_DXT1;
                    block_size = 8;
                    break;
                case CompressionAlgorithm.D3DFMT_DXT3:
                    S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;// SOIL_RGBA_S3TC_DXT3;
                    block_size = 16;
                    break;
                case CompressionAlgorithm.D3DFMT_DXT5:
                    S3TC_type = A.PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;// SOIL_RGBA_S3TC_DXT5;
                    block_size = 16;
                    break;
                default:
                    throw new Exception();
            }
            DDS_main_size = (uint)(((width + 3) >> 2) * ((height + 3) >> 2) * block_size);
            //--------------------------------------


            if (cubemap != 0)
            {
                /* does the user want a cubemap?	*/
                if (loading_as_cubemap == 0)
                {
                    /*	we can't do it!	*/
                    return 0;
                }
                /*	can we even handle cubemaps with the OpenGL driver?	*/
                //if (query_cubemap_capability() != SOILCapability.Present)
                //{
                //    /*	we can't do it!	*/
                //    return 0;
                //}
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
                    return 0;
                }
                ogl_target_start = A.TextureTarget.Texture2D;
                ogl_target_end = A.TextureTarget.Texture2D;
                opengl_texture_type = A.TextureTarget.Texture2D;
            }

            //if (((header.sCaps.dwCaps1 & DDSCAPS_MIPMAP) != 0) && (header.dwMipMapCount > 1))
            if (HasFlag(header.Caps1, DDSCAPS_MIPMAP) && (header.MipMapCount > 1))
            {
                mipmaps = (int)(header.MipMapCount - 1);
                DDS_full_size = DDS_main_size;
                //---------------------------------
                /*	compressed DDS, MIPmap size calculation is block based	*/
                int shift_offset = 2;
                //--------------------------------
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
            for (var cf_target = ogl_target_start; cf_target <= ogl_target_end; ++cf_target)
            {

                if (buffer_index + DDS_full_size <= (uint)buffer_length)
                {

                    uint byte_offset = DDS_main_size;

                    Array.Copy(buffer, (int)buffer_index, DDS_data, 0, (int)DDS_full_size);

                    buffer_index += DDS_full_size;
                    /*	upload the main chunk	*/

                    //----------------------------------------------
                    A.GL.CompressedTexImage2D(
                        cf_target, 0,
                        S3TC_type, width, height, 0,
                        (int)DDS_main_size, DDS_data);
                    //----------------------------------------------

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

                                //-------------------------------------------
                                mip_size = ((w + 3) / 4) * ((h + 3) / 4) * block_size;
                                A.GL.CompressedTexImage2D(cf_target, i,
                                    S3TC_type, w, h, 0,
                                    mip_size, (IntPtr)pt);
                                //--------------------------------------------
                            }
                        }
                        /*	and move to the next mipmap	*/
                        byte_offset += (uint)mip_size;
                    }
                    /*	it worked!	*/
                }
                else
                {
                    A.GL.DeleteTexture(tex_ID);
                    tex_ID = 0;
                    cf_target = ogl_target_end + 1;
                }
            }/* end reading each face */


            return tex_ID;
        }

    }
}
