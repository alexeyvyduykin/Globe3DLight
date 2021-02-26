﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Globe3DLight.Renderer
{
    public interface IShaderVertexAttribute
    {
        string Name { get; }

        int Location { get; }

        ActiveAttribType Datatype { get; }

        int Length { get; }
    }
}
