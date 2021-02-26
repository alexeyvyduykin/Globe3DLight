using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal enum IndexBufferDatatype
    {
        UnsignedShort,
        UnsignedInt
    }

    internal class IndexBuffer : Disposable
    {
        public IndexBuffer(A.BufferUsageHint usageHint, int sizeInBytes)
        {
            bufferObject = new Buffer(A.BufferTarget.ElementArrayBuffer, usageHint, sizeInBytes);
        }

        internal void Bind()
        {
            bufferObject.Bind();
        }

        internal static void UnBind()
        {
            A.GL.BindBuffer(A.BufferTarget.ElementArrayBuffer, 0);
        }

        public int Count
        {
            get { return count; }
        }

        public void CopyFromSystemMemory<T>(T[] bufferInSystemMemory) where T : struct
        {
            CopyFromSystemMemory<T>(bufferInSystemMemory, 0);
        }

        public void CopyFromSystemMemory<T>(T[] bufferInSystemMemory, int destinationOffsetInBytes) where T : struct
        {
            CopyFromSystemMemory<T>(bufferInSystemMemory, destinationOffsetInBytes, ArraySizeInBytes.Size(bufferInSystemMemory));
        }

        public void CopyFromSystemMemory<T>(
            T[] bufferInSystemMemory,
            int destinationOffsetInBytes,
            int lengthInBytes) where T : struct
        {
            if (typeof(T) == typeof(ushort))
            {
                dataType = IndexBufferDatatype.UnsignedShort;
            }
            else if (typeof(T) == typeof(uint))
            {
                dataType = IndexBufferDatatype.UnsignedInt;
            }
            else
            {
                throw new ArgumentException(
                    "bufferInSystemMemory must be an array of ushort or uint.",
                    "bufferInSystemMemory");
            }

            count = bufferObject.SizeInBytes / SizeInBytes<T>.Value;
            bufferObject.CopyFromSystemMemory(bufferInSystemMemory, destinationOffsetInBytes, lengthInBytes);
        }

        public T[] CopyToSystemMemory<T>() where T : struct
        {
            return CopyToSystemMemory<T>(0, SizeInBytes);
        }

        public T[] CopyToSystemMemory<T>(int offsetInBytes, int sizeInBytes) where T : struct
        {
            return bufferObject.CopyToSystemMemory<T>(offsetInBytes, sizeInBytes);
        }

        public int SizeInBytes
        {
            get { return bufferObject.SizeInBytes; }
        }

        public A.BufferUsageHint UsageHint
        {
            get { return bufferObject.UsageHint; }
        }

        public IndexBufferDatatype Datatype
        {
            get { return dataType; }
        }

        #region Disposable Members

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bufferObject.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        Buffer bufferObject;
        IndexBufferDatatype dataType;
        int count;
    }
}
