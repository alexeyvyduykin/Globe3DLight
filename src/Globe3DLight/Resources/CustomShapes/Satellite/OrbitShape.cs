using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Scene;
using System.Collections.Immutable;
using Globe3DLight.Geometry;
using System.Linq;
using Globe3DLight.Style;

namespace Globe3DLight.Resources
{
    //public class OrbitShape : ISceneShape
    //{
    //    private readonly IDevice _device;
    //    private readonly IShaderProgram _shaderProgram;

    //    private readonly IRenderState _renderState = new RenderState();
    //    private readonly IList<IVertexArray> _vertexArray = new List<IVertexArray>();

    //    // DrawState
    //    public IShaderProgram ShaderProgram => _shaderProgram;
    //    public IRenderState RenderState => _renderState;
    //    public IList<IVertexArray> VertexArray => _vertexArray;

    //    public IList<IMesh> Meshes { get; set; }


    //    public IEnumerable<vec3> Orbit { get; set; }

    //    public OrbitShape(IDevice device)
    //    {
    //        this._device = device;

    //    }
    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        context.LoadProjectionMatrix(scene.ProjectionMatrix);
    //        context.LoadModelviewMatrix(scene.ViewMatrix);

    //        context.DrawLineLoop(Orbit, Colors.White, 1.0f);
    //    }
    //}
}
