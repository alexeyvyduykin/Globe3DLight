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
        private bool _popupIsOpen;

        public IntervalVisual()
        {                               
            PointerEnter += IntervalVisual_PointerEnter;
            PointerLeave += IntervalVisual_PointerLeave;
         
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

        private void IntervalVisual_PointerLeave(object? sender, PointerEventArgs e)
        {
            if (_popupIsOpen == true)
            {
                _popupIsOpen = false;

                Scheduler?.HideTooltip();
                
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

                if (Marker is not null && Marker is IInterval ival)
                {                   
                    var tooltip = ival.SeriesControl.Tooltip;
                    tooltip.DataContext = ival.SeriesControl.CreateTooltip(ival);

                    Scheduler?.ShowTooltip(this, tooltip);
                }

                Cursor = new Cursor(StandardCursorType.Hand);

                _scale.ScaleY = 1.5;
              
                InvalidateVisual();    
            }
        }

        protected override void Update()
        {        
            if (Scheduler is not null && Marker is not null && Marker is IInterval marker)
            {
                var d1 = Scheduler.FromLocalToAbsolute(new Point2D(marker.Left, marker.LocalPosition.Y)).X;
                var d2 = Scheduler.FromLocalToAbsolute(new Point2D(marker.Right, marker.LocalPosition.Y)).X;

                _widthX = d2 - d1;

         //       InvalidateVisual();
            }               
        }
        
        public override void Render(DrawingContext context)
        {
            if (_widthX == 0.0)
            {
                return;
            }

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

        public override BaseIntervalVisual Clone(IInterval interval)
        {
            return new IntervalVisual() 
            {
                Background = this.Background,
                DataContext = interval 
            };
        }
    }
}
