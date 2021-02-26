using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using Globe3DLight.Geometry;
using GlmSharp;
using Globe3DLight.Scene;


namespace Globe3DLight.Resources
{
    public interface ISceneShape
    {
        IShaderProgram ShaderProgram { get; }

        //IRenderState RenderState { get; }
       
        IList<IAMesh> Meshes { get; set; }

        IList<IVertexArray> VertexArray { get; }

        void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene);
    }
}
