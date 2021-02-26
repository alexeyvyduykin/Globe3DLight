using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Geometry
{
    public class IndicesUnsignedInt : BaseIndices, IIndices<uint>
    {
        public IndicesUnsignedInt()
            : base(IndicesType.UnsignedInt)
        {
            values = new List<uint>();
        }

        public IndicesUnsignedInt(int capacity)
            : base(IndicesType.UnsignedInt)
        {
            values = new List<uint>(capacity);
        }

        public IList<uint> Values
        {
            get { return values; }
        }

        public void AddTriangle(TriangleIndicesUnsignedInt triangle)
        {
            values.Add(triangle.UI0);
            values.Add(triangle.UI1);
            values.Add(triangle.UI2);
        }

        private List<uint> values;
    }

}
