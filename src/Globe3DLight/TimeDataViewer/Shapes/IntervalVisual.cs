using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using System.Windows.Input;
using Avalonia.Media;
using System.Globalization;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Specialized;
using Avalonia.Controls.Shapes;
using TimeDataViewer.Spatial;
using TimeDataViewer.ViewModels;
using TimeDataViewer.Models;
using Avalonia.LogicalTree;

namespace TimeDataViewer.Shapes
{
    public class IntervalVisual : BaseIntervalVisual
    {
        private double _widthX = 0.0;           
        private readonly ScaleTransform _scale;
        private IScheduler? _map;
        private IntervalViewModel? _marker;
        private bool _popupIsOpen;

        public IntervalVisual()
        {                               
            PointerEnter += IntervalVisual_PointerEnter;
            PointerLeave += IntervalVisual_PointerLeave;
         
            DataContextProperty.Changed.AddClassHandler<IntervalVisual>((d, e) => d.MarkerChanged(e));

            _popupIsOpen = false;

            _scale = new ScaleTransform(1, 1);                
        }

        public static readonly StyledProperty<Color> BackgroundProperty =    
            AvaloniaProperty.Register<IntervalVisual, Color>(nameof(Background), Colors.LightGray);

        public Color Background
        {
            get { return GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly StyledProperty<double> HeightYProperty =    
            AvaloniaProperty.Register<IntervalVisual, double>(nameof(HeightY), 20.0);

        public double HeightY
        {
            get { return GetValue(HeightYProperty); }
            set { SetValue(HeightYProperty, value); }
        }

        public static readonly StyledProperty<Color> StrokeColorProperty =   
            AvaloniaProperty.Register<IntervalVisual, Color>(nameof(StrokeColor), Colors.Black);

        public Color StrokeColor
        {
            get { return GetValue(StrokeColorProperty); }
            set { SetValue(StrokeColorProperty, value); }
        }

        public static readonly StyledProperty<double> StrokeThicknessProperty =    
            AvaloniaProperty.Register<IntervalVisual, double>(nameof(StrokeThickness), 1.0);

        public double StrokeThickness
        {
            get { return GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        private void MarkerChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IntervalViewModel marker)
            {
                _marker = marker;
                _marker.ZIndex = 100;
            }
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);

            _map = (_marker is not null) ? _marker.Scheduler : null;

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

        private void IntervalVisual_PointerLeave(object? sender, PointerEventArgs e)
        {
            if (_popupIsOpen == true)
            {
                _popupIsOpen = false;

                _map?.HideTooltip();
                
                Cursor = new Cursor(StandardCursorType.Arrow);

                _scale.ScaleY = 1;

                InvalidateVisual();   
            }
        }

        private void IntervalVisual_PointerEnter(object? sender, PointerEventArgs e)
        {
            if (_popupIsOpen == false)
            {
                _popupIsOpen = true;

                if (_marker is not null)
                {
                    var tooltip = Series.Tooltip;
                    tooltip.DataContext = Series.CreateTooltip(_marker);

                    _map?.ShowTooltip(this, tooltip);
                }

                Cursor = new Cursor(StandardCursorType.Hand);

                _scale.ScaleY = 1.5;
              
                InvalidateVisual();    
            }
        }

        private void Update()
        {        
            if (_map is not null && _marker is not null)
            {
                var d1 = _map.FromLocalToAbsolute(new Point2D(_marker.Left, _marker.LocalPosition.Y)).X;
                var d2 = _map.FromLocalToAbsolute(new Point2D(_marker.Right, _marker.LocalPosition.Y)).X;

                _widthX = d2 - d1;
            }

            InvalidateVisual();        
        }
        
        public override void Render(DrawingContext context)
        {                      
            if (_widthX == 0.0)
                return;

            var p0 = new Point(-_widthX / 2.0, -HeightY / 2.0);
            var p1 = new Point(_widthX / 2.0, HeightY / 2.0);

            var rect = new Rect(p0, p1);
                        
            var brush = new SolidColorBrush() { Color = Background };
            var pen = new Pen(new SolidColorBrush() { Color = StrokeColor }, StrokeThickness);

            using (context.PushPreTransform(_scale.Value))
            {        
                context.DrawRectangle(brush, pen, rect);
            }
        }

        public override BaseIntervalVisual Clone()
        {   
            return new IntervalVisual() 
            {              
                Background = this.Background,
                HeightY = this.HeightY,
                Series = this.Series,
            };
        }
    }
}
