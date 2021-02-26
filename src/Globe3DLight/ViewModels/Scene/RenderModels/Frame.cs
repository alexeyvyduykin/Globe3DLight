using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;


namespace Globe3DLight.Scene
{
    public class FrameRenderModel : BaseRenderModel, IFrameRenderModel
    {
        public float Scale { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }


    //public class Frame : ObservableObject, IFrame
    //{
    //    private dvec3 _position;
    //    private dmat4 _modelMatrix;

    //    public dvec3 Position
    //    {
    //        get => _position;
    //        set => Update(ref _position, value);
    //    }

    //    public dmat4 ModelMatrix
    //    {
    //        get => _modelMatrix;
    //        set => Update(ref _modelMatrix, value);
    //    }

    //    public override object Copy(IDictionary<object, object> shared)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Frame : BaseRenderModel, IFrame
    //{   
    //    private dvec3 _position;
    //    private dmat4 _modelMatrix;
    //    private float _scale;

    //    public dvec3 Position
    //    {
    //        get => _position;
    //        set => Update(ref _position, value);
    //    }

    //    public dmat4 ModelMatrix
    //    {
    //        get => _modelMatrix;
    //        set => Update(ref _modelMatrix, value);
    //    }
    //    public float Scale
    //    {
    //        get => _scale; 
    //        set => Update(ref _scale, value);
    //    }

    //    // IFrameObject Object { get; set; }

    //    //public ScFrame(IFrameObject obj) : base(null)
    //    //{
    //    //    this.Object = obj;
    //    //}

    //    private double ScaleAxis { get; set; } = 1.2;

    //    public override object Copy(IDictionary<object, object> shared)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    //public override void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
    //    //{
    //    //    // visble == true
    //    //    renderer.DrawFrame(dc, this, _modelMatrix, scene);
    //    //}

    //    //public override void Render(Context context, SceneState scene)
    //    //{
    //    //    GL.MatrixMode(MatrixMode.Projection);
    //    //    GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

    //    //    GL.MatrixMode(MatrixMode.Modelview);
    //    //    dmat4 modelView = scene.ViewMatrix * Object.ModelMatrix;
    //    //    GL.LoadMatrix(modelView.Values1D);

    //    //    GL.Scale(Object.Size.x * ScaleAxis, Object.Size.y * ScaleAxis, Object.Size.z * ScaleAxis);

    //    //    // zAxis
    //    //    GL.Color3(0.0, 0.0, 1.0);
    //    //    glDrawAxis();

    //    //    //	glRasterPos3f(0.0, 0.0, 1.2);
    //    //    //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'z');

    //    //    // xAxis
    //    //    GL.Color3(1.0, 0.0, 0.0);
    //    //    GL.Rotate(90, 0, 1, 0);
    //    //    glDrawAxis();

    //    //    //	glRasterPos3f(0.0, 0.0, 1.2);
    //    //    //    glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'x');

    //    //    // yAxis
    //    //    GL.Color3(0.0, 1.0, 0.0);
    //    //    GL.Rotate(-90, 1, 0, 0);
    //    //    glDrawAxis();

    //    //    //	glRasterPos3f(0.0, 0.0, 1.2);
    //    //    //	glutBitmapCharacter(GLUT_BITMAP_HELVETICA_12, 'y');
    //    //}


    //    //private void glDrawAxis()   // zAxis
    //    //{
    //    //    GL.Begin(BeginMode.Lines);
    //    //    GL.Vertex3(0.0, 0.0, 0.0);
    //    //    GL.Vertex3(0.0, 0.0, 1.0);
    //    //    GL.End();
    //    //}
    //}

}
