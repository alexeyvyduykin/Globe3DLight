using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Data;
using Globe3DLight.Renderer;
using Globe3DLight.Geometry;
using Globe3DLight.Image;


namespace Globe3DLight.Scene
{
    public interface ISunRenderModel : IRenderModel
    {
        IAMesh Billboard { get; set; }

        string SunGlowKey { get; set; }

        //int SunGlowName { get; }
      //  string SunGlowTexturePath { get; set; }
      //  IImageLoader ImageLoader { get; set; }

       // bool IsLoading { get; set; }

      //  void Load(IImageLoader imageLoader);
//
      //  IDdsImage DdsImage { get; set; }

    //   IDdsImage GetImage(string key);
    }

    public class SunRenderModel : BaseRenderModel, ISunRenderModel
    {
        public IAMesh Billboard { get; set; }

        public string SunGlowKey { get; set; }

        //public int SunGlowName { get; protected set; }
        //   public string SunGlowTexturePath { get; set; }

        //   public IImageLoader ImageLoader { get; set; }

        // public bool IsLoading { get; set; }

        //        public IDdsImage DdsImage { get; set; }

        public override object Copy(IDictionary<object, object> shared)
        {
            throw new NotImplementedException();
        }
    }

}
