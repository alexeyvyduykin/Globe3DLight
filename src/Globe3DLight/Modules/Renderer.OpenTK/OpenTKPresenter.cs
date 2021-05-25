using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Globe3DLight.Models.Renderer;

namespace Globe3DLight.Renderer.OpenTK
{
    public class OpenTKPresenter : IPresenterContract
    {
        private readonly GameWindow _window;

        public OpenTKPresenter()
        {             
            _window = new GameWindow(1,1,
                new GraphicsMode(24, 24, 8),
                "Globe3DNative",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, 0, // Request OpenGL 3.3   
                GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug);

            _window.Visible = false;
            _window.MakeCurrent();
        }

        public int Width => _window.Width;    
        
        public int Height => _window.Height;       
        
        public void Resize(double width, double height)
        {
            var w = (width != 0.0) ? width : 1.0;            
            var h = (height != 0.0) ? height : 1.0;
            
            _window.ClientSize = new Size((int)w, (int)h);

            if (w >= h)
            {
                var d = (w - h) / 2.0;
                GL.Viewport(0, (int)-d, (int)w, (int)w);             
            }

            if (h > w)
            {
                var d = (h - w) / 2.0;
                GL.Viewport((int)-d, 0, (int)h, (int)h);           
            }          
        }

        public void DrawBegin()
        {
            GL.ClearColor(0.25f, 0.25f, 0.25f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ReadPixels(IntPtr pixels, int rowBytes)
        {
            GL.Flush();

            GL.PixelStore(PixelStoreParameter.PackRowLength, rowBytes / 4);
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        }
    }
}
