using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public abstract class VertexAttribute : IVertexAttribute
    {
        private readonly string _name;
        private readonly VertexAttributeType _type;
        
        public string Name => _name;

        public VertexAttributeType Datatype => _type; 
        
        protected VertexAttribute(string name, VertexAttributeType type)
        {
            this._name = name;
            this._type = type;
        }
    }

    public class VertexAttribute<T> : VertexAttribute, IVertexAttribute<T>
    {
        private readonly List<T> _values;

        public IList<T> Values => _values; 
        
        public VertexAttribute(string name, VertexAttributeType type)
            : base(name, type)
        {
            _values = new List<T>();
        }

        protected VertexAttribute(string name, VertexAttributeType type, int capacity)
            : base(name, type)
        {
            _values = new List<T>(capacity);
        }



        
    }
}
