using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;

namespace Globe3DLight.Models.Renderer
{
    public interface IIndexBuffer : IBuffer
    {
        int Count { get; }

        //void CopyFromSystemMemory<T>(T[] bufferInSystemMemory) where T : struct;

        void CopyFromSystemMemory<T>(T[] bufferInSystemMemory, int destinationOffsetInBytes) where T : struct;

        T[] CopyToSystemMemory<T>() where T : struct;

        IndexBufferDatatype Datatype { get; }
    }
}
