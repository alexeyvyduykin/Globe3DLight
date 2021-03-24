using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Models.Geometry;

namespace Globe3DLight.ViewModels.Geometry
{
    public class VertexAttributeCollection : ICollection<IVertexAttribute>, IVertexAttributeCollection
    {
        private readonly Dictionary<string, IVertexAttribute> m_collection;

        public VertexAttributeCollection()
        {
            m_collection = new Dictionary<string, IVertexAttribute>();
        }

        public void Add(IVertexAttribute vertexAttribute)
        {
            m_collection.Add(vertexAttribute.Name, vertexAttribute);
        }

        public void Clear()
        {
            m_collection.Clear();
        }

        public bool Contains(IVertexAttribute vertexAttribute)
        {
            return Contains(vertexAttribute.Name);
        }

        public bool Contains(string vertexAttributeName)
        {
            return m_collection.ContainsKey(vertexAttributeName);
        }

        public void CopyTo(IVertexAttribute[] array, int arrayIndex)
        {
            ICollection<IVertexAttribute> values = m_collection.Values;
            values.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_collection.Count; }
        }

        bool ICollection<IVertexAttribute>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(IVertexAttribute item)
        {
            return Remove(item.Name);
        }

        public bool Remove(string vertexAttributeName)
        {
            return m_collection.Remove(vertexAttributeName);
        }

        public IEnumerator<IVertexAttribute> GetEnumerator()
        {
            ICollection<IVertexAttribute> values = m_collection.Values;
            return values.GetEnumerator();
        }

#if !CSToJava
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
#endif

        public IVertexAttribute this[string vertexAttributeName]
        {
            get { return m_collection[vertexAttributeName]; }
        }

    }

}
