using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Image;

namespace Globe3DLight.Renderer
{
    public interface IThreadLoadingNode
    {
        bool IsComplete { get; }
        string WaitKey { get; }
        int SetImage(IDdsImage image);
        void SetName(int name);     
    }
}
