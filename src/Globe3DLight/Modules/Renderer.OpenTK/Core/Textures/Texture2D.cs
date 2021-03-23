using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;
using OpenTKTextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;
using System.Drawing;
using System.Diagnostics;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class Texture2D : Disposable
    {
        //public Texture2D(Texture2DDescription description, TextureTarget textureTarget)
        //{
        //    if (description.Width <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("description.Width", "description.Width must be greater than zero.");
        //    }

        //    if (description.Height <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("description.Height", "description.Height must be greater than zero.");
        //    }

        //    if (description.GenerateMipmaps)
        //    {
        //        if (textureTarget == TextureTarget.TextureRectangle)
        //        {
        //            throw new ArgumentException("description.GenerateMipmaps cannot be true for texture rectangles.", "description");
        //        }

        //        if (!TextureUtility.IsPowerOfTwo(Convert.ToUInt32(description.Width)))
        //        {
        //            throw new ArgumentException("When description.GenerateMipmaps is true, the width must be a power of two.", "description");
        //        }

        //        if (!TextureUtility.IsPowerOfTwo(Convert.ToUInt32(description.Height)))
        //        {
        //            throw new ArgumentException("When description.GenerateMipmaps is true, the height must be a power of two.", "description");
        //        }
        //    }

        //    this.name = GL.GenTexture();
        //    this.target = textureTarget;
        //    this.description = description;
        //    //this.lastTextureUnit = OpenTKTextureUnit.Texture0 + (Device.NumberOfTextureUnits - 1);

        //    //
        //    // TexImage2D is just used to allocate the texture so a PBO can't be bound.
        //    //
        //    WritePixelBuffer.UnBind();
        //    BindToLastTextureUnit();
        //    GL.TexImage2D(target, 0,
        //        description.TextureToPixelInternalFormat(),
        //        description.Width,
        //        description.Height,
        //        0,
        //        description.TextureToPixelFormat(),
        //        description.TextureToPixelType(),
        //        new IntPtr());

        //    //
        //    // Default sampler, compatiable when attaching a non-mimapped 
        //    // texture to a frame buffer object.
        //    //
        //    ApplySampler(Device.TextureSamplers.LinearClamp);

        //    GC.AddMemoryPressure(description.ApproximateSizeInBytes);
        //}

        public Texture2D(int name, A.TextureTarget target)
        {
            this.name = name;
            this.target = target;
            //this.lastTextureUnit = OpenTKTextureUnit.Texture0 + (Device.NumberOfTextureUnits - 1);
        }

        //public void CopyFromBuffer(WritePixelBuffer pixelBuffer, PixelFormat format, PixelType dataType)
        //{
        //    CopyFromBuffer(pixelBuffer, 0, 0, Description.Width, Description.Height, format, dataType, 4);
        //}

        //public void CopyFromBuffer(WritePixelBuffer pixelBuffer, PixelFormat format, PixelType dataType, int rowAlignment)
        //{
        //    CopyFromBuffer(pixelBuffer, 0, 0, Description.Width, Description.Height, format, dataType, rowAlignment);
        //}

        //public void CopyFromBuffer(
        //    WritePixelBuffer pixelBuffer,
        //    int xOffset,
        //    int yOffset,
        //    int width,
        //    int height,
        //    PixelFormat format,
        //    PixelType dataType,
        //    int rowAlignment)
        //{
        //    if (pixelBuffer.SizeInBytes < TextureUtility.RequiredSizeInBytes(
        //        width, height, format, dataType, rowAlignment))
        //    {
        //        throw new ArgumentException("Pixel buffer is not big enough for provided width, height, format, and datatype.");
        //    }

        //    if (xOffset < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("xOffset", "xOffset must be greater than or equal to zero.");
        //    }

        //    if (yOffset < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("yOffset", "yOffset must be greater than or equal to zero.");
        //    }

        //    if (xOffset + width > description.Width)
        //    {
        //        throw new ArgumentOutOfRangeException("xOffset + width must be less than or equal to Description.Width");
        //    }

        //    if (yOffset + height > description.Height)
        //    {
        //        throw new ArgumentOutOfRangeException("yOffset + height must be less than or equal to Description.Height");
        //    }

        //    VerifyRowAlignment(rowAlignment);

        //    WritePixelBuffer bufferObjectGL = pixelBuffer as WritePixelBuffer;

        //    bufferObjectGL.Bind();
        //    BindToLastTextureUnit();
        //    GL.PixelStore(PixelStoreParameter.UnpackAlignment, rowAlignment);
        //    GL.TexSubImage2D(target, 0, xOffset, yOffset, width, height, format, dataType, new IntPtr());

        //    GenerateMipmaps();
        //}


        //public ReadPixelBuffer CopyToBuffer(PixelFormat format, PixelType dataType)
        //{
        //    return CopyToBuffer(format, dataType, 4);
        //}


        //public ReadPixelBuffer CopyToBuffer(PixelFormat format, PixelType dataType, int rowAlignment)
        //{
        //    if (format == PixelFormat.StencilIndex)
        //    {
        //        throw new ArgumentException("StencilIndex is not supported by CopyToBuffer.  Try DepthStencil instead.", "format");
        //    }

        //    VerifyRowAlignment(rowAlignment);

        //    ReadPixelBuffer pixelBuffer = new ReadPixelBuffer(PixelBufferHint.Stream,
        //        TextureUtility.RequiredSizeInBytes(description.Width, description.Height, format, dataType, rowAlignment));

        //    pixelBuffer.Bind();
        //    BindToLastTextureUnit();
        //    GL.PixelStore(PixelStoreParameter.PackAlignment, rowAlignment);
        //    GL.GetTexImage(target, 0, format, dataType, new IntPtr());

        //    return pixelBuffer;
        //}

        //public Texture2DDescription Description
        //{
        //    get { return description; }
        //}

        //public virtual void Save(string filename)
        //{
        //    if (Description.TextureFormat == TextureFormat.RedGreenBlue8)
        //    {
        //        SaveColor(filename);
        //    }
        //    else if ((Description.TextureFormat == TextureFormat.Depth16) ||
        //             (Description.TextureFormat == TextureFormat.Depth24) ||
        //             (Description.TextureFormat == TextureFormat.Depth32f))
        //    {
        //        SaveDepth(filename);
        //    }
        //    else if ((Description.TextureFormat == TextureFormat.Red32f))
        //    {
        //        SaveRed(filename);
        //    }
        //    else
        //    {
        //        Debug.Fail("Texture2D.Save() is not implement for this TextureFormat.");
        //    }
        //}

        //private void SaveColor(string filename)
        //{
        //    //
        //    // The pixel buffer uses four byte row alignment because it matches
        //    // a bitmap's row alignment (BitmapData.Stride).
        //    //
        //    using (ReadPixelBuffer pixelBuffer = CopyToBuffer(PixelFormat.Bgr, PixelType.UnsignedByte, 4))
        //    {
        //        Bitmap bitmap = pixelBuffer.CopyToBitmap(Description.Width, Description.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //        bitmap.Save(filename);
        //    }
        //}

        //private void SaveDepth(string filename)
        //{
        //    SaveFloat(filename, PixelFormat.DepthComponent);
        //}

        //private void SaveRed(string filename)
        //{
        //    SaveFloat(filename, PixelFormat.Red);
        //}

        //private void SaveFloat(string filename, PixelFormat imageFormat)
        //{
        //    using (ReadPixelBuffer pixelBuffer = CopyToBuffer(imageFormat, PixelType.Float, 1))
        //    {
        //        float[] depths = pixelBuffer.CopyToSystemMemory<float>();

        //        float minValue = depths[0];
        //        float maxValue = depths[0];
        //        for (int i = 0; i < depths.Length; ++i)
        //        {
        //            minValue = Math.Min(depths[i], minValue);
        //            maxValue = Math.Max(depths[i], maxValue);
        //        }
        //        float deltaValue = maxValue - minValue;

        //        //
        //        // Avoid divide by zero if all depths are the same.
        //        //
        //        float oneOverDelta = (deltaValue > 0) ? (1 / deltaValue) : 1;

        //        Bitmap bitmap = new Bitmap(Description.Width, Description.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //        int j = 0;
        //        for (int y = Description.Height - 1; y >= 0; --y)
        //        {
        //            for (int x = 0; x < Description.Width; ++x)
        //            {
        //                float linearDepth = (depths[j++] - minValue) * oneOverDelta;
        //                int intensity = (int)(linearDepth * 255.0f);
        //                bitmap.SetPixel(x, y, Color.FromArgb(intensity, intensity, intensity));
        //            }
        //        }
        //        bitmap.Save(filename);
        //    }
        //}

        //public int Handle
        //{
        //    get { return name; }
        //}

        internal A.TextureTarget Target
        {
            get { return target; }
        }

        internal void Bind()
        {
            A.GL.BindTexture(target, name);
        }

        //private void BindToLastTextureUnit()
        //{
        //    GL.ActiveTexture(lastTextureUnit);
        //    Bind();
        //}

        internal static void UnBind(A.TextureTarget textureTarget)
        {
            A.GL.BindTexture(textureTarget, 0);
        }

        //private void GenerateMipmaps()
        //{
        //    if (description.GenerateMipmaps)
        //    {
        //        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        //    }
        //}

        //private void ApplySampler(TextureSampler sampler)
        //{
        //    GL.TexParameter(target, TextureParameterName.TextureMinFilter, (int)sampler.MinificationFilter);
        //    GL.TexParameter(target, TextureParameterName.TextureMagFilter, (int)sampler.MagnificationFilter);
        //    GL.TexParameter(target, TextureParameterName.TextureWrapS, (int)sampler.WrapS);
        //    GL.TexParameter(target, TextureParameterName.TextureWrapT, (int)sampler.WrapT);
        //}

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (name != 0)
                {
                    A.GL.DeleteTexture(name);
                }
             //   GC.RemoveMemoryPressure(description.ApproximateSizeInBytes);
            }
            base.Dispose(disposing);
        }

        #endregion

        private void VerifyRowAlignment(int rowAlignment)
        {
            if ((rowAlignment != 1) &&
                (rowAlignment != 2) &&
                (rowAlignment != 4) &&
                (rowAlignment != 8))
            {
                throw new ArgumentException("rowAlignment");
            }
        }

        private readonly int name;
        private readonly A.TextureTarget target;
    //    private readonly Texture2DDescription description;
        //private readonly OpenTKTextureUnit lastTextureUnit;
    }


}
