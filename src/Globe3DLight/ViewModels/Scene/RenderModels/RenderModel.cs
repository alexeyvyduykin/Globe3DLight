using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.ViewModels.Geometry;

namespace Globe3DLight.ViewModels.Scene
{
    public class RenderModel : BaseRenderModel
    {
        public FrameRenderModel Frame { get; set; }

        public Model Model { get; set; }

        public double Scale { get; set; }
    }
}
