using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Scene;

namespace Globe3DLight.Entities
{
    public interface IGroundObject : /*IEntity,*/ IDrawable, ITargetable
    {
        IGroundObjectRenderModel RenderModel { get; set; }

        IFrameRenderModel FrameRenderModel { get; set; }

        Logical Logical { get; set; }
    }
}
