using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;


namespace Globe3DLight.Geometry
{
    public interface IAMesh
    {
        ImmutableArray<IVertexAttribute> Attributes { get; set; }
        //IVertexAttributeCollection Attributes { get; }

        IIndices Indices { get; set; }

        PrimitiveType PrimitiveType { get; set; }

        FrontFaceDirection FrontFaceWindingOrder { get; set; }


        // refactoring
        List<Vertex> vertices { get; set; }

        List<short> indices { get; set; }

        IAMaterial material { get; set; }
    }
}
