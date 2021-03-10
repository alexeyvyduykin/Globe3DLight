﻿using System;
using System.Collections.Immutable;
using System.Text;
using GlmSharp;
using Globe3DLight.Containers;
using Globe3DLight.Scene;
using Globe3DLight.Data;

namespace Globe3DLight.ScenarioObjects
{
    public interface IRetranslator : IScenarioObject, IDrawable
    {
        IRetranslatorRenderModel RenderModel { get; set; }
        
        ILogical Logical { get; set; }
    }
}
