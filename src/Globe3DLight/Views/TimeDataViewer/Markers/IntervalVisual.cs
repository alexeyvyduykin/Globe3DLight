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
using Globe3DLight.Spatial;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class IntervalVisual : Control
    {
     //   public readonly Popup Popup = new Popup();
        SchedulerGridControl Map;
     //   public readonly IntervalTooltip Tooltip;// = new IntervalTooltip();
        public readonly SchedulerInterval Marker;

        public IntervalVisual(SchedulerInterval m)
        {
            Marker = m;
            Marker.ZIndex = 100;

            //Tooltip = new IntervalTooltip()
            //{
            //    DataContext = new IntervalTooltipViewModel(m),
            //};


            // Popup.AllowsTransparency = true;
      //      Popup.PlacementTarget = this;
      //      Popup.PlacementMode = PlacementMode.Pointer;
      //      Popup.Child = Tooltip;
      //      Popup.Child.Opacity = 0.777;



            base.PointerEnter += IntervalVisual_PointerEnter;
            base.PointerLeave += IntervalVisual_PointerLeave;

            base.LayoutUpdated += IntervalVisual_LayoutUpdated;
            base.Initialized += IntervalVisual_Initialized;

            //    RenderTransform = scale;

            HeightY = 20;
        }

        private void IntervalVisual_Initialized(object sender, EventArgs e)
        {
            Map = Marker.Map as SchedulerGridControl;

     //       Map.TopLevelForToolTips.Children.Add(Popup);

            Map.OnSchedulerZoomChanged += Map_OnMapZoomChanged;
            Map.LayoutUpdated += Map_LayoutUpdated;
            UpdateInterval();

            InvalidateVisual();
            // UpdateVisual(true);
        }

        private void Map_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateInterval();
            InvalidateVisual();
            // UpdateVisual(true);
        }

        private void IntervalVisual_LayoutUpdated(object sender, EventArgs e)
        {

            Marker.Offset = new Point2D(-base.DesiredSize.Width / 2, -base.DesiredSize.Height / 2);
            //   scale.CenterX = -Marker.Offset.X;
            //   scale.CenterY = -Marker.Offset.Y;
        }

        private void IntervalVisual_PointerLeave(object sender, PointerEventArgs e)
        {
            //if (Popup.IsOpen)
            //{
            //    Popup.IsOpen = false;
            //}

            Marker.ZIndex -= 10000;
            Cursor = new Cursor(StandardCursorType.Arrow);// Cursors.Arrow;

            //this.Effect = null;

            scale.ScaleY = 1;
            //  scale.ScaleX = 1;
        }

        private void IntervalVisual_PointerEnter(object sender, PointerEventArgs e)
        {
            //if (Popup.IsOpen == false)
            //{
            //    Popup.IsOpen = true;

            //    // Popup.InvalidateVisual();
            //}

            Marker.ZIndex += 10000;
            Cursor = new Cursor(StandardCursorType.Hand);// Cursors.Hand;

            // this.Effect = ShadowEffect;

            scale.ScaleY = 1.5;
            // scale.ScaleX = 1;
        }

        double WidthX = 0.0;
        double HeightY = 0.0;
        private void Map_OnMapZoomChanged()
        {
            UpdateInterval();


            InvalidateVisual();
            //  UpdateVisual(true);
        }

        private void UpdateInterval()
        {
            //   var d1 = Map.FromLocalValueToPixelX(Marker.Left);
            //   var d2 = Map.FromLocalValueToPixelX(Marker.Right);


            var d1 = Map.FromLocalToAbsolute(new Point2D(Marker.Left, Marker.LocalPosition.Y)).X;
            var d2 = Map.FromLocalToAbsolute(new Point2D(Marker.Right, Marker.LocalPosition.Y)).X;

            //    var d1 = Map.FromSchedulerPointToLocal(new SCSchedulerPoint(Marker.Left, Marker.Position.Y)).X;
            //    var d2 = Map.FromSchedulerPointToLocal(new SCSchedulerPoint(Marker.Right, Marker.Position.Y)).X;

            WidthX = d2 - d1;
        }

        readonly ScaleTransform scale = new ScaleTransform(1, 1);

        //  public DropShadowEffect ShadowEffect;

        private Brush background = new SolidColorBrush() { Color = Colors.LightGray };
        public Brush Background
        {
            get
            {
                return background;
            }
            set
            {
                if (background != value)
                {
                    background = value;
                    IsChanged = true;
                }
            }
        }

        private Brush foreground = new SolidColorBrush() { Color = Colors.White };
        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                if (foreground != value)
                {
                    foreground = value;
                    IsChanged = true;
                }
            }
        }

        private Pen stroke = new Pen(Brushes.Black, 1.0);
        public Pen Stroke
        {
            get
            {
                return stroke;
            }
            set
            {
                if (stroke != value)
                {
                    stroke = value;
                    IsChanged = true;
                }
            }
        }

        public bool IsChanged = true;

        public override void Render(DrawingContext drawingContext)
        {
            //  base.Render(drawingContext);

            if (WidthX == 0.0)
                return;


            double thick_half = Stroke.Thickness / 2.0;

            Point p0 = new Point(-WidthX / 2.0, -HeightY / 2.0);
            Point p1 = new Point(WidthX / 2.0, HeightY / 2.0);

            Rect RectBorder = new Rect(
                      new Point(-WidthX / 2.0 + thick_half, -HeightY / 2.0 + thick_half),
                      new Point(WidthX / 2.0 - thick_half, HeightY / 2.0 - thick_half));

            Rect RectSolid = new Rect(p0, p1);

            //   using (drawingContext.PushPreTransform /*PushTransform*/(scale))
            {
                RectangleGeometry rectangle1 = new RectangleGeometry(RectSolid); //new RectangleGeometry(RectSolid, 5, 5);
                RectangleGeometry rectangle2 = new RectangleGeometry(RectBorder);//, 5, 5);
                drawingContext.DrawGeometry(Background, null, rectangle1);
                //  drawingContext.DrawRectangle(Background, null, RectSolid);
                drawingContext.DrawGeometry(null, Stroke, rectangle2);
                // drawingContext.DrawRectangle(null, Stroke, RectBorder);
            }

            //  drawingContext.Pop();

            //         drawingContext.DrawEllipse(Brushes.Black, null, new Point(0, 0), 5, 5);                  
        }

    }
}
