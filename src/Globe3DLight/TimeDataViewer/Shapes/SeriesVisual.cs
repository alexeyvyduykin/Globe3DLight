using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using TimeDataViewer.Models;
using TimeDataViewer.ViewModels;

namespace TimeDataViewer.Shapes
{
    public class SeriesVisual : BaseVisual
    {  
        private double _widthX = 0.0;

        public SeriesVisual()
        {        

        }

        public static readonly StyledProperty<double> HeightYProperty =
            AvaloniaProperty.Register<IntervalVisual, double>(nameof(HeightY), 2.0);

        public double HeightY
        {
            get { return GetValue(HeightYProperty); }
            set { SetValue(HeightYProperty, value); }
        }

        protected override void Update()
        {
            if (Scheduler is not null)
            {
                var left = Scheduler.AbsoluteWindow.Left;
                var right = Scheduler.AbsoluteWindow.Right;

                _widthX = right - left;
                
        //        InvalidateVisual();
            }           
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(Brushes.Black, new Rect(new Point(0, -HeightY / 2.0), new Point(_widthX, HeightY / 2.0)));
        }
    }
}
