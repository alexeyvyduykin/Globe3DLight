using Globe3DLight.Geometry;
using Globe3DLight.Renderer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Scene
{
    public interface ISpaceboxRenderModel : IRenderModel
    {
        double Scale { get; set; }
        IAMesh Mesh { get; set; }

        string SpaceboxCubemapKey { get; set; }

        //  int IdCubemapText { get; set; }

        //  IDDSLoader Loader { get; set; }

        //  string Filename { get; set; }

        //  bool IsTextureLoading { get; set; }
    }

    public class SpaceboxRenderModel : BaseRenderModel, ISpaceboxRenderModel
    {
        public double Scale { get; set; }
        public IAMesh Mesh { get; set; }

    //    public int IdCubemapText { get; set; }

        public string SpaceboxCubemapKey { get; set; }

     //   public IDDSLoader Loader { get; set; }

     //   public string Filename { get; set; }

    //   public bool IsTextureLoading { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }

        //public override void DrawShape(object dc, IRenderContext renderer, ISceneState scene)
        //{
        //    renderer.DrawSpacebox(dc, this, ModelMatrix, scene);
        //}
    }
}
