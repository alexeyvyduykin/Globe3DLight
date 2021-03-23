using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Scene;
namespace Globe3DLight.Renderer
{
    public interface ISunDrawNode : IDrawNode, IThreadLoadingNode
    {
        SunRenderModel Sun { get; set; } 
    }
}
