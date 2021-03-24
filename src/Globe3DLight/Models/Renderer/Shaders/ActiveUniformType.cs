using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Models.Renderer
{
    //
    // Сводка:
    //     Used in GL.GetActiveUniform
    public enum ActiveUniformType
    {
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
        //     Original was GL_FLOAT_VEC2 = 0x8B50
        FloatVec2 = 35664,
        //
        // Сводка:
        //     Original was GL_FLOAT_VEC3 = 0x8B51
        FloatVec3 = 35665,
        //
        // Сводка:
        //     Original was GL_FLOAT_VEC4 = 0x8B52
        FloatVec4 = 35666,
        //
        // Сводка:
        //     Original was GL_INT_VEC2 = 0x8B53
        IntVec2 = 35667,
        //
        // Сводка:
        //     Original was GL_INT_VEC3 = 0x8B54
        IntVec3 = 35668,
        //
        // Сводка:
        //     Original was GL_INT_VEC4 = 0x8B55
        IntVec4 = 35669,
        //
        // Сводка:
        //     Original was GL_BOOL = 0x8B56
        Bool = 35670,
        //
        // Сводка:
        //     Original was GL_BOOL_VEC2 = 0x8B57
        BoolVec2 = 35671,
        //
        // Сводка:
        //     Original was GL_BOOL_VEC3 = 0x8B58
        BoolVec3 = 35672,
        //
        // Сводка:
        //     Original was GL_BOOL_VEC4 = 0x8B59
        BoolVec4 = 35673,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT2 = 0x8B5A
        FloatMat2 = 35674,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT3 = 0x8B5B
        FloatMat3 = 35675,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT4 = 0x8B5C
        FloatMat4 = 35676,
        //
        // Сводка:
        //     Original was GL_SAMPLER_1D = 0x8B5D
        Sampler1D = 35677,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D = 0x8B5E
        Sampler2D = 35678,
        //
        // Сводка:
        //     Original was GL_SAMPLER_3D = 0x8B5F
        Sampler3D = 35679,
        //
        // Сводка:
        //     Original was GL_SAMPLER_CUBE = 0x8B60
        SamplerCube = 35680,
        //
        // Сводка:
        //     Original was GL_SAMPLER_1D_SHADOW = 0x8B61
        Sampler1DShadow = 35681,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_SHADOW = 0x8B62
        Sampler2DShadow = 35682,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_RECT = 0x8B63
        Sampler2DRect = 35683,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_RECT_SHADOW = 0x8B64
        Sampler2DRectShadow = 35684,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT2x3 = 0x8B65
        FloatMat2x3 = 35685,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT2x4 = 0x8B66
        FloatMat2x4 = 35686,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT3x2 = 0x8B67
        FloatMat3x2 = 35687,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT3x4 = 0x8B68
        FloatMat3x4 = 35688,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT4x2 = 0x8B69
        FloatMat4x2 = 35689,
        //
        // Сводка:
        //     Original was GL_FLOAT_MAT4x3 = 0x8B6A
        FloatMat4x3 = 35690,
        //
        // Сводка:
        //     Original was GL_SAMPLER_1D_ARRAY = 0x8DC0
        Sampler1DArray = 36288,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_ARRAY = 0x8DC1
        Sampler2DArray = 36289,
        //
        // Сводка:
        //     Original was GL_SAMPLER_BUFFER = 0x8DC2
        SamplerBuffer = 36290,
        //
        // Сводка:
        //     Original was GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3
        Sampler1DArrayShadow = 36291,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4
        Sampler2DArrayShadow = 36292,
        //
        // Сводка:
        //     Original was GL_SAMPLER_CUBE_SHADOW = 0x8DC5
        SamplerCubeShadow = 36293,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_VEC2 = 0x8DC6
        UnsignedIntVec2 = 36294,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_VEC3 = 0x8DC7
        UnsignedIntVec3 = 36295,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_VEC4 = 0x8DC8
        UnsignedIntVec4 = 36296,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_1D = 0x8DC9
        IntSampler1D = 36297,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_2D = 0x8DCA
        IntSampler2D = 36298,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_3D = 0x8DCB
        IntSampler3D = 36299,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_CUBE = 0x8DCC
        IntSamplerCube = 36300,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_2D_RECT = 0x8DCD
        IntSampler2DRect = 36301,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_1D_ARRAY = 0x8DCE
        IntSampler1DArray = 36302,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_2D_ARRAY = 0x8DCF
        IntSampler2DArray = 36303,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_BUFFER = 0x8DD0
        IntSamplerBuffer = 36304,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1
        UnsignedIntSampler1D = 36305,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2
        UnsignedIntSampler2D = 36306,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3
        UnsignedIntSampler3D = 36307,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4
        UnsignedIntSamplerCube = 36308,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5
        UnsignedIntSampler2DRect = 36309,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6
        UnsignedIntSampler1DArray = 36310,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7
        UnsignedIntSampler2DArray = 36311,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8
        UnsignedIntSamplerBuffer = 36312,
        //
        // Сводка:
        //     Original was GL_DOUBLE_VEC2 = 0x8FFC
        DoubleVec2 = 36860,
        //
        // Сводка:
        //     Original was GL_DOUBLE_VEC3 = 0x8FFD
        DoubleVec3 = 36861,
        //
        // Сводка:
        //     Original was GL_DOUBLE_VEC4 = 0x8FFE
        DoubleVec4 = 36862,
        //
        // Сводка:
        //     Original was GL_SAMPLER_CUBE_MAP_ARRAY = 0x900C
        SamplerCubeMapArray = 36876,
        //
        // Сводка:
        //     Original was GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW = 0x900D
        SamplerCubeMapArrayShadow = 36877,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900E
        IntSamplerCubeMapArray = 36878,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY = 0x900F
        UnsignedIntSamplerCubeMapArray = 36879,
        //
        // Сводка:
        //     Original was GL_IMAGE_1D = 0x904C
        Image1D = 36940,
        //
        // Сводка:
        //     Original was GL_IMAGE_2D = 0x904D
        Image2D = 36941,
        //
        // Сводка:
        //     Original was GL_IMAGE_3D = 0x904E
        Image3D = 36942,
        //
        // Сводка:
        //     Original was GL_IMAGE_2D_RECT = 0x904F
        Image2DRect = 36943,
        //
        // Сводка:
        //     Original was GL_IMAGE_CUBE = 0x9050
        ImageCube = 36944,
        //
        // Сводка:
        //     Original was GL_IMAGE_BUFFER = 0x9051
        ImageBuffer = 36945,
        //
        // Сводка:
        //     Original was GL_IMAGE_1D_ARRAY = 0x9052
        Image1DArray = 36946,
        //
        // Сводка:
        //     Original was GL_IMAGE_2D_ARRAY = 0x9053
        Image2DArray = 36947,
        //
        // Сводка:
        //     Original was GL_IMAGE_CUBE_MAP_ARRAY = 0x9054
        ImageCubeMapArray = 36948,
        //
        // Сводка:
        //     Original was GL_IMAGE_2D_MULTISAMPLE = 0x9055
        Image2DMultisample = 36949,
        //
        // Сводка:
        //     Original was GL_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9056
        Image2DMultisampleArray = 36950,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_1D = 0x9057
        IntImage1D = 36951,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_2D = 0x9058
        IntImage2D = 36952,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_3D = 0x9059
        IntImage3D = 36953,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_2D_RECT = 0x905A
        IntImage2DRect = 36954,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_CUBE = 0x905B
        IntImageCube = 36955,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_BUFFER = 0x905C
        IntImageBuffer = 36956,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_1D_ARRAY = 0x905D
        IntImage1DArray = 36957,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_2D_ARRAY = 0x905E
        IntImage2DArray = 36958,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_CUBE_MAP_ARRAY = 0x905F
        IntImageCubeMapArray = 36959,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_2D_MULTISAMPLE = 0x9060
        IntImage2DMultisample = 36960,
        //
        // Сводка:
        //     Original was GL_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x9061
        IntImage2DMultisampleArray = 36961,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_1D = 0x9062
        UnsignedIntImage1D = 36962,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_2D = 0x9063
        UnsignedIntImage2D = 36963,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_3D = 0x9064
        UnsignedIntImage3D = 36964,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_2D_RECT = 0x9065
        UnsignedIntImage2DRect = 36965,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_CUBE = 0x9066
        UnsignedIntImageCube = 36966,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_BUFFER = 0x9067
        UnsignedIntImageBuffer = 36967,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_1D_ARRAY = 0x9068
        UnsignedIntImage1DArray = 36968,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_2D_ARRAY = 0x9069
        UnsignedIntImage2DArray = 36969,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY = 0x906A
        UnsignedIntImageCubeMapArray = 36970,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE = 0x906B
        UnsignedIntImage2DMultisample = 36971,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY = 0x906C
        UnsignedIntImage2DMultisampleArray = 36972,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_MULTISAMPLE = 0x9108
        Sampler2DMultisample = 37128,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109
        IntSampler2DMultisample = 37129,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A
        UnsignedIntSampler2DMultisample = 37130,
        //
        // Сводка:
        //     Original was GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B
        Sampler2DMultisampleArray = 37131,
        //
        // Сводка:
        //     Original was GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C
        IntSampler2DMultisampleArray = 37132,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D
        UnsignedIntSampler2DMultisampleArray = 37133,
        //
        // Сводка:
        //     Original was GL_UNSIGNED_INT_ATOMIC_COUNTER = 0x92DB
        UnsignedIntAtomicCounter = 37595
    }
}
