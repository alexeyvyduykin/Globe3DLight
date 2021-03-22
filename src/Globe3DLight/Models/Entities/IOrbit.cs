﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globe3DLight.Containers;
using Globe3DLight.Scene;

namespace Globe3DLight.Entities
{
    public interface IOrbit : IEntity, IDrawable
    {
        IOrbitRenderModel RenderModel { get; set; }

        ILogical Logical { get; set; }
    }
}