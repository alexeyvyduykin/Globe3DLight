using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    //
    // Сводка:
    //     Not used directly.
    public enum TextureWrapMode
    {
        //
        // Сводка:
        //     Original was GL_CLAMP = 0x2900
        Clamp = 10496,
        //
        // Сводка:
        //     Original was GL_REPEAT = 0x2901
        Repeat = 10497,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_BORDER = 0x812D
        ClampToBorder = 33069,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_BORDER_ARB = 0x812D
        ClampToBorderArb = 33069,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_BORDER_NV = 0x812D
        ClampToBorderNv = 33069,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_BORDER_SGIS = 0x812D
        ClampToBorderSgis = 33069,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_EDGE = 0x812F
        ClampToEdge = 33071,
        //
        // Сводка:
        //     Original was GL_CLAMP_TO_EDGE_SGIS = 0x812F
        ClampToEdgeSgis = 33071,
        //
        // Сводка:
        //     Original was GL_MIRRORED_REPEAT = 0x8370
        MirroredRepeat = 33648
    }
}
