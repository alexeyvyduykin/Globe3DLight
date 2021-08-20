using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.VisualTree;
using TimeDataViewer.Spatial;
using A = Avalonia.Layout;

namespace TimeDataViewer
{
    public class TrackerControl : ContentControl
    {
        public static readonly StyledProperty<OxyRect> LineExtentsProperty = AvaloniaProperty.Register<TrackerControl, OxyRect>(nameof(LineExtents), new OxyRect());
        public static readonly StyledProperty<double> DistanceProperty = AvaloniaProperty.Register<TrackerControl, double>(nameof(Distance), 7.0);
        public static readonly StyledProperty<bool> CanCenterHorizontallyProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(CanCenterHorizontally), true);
        public static readonly StyledProperty<bool> CanCenterVerticallyProperty = AvaloniaProperty.Register<TrackerControl, bool>(nameof(CanCenterVertically), true);
        public static readonly StyledProperty<ScreenPoint> PositionProperty = AvaloniaProperty.Register<TrackerControl, ScreenPoint>(nameof(Position), new ScreenPoint());
        public static readonly StyledProperty<Thickness> MarginPointerProperty = AvaloniaProperty.Register<TrackerControl, Thickness>(nameof(MarginPointer), new Thickness());

        private ContentPresenter? _content;
        private Panel? _contentContainer;

        static TrackerControl()
        {
            ClipToBoundsProperty.OverrideDefaultValue<TrackerControl>(false);
            PositionProperty.Changed.AddClassHandler<TrackerControl>(PositionChanged);
        }

        public OxyRect LineExtents
        {
            get
            {
                return GetValue(LineExtentsProperty);
            }

            set
            {
                SetValue(LineExtentsProperty, value);
            }
        }

        public double Distance
        {
            get
            {
                return GetValue(DistanceProperty);
            }

            set
            {
                SetValue(DistanceProperty, value);
            }
        }

        public bool CanCenterHorizontally
        {
            get
            {
                return GetValue(CanCenterHorizontallyProperty);
            }

            set
            {
                SetValue(CanCenterHorizontallyProperty, value);
            }
        }

        public bool CanCenterVertically
        {
            get
            {
                return GetValue(CanCenterVerticallyProperty);
            }

            set
            {
                SetValue(CanCenterVerticallyProperty, value);
            }
        }

        public ScreenPoint Position
        {
            get
            {
                return GetValue(PositionProperty);
            }

            set
            {
                SetValue(PositionProperty, value);
            }
        }

        public Thickness MarginPointer
        {
            get
            {
                return GetValue(MarginPointerProperty);
            }

            set
            {
                SetValue(MarginPointerProperty, value);
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _content = e.NameScope.Get<ContentPresenter>("PART_Content");
            _contentContainer = e.NameScope.Get<Panel>("PART_ContentContainer");

            UpdatePositionAndBorder();
        }

        private static void PositionChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs e)
        {
            ((TrackerControl)sender).OnPositionChanged(e);
        }

        private void OnPositionChanged(AvaloniaPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            UpdatePositionAndBorder();
        }

        private void UpdatePositionAndBorder()
        {
            if (_contentContainer == null || _content == null)
            {
                return;
            }

            Canvas.SetLeft(this, Position.X);
            Canvas.SetTop(this, Position.Y);
            Control parent = this;
            while (!(parent is Canvas) && parent != null)
            {
                parent = parent.GetVisualParent() as Control;
            }

            if (parent == null)
            {
                return;
            }

            // throw new InvalidOperationException("The TrackerControl must have a Canvas parent.");
            var canvasWidth = parent.Bounds.Width;
            var canvasHeight = parent.Bounds.Height;

            _content.Measure(new Size(canvasWidth, canvasHeight));
            _content.Arrange(new Rect(0, 0, _content.DesiredSize.Width, _content.DesiredSize.Height));

            var contentWidth = _content.DesiredSize.Width;
            var contentHeight = _content.DesiredSize.Height;

            // Minimum allowed margins around the tracker
            const double MarginLimit = 10;

            var ha = A.HorizontalAlignment.Center;
            if (CanCenterHorizontally)
            {
                if (Position.X - (contentWidth / 2) < MarginLimit)
                {
                    ha = A.HorizontalAlignment.Left;
                }

                if (Position.X + (contentWidth / 2) > canvasWidth - MarginLimit)
                {
                    ha = A.HorizontalAlignment.Right;
                }
            }
            else
            {
                ha = Position.X < canvasWidth / 2 ? A.HorizontalAlignment.Left : A.HorizontalAlignment.Right;
            }

            var va = A.VerticalAlignment.Center;
            if (CanCenterVertically)
            {
                if (Position.Y - (contentHeight / 2) < MarginLimit)
                {
                    va = A.VerticalAlignment.Top;
                }

                if (ha == A.HorizontalAlignment.Center)
                {
                    va = A.VerticalAlignment.Bottom;
                    if (Position.Y - contentHeight < MarginLimit)
                    {
                        va = A.VerticalAlignment.Top;
                    }
                }

                if (va == A.VerticalAlignment.Center && Position.Y + (contentHeight / 2) > canvasHeight - MarginLimit)
                {
                    va = A.VerticalAlignment.Bottom;
                }

                if (va == A.VerticalAlignment.Top && Position.Y + contentHeight > canvasHeight - MarginLimit)
                {
                    va = A.VerticalAlignment.Bottom;
                }
            }
            else
            {
                va = Position.Y < canvasHeight / 2 ? A.VerticalAlignment.Top : A.VerticalAlignment.Bottom;
            }

            var dx = ha == A.HorizontalAlignment.Center ? -0.5 : ha == A.HorizontalAlignment.Left ? 0 : -1;
            var dy = va == A.VerticalAlignment.Center ? -0.5 : va == A.VerticalAlignment.Top ? 0 : -1;

            MarginPointer = CreateMargin(ha, va);

            _content.Margin = MarginPointer;

            _contentContainer.Measure(new Size(canvasWidth, canvasHeight));
            var contentSize = _contentContainer.DesiredSize;

            _contentContainer.RenderTransform = new TranslateTransform
            {
                X = dx * contentSize.Width,
                Y = dy * contentSize.Height
            };
        }

        private Thickness CreateMargin(A.HorizontalAlignment ha, A.VerticalAlignment va)
        {
            var m = Distance;

            return new Thickness(
                ha == A.HorizontalAlignment.Left ? m : 0,
                va == A.VerticalAlignment.Top ? m : 0,
                ha == A.HorizontalAlignment.Right ? m : 0,
                va == A.VerticalAlignment.Bottom ? m : 0);
        }
    }
}
