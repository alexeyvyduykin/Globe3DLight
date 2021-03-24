using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight;

namespace Globe3DLight.Models.Renderer
{
    public interface IVertexBufferAttribute
    {
        IBuffer VertexBuffer { get; }

        VertexAttribPointerType ComponentDatatype { get; }

        int NumberOfComponents { get; }

        bool Normalize { get; }

        int OffsetInBytes { get; }

        int StrideInBytes { get; }
    }
}
