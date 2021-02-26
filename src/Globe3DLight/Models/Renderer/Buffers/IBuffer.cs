using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Globe3DLight.Renderer
{
    public interface IBuffer : IDisposable
    {
        void CopyFromSystemMemory<T>(T[] bufferInSystemMemory, int destinationOffsetInBytes, int lengthInBytes) where T : struct;
        
        void CopyFromSystemMemory<T>(T[] bufferInSystemMemory) where T : struct;
        
        T[] CopyToSystemMemory<T>(int offsetInBytes, int lengthInBytes) where T : struct;

        int SizeInBytes { get; }

        BufferTarget Target { get; }

        BufferUsageHint UsageHint { get; }

        object Handle { get; }

        void Bind();

        void UnBind();
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
