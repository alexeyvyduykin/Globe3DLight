using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;


namespace Globe3DLight.Scene
{
    //public class ScBox : VisualObject
    //{
    //    IFrameObject Object { get; set; }

    //    private double Scale => 0.1;

    //    public ScBox(IFrameObject obj) : base(null)
    //    {
    //        this.Object = obj;
    //    }

    //    public override void Render(Context context, SceneState scene)
    //    {
    //        GL.MatrixMode(MatrixMode.Projection);
    //        GL.LoadMatrix(scene.ProjectionMatrix.Values1D);

    //        GL.MatrixMode(MatrixMode.Modelview);
    //        dmat4 modelView = scene.ViewMatrix * Object.ModelMatrix;
    //        GL.LoadMatrix(modelView.Values1D);

    //        GL.Scale(Object.Size.x, Object.Size.y, Object.Size.z);


    //        GL.Color3(1.0, 1.0, 1.0);
            
    //        GL.Begin(BeginMode.LineLoop);
    //        {
    //            GL.Vertex3(1.0, -1.0, 1.0); // p1
    //            GL.Vertex3(-1.0, -1.0, 1.0);  // p2
    //            GL.Vertex3(-1.0, 1.0, 1.0); // p3
    //            GL.Vertex3(1.0, 1.0, 1.0); // p4
    //        }
    //        GL.End();

    //        GL.Begin(BeginMode.LineLoop);
    //        {
    //            GL.Vertex3(1.0, -1.0, -1.0); // p5
    //            GL.Vertex3(-1.0, -1.0, -1.0); // p6
    //            GL.Vertex3(-1.0, 1.0, -1.0); // p7
    //            GL.Vertex3(1.0, 1.0, -1.0); // p8
    //        }
    //        GL.End();

    //        GL.Begin(BeginMode.Lines);
    //        {
    //            GL.Vertex3(1.0, -1.0, 1.0); // p1
    //            GL.Vertex3(1.0, -1.0, -1.0); // p5

    //            GL.Vertex3(-1.0, -1.0, 1.0);  // p2
    //            GL.Vertex3(-1.0, -1.0, -1.0); // p6

    //            GL.Vertex3(-1.0, 1.0, 1.0); // p3
    //            GL.Vertex3(-1.0, 1.0, -1.0); // p7

    //            GL.Vertex3(1.0, 1.0, 1.0); // p4
    //            GL.Vertex3(1.0, 1.0, -1.0); // p8
    //        }
    //        GL.End();

    //        GL.LineWidth(3.0f);

    //        VertexSolid(new dvec3(1.0, -1.0, 1.0)); // p1
    //        VertexSolid(new dvec3(-1.0, -1.0, 1.0)); // p2
    //        VertexSolid(new dvec3(-1.0, 1.0, 1.0)); // p3
    //        VertexSolid(new dvec3(1.0, 1.0, 1.0)); // p4
    //        VertexSolid(new dvec3(1.0, -1.0, -1.0)); // p5
    //        VertexSolid(new dvec3(-1.0, -1.0, -1.0)); // p6
    //        VertexSolid(new dvec3(-1.0, 1.0, -1.0)); // p7
    //        VertexSolid(new dvec3(1.0, 1.0, -1.0)); // p8

    //        GL.LineWidth(1.0f);
    //    }

    //    void VertexSolid(dvec3 p)
    //    {
    //        GL.Begin(BeginMode.Lines);
    //        {
    //            GL.Vertex3(p.x, p.y, p.z);
    //            GL.Vertex3(p.x - p.x * Scale, p.y, p.z);

    //            GL.Vertex3(p.x, p.y, p.z); 
    //            GL.Vertex3(p.x, p.y - p.y * Scale, p.z);

    //            GL.Vertex3(p.x, p.y, p.z);
    //            GL.Vertex3(p.x, p.y, p.z - p.z * Scale);             
    //        }
    //        GL.End();
    //    }
    //}

}