using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
//using System.Windows.Interop;
//using System.Windows.Media;
//using OpenTK.Graphics.Wgl;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;
//using Window = System.Windows.Window;
//using WindowState = OpenTK.Windowing.Common.WindowState;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace Globe3DLight.AvaloniaUI.Wpf
{

    //public sealed class GLWpfControl : UserControl// FrameworkElement
    //{
    //    private readonly TranslateTransform _translateTransform = new TranslateTransform();
    //    private readonly ScaleTransform _flipYTransform = new ScaleTransform(1, -1);
    //    private GameWindow _glfwWindow;
        
    //    private int _width;
    //    private int _height;
    //    private System.Timers.Timer timer;

    //    public GLWpfControl() { base.ClipToBounds = false; }

    //    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    //    {
    //        //OpenTK.Toolkit.Init();

    //        _glfwWindow = new OpenTK.GameWindow(600, 600, 
    //            new OpenTK.Graphics.GraphicsMode(24, 24, 8), 
    //            "Globe3DNative", 
    //            OpenTK.GameWindowFlags.Default, OpenTK.DisplayDevice.Default, 
    //            3, 0, // Request OpenGL 3.3 
    //            OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible | OpenTK.Graphics.GraphicsContextFlags.Debug);

    //        _glfwWindow.Visible = false;
    //        //    _glfwWindow.WindowState = OpenTK.WindowState.Minimized;
    //        //    _glfwWindow.WindowBorder = WindowBorder.Hidden;

    //        _glfwWindow.MakeCurrent();

    //        timer = new System.Timers.Timer(1000.0 / 60);
    //        // Hook up the Elapsed event for the timer. 
    //        timer.Elapsed += (s, e) => base.InvalidateVisual();
    //        timer.AutoReset = true;
    //        timer.Enabled = true;
               
    //        base.OnAttachedToVisualTree(e);
    //    }

    //    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    //    {                  
    //        _glfwWindow?.Dispose();

    //        base.OnDetachedFromVisualTree(e);
    //    }

    //    public override void Render(DrawingContext drawingContext)
    //    {
    //        drawingContext.DrawRectangle(new Pen() { Brush = Brushes.Red, Thickness = 3 },    
    //            new Rect(new Avalonia.Point(), new Avalonia.Size(_width, _height)));

    //        GL.ClearColor(0.0f, 1.0f, 1.0f, 1.0f);                
    //        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
           
    //        GL.Flush();
            
    //        var _d3dImage = UpdateImage();

    //        // Transforms are applied in reverse order
    //        //  drawingContext.PushTransform(_translateTransform);              // Apply translation to the image on the Y axis by the height. This assures that in the next step, where we apply a negative scale the image is still inside of the window
    //        //  drawingContext.PushTransform(_flipYTransform);                  // Apply a scale where the Y axis is -1. This will rotate the image by 180 deg


    //          using (drawingContext.PushPreTransform(_translateTransform.Value))
    //          using (drawingContext.PushPreTransform(_flipYTransform.Value))

    //        {
    //            drawingContext.DrawImage(_d3dImage, new Rect(_d3dImage.Size));            // Draw the image source 
    //        }
               
    //      //      drawingContext.Pop();                                           // Remove the scale transform
    //      //      drawingContext.Pop();                                           // Remove the translation transform

            
    //        base.Render(drawingContext);
    //    }

    //    protected override Avalonia.Size ArrangeOverride(Avalonia.Size finalSize)
    //    {
    //        _width = (int)finalSize.Width;
    //        _height = (int)finalSize.Height;

    //        _glfwWindow.Size = new OpenTK.Size(_width, _height);
    //        _glfwWindow.Bounds = new Rectangle(0, 0, _width, _height);

    //        _translateTransform.Y = _height;

    //        if (finalSize.Width > 0 && finalSize.Height > 0)
    //        {         
    //            InvalidateVisual();
    //        }

    //        return base.ArrangeOverride(finalSize);
    //    }

    //    public IImage UpdateImage()
    //    {           
    //        var image = new WriteableBitmap(new PixelSize(_width, _height), new Vector(96, 96), Avalonia.Platform.PixelFormat.Rgba8888);

    //        using (var buffer = image.Lock())
    //        {
    //            byte[] pixels = new byte[_width * _height * 4];

    //            GL.PixelStore(PixelStoreParameter.PackRowLength, buffer.RowBytes / 4);

    //            GL.ReadPixels(0, 0, _width, _height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);

    //            byte[] fixedPixels = new byte[_width * _height * 4];
               
    //            for (int h = 0; h < _height; h++)
    //            {
    //                for (int w = 0; w < _width; w++)
    //                {
    //                    // Remove alpha blending from the end image - we just want the post-render colors
    //                    pixels[((w + h * _width) * 4) + 3] = 255;

    //                    // Copy a 4 byte pixel one at a time
    //                    Array.Copy(pixels, (w + h * _width) * 4, fixedPixels, ((_height - h - 1) * _width + w) * 4, 4);
    //                }
    //            }

    //            Marshal.Copy(fixedPixels, 0, buffer.Address, fixedPixels.Length);
    //        }

    //        return image;
    //    }

    //}

}
