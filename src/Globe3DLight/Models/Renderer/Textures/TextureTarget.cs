using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    //
    // Сводка:
    //     Used in GL.Arb.CompressedTexImage1D, GL.Arb.CompressedTexImage2D and 124 other
    //     functions
    public enum TextureTarget
    {
        //
        // Сводка:
        //     Original was GL_TEXTURE_1D = 0x0DE0
        Texture1D = 3552,
        //
        // Сводка:
        //     Original was GL_TEXTURE_2D = 0x0DE1
        Texture2D = 3553,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_1D = 0x8063
        ProxyTexture1D = 32867,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_1D_EXT = 0x8063
        ProxyTexture1DExt = 32867,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_2D = 0x8064
        ProxyTexture2D = 32868,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_2D_EXT = 0x8064
        ProxyTexture2DExt = 32868,
        //
        // Сводка:
        //     Original was GL_TEXTURE_3D = 0x806F
        Texture3D = 32879,
        //
        // Сводка:
        //     Original was GL_TEXTURE_3D_EXT = 0x806F
        Texture3DExt = 32879,
        //
        // Сводка:
        //     Original was GL_TEXTURE_3D_OES = 0x806F
        Texture3DOes = 32879,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_3D = 0x8070
        ProxyTexture3D = 32880,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_3D_EXT = 0x8070
        ProxyTexture3DExt = 32880,
        //
        // Сводка:
        //     Original was GL_DETAIL_TEXTURE_2D_SGIS = 0x8095
        DetailTexture2DSgis = 32917,
        //
        // Сводка:
        //     Original was GL_TEXTURE_4D_SGIS = 0x8134
        Texture4DSgis = 33076,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_4D_SGIS = 0x8135
        ProxyTexture4DSgis = 33077,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MIN_LOD = 0x813A
        TextureMinLod = 33082,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MIN_LOD_SGIS = 0x813A
        TextureMinLodSgis = 33082,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MAX_LOD = 0x813B
        TextureMaxLod = 33083,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MAX_LOD_SGIS = 0x813B
        TextureMaxLodSgis = 33083,
        //
        // Сводка:
        //     Original was GL_TEXTURE_BASE_LEVEL = 0x813C
        TextureBaseLevel = 33084,
        //
        // Сводка:
        //     Original was GL_TEXTURE_BASE_LEVEL_SGIS = 0x813C
        TextureBaseLevelSgis = 33084,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MAX_LEVEL = 0x813D
        TextureMaxLevel = 33085,
        //
        // Сводка:
        //     Original was GL_TEXTURE_MAX_LEVEL_SGIS = 0x813D
        TextureMaxLevelSgis = 33085,
        //
        // Сводка:
        //     Original was GL_TEXTURE_RECTANGLE = 0x84F5
        TextureRectangle = 34037,
        //
        // Сводка:
        //     Original was GL_TEXTURE_RECTANGLE_ARB = 0x84F5
        TextureRectangleArb = 34037,
        //
        // Сводка:
        //     Original was GL_TEXTURE_RECTANGLE_NV = 0x84F5
        TextureRectangleNv = 34037,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_RECTANGLE = 0x84F7
        ProxyTextureRectangle = 34039,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP = 0x8513
        TextureCubeMap = 34067,
        //
        // Сводка:
        //     Original was GL_TEXTURE_BINDING_CUBE_MAP = 0x8514
        TextureBindingCubeMap = 34068,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515
        TextureCubeMapPositiveX = 34069,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516
        TextureCubeMapNegativeX = 34070,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517
        TextureCubeMapPositiveY = 34071,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518
        TextureCubeMapNegativeY = 34072,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519
        TextureCubeMapPositiveZ = 34073,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A
        TextureCubeMapNegativeZ = 34074,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_CUBE_MAP = 0x851B
        ProxyTextureCubeMap = 34075,
        //
        // Сводка:
        //     Original was GL_TEXTURE_1D_ARRAY = 0x8C18
        Texture1DArray = 35864,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19
        ProxyTexture1DArray = 35865,
        //
        // Сводка:
        //     Original was GL_TEXTURE_2D_ARRAY = 0x8C1A
        Texture2DArray = 35866,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_2D_ARRAY = 0x8C1B
        ProxyTexture2DArray = 35867,
        //
        // Сводка:
        //     Original was GL_TEXTURE_BUFFER = 0x8C2A
        TextureBuffer = 35882,
        //
        // Сводка:
        //     Original was GL_TEXTURE_CUBE_MAP_ARRAY = 0x9009
        TextureCubeMapArray = 36873,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_CUBE_MAP_ARRAY = 0x900B
        ProxyTextureCubeMapArray = 36875,
        //
        // Сводка:
        //     Original was GL_TEXTURE_2D_MULTISAMPLE = 0x9100
        Texture2DMultisample = 37120,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_2D_MULTISAMPLE = 0x9101
        ProxyTexture2DMultisample = 37121,
        //
        // Сводка:
        //     Original was GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102
        Texture2DMultisampleArray = 37122,
        //
        // Сводка:
        //     Original was GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9103
        ProxyTexture2DMultisampleArray = 37123
    }
}
