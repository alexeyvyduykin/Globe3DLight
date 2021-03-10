using GlmSharp;
using OpenTK;
using OpenTK.Graphics;
using A = OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class Device
    {
        private int s_maximumNumberOfVertexAttributes = 16;
        //private int s_numberOfTextureUnits = 192;
        //private int s_maximumNumberOfColorAttachments;

        public bool AnisotropicFiltering => true;

        public int MaximumNumberOfVertexAttributes => s_maximumNumberOfVertexAttributes;

        public VertexBuffer CreateVertexBuffer<T>(IList<T> values, A.BufferUsageHint usageHint) where T : struct
        {
            T[] valuesArray = new T[values.Count];
            values.CopyTo(valuesArray, 0);

            VertexBuffer vertexBuffer = CreateVertexBuffer(usageHint, ArraySizeInBytes.Size(valuesArray));
            vertexBuffer.CopyFromSystemMemory(valuesArray);
            return vertexBuffer;
        }

        public VertexBuffer CreateVertexBuffer(A.BufferUsageHint usageHint, int sizeInBytes)
        {
            return new VertexBuffer(usageHint, sizeInBytes);
        }

        public IndexBuffer CreateIndexBuffer(A.BufferUsageHint usageHint, int sizeInBytes)
        {
            return new IndexBuffer(usageHint, sizeInBytes);
        }

        //public static IndexBuffer CreateIndexBuffer<T>(IList<T> values, BufferUsageHint usageHint) where T : struct
        //{
        //    T[] valuesArray = new T[values.Count];
        //    values.CopyTo(valuesArray, 0);

        //    IndexBuffer indexBuffer = Device.CreateIndexBuffer(usageHint, ArraySizeInBytes.Size(valuesArray));
        //    indexBuffer.CopyFromSystemMemory(valuesArray);
        //    return indexBuffer;
        //}

        //public static MeshBuffers CreateMeshBuffers(Globe3DCore.Mesh mesh, ShaderVertexAttributeCollection shaderAttributes, BufferUsageHint usageHint)
        //{
        //    if (mesh == null)
        //    {
        //        throw new ArgumentNullException("mesh");
        //    }

        //    if (shaderAttributes == null)
        //    {
        //        throw new ArgumentNullException("shaderAttributes");
        //    }

        //    MeshBuffers meshBuffers = new MeshBuffers();

        //    if (mesh.Indices != null)
        //    {
        //        if (mesh.Indices.Datatype == IndicesType.UnsignedShort)
        //        {
        //            IList<ushort> meshIndices = ((IndicesUnsignedShort)mesh.Indices).Values;

        //            ushort[] indices = new ushort[meshIndices.Count];
        //            for (int j = 0; j < meshIndices.Count; ++j)
        //            {
        //                indices[j] = meshIndices[j];
        //            }

        //            IndexBuffer indexBuffer = Device.CreateIndexBuffer(usageHint, indices.Length * sizeof(ushort));
        //            indexBuffer.CopyFromSystemMemory(indices);
        //            meshBuffers.IndexBuffer = indexBuffer;
        //        }
        //        else if (mesh.Indices.Datatype == IndicesType.UnsignedInt)
        //        {
        //            IList<uint> meshIndices = ((IndicesUnsignedInt)mesh.Indices).Values;

        //            uint[] indices = new uint[meshIndices.Count];
        //            for (int j = 0; j < meshIndices.Count; ++j)
        //            {
        //                indices[j] = meshIndices[j];
        //            }

        //            IndexBuffer indexBuffer = Device.CreateIndexBuffer(usageHint, indices.Length * sizeof(uint));
        //            indexBuffer.CopyFromSystemMemory(indices);
        //            meshBuffers.IndexBuffer = indexBuffer;
        //        }
        //        else
        //        {
        //            throw new NotSupportedException("mesh.Indices.Datatype " +
        //                mesh.Indices.Datatype.ToString() + " is not supported.");
        //        }
        //    }

        //    // TODO:  Not tested exhaustively
        //    foreach (ShaderVertexAttribute shaderAttribute in shaderAttributes)
        //    {
        //        if (!mesh.Attributes.Contains(shaderAttribute.Name))
        //        {
        //            throw new ArgumentException("Shader requires vertex attribute \"" + shaderAttribute.Name + "\", which is not present in mesh.");
        //        }

        //        VertexAttribute attribute = mesh.Attributes[shaderAttribute.Name];


        //        if (attribute.Datatype == VertexAttributeType.EmulatedDoubleVector3)
        //        {
        //            IList<dvec3> values = ((VertexAttribute<dvec3>)attribute).Values;

        //            vec3[] valuesArray = new vec3[values.Count];
        //            for (int i = 0; i < values.Count; ++i)
        //            {
        //                valuesArray[i] = values[i].ToVec3();
        //            }

        //            VertexBuffer vertexBuffer = Device.CreateVertexBuffer(usageHint, ArraySizeInBytes.Size(valuesArray));
        //            vertexBuffer.CopyFromSystemMemory(valuesArray);
        //            meshBuffers.Attributes[shaderAttribute.Location] =
        //                new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.Float, 3);
        //        }
        //        else if (attribute.Datatype == VertexAttributeType.Float)
        //        {
        //            VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<float>)attribute).Values, usageHint);

        //            meshBuffers.Attributes[shaderAttribute.Location] =
        //                new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.Float, 1);
        //        }
        //        else if (attribute.Datatype == VertexAttributeType.FloatVector2)
        //        {
        //            VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<vec2>)attribute).Values, usageHint);

        //            meshBuffers.Attributes[shaderAttribute.Location] =
        //                new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.Float, 2);
        //        }
        //        else if (attribute.Datatype == VertexAttributeType.FloatVector3)
        //        {
        //            VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<vec3>)attribute).Values, usageHint);

        //            meshBuffers.Attributes[shaderAttribute.Location] =
        //                new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.Float, 3);
        //        }
        //        else if (attribute.Datatype == VertexAttributeType.FloatVector4)
        //        {
        //            VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<vec4>)attribute).Values, usageHint);

        //            meshBuffers.Attributes[shaderAttribute.Location] =
        //                new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.Float, 4);
        //        }
        //        else if (attribute.Datatype == VertexAttributeType.UnsignedByte)
        //        {
        //            if (attribute is VertexAttributeRGBA)
        //            {
        //                VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<byte>)attribute).Values, usageHint);

        //                meshBuffers.Attributes[shaderAttribute.Location] =
        //                    new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.UnsignedByte, 4, true, 0, 0);
        //            }
        //            //else if (attribute is VertexAttributeRGB)
        //            //{
        //            //    VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<byte>)attribute).Values, usageHint);

        //            //    meshBuffers.Attributes[shaderAttribute.Location] =
        //            //        new VertexBufferAttribute(vertexBuffer, ComponentDatatype.UnsignedByte, 3, true, 0, 0);
        //            //}
        //            else
        //            {
        //                VertexBuffer vertexBuffer = CreateVertexBuffer(((VertexAttribute<byte>)attribute).Values, usageHint);

        //                meshBuffers.Attributes[shaderAttribute.Location] =
        //                    new VertexBufferAttribute(vertexBuffer, VertexAttribPointerType.UnsignedByte, 1);
        //            }
        //        }
        //        else
        //        {
        //            Debug.Fail("attribute.Datatype");
        //        }
        //    }

        //    return meshBuffers;
        //}

        public ShaderProgram CreateShaderProgram(string vertexShaderSource, string fragmentShaderSource)
        {
            return new ShaderProgram(vertexShaderSource, fragmentShaderSource);
        }

        public ShaderProgram CreateShaderProgram(string vertexShaderSource, string geometryShaderSource, string fragmentShaderSource)
        {
            return new ShaderProgram(vertexShaderSource, geometryShaderSource, fragmentShaderSource);
        }

        public DrawState CreateDrawState(ShaderProgram sp)
        {
            var drawState = new DrawState();
            drawState.ShaderProgram = sp;
            return drawState;
        }

        public TextureCreator CreateTextureCreator()
        {
            return new TextureCreator();
        }

        //public static Texture2D CreateTexture2D(Texture2DDescription description)
        //{
        //    return new Texture2D(description, TextureTarget.Texture2D);
        //}

        //public static Texture2D CreateTexture2D(Bitmap bitmap, TextureFormat format, bool generateMipmaps)
        //{
        //    return CreateTexture2DFromBitmap(bitmap, format, generateMipmaps, TextureTarget.Texture2D);
        //}

        //public static Texture2D CreateTexture2DRectangle(Texture2DDescription description)
        //{
        //    return new Texture2D(description, TextureTarget.TextureRectangle);
        //}

        //public static Texture2D CreateTexture2DRectangle(Bitmap bitmap, TextureFormat format)
        //{
        //    return CreateTexture2DFromBitmap(bitmap, format, false, TextureTarget.TextureRectangle);
        //}

        //private static Texture2D CreateTexture2DFromBitmap(Bitmap bitmap, TextureFormat format, bool generateMipmaps, TextureTarget textureTarget)
        //{
        //    using (WritePixelBuffer pixelBuffer = Device.CreateWritePixelBuffer(PixelBufferHint.Stream,
        //        BitmapAlgorithms.SizeOfPixelsInBytes(bitmap)))
        //    {
        //        pixelBuffer.CopyFromBitmap(bitmap);

        //        Texture2DDescription description = new Texture2DDescription(bitmap.Width, bitmap.Height, format, generateMipmaps);
        //        Texture2D texture = new Texture2D(description, textureTarget);
        //        texture.CopyFromBuffer(pixelBuffer,
        //            TextureUtility.ImagingPixelFormatToImageFormat(bitmap.PixelFormat),
        //            TextureUtility.ImagingPixelFormatToDatatype(bitmap.PixelFormat));

        //        return texture;
        //    }
        //}

        //public static WritePixelBuffer CreateWritePixelBuffer(PixelBufferHint usageHint, int sizeInBytes)
        //{
        //    return new WritePixelBuffer(usageHint, sizeInBytes);
        //}

        //public static TextureSamplerGL3x CreateTexture2DSampler(TextureMinFilter minificationFilter, TextureMagFilter magnificationFilter, TextureWrapMode wrapS, TextureWrapMode wrapT)
        //{
        //    return new TextureSamplerGL3x(minificationFilter, magnificationFilter, wrapS, wrapT, 1);
        //}

        //public static TextureSampler CreateTexture2DSampler(TextureMinFilter minificationFilter, TextureMagFilter magnificationFilter, TextureWrapMode wrapS, TextureWrapMode wrapT, float maximumAnistropy)
        //{
        //    return new TextureSamplerGL3x(minificationFilter, magnificationFilter, wrapS, wrapT, maximumAnistropy);
        //}

        //public static TextureSamplers TextureSamplers
        //{
        //    get { return s_textureSamplers; }
        //}

     

    }
}
