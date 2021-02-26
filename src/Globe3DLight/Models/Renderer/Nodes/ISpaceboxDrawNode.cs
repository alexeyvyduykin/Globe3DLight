using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;

namespace Globe3DLight.Renderer
{
    public interface ISpaceboxDrawNode : IDrawNode, IThreadLoadingNode
    {
        ISpaceboxRenderModel Spacebox { get; set; }
    }
}
