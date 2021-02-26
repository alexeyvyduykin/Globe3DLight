using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Geometry
{
    public interface IVertexAttribute
    {
        string Name { get; }

        VertexAttributeType Datatype { get; }
    }
    public interface IVertexAttribute<T> : IVertexAttribute
    {
        IList<T> Values { get; }
    }
}
