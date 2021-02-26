using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Geometry;


namespace Globe3DLight.Scene
{



    public class RetranslatorRenderModel : BaseRenderModel, IRetranslatorRenderModel
    {
        public IAMesh Mesh { get; set; }

        public double Scale { get; set; }
        //   RenderModel Model { get; set; }
        //   LogicalTreeNode LogicalTreeNode { get; set; }


        // public ScRetranslator(ObjectTreeNode parentNode /*string name, ILogicalTreeNode parentNode*/) : base(parentNode)
        // {
        //  this.Name = name;

        //   this.LogicalTreeNode = logicalTreeNode;

        //     this.Model = RenderModelFactory.CreateModel(RenderModelTypes.Retranslator);

        //    this.LogicalTreeNode = parentNode as LogicalTreeNode;// parentNode.AddChild(new TreeNodeContent(name));
        // }


        //     public dmat4 ModelMatrix => ParentNode.LastLogicalTreeNode.ModelMatrix;// LogicalTreeNode.ModelMatrix;

        //   public string Name { get; set; }

        //  public dmat4 InverseAbsoluteModel => LogicalTreeNode.ModelMatrix.Inverse;

        //   public override void Render(Context context, SceneState scene)
        //    {
        //       var modelMatrix = ParentNode.LastLogicalTreeNode.ModelMatrix;// LogicalTreeNode.ModelMatrix;

        //       Model.Render(modelMatrix, context, scene);
        //   }
        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        //public override void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        //{
        //    // visble == true
        //    renderer.DrawRetranslator(dc, this, ModelMatrix, scene);
        //}
    }

}
