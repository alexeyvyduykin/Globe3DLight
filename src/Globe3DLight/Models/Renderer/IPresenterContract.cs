using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe3DLight.Models.Renderer
{
    public interface IPresenterContract
    {
        void ReadPixels(IntPtr pixels, int rowBytes);
        void DrawBegin();

        void Resize(int w, int h);

        int Width { get; }

        int Height { get; }
    }

}
