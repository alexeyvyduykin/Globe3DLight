using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;


namespace Globe3DLight.Entities
{
    public interface ISensor : IEntity, IDrawable
    {
        ISensorRenderModel RenderModel { get; set; }

        ILogical Logical { get; set; }
    }
}
