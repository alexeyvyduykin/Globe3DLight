using System;
using System.Collections.Generic;
using System.Text;
using GlmSharp;

namespace Globe3DLight.Models
{
    public interface IFrameable
    {
        dmat4 ModelMatrix { get; }
    }
}
