using System;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Geometry;


namespace Globe3DLight
{
    public static class AMeshExtensions
    {
        public static void AddAttribute(this IAMesh mesh, IVertexAttribute attribute)
        {
            var builder = mesh.Attributes.ToBuilder();

            builder.Add(attribute);

            mesh.Attributes = builder.ToImmutable();
        }

    }
}
