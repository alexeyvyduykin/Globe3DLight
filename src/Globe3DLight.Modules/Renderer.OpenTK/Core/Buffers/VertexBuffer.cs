using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class VertexBuffer : Disposable
    {
        public VertexBuffer(A.BufferUsageHint usageHint, int sizeInBytes)
        {
            bufferObject = new Buffer(A.BufferTarget.ArrayBuffer, usageHint, sizeInBytes);
        }

        public void CopyFromSystemMemory<T>(T[] bufferInSystemMemory, int destinationOffsetInBytes, int lengthInBytes) where T : struct
        {
            bufferObject.CopyFromSystemMemory(bufferInSystemMemory, destinationOffsetInBytes, lengthInBytes);
        }

        public void CopyFromSystemMemory<T>(T[] bufferInSystemMemory) where T : struct
        {
            CopyFromSystemMemory<T>(bufferInSystemMemory, 0);
        }

        public void CopyFromSystemMemory<T>(T[] bufferInSystemMemory, int destinationOffsetInBytes) where T : struct
        {
            CopyFromSystemMemory<T>(bufferInSystemMemory, destinationOffsetInBytes, ArraySizeInBytes.Size(bufferInSystemMemory));
        }

        public T[] CopyToSystemMemory<T>(int offsetInBytes, int sizeInBytes) where T : struct
        {
            return bufferObject.CopyToSystemMemory<T>(offsetInBytes, sizeInBytes);
        }

        public T[] CopyToSystemMemory<T>() where T : struct
        {
            return CopyToSystemMemory<T>(0, SizeInBytes);
        }

        public int SizeInBytes
        {
            get { return bufferObject.SizeInBytes; }
        }

        public A.BufferUsageHint UsageHint
        {
            get { return bufferObject.UsageHint; }
        }

        public void Bind()
        {
            bufferObject.Bind();
        }

        internal static void UnBind()
        {
            A.GL.BindBuffer(A.BufferTarget.ArrayBuffer, 0);
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
    }
}