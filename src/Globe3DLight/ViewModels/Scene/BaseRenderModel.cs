using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Globe3DLight.Renderer;
using GlmSharp;
using Globe3DLight.Models.Renderer;

namespace Globe3DLight.ViewModels.Scene
{
    public abstract class BaseRenderModel : ViewModelBase
    {
        public virtual bool Invalidate(IRenderContext renderer)
        {
            return false;
        }
    }
}
