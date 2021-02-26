using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;


namespace Globe3DLight
{
    public static class GlmSharpExtensions
    {
        public static mat4 ToMat4(this dmat4 dmat)
        {
            return new mat4(
                (float)dmat.m00, (float)dmat.m01, (float)dmat.m02, (float)dmat.m03,
                (float)dmat.m10, (float)dmat.m11, (float)dmat.m12, (float)dmat.m13,
                (float)dmat.m20, (float)dmat.m21, (float)dmat.m22, (float)dmat.m23,
                (float)dmat.m30, (float)dmat.m31, (float)dmat.m32, (float)dmat.m33);
        }

        public static mat3 ToMat3(this dmat3 dmat)
        {
            return new mat3(
                (float)dmat.m00, (float)dmat.m01, (float)dmat.m02,
                (float)dmat.m10, (float)dmat.m11, (float)dmat.m12, 
                (float)dmat.m20, (float)dmat.m21, (float)dmat.m22);
        }

        public static vec3 ToVec3(this dvec3 dvec)
        {
            return new vec3((float)dvec.x, (float)dvec.y, (float)dvec.z);
        }

        public static vec4 ToVec4(this dvec3 dvec)
        {
            return new vec4((float)dvec.x, (float)dvec.y, (float)dvec.z, 1.0f);
        }

        public static vec4 ToVec4(this dvec4 dvec)
        {
            return new vec4((float)dvec.x, (float)dvec.y, (float)dvec.z, (float)dvec.w);
        }

        public static dvec3 ToDvec3(this dvec4 dvec)
        {
            return new dvec3(dvec.x, dvec.y, dvec.z);
        }
    }
}
