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
    //public class FrameShape : ISceneShape
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

    //    public float Scale { get; set; }

    //    public FrameShape(IDevice device)
    //    {
    //        this._device = device;

    //    }

    //    public void Render(dmat4 modelMatrix, IRenderContext context, ISceneState scene)
    //    {
    //        dmat4 modelView = scene.ViewMatrix * modelMatrix;

    //        context.LoadProjectionMatrix(scene.ProjectionMatrix);
    //        context.LoadModelviewMatrix(modelView);

    //        //GL.Scale(scale, scale, scale);

    //        // zAxis          
    //        context.DrawLine(vec3.Zero, vec3.UnitZ * Scale, Colors.Blue, 1.0f);

    //        //	glRasterPos3f(0.0, 0.0, 1.2);
    //        //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'z');

    //        // xAxis      
    //        //GL.Rotate(90, 0, 1, 0);
    //        context.DrawLine(vec3.Zero, vec3.UnitX * Scale, Colors.Red, 1.0f);

    //        //	glRasterPos3f(0.0, 0.0, 1.2);
    //        //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'x');

    //        // yAxis      
    //        //GL.Rotate(-90, 1, 0, 0);
    //        context.DrawLine(vec3.Zero, vec3.UnitY * Scale, Colors.Green, 1.0f);

    //        //	glRasterPos3f(0.0, 0.0, 1.2);
    //        //	glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'y');
    //    }



    //}

}
