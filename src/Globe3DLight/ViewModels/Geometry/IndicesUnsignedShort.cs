using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public class IndicesUnsignedShort : BaseIndices, IIndices<ushort>
    {
        private readonly List<ushort> _values;
        
        public IList<ushort> Values => _values;
        
        public IndicesUnsignedShort() : base(IndicesType.UnsignedShort)
        {
            _values = new List<ushort>();
        }

        public IndicesUnsignedShort(int capacity) : base(IndicesType.UnsignedShort)
        {
            _values = new List<ushort>(capacity);
        }
          
        public void AddTriangle(TriangleIndicesUnsignedShort triangle)
        {
            _values.Add(triangle.UI0);
            _values.Add(triangle.UI1);
            _values.Add(triangle.UI2);
        }
    }
}
