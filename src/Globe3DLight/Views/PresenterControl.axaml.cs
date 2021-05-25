using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.Visuals.Media.Imaging;
using Globe3DLight.Models.Renderer;
using Globe3DLight.Modules.Renderer;
using Globe3DLight.ViewModels.Containers;
using Globe3DLight.ViewModels.Renderer.Presenters;
using System.Diagnostics;
using Avalonia.Input;

namespace Globe3DLight.Views
{
    public class PresenterControl : UserControl
    {
        private readonly TranslateTransform _transform;
        private readonly ScaleTransform _flipYTransform;
        private static readonly IContainerPresenter s_editorPresenter = new EditorPresenter();

        private int _width;
        private int _height;
        private DispatcherTimer _timer;
        private double _fps = 60; 
        private double _currentFps = 0.0;

#if USE_DIAGNOSTICS
        private double _last = 0.0;
        private int _frames = 0; 
        private double _totalTime = 0.0;
#endif

        internal struct CustomState
        {
            public ScenarioContainerViewModel Container;
            public IRenderContext Renderer;
        }

        public PresenterControl()
        {
            InitializeComponent();

            _transform = new TranslateTransform();
            _flipYTransform = new ScaleTransform(1, -1);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
       
        public static readonly StyledProperty<ScenarioContainerViewModel> ContainerProperty =
            AvaloniaProperty.Register<PresenterControl, ScenarioContainerViewModel>(nameof(Container), null);

        public static readonly StyledProperty<IRenderContext> RendererProperty =
            AvaloniaProperty.Register<PresenterControl, IRenderContext>(nameof(Renderer), null);

        public static readonly StyledProperty<IPresenterContract> PresenterContractProperty =
            AvaloniaProperty.Register<PresenterControl, IPresenterContract>(nameof(PresenterContract), null);

        public ScenarioContainerViewModel Container
        {
            get => GetValue(ContainerProperty);
            set => SetValue(ContainerProperty, value);
        }

        public IRenderContext Renderer
        {
            get => GetValue(RendererProperty);
            set => SetValue(RendererProperty, value);
        }

        public IPresenterContract PresenterContract
        {
            get => GetValue(PresenterContractProperty);
            set => SetValue(PresenterContractProperty, value);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var customState = new CustomState()
            {
                Container = Container,
                Renderer = Renderer ?? GetValue(RendererOptions.RendererProperty),
            };

#if USE_DIAGNOSTICS
            double current = Stopwatch.GetTimestamp() - _last;
#endif

            Container.LogicalUpdate();

            Draw(customState, context);

#if USE_DIAGNOSTICS
            _frames++;
            _totalTime += current / Stopwatch.Frequency;

            if (_totalTime >= 1.0)
            {
                _currentFps = _frames;            
                _frames = 0;
                _totalTime = 0.0;
            }
            
            _last = Stopwatch.GetTimestamp();
          
            DrawDiagnostics(context);                  
#endif
        }

        private void DrawDiagnostics(DrawingContext context)
        {
            var foreground = new SolidColorBrush(Colors.White, 0.85);

            var topLeft = new Point(4, 4);
            var size = new Size(60, 20);

            var text = new FormattedText()
            {
                Text = string.Format("Fps: {0}", _currentFps),
                Typeface = new Typeface(new FontFamily("Comic Sans MS, Verdana"), FontStyle.Normal, FontWeight.Normal),
                FontSize = 14,
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.NoWrap,
                Constraint = size,
            };

            context.DrawRectangle(new SolidColorBrush(Colors.Black, 0.70), null, new Rect(topLeft, size));
            context.DrawText(foreground, topLeft, text);
        }

        internal void Draw(CustomState customState, object context)
        {
            if (context is DrawingContext drawingContext)
            {
                if (customState.Container != null && customState.Renderer != null)
                {
                    UpdatePresenterContract();
                    //          drawingContext.DrawRectangle(new Pen() { Brush = Brushes.Red, Thickness = 3 },
                    //new Rect(new Avalonia.Point(), new Avalonia.Size(_width, _height)));

                    try
                    {
                        PresenterContract.DrawBegin();
                        {
                            s_editorPresenter.Render(context, customState.Renderer, customState.Container);
                        }

                        var bitmap = new WriteableBitmap(
                            new PixelSize(PresenterContract.Width, PresenterContract.Height), 
                            new Vector(/*144,144*/96.0, 96.0),            
                            Avalonia.Platform.PixelFormat.Rgba8888, Avalonia.Platform.AlphaFormat.Unpremul);

                        using (var buffer = bitmap.Lock())
                        {
                            PresenterContract.ReadPixels(buffer.Address, buffer.RowBytes);
                        }

                        using (drawingContext.PushPreTransform(_transform.Value))
                        using (drawingContext.PushPreTransform(_flipYTransform.Value))
                        {
                            drawingContext.DrawImage(
                                bitmap,
                                new Rect(new Size(PresenterContract.Width, PresenterContract.Height)),
                                new Rect(new Size(_width, _height)),
                                BitmapInterpolationMode.LowQuality);
                        }

                        //             Debug.WriteLine($"Width: {PresenterContract.Width}, Height: {PresenterContract.Height}");
                        //             Debug.WriteLine($"_Width: {_width}, _Height: {_height}");
                        
                        customState.Container?.Invalidate();
                    }
                    catch
                    {
                        throw new Exception("Error: PresenterControl -> Draw");
                    }
                }
            }
        }

        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);

            var width = (int)finalRect.Width;
            var height = (int)finalRect.Height;

            if (_width != width || _height != height)
            {
                _width = width;
                _height = height;

                if (Container is not null)
                {
                    Container.Width = _width;
                    Container.Height = _height;
                }

                PresenterContract.Resize(_width, _height);

                _transform.Y = _height;
            }
        }

        void UpdatePresenterContract()
        {
            if (PresenterContract.Width != _width || PresenterContract.Height != _height)
            {
                PresenterContract.Resize(_width, _height);
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / _fps);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            InvalidateVisual();
        }
  
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if (_timer is not null)
            {
                _timer.Stop();
                _timer.Tick -= _timer_Tick;              
            }
           
            base.OnDetachedFromVisualTree(e);
        }
    }
}
