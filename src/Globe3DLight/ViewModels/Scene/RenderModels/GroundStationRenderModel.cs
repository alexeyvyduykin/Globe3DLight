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



    public class GroundStationRenderModel : BaseRenderModel, IGroundStationRenderModel
    {
        public IAMesh Mesh { get; set; }
        //   ILogicalTreeNode LogicalTreeNode { get; set; }
        //  GroundStationModelDefault Model { get; set; }
        // FrameGraphicsComponent Frame { get; set; }
      
        //    public dmat4 ModelMatrix => ParentNode.LastLogicalTreeNode.ModelMatrix;// LogicalTreeNode.ModelMatrix;

        // public string Name { get; set; }

      //  public dvec3 Size => new dvec3(1.0, 1.0, 1.0);

        public double Scale { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        //public override void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        //{
        //    // visble == true
        //    renderer.DrawGroundStation(dc, this, ModelMatrix, scene);
        //}

        // public GroundStation(ObjectTreeNode parentNode/* string name, ILogicalTreeNode parentNode*/) : base(parentNode)
        // {
        //     this.Name = name;

        //     this.Model = new GroundStationModelDefault();
        //  this.Frame = new FrameGraphicsComponent(1.0);

        //      this.LogicalTreeNode = parentNode;// parentNode.AddChild(new TreeNodeContent(name));
        // }

        // public override void Render(Context context, SceneState scene)
        // {
        //     var nodej2000 = ParentNode.LastLogicalTreeNode.Root/* LogicalTreeNode.Root*/.Find(s => s.Name == "J2000") as LogicalTreeNode;


        //      var m = nodej2000.ModelMatrix * ModelMatrix.ToMat4;

        //  Frame.Render(m, context, scene);

        //     Model.Render(m, context, scene);
        //  }


    }
}
