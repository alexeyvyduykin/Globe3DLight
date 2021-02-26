using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using System.Collections.Immutable;
using Globe3DLight.Geometry.Models;

namespace Globe3DLight.Scene
{
    public interface ISatelliteRenderModel : IRenderModel
    {
    //   IModelLoader Loader { get; set; }

       // string Filename { get; set; }

        IModel Model { get; set; }

    //    bool IsTextureLoading { get; set; }

        //ImmutableArray<IMesh> Meshes { get; set; }

        double Scale { get; set; }
    }

    public class SatelliteRenderModel : BaseRenderModel, ISatelliteRenderModel
    {
       // public ImmutableArray<IMesh> Meshes { get; set; }

        public double Scale { get; set; }

      //  public IModelLoader Loader { get; set; }

        //   public string Filename { get; set; }

        public IModel Model { get; set; }

      //  public bool IsTextureLoading { get; set; }

        // RenderModel Model { get; set; }
        //  LogicalTreeNode LogicalTreeNode { get; set; }

        //FrameGraphicsComponent Frame { get; set; }

        //   public Satellite(ObjectTreeNode parentNode/* string name, ILogicalTreeNode parentNode*/) : base(parentNode)
        //   {
        ////      this.Name = name;

        //    //   this.LogicalTreeNode = logicalTreeNode;

        //       this.Model = RenderModelFactory.CreateModel(RenderModelTypes.Satellite);

        //     //  this.Frame = new FrameGraphicsComponent(0.3);

        //   //    this.LogicalTreeNode = parentNode as LogicalTreeNode;// parentNode.AddChild(new TreeNodeContent(name));
        //   }

        //    public dmat4 ModelMatrix => ParentNode.LastLogicalTreeNode.ModelMatrix;// LogicalTreeNode.ModelMatrix;

        //  public string Name { get; set; }

        //     public dmat4 InverseAbsoluteModel => ParentNode.LastLogicalTreeNode.ModelMatrix.Inverse;// LogicalTreeNode.ModelMatrix.Inverse;

      //  public dvec3 Eye { get; } = new dvec3(-2.0, 2.0, -2.0);

     //   public dvec3 Target { get; } = dvec3.Zero;

     //   public dvec3 Up { get; } = dvec3.UnitY;

      //  public dvec3 Size { get; } = new dvec3(0.3, 0.3, 0.1);

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        //public override void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        //{
        //    // visible == true
        //    renderer.DrawSatellite(dc, this, ModelMatrix, scene);
        //}

        //public override void Render(Context context, SceneState scene)
        //{
        //    var modelMatrix = ParentNode.LastLogicalTreeNode.ModelMatrix;// LogicalTreeNode.ModelMatrix;

        //  //  Frame.Render(modelMatrix, context, scene);

        //    Model.Render(modelMatrix, context, scene);
        //}

    }


}
