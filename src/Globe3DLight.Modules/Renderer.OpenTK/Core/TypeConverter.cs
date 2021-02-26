using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;


namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal static class TypeConverter
    {
        //public static BeginMode To(Globe3DCore.PrimitiveType type)
        //{
        //    switch (type)
        //    {
        //        case Globe3DCore.PrimitiveType.Points:
        //            return BeginMode.Points;
        //        case Globe3DCore.PrimitiveType.Lines:
        //            return BeginMode.Lines;
        //        case Globe3DCore.PrimitiveType.LineLoop:
        //            return BeginMode.LineLoop;
        //        case Globe3DCore.PrimitiveType.LineStrip:
        //            return BeginMode.LineStrip;
        //        case Globe3DCore.PrimitiveType.Triangles:
        //            return BeginMode.Triangles;
        //        case Globe3DCore.PrimitiveType.TriangleStrip:
        //            return BeginMode.TriangleStrip;
        //        case Globe3DCore.PrimitiveType.LinesAdjacency:
        //            return BeginMode.LinesAdjacency; ;
        //        case Globe3DCore.PrimitiveType.LineStripAdjacency:
        //            return BeginMode.LineStripAdjacency;
        //        case Globe3DCore.PrimitiveType.TrianglesAdjacency:
        //            return BeginMode.TrianglesAdjacency;
        //        case Globe3DCore.PrimitiveType.TriangleStripAdjacency:
        //            return BeginMode.TriangleStripAdjacency;
        //        case Globe3DCore.PrimitiveType.TriangleFan:
        //            return BeginMode.TriangleFan;
        //    }

        //    throw new ArgumentException("type");
        //}

        public static DrawElementsType To(IndexBufferDatatype type)
        {
            switch (type)
            {
                case IndexBufferDatatype.UnsignedShort:
                    return DrawElementsType.UnsignedShort;
                case IndexBufferDatatype.UnsignedInt:
                    return DrawElementsType.UnsignedInt;
            }

            throw new ArgumentException("type");
        }

        //public static PolygonMode To(RasterizationMode mode)
        //{
        //    switch (mode)
        //    {
        //        case RasterizationMode.Point:
        //            return PolygonMode.Point;
        //        case RasterizationMode.Line:
        //            return PolygonMode.Line;
        //        case RasterizationMode.Fill:
        //            return PolygonMode.Fill;
        //    }

        //    throw new ArgumentException("mode");
        //}

    }
}
