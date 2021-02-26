using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    //
    // Сводка:
    //     Used in GL.BufferData, GL.NamedBufferData and 1 other function
    public enum BufferUsageHint
    {
        //
        // Сводка:
        //     Original was GL_STREAM_DRAW = 0x88E0
        StreamDraw = 35040,
        //
        // Сводка:
        //     Original was GL_STREAM_READ = 0x88E1
        StreamRead = 35041,
        //
        // Сводка:
        //     Original was GL_STREAM_COPY = 0x88E2
        StreamCopy = 35042,
        //
        // Сводка:
        //     Original was GL_STATIC_DRAW = 0x88E4
        StaticDraw = 35044,
        //
        // Сводка:
        //     Original was GL_STATIC_READ = 0x88E5
        StaticRead = 35045,
        //
        // Сводка:
        //     Original was GL_STATIC_COPY = 0x88E6
        StaticCopy = 35046,
        //
        // Сводка:
        //     Original was GL_DYNAMIC_DRAW = 0x88E8
        DynamicDraw = 35048,
        //
        // Сводка:
        //     Original was GL_DYNAMIC_READ = 0x88E9
        DynamicRead = 35049,
        //
        // Сводка:
        //     Original was GL_DYNAMIC_COPY = 0x88EA
        DynamicCopy = 35050
    }
}
