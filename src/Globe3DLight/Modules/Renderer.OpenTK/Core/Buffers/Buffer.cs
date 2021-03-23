using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal sealed class Buffer : IDisposable
    {
        public Buffer(A.BufferTarget type, A.BufferUsageHint usageHint, int sizeInBytes)
        {
            if (sizeInBytes <= 0)
            {
                throw new ArgumentOutOfRangeException("sizeInBytes", "sizeInBytes must be greater than zero.");
            }

            name = A.GL.GenBuffer();

            this.sizeInBytes = sizeInBytes;
            this.type = type;
            this.usageHint = usageHint;

            A.GL.BindVertexArray(0);
            Bind();
            A.GL.BufferData(type, new IntPtr(sizeInBytes), new IntPtr(), usageHint);

            GC.AddMemoryPressure(sizeInBytes);
        }

        public void CopyFromSystemMemory<T>(
            T[] bufferInSystemMemory, int destinationOffsetInBytes, int lengthInBytes) where T : struct
        {
            if (destinationOffsetInBytes < 0)
            {
                throw new ArgumentOutOfRangeException("destinationOffsetInBytes",
                    "destinationOffsetInBytes must be greater than or equal to zero.");
            }

            if (destinationOffsetInBytes + lengthInBytes > sizeInBytes)
            {
                throw new ArgumentOutOfRangeException(
                    "destinationOffsetInBytes + lengthInBytes must be less than or equal to SizeInBytes.");
            }

            if (lengthInBytes < 0)
            {
                throw new ArgumentOutOfRangeException("lengthInBytes",
                    "lengthInBytes must be greater than or equal to zero.");
            }

            if (lengthInBytes > ArraySizeInBytes.Size(bufferInSystemMemory))
            {
                throw new ArgumentOutOfRangeException("lengthInBytes",
                    "lengthInBytes must be less than or equal to the size of bufferInSystemMemory in bytes.");
            }

            A.GL.BindVertexArray(0);
            Bind();
            A.GL.BufferSubData<T>(type, new IntPtr(destinationOffsetInBytes), new IntPtr(lengthInBytes), bufferInSystemMemory);
        }

        public T[] CopyToSystemMemory<T>(int offsetInBytes, int lengthInBytes) where T : struct
        {
            if (offsetInBytes < 0)
            {
                throw new ArgumentOutOfRangeException("offsetInBytes",
                    "offsetInBytes must be greater than or equal to zero.");
            }

            if (lengthInBytes <= 0)
            {
                throw new ArgumentOutOfRangeException("lengthInBytes",
                    "lengthInBytes must be greater than zero.");
            }

            if (offsetInBytes + lengthInBytes > sizeInBytes)
            {
                throw new ArgumentOutOfRangeException(
                    "offsetInBytes + lengthInBytes must be less than or equal to SizeInBytes.");
            }

            T[] bufferInSystemMemory = new T[lengthInBytes / SizeInBytes<T>.Value];

            A.GL.BindVertexArray(0);
            Bind();
            A.GL.GetBufferSubData(type, new IntPtr(offsetInBytes), new IntPtr(lengthInBytes), bufferInSystemMemory);
            return bufferInSystemMemory;
        }

        public int SizeInBytes
        {
            get { return sizeInBytes; }
        }

        public A.BufferUsageHint UsageHint
        {
            get { return usageHint; }
        }

        public int Handle
        {
            get { return name; }
        }

        public void Dispose()
        {
            if (name != 0)
            {
                A.GL.DeleteBuffers(1, ref name);
                name = 0;
            }
            GC.RemoveMemoryPressure(sizeInBytes);
        }
    
        public void Bind()
        {
            A.GL.BindBuffer(type, name);
        }

        private int name;
        private readonly int sizeInBytes;
        private readonly A.BufferTarget type;
        private readonly A.BufferUsageHint usageHint;
    }

    //BuferHint

    //    DYNAMIC_DRAW - данные, записанные в буфферном объекте, скорее всего часто
    //    менятся не будут, но, вполне вероятно, несколько раз будут
    //    использоваться в качестве источника информации для рисования
    //    Подсказка сообщает OpenGL поместить данные туда, где однократное
    //    обновление не будет слишком болезненным

    //    STATIC_DRAW - данные, записанные в буфферном объекте, скорее всего часто
    //    менятся не будут, но, вполне вероятно, много раз будут использоваться
    //    в качестве источника информации для рисования
    //    Подсказка сообщает реализации поместить данные туда, где их будет удобно 
    //    использовать для быстрого рисования, но обновление, возможно,
    //    не будет таким быстрым

    //    STREAM_DRAW - данные, записанные в буфферном объекте, скорее всего 
    //    будут часто меняться и, вполне вероятно, будут использованы всего один раз
    //    (максимум два-три раза)
    //    Подсказка сообщает реализации, что это данные срочные, например - анимрованная
    //    геометрия, которая будет использована единожды, а затем измениться 
    //    Важно, чтобы такие данные были помещены туда, где их можно быстро обновить,
    //    даже если это условие выполниться за счёт быстрой визуализации

}
