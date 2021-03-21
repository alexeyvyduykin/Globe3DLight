using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class StencilTestFace
    {
        public StencilTestFace()
        {
            StencilFailOperation = StencilOp.Keep;
            DepthFailStencilPassOperation = StencilOp.Keep;
            DepthPassStencilPassOperation = StencilOp.Keep;
            Function = StencilFunction.Always;
            ReferenceValue = 0;
            Mask = ~0;
        }

        public StencilOp StencilFailOperation { get; set; }
        public StencilOp DepthFailStencilPassOperation { get; set; }
        public StencilOp DepthPassStencilPassOperation { get; set; }

        public StencilFunction Function { get; set; }
        public int ReferenceValue { get; set; }
        public int Mask { get; set; }
    }
}
