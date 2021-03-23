using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Renderer.OpenTK.Core
{
    internal class DrawState
    {
        public DrawState()
        {
            RenderState = new RenderState();
        }

        public DrawState(RenderState renderState, ShaderProgram shaderProgram, VertexArray vertexArray)
        {
            RenderState = renderState;
            ShaderProgram = shaderProgram;
            VertexArray = vertexArray;
        }

        public RenderState RenderState { get; set; }
        public ShaderProgram ShaderProgram { get; set; }
        public VertexArray VertexArray { get; set; }
    }

}
