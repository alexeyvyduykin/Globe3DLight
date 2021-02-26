using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    //
    // Сводка:
    //     Used in GL.GetActiveAttrib, GL.GetTransformFeedbackVarying and 1 other function
    public enum ActiveAttribType
    {
        //
        // Сводка:
        //     Original was GL_NONE = 0
        None = 0,
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
        //     Original was GL_DOUBLE_MAT2 = 0x8F46
        DoubleMat2 = 36678,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT3 = 0x8F47
        DoubleMat3 = 36679,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT4 = 0x8F48
        DoubleMat4 = 36680,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT2x3 = 0x8F49
        DoubleMat2x3 = 36681,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT2x4 = 0x8F4A
        DoubleMat2x4 = 36682,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT3x2 = 0x8F4B
        DoubleMat3x2 = 36683,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT3x4 = 0x8F4C
        DoubleMat3x4 = 36684,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT4x2 = 0x8F4D
        DoubleMat4x2 = 36685,
        //
        // Сводка:
        //     Original was GL_DOUBLE_MAT4x3 = 0x8F4E
        DoubleMat4x3 = 36686,
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
        DoubleVec4 = 36862
    }
}
