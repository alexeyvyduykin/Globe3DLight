using System;
using A = OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal static class VertexArraySizes
    {

        //public static int SizeOf(IndexBufferDatatype type)
        //{
        //    switch (type)
        //    {
        //        case IndexBufferDatatype.UnsignedShort:
        //            return sizeof(ushort);
        //        case IndexBufferDatatype.UnsignedInt:
        //            return sizeof(uint);
        //    }

        //    throw new ArgumentException("type");
        //}

        public static int SizeOf(A.VertexAttribPointerType type)
        {
            switch (type)
            {
                case A.VertexAttribPointerType.Byte:
                case A.VertexAttribPointerType.UnsignedByte:
                    return sizeof(byte);
                case A.VertexAttribPointerType.Short:
                    return sizeof(short);
                case A.VertexAttribPointerType.UnsignedShort:
                    return sizeof(ushort);
                case A.VertexAttribPointerType.Int:
                    return sizeof(int);
                case A.VertexAttribPointerType.UnsignedInt:
                    return sizeof(uint);
                case A.VertexAttribPointerType.Float:
                    return sizeof(float);
                //case A.VertexAttribPointerType.HalfFloat:
                //    return SizeInBytes<System.Half>.Value;
            }

            throw new ArgumentException("type");
        }
    }
}
