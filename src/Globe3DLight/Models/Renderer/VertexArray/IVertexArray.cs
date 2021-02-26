using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;

namespace Globe3DLight.Renderer
{
    public interface IVertexArray : IDisposable
    {
        void Bind();

        void Clean();

        int MaximumArrayIndex();

        IVertexBufferAttributes Attributes { get; set; }

        IIndexBuffer IndexBuffer { get; set; }

        bool DisposeBuffers { get; set; }
    }

}
