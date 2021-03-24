using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Models.Image;

namespace Globe3DLight.Models.Renderer
{
    public interface IThreadLoadingNode
    {
        bool IsComplete { get; }
        string WaitKey { get; }
        int SetImage(IDdsImage image);
        void SetName(int name);     
    }
}
