﻿using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;

namespace Globe3DLight.Entities
{
    public interface IEarth : IEntity, IDrawable, ITargetable
    { 
        IEarthRenderModel RenderModel { get; set; }

        IFrameRenderModel FrameRenderModel { get; set; }

        ILogical Logical { get; set; }
    }
}