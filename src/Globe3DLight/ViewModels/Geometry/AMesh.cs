using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Immutable;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public class AMesh : IAMesh
    {
        // private readonly IVertexAttributeCollection _attributes;
        private ImmutableArray<IVertexAttribute> _attributes;
        //  public Mesh()
        //  {
        //_attributes = ImmutableArray.Create<IVertexAttribute>();// new VertexAttributeCollection();
        //  }
        public ImmutableArray<IVertexAttribute> Attributes
        {
            get => _attributes;
            set => _attributes = value;
        }
        // public IVertexAttributeCollection Attributes => _attributes;

        public IIndices Indices { get; set; }

        public PrimitiveType PrimitiveType { get; set; }

        public FrontFaceDirection FrontFaceWindingOrder { get; set; }

        // refactoring
        public List<Vertex> vertices { get; set; }

        public List<short> indices { get; set; }

        public IAMaterial material { get; set; }
    }
}
