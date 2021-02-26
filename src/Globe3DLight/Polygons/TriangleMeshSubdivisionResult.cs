using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Globe3DLight.Geometry;

namespace Globe3DLight
{
   // [CLSCompliant(false)]
    public class TriangleMeshSubdivisionResult
    {
        internal TriangleMeshSubdivisionResult(ICollection<dvec3> positions, IndicesUnsignedInt indices)
        {
            _positions = positions;
            _indices = indices;
        }

        public ICollection<dvec3> Positions
        {
            get { return _positions; }
        }

        public IndicesUnsignedInt Indices
        {
            get { return _indices; }
        }

        private readonly ICollection<dvec3> _positions;
        private readonly IndicesUnsignedInt _indices;
    }
}
