using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    using GlmSharp;

    public static class ExtensionGLMSharp
    {
        public static vec2 ToVec2(this dvec2 dv)
        {
            return new vec2((float)dv.x, (float)dv.y);
        }

        public static vec3 ToVec3(this dvec3 dv)
        {
            return new vec3((float)dv.x, (float)dv.y, (float)dv.z);
        }

        public static dvec3 ToDVec3(this vec3 v)
        {
            return new dvec3(v.x, v.y, v.z);
        }

        public static vec4 ToVec4(this dvec4 dv)
        {
            return new vec4((float)dv.x, (float)dv.y, (float)dv.z, (float)dv.w);
        }

        public static mat4 ToMat4(this dmat4 dm)
        {
            return new mat4(
                (float)dm.m00, (float)dm.m01, (float)dm.m02, (float)dm.m03,
                (float)dm.m10, (float)dm.m11, (float)dm.m12, (float)dm.m13,
                (float)dm.m20, (float)dm.m21, (float)dm.m22, (float)dm.m23,
                (float)dm.m30, (float)dm.m31, (float)dm.m32, (float)dm.m33);
        }

        public static mat3 ToMat3(this dmat3 dm)
        {
            return new mat3(
                (float)dm.m00, (float)dm.m01, (float)dm.m02,
                (float)dm.m10, (float)dm.m11, (float)dm.m12,
                (float)dm.m20, (float)dm.m21, (float)dm.m22);
        }


        public static dvec3 MostOrthogonalAxis(this dvec3 v)
        {
            double x = Math.Abs(v.x);
            double y = Math.Abs(v.y);
            double z = Math.Abs(v.z);

            if ((x < y) && (x < z))
            {
                return dvec3.UnitX;
            }
            else if ((y < x) && (y < z))
            {
                return dvec3.UnitY;
            }
            else
            {
                return dvec3.UnitZ;
            }
        }

    }
}
