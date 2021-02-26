using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer
{
    public interface IVertexBufferAttributes : IEnumerable
    {   
        IVertexBufferAttribute this[int index] { get; set; }

        int Count { get; }

        int MaximumCount { get; }

        void Clean();

        void Attach(int index);

        void Detach(int index);

        int MaximumArrayIndex { get; }

        int NumberOfVertices(IVertexBufferAttribute attribute);
    }
}
