#nullable disable
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

namespace Globe3DLight.Views
{
    public class PresenterControl : UserControl
    {
        private readonly TranslateTransform _translateTransform = new TranslateTransform();
        private readonly ScaleTransform _flipYTransform = new ScaleTransform(1, -1);

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

        private static readonly IContainerPresenter s_editorPresenter = new EditorPresenter();

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

        public PresenterControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        internal struct CustomState
        {
            public ScenarioContainerViewModel Container;
            public IRenderContext Renderer;
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
                    //          drawingContext.DrawRectangle(new Pen() { Brush = Brushes.Red, Thickness = 3 },
                    //new Rect(new Avalonia.Point(), new Avalonia.Size(_width, _height)));


                    try
                    {

                        PresenterContract.DrawBegin();
                        {
                            s_editorPresenter.Render(context, customState.Renderer, customState.Container);
                        }

                        WriteableBitmap bitmap =
            new WriteableBitmap(new PixelSize(PresenterContract.Width, PresenterContract.Height), new Vector(/*144,144*/96.0, 96.0),
            Avalonia.Platform.PixelFormat.Rgba8888, Avalonia.Platform.AlphaFormat.Unpremul);


                        using (var buffer = bitmap.Lock())
                        {
                            PresenterContract.ReadPixels(buffer.Address, buffer.RowBytes);
                        }

                        using (drawingContext.PushPreTransform(_translateTransform.Value))
                        using (drawingContext.PushPreTransform(_flipYTransform.Value))
                        {
                            drawingContext.DrawImage(
                                bitmap/*, 1.0*/,
                                new Rect(/*bitmap.Size*/new Avalonia.Size(PresenterContract.Width, PresenterContract.Height)),
                                new Rect(new Avalonia.Size(_width, _height)),
                                BitmapInterpolationMode.LowQuality);
                        }


                        customState.Container?.Invalidate();
                        //customState.Renderer.State.PointStyle.Invalidate();
                        //customState.Renderer.State.SelectedPointStyle.Invalidate();
                    }
                    catch
                    {
                        throw new System.Exception();
                    }
                }
            }
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            //      _timer = new System.Timers.Timer(1000.0 / _fps);
            // Hook up the Elapsed event for the timer. 
            //       _timer.Elapsed += (s, e) => base.InvalidateVisual();
            //       _timer.AutoReset = true;
            //        _timer.Enabled = true;


            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1.0 / _fps);
            _timer.Tick += (s, e) => base.InvalidateVisual();
            _timer.Start();
        }

        protected override Size MeasureCore(Size availableSize)
        {
            _width = (int)availableSize.Width;
            _height = (int)availableSize.Height;

            if (Container != null)
            {
                Container.Width = _width;
                Container.Height = _height;
            }

            PresenterContract.Resize(_width, _height);

            _translateTransform.Y = _height;

            return base.MeasureCore(availableSize);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _timer?.Stop();

            //         _timer.Stop();
            //         _timer.Dispose();
            base.OnDetachedFromVisualTree(e);
        }
    }
}
