using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using TimeDataViewer.Models;
using TimeDataViewer.ViewModels;

namespace TimeDataViewer.Shapes
{
    public class StringVisual : Control
    {
        private readonly SeriesViewModel _marker;
        private IScheduler? _map;
        private double _widthX = 0.0;

        public StringVisual(SeriesViewModel marker)
        {
            _marker = marker;
            _marker.ZIndex = 30;

            RenderTransform = new ScaleTransform(1, 1);
        }

        public static readonly StyledProperty<double> HeightYProperty =
            AvaloniaProperty.Register<IntervalVisual, double>(nameof(HeightY), 2.0);

        public double HeightY
        {
            get { return GetValue(HeightYProperty); }
            set { SetValue(HeightYProperty, value); }
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);

            _map = _marker.Scheduler;
            
            if (_map is not null)
            {
                _map.OnZoomChanged += (s, e) => Update();
                _map.OnSizeChanged += (s, e) => Update();
            }
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            
            if (_map is not null)
            {
                _map.OnZoomChanged -= (s, e) => Update();
                _map.OnSizeChanged -= (s, e) => Update();
            }
        }

        protected void Update()
        {
            if (_map is not null)
            {
                var left = _map.AbsoluteWindow.Left;
                var right = _map.AbsoluteWindow.Right;

                _widthX = right - left;
            }

            InvalidateVisual();
        }

        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Brushes.Black,
                new Rect(new Point(0, -HeightY / 2.0), new Point(_widthX, HeightY / 2.0)));
        }
    }
}
