using System;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Media;
//using Avalonia.OpenGL.Imaging;
//using static Avalonia.OpenGL.GlConsts;

using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Shapes;
using Avalonia.Styling;
using Avalonia.Visuals.Media.Imaging;
using GlmSharp;
using Globe3DLight;
using Globe3DLight.Renderer;
using System.Timers;
using System.Runtime.InteropServices;
using Globe3DLight.AvaloniaUI.OpenTK;


namespace Globe3DLight.AvaloniaUI.Views
{
    
    public class OpenTKControl2 : UserControl
    {
        private readonly TranslateTransform _translateTransform = new TranslateTransform();
        private readonly ScaleTransform _flipYTransform = new ScaleTransform(1, -1);
        
        private int _width;
        private int _height;
        private System.Timers.Timer _timer;
        private double _fps = 60;

        private readonly IPresenter _presenter = new OpenTKPresenter();

        public OpenTKControl2()
        {
            this.InitializeComponent(); 
            
            base.ClipToBounds = false;        
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e); 
            


            _timer = new System.Timers.Timer(1000.0 / _fps);
            // Hook up the Elapsed event for the timer. 
            _timer.Elapsed += (s, e) => base.InvalidateVisual();
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        protected override Avalonia.Size ArrangeOverride(Avalonia.Size finalSize)
        {
            _width = (int)finalSize.Width;
            _height = (int)finalSize.Height;

            //         _window.Size = new OpenTK.Size(_width, _height);
            //         _window.Bounds = new OpenTK.Rectangle(0, 0, _width, _height);

            _translateTransform.Y = _height;

            if (_width >= _height)
            {
                int d = (int)((_width - _height) / 2.0);
                //        GlWindow.Viewport = new OpenTK.Rectangle(0, -d, w, w);
            }

            if (_height > _width)
            {
                int d = (int)((_height - _width) / 2.0);
                //     GlWindow.Viewport = new OpenTK.Rectangle(-d, 0, h, h);
            }



            return base.ArrangeOverride(finalSize);
        }
  
        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(new Pen() { Brush = Brushes.Red, Thickness = 3 },
                new Rect(new Avalonia.Point(), new Avalonia.Size(_width, _height)));

            WriteableBitmap bitmap =    
                new WriteableBitmap(new PixelSize(_presenter.Width, _presenter.Height), new Vector(96.0, 96.0), Avalonia.Platform.PixelFormat.Rgba8888);

            using (var buffer = bitmap.Lock())
            {
                _presenter.ReadPixels(buffer.Address, buffer.RowBytes);
            }

            using (drawingContext.PushPreTransform(_translateTransform.Value))
            using (drawingContext.PushPreTransform(_flipYTransform.Value))
            {
                drawingContext.DrawImage(
                    bitmap/*, 1.0*/, 
                    new Rect(bitmap.Size), 
                    new Rect(new Avalonia.Size(_width, _height)),          
                    BitmapInterpolationMode.LowQuality);
            }
        }


      
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            base.OnDetachedFromVisualTree(e);
        }
    }
}
