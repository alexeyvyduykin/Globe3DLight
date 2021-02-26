using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
//using OpenTK.Graphics.OpenGL4;
using OpenTK;
using OpenTK.Graphics;



namespace Globe3DLight.AvaloniaUI.OpenTK
{

    public interface IPresenter
    {      
        void ReadPixels(IntPtr pixels, int rowBytes);
        void DrawBegin();

        void Resize(int w, int h);

        int Width { get; }

        int Height { get; }
    }


    public class OpenTKPresenter : IPresenter
    {
        private readonly GameWindow _window;
        
        public int Width 
        {
            get => _window.Width;
            set => _window.Width = value;
        }

        public int Height 
        {
            get => _window.Height;
            set => _window.Height = value;
        }

        public OpenTKPresenter()
        {
            //  OpenTK.Toolkit.Init();

            _window = new GameWindow(/*1112,941,*/600, 600,
                new GraphicsMode(24, 24, 8),
                "Globe3DNative",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, 0, // Request OpenGL 3.3   
                GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug);

            _window.Visible = false;
            _window.MakeCurrent();

            


            //   GlWindow = new AvaloniaGraphicsWindow(this, 1/*1*/, 1/*1*/);
        }

        public void Resize(int w, int h)
        {
            float fWidth = (float)w;
            float fHeight = (float)h;
            //  if(Height == 0.) Height = 1.;

            float scale = 1.0f;
            if (fHeight / fWidth >= scale)
            {
                this.Width = w;
                this.Height = (int)(scale * w);
            }
            else
            {
                this.Width = (int)(fHeight / scale);
                this.Height = h;
            }

            if (fWidth >= fHeight)
            {
                float d = (fWidth - fHeight) / 2.0f;
                GL.Viewport(0, (int)-d, (int)fWidth, (int)fWidth);
                //viewport = new IntRect(0, (int)fWidth, (int)-d, (int)(fHeight + d));
            }

            if (fHeight > fWidth)
            {
                float d = (fHeight - fWidth) / 2.0f;
                GL.Viewport((int)-d, 0, (int)fHeight, (int)fHeight);
                //viewport = new IntRect((int)-d, (int)(fWidth + d), 0, (int)fHeight);
            }
        
            _window.ClientSize = new Size(w, h);           
        }

        public void DrawBegin()
        {
            GL.ClearColor(0.25f, 0.25f, 0.25f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void ReadPixels(IntPtr pixels, int rowBytes)
        {
            //GL.ClearColor(0.0f, 1.0f, 1.0f, 1.0f);
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Flush();

            GL.PixelStore(PixelStoreParameter.PackRowLength, rowBytes / 4);
            GL.ReadPixels(0, 0, Width, Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
        }
    }
}
