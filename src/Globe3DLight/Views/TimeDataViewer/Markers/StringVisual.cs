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
using Globe3DLight.Spatial;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class StringVisual : Control
    {
        public readonly SchedulerString Marker;
        SchedulerGridControl Map;
    //    public readonly Popup Popup = new Popup();
    //    public readonly StringTooltip Tooltip = new StringTooltip();

        public StringVisual(SchedulerString m)
        {
            Marker = m;
            Marker.ZIndex = 30;

            // Popup.AllowsTransparency = true;
            //Popup.PlacementTarget = this;
            //Popup.PlacementMode = PlacementMode.Pointer;
            //Popup.Child = Tooltip;
            //Popup.Child.Opacity = 0.777;

            base.LayoutUpdated += StringVisual_LayoutUpdated;
            base.PointerEnter += StringVisual_PointerEnter;
            base.PointerLeave += StringVisual_PointerLeave;
            base.PointerMoved += StringVisual_PointerMoved;
            base.Initialized += StringVisual_Initialized;

            base.PointerPressed += StringVisual_PointerPressed;
            base.PointerReleased += StringVisual_PointerReleased;

            //Height = 4;
            HeightY = 5;
            RenderTransform = scale;
        }

        private void StringVisual_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                if (e.Pointer.Captured == this/*IsMouseCaptured*/)
                {
         //           Popup.IsOpen = true;

                    e.Pointer.Capture(null);
                    //Mouse.Capture(null);
                    base.Cursor = saveCursor;

                    Marker.ResetOffset();

                    Marker.Intervals.ToList().ForEach(s => s.ResetOffset());
                }
            }
        }

        private void StringVisual_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed == true)
            {
                //   if(e.Pointer.Captured != this/*IsMouseCaptured == false*/)          
                {
                    //if (Popup.IsOpen == true)
                    //{
                    //    Popup.IsOpen = false;
                    //}

                    //Mouse.Capture(this);
                    e.Pointer.Capture(this);
                    StartDrag = e.GetPosition(Map);
                    saveCursor = base.Cursor;
                    base.Cursor = new Cursor(StandardCursorType.SizeNorthSouth);// Cursors.SizeNS;
                    //  pLeftSave = pLeft;
                    //  pRightSave = pRight;
                }
            }
        }

        private void StringVisual_Initialized(object sender, EventArgs e)
        {
            Map = Marker.Map as SchedulerGridControl;

            Map.OnSchedulerZoomChanged += Map_OnMapZoomChanged;

    //        Map.TopLevelForToolTips.Children.Add(Popup);


            //    Map.SizeChanged += StringVisual_SizeChanged;

            //       List<SCSchedulerString__> strings = Map.Markers.ToList().Select(s => s.Tag).OfType<SCSchedulerString__>().ToList();
            //       int count = strings.Count();
            //       var index = strings.FindIndex(s => s.Equals(String));

            Map.LayoutUpdated += Map_LayoutUpdated;

            UpdateWidthString();

            base.InvalidateVisual();
            //UpdateVisual(true);
        }

        private void Map_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateWidthString();

            base.InvalidateVisual();

            // UpdateVisual(true);
        }

        private void StringVisual_PointerMoved(object sender, PointerEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed == true && e.Pointer.Captured == this /*&& IsMouseCaptured*/)
            {
                Point pt = e.GetPosition(Map);

                offset = new Point2D(offset.X, pt.Y - StartDrag.Y);

                //  Debug.WriteLine("Dragging String: pLeftY=" + pLeft.Y + " pRightY=" + pRight.Y);

                var min0 = Map.FromLocalToAbsolute(Map.ViewportAreaScreen.TopLeft/*P0*/).Y;
                var max0 = Map.FromLocalToAbsolute(Map.ViewportAreaScreen.BottomRight/*P1*/).Y;

                int min = Math.Min(min0, max0);
                int max = Math.Max(min0, max0);

                var value = pt.Y;

                Debug.WriteLine("min_Y=" + min + " max_Y=" + max + " MouseY=" + pt.Y + " offsetY=" + offset.Y);

                if (min < value && max > value)
                {
                    Marker.Offset = offset;
                }

                //   Marker.Position = Map.FromLocalToSchedulerPoint((int)(0), (int)(pt.Y));
                //    Debug.WriteLine("Mouse: X=" + pt.X + " Y=" + pt.Y);
                //   Debug.WriteLine("Dragging String: locX=" + Marker.LocalPositionX + " locY=" + Marker.LocalPositionY);

                // UpdateVisual(true);
            }
        }

        private void StringVisual_PointerLeave(object sender, PointerEventArgs e)
        {
            //if (Popup.IsOpen == true)
            //{
            //    Popup.IsOpen = false;
            //}
            //   Marker.ZIndex -= 10000;
            Cursor = tempCursor;
            // scale.ScaleY = 1.0;
            // Stroke = new Pen(Brushes.Yellow, 4.0);
            HeightY = 5;

            base.InvalidateVisual();

            // UpdateVisual(true);
        }

        private void StringVisual_PointerEnter(object sender, PointerEventArgs e)
        {
            //if (Popup.IsOpen == false)
            //{
            //    Popup.IsOpen = true;
            //}
            //  Marker.ZIndex += 10000;
            tempCursor = Cursor;
            Cursor = new Cursor(StandardCursorType.Hand);// Cursors.Hand;

            //  scale.ScaleY = 2.0;
            HeightY = 8;
            //  Stroke = new Pen(Brushes.Orange, 8.0);

            base.InvalidateVisual();
            // UpdateVisual(true);
        }

        private void StringVisual_LayoutUpdated(object sender, EventArgs e)
        {
            Marker.Offset = new Point2D(-base.Bounds.Width / 2, -base.Bounds.Height / 2);
            //   scale.CenterX = -Marker.Offset.X;
            //   scale.CenterY = -Marker.Offset.Y;
        }

        #region Dragging

        Point StartDrag;
        Cursor saveCursor;
        Point2D offset = new Point2D();


        double Clamp(double value, double min, double max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        #endregion

        readonly ScaleTransform scale = new ScaleTransform(1, 1);

        double WidthX = 0.0;
        double HeightY = 0.0;
        private void UpdateWidthString()
        {
            //  var p0 = Map.FromLocalToAbsolute(Map.ViewportAreaScreen.P0);
            //  var p1 = Map.FromLocalToAbsolute(Map.ViewportAreaScreen.P1);
            //  WidthX = (p1.X - p0.X);


            var left = Map.AbsoluteWindow.Left;
            var right = Map.AbsoluteWindow.Right;

            WidthX = right - left;
        }

        private void Map_OnMapZoomChanged()
        {
            UpdateWidthString();

            base.InvalidateVisual();

            // UpdateVisual(true);
        }

        Cursor tempCursor;

        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Brushes.Black,
                new Rect(new Point(0, -HeightY / 2.4), new Point(WidthX, HeightY / 2.4)));
        }

    }
}
