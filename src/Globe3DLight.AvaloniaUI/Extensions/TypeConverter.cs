using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using Globe3DLight;

namespace Globe3DLight.Renderer
{
    public static class TypeConverter
    {
        //public static BeginMode To(Globe3D.PrimitiveType type)
        //{
        //    switch (type)
        //    {
        //        case Globe3D.PrimitiveType.Points:
        //            return BeginMode.Points;
        //        case Globe3D.PrimitiveType.Lines:
        //            return BeginMode.Lines;
        //        case Globe3D.PrimitiveType.LineLoop:
        //            return BeginMode.LineLoop;
        //        case Globe3D.PrimitiveType.LineStrip:
        //            return BeginMode.LineStrip;
        //        case Globe3D.PrimitiveType.Triangles:
        //            return BeginMode.Triangles;
        //        case Globe3D.PrimitiveType.TriangleStrip:
        //            return BeginMode.TriangleStrip;
        //        case Globe3D.PrimitiveType.LinesAdjacency:
        //            return BeginMode.LinesAdjacency; ;
        //        case Globe3D.PrimitiveType.LineStripAdjacency:
        //            return BeginMode.LineStripAdjacency;
        //        case Globe3D.PrimitiveType.TrianglesAdjacency:
        //            return BeginMode.TrianglesAdjacency;
        //        case Globe3D.PrimitiveType.TriangleStripAdjacency:
        //            return BeginMode.TriangleStripAdjacency;
        //        case Globe3D.PrimitiveType.TriangleFan:
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
