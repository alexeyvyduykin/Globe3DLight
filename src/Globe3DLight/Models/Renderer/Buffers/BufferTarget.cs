using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Renderer
{
    //
    // Сводка:
    //     Used in GL.Apple.BufferParameter, GL.Apple.FlushMappedBufferRange and 16 other
    //     functions
    public enum BufferTarget
    {
        //
        // Сводка:
        //     Original was GL_ARRAY_BUFFER = 0x8892
        ArrayBuffer = 34962,
        //
        // Сводка:
        //     Original was GL_ELEMENT_ARRAY_BUFFER = 0x8893
        ElementArrayBuffer = 34963,
        //
        // Сводка:
        //     Original was GL_PIXEL_PACK_BUFFER = 0x88EB
        PixelPackBuffer = 35051,
        //
        // Сводка:
        //     Original was GL_PIXEL_UNPACK_BUFFER = 0x88EC
        PixelUnpackBuffer = 35052,
        //
        // Сводка:
        //     Original was GL_UNIFORM_BUFFER = 0x8A11
        UniformBuffer = 35345,
        //
        // Сводка:
        //     Original was GL_TEXTURE_BUFFER = 0x8C2A
        TextureBuffer = 35882,
        //
        // Сводка:
        //     Original was GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E
        TransformFeedbackBuffer = 35982,
        //
        // Сводка:
        //     Original was GL_COPY_READ_BUFFER = 0x8F36
        CopyReadBuffer = 36662,
        //
        // Сводка:
        //     Original was GL_COPY_WRITE_BUFFER = 0x8F37
        CopyWriteBuffer = 36663,
        //
        // Сводка:
        //     Original was GL_DRAW_INDIRECT_BUFFER = 0x8F3F
        DrawIndirectBuffer = 36671,
        //
        // Сводка:
        //     Original was GL_SHADER_STORAGE_BUFFER = 0x90D2
        ShaderStorageBuffer = 37074,
        //
        // Сводка:
        //     Original was GL_DISPATCH_INDIRECT_BUFFER = 0x90EE
        DispatchIndirectBuffer = 37102,
        //
        // Сводка:
        //     Original was GL_QUERY_BUFFER = 0x9192
        QueryBuffer = 37266,
        //
        // Сводка:
        //     Original was GL_ATOMIC_COUNTER_BUFFER = 0x92C0
        AtomicCounterBuffer = 37568
    }
}
