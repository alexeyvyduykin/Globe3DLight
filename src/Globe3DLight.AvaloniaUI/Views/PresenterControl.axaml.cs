using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.Visuals.Media.Imaging;
using Globe3DLight.AvaloniaUI.Renderer;
using Globe3DLight.Containers;
using Globe3DLight.Renderer;
using Globe3DLight.Renderer.Presenters;


namespace Globe3DLight.AvaloniaUI.Views
{
    public class PresenterControl : UserControl
    {
        private readonly TranslateTransform _translateTransform = new TranslateTransform();
        private readonly ScaleTransform _flipYTransform = new ScaleTransform(1, -1);

        private int _width;
        private int _height;
        //   private System.Timers.Timer _timer;
        private DispatcherTimer _timer;
        private double _fps = 40;

        private static readonly IContainerPresenter s_editorPresenter = new EditorPresenter();

        public static readonly StyledProperty<IScenarioContainer> ContainerProperty =
            AvaloniaProperty.Register<PresenterControl, IScenarioContainer>(nameof(Container), null);

        public static readonly StyledProperty<IRenderContext> RendererProperty =
            AvaloniaProperty.Register<PresenterControl, IRenderContext>(nameof(Renderer), null);

        public static readonly StyledProperty<IPresenterContract> PresenterContractProperty =
            AvaloniaProperty.Register<PresenterControl, IPresenterContract>(nameof(PresenterContract), null);


        public IScenarioContainer Container
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
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        internal struct CustomState
        {
            public IScenarioContainer Container;
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

            Draw(customState, context);
        }

        internal void Draw(CustomState customState, object context)
        {
            var drawingContext = context as DrawingContext;

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
            _timer.Stop();

            //         _timer.Stop();
            //         _timer.Dispose();
            base.OnDetachedFromVisualTree(e);
        }
    }
}
