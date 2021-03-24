using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Renderer
{
    //
    // Сводка:
    //     Used in GL.Ati.VertexAttribArrayObject, GL.VertexAttribPointer and 1 other function
    public enum VertexAttribPointerType
    {
        //
        // Сводка:
        //     Original was GL_BYTE = 0x1400
        Byte = 5120,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_BYTE = 0x1401
        UnsignedByte = 5121,
        //
        // Сводка:
        //     Original was GL_SHORT = 0x1402
        Short = 5122,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_SHORT = 0x1403
        UnsignedShort = 5123,
        //
        // Сводка:
        //     Original was GL_INT = 0x1404
        Int = 5124,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT = 0x1405
        UnsignedInt = 5125,
        //
        // Сводка:
        //     Original was GL_FLOAT = 0x1406
        Float = 5126,
        //
        // Сводка:
        //     Original was GL_DOUBLE = 0x140A
        Double = 5130,
        //
        // Сводка:
        //     Original was GL_HALF_FLOAT = 0x140B
        HalfFloat = 5131,
        //
        // Сводка:
        //     Original was GL_FIXED = 0x140C
        Fixed = 5132,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368
        UnsignedInt2101010Rev = 33640,
        //
        // Сводка:
        //     Original was GL_INT_2_10_10_10_REV = 0x8D9F
        Int2101010Rev = 36255
    }
}
