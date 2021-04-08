#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class Core
    {
        internal int Width__;
        internal int Height__;

        public int TrueHeight { get { return Height__; } }

        SCAxisBase _axisX, _axisY;
        public SCAxisBase AxisX
        {
            get
            {
                return _axisX;
            }
            set
            {
                if (_axisX != null)
                {
                    this.OnSizeChanged -= _axisX.UpdateAreaSize;
                }

                if (value != null)
                {
                    this.OnSizeChanged += value.UpdateAreaSize;
                }

                _axisX = value;
            }
        }
        public SCAxisBase AxisY
        {
            get
            {


                return _axisY;
            }
            set
            {
                if (_axisY != null)
                {
                    this.OnSizeChanged -= _axisY.UpdateAreaSize;
                }

                if (value != null)
                {
                    this.OnSizeChanged += value.UpdateAreaSize;
                }

                _axisY = value;
            }
        }

        RectI _windowAreaZoom;
        public RectI WindowAreaZoom
        {
            get
            {
                return _windowAreaZoom;
            }
            private set
            {
                if (_windowAreaZoom != value)
                {
                    _windowAreaZoom = value;

                    AxisX.UpdateWindow(_windowAreaZoom);
                    AxisY.UpdateWindow(_windowAreaZoom);

                    //   if (OnSchedulerWindowChanged != null)
                    //       OnSchedulerWindowChanged(_zoomingWindowArea);
                }
            }
        }

        SCViewport _viewportAreaScreen;
        public SCViewport ViewportAreaScreen
        {
            get
            {
                return _viewportAreaScreen;
            }
            private set
            {
                if (_viewportAreaScreen != value)
                {
                    _viewportAreaScreen = value;


                    if (AxisX is SCDateTimeAxis)
                    {
                        var axis = AxisX as SCDateTimeAxis;
                        string x = string.Format("{0:dd/MMM/yyyy HH:mm}", axis.Epoch0.AddSeconds(value.X));
                        string w = TimeSpan.FromSeconds(value.Width).ToString(@"dd\.hh\:mm\:ss");
                        Debug.WriteLine("Core: ViewportAreaScreen -> X = {0}; Y = {1}; W = {2}; H = {3}", x, value.Y, w, value.Height);
                    }
                    else
                    {
                        Debug.WriteLine("Core: ViewportAreaScreen -> X = {0}; Y = {1}; W = {2}; H = {3}", value.X, value.Y, value.Width, value.Height);
                    }

                    AxisX.UpdateScreen(_viewportAreaScreen);
                    AxisY.UpdateScreen(_viewportAreaScreen);

                    //  if (OnSchedulerViewportChanged != null)
                    //      OnSchedulerViewportChanged(_currentViewportArea);
                }
            }
        }

        private SCViewport _viewportAreaData = SCViewport.Empty;
        public SCViewport ViewportAreaData
        {
            get
            {
                if (_viewportAreaData.IsEmpty == true)
                {
                    //      _viewportAreaData = new SCViewport(0.0, 0.0, 1.0, 1.0);
                }

                return _viewportAreaData;
            }
            private set
            {
                _viewportAreaData = value;

                if (AxisX is SCDateTimeAxis)
                {
                    var axis = AxisX as SCDateTimeAxis;
                    string x = string.Format("{0:dd/MMM/yyyy HH:mm}", axis.Epoch0.AddSeconds(value.X));
                    string w = TimeSpan.FromSeconds(value.Width).ToString(@"dd\.hh\:mm\:ss");
                    Debug.WriteLine("Core: ViewportAreaData -> X = {0}; Y = {1}; W = {2}; H = {3}", x, value.Y, w, value.Height);
                }
                else
                {
                    Debug.WriteLine("Core: ViewportAreaData -> X = {0}; Y = {1}; W = {2}; H = {3}", value.X, value.Y, value.Width, value.Height);
                }

                AxisX.UpdateViewport(_viewportAreaData);
                AxisY.UpdateViewport(_viewportAreaData);

                //  if (OnSchedulerViewportChanged != null)
                //      OnSchedulerViewportChanged(value);

                IsViewportInit = true;
            }
        }




        public Core() { }

        public void SetViewportArea(SCViewport viewport)
        {
            ViewportAreaData = viewport;
            ViewportAreaScreen = viewport;
        }

        // size
        public void UpdateSize(int width, int height)
        {
            var temp_pos = FromScreenToLocal(Width__ / 2, Height__ / 2);


            //========================================
            this.Width__ = width;
            this.Height__ = height;

            if (OnSizeChanged != null)
            {
                OnSizeChanged(width, height);
            }

            Debug.WriteLine("OnSizeChanged : Core, w: " + width + ", h: " + height);
            //========================================

            WindowAreaZoom = CreateWindowAreaZoom(_zoom, scaleX, scaleY);

            RenderOffsetAbsolute = GetRenderOffset(temp_pos);

            ViewportAreaScreen = CreateViewportAreaScreen();

            ZoomScreenPosition = FromLocalToScreen(temp_pos);


            //    WindowAreaZoom = CreateWindowAreaZoom(_zoom, scaleX, scaleY);
            //    ViewportAreaScreen = CreateViewportAreaScreen();
        }


        public bool IsStarted
        {
            get
            {
                return IsViewportInit == true;
            }
        }

        public bool IsWindowArea(Point2D point)
        {
            Point2I sc0 = RenderOffsetAbsolute;

            int x0 = Math.Max(sc0.X, 0);
            int y0 = Math.Max(sc0.Y, 0);
            int x1 = Math.Min(WindowAreaZoom.Width + sc0.X, Width__);
            int y1 = Math.Min(WindowAreaZoom.Height + sc0.Y, Height__);
            var rect = new RectD(x0, y0, Math.Abs(x1 - x0), Math.Abs(y1 - y0));

            return rect.Contains(point);
        }

        public RectD RenderVisibleWindow
        {
            get
            {
                Point2I sc0 = RenderOffsetAbsolute;

                var result = RectI.Intersect(
                    new RectI(0, 0, Width__, Height__),
                    new RectI(sc0.X, sc0.Y, WindowAreaZoom.Width, WindowAreaZoom.Height));

                return new RectD(result.X, result.Y, result.Width, result.Height);

                //int x0 = Math.Max(sc0.X, 0);
                //int y0 = Math.Max(sc0.Y, 0);
                //int x1 = Math.Min(ZoomingWindowArea.Width + sc0.X, Width__);
                //int y1 = Math.Min(ZoomingWindowArea.Height + sc0.Y, Height__);

                //return new Rect(x0, y0, x1 - x0, y1 - y0);
            }
        }

        Point2I _renderOffset;

        public Point2I RenderOffsetAbsolute
        {
            get
            {
                return _renderOffset;
            }
            set
            {
                _renderOffset = RenderOffsetValidate(value);
            }
        }

        Point2I RenderOffsetValidate(Point2I offset)
        {
            var x = offset.X;
            var y = offset.Y;

            x = Math.Min(x, 0);
            x = Math.Max(x + WindowAreaZoom.Width, this.Width__) - WindowAreaZoom.Width;

            y = Math.Min(y, 0);
            y = Math.Max(y + WindowAreaZoom.Height, this.Height__) - WindowAreaZoom.Height;

            return new Point2I(x, y);
        }

        public Point2I mouseDown;
        public Point2I mouseCurrent;

        public Point2D mouseCurrentAbsolute;

        public Point2I ZoomScreenPosition/* ZoomPositionAbsolute*/ { get; set; }
        public Point2D ZoomPositionLocal
        {
            get
            {
                return FromScreenToLocal(ZoomScreenPosition/*ZoomPositionAbsolute*/.X, ZoomScreenPosition/*ZoomPositionAbsolute*/.Y);
            }
        }

        internal int maxZoom = 100;
        internal int minZoom = 0;

        public int MaxZoom { get { return maxZoom; } set { maxZoom = value; } }
        public int MinZoom { get { return minZoom; } set { minZoom = value; } }

        //    public MouseWheelZoomType MouseWheelZoomType = MouseWheelZoomType.ViewCenter;
        public bool MouseWheelZoomEnabled = true;

        double scaleX = 1.0; // 30 %        
        double scaleY = 0.0;


        bool IsViewportInit { get; set; } = false;

        public RectI RenderWindowArea
        {
            get
            {
                return new RectI(RenderOffsetAbsolute.X, RenderOffsetAbsolute.Y, WindowAreaZoom.Width, WindowAreaZoom.Height);
            }
        }



        // Drag
        // Zoom

        bool Zooming(int zm)
        {
            if (IsStarted == true)
            {
                var posLoc = ZoomPositionLocal;

                WindowAreaZoom = CreateWindowAreaZoom(zm, scaleX, scaleY);

                RenderOffsetAbsolute = GetRenderOffset(posLoc);

                ViewportAreaScreen = CreateViewportAreaScreen();

                ZoomScreenPosition/*ZoomPositionAbsolute*/ = FromLocalToScreen(posLoc);

                return true;
            }

            return false;
        }
        public Point2I GetRenderOffset(Point2D pos)
        {
            var xAbs = AxisX.FromLocalToAbsolute(pos.X);
            var yAbs = AxisY.FromLocalToAbsolute(pos.Y);

            //  var wz = WindowAreaZoom.Width;
            //  var hz = WindowAreaZoom.Height;

            var offsetX = Width__ / 2 - xAbs;
            var offsetY = Height__ / 2 - yAbs;

            // var offsetX = wz / 2.0 - this.Width__ / 2.0;
            // var offsetY = hz / 2.0 - this.Height__ / 2.0;

            return new Point2I(offsetX, offsetY);
        }

        RectI CreateWindowAreaZoom(int zm, double sclX, double sclY)
        {
            var w0 = this.Width__;
            var h0 = this.Height__;

            double stepx = w0 * sclX;
            double stepy = h0 * sclY;

            int w = w0 + (int)(zm * stepx);
            int h = h0 + (int)(zm * stepy);

            return new RectI(0, 0, w, h);
        }

        private SCViewport CreateViewportAreaScreen__2()
        {
            var wnd1 = RenderVisibleWindow;
            //  var wnd0 = WindowArea0;
            var VpData = ViewportAreaData;

            var left = wnd1.Left * VpData.Width / this.Width__;// wnd0.Width;
            var right = wnd1.Right * VpData.Width / this.Height__;// wnd0.Width;

            return new SCViewport(left, ViewportAreaData.Y, right - left, ViewportAreaData.Height);
        }

        SCViewport CreateViewportAreaScreen__()
        {
            RectI Abs = WindowAreaZoom;
            SCViewport Loc = ViewportAreaData;

            int x00 = -RenderOffsetAbsolute.X;
            int y00 = -RenderOffsetAbsolute.Y;
            int h = this.Height__;
            // int x01 = x00;
            int y01 = y00 + h;
            int w = this.Width__;
            int x10 = x00 + w;
            // int y10 = y00;
            // int x11 = x10;
            // int y11 = y01;

            double bottom = y00 * Loc.Height / Abs.Height;
            double top = y01 * Loc.Height / Abs.Height;

            double left = x00 * Loc.Width / Abs.Width;
            double right = x10 * Loc.Width / Abs.Width;

            if (right - left < 0 || top - bottom < 0)
                throw new Exception();

            return new SCViewport(left, bottom, right - left, top - bottom);
        }

        SCViewport CreateViewportAreaScreen()
        {
            RectI Abs = WindowAreaZoom;
            SCViewport Loc = ViewportAreaData;

            int x00 = -RenderOffsetAbsolute.X;
            int y00 = -RenderOffsetAbsolute.Y;

            double bottom = y00 * Loc.Height / Abs.Height + Loc.Y;
            double left = x00 * Loc.Width / Abs.Width + Loc.X;

            double w = this.Width__ * Loc.Width / Abs.Width;
            double h = this.Height__ * Loc.Height / Abs.Height;

            if (w < 0 || h < 0)
                throw new Exception();

            return new SCViewport(left, bottom, w, h);
        }

        private int _zoom;
        public int Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                if (_zoom != value && IsDragging == false)
                {
                    _zoom = value;

                    if (Zooming(_zoom) == true)
                    {
                        if (OnZoomChanged != null)
                        {
                            OnZoomChanged();
                        }
                    }
                }
            }
        }

        #region Dragging

        public Point2I dragPoint;

        public bool IsDragging = false;

        public bool CanDragMap = true;

        public void BeginDrag(Point2I pt)
        {
            dragPoint.X = pt.X - RenderOffsetAbsolute.X;
            dragPoint.Y = pt.Y - RenderOffsetAbsolute.Y;
            IsDragging = true;
        }

        public void EndDrag()
        {
            IsDragging = false;
            mouseDown = Point2I.Empty;
        }



        public void Drag(Point2I pt)
        {
            //  _renderOffset.X = pt.X - dragPoint.X;
            //  _renderOffset.Y = pt.Y - dragPoint.Y;

            RenderOffsetAbsolute = new Point2I(pt.X - dragPoint.X, pt.Y - dragPoint.Y);

            if (IsDragging == true)
            {
                ViewportAreaScreen = CreateViewportAreaScreen();

                if (OnDragChanged != null)
                {
                    OnDragChanged();
                }
            }
        }


        #endregion

        public RectD RenderSize
        {
            get
            {
                return new RectD(RenderOffsetAbsolute.X, RenderOffsetAbsolute.Y, WindowAreaZoom.Width, WindowAreaZoom.Height);
            }
        }

        public RectI Screen
        {
            get
            {
                return new RectI(0, 0, Width__, Height__);
            }
        }

        protected static double Clipp(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clipp(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        #region From...To...



        public Point2D FromScreenToLocal(int x, int y)
        {
            var pLocal = new Point2I(x, y);

            pLocal.OffsetNegative(RenderOffsetAbsolute);

            return new Point2D(
                AxisX.FromAbsoluteToLocal(pLocal.X),
                AxisY.FromAbsoluteToLocal(pLocal.Y));

            //   return Provider.Projection.FromPixelToSchedulerPoint(pLocal, Zoom);
        }

        public Point2D FromAbsoluteToLocal(int x, int y)
        {
            var pLocal = new Point2I(x, y);

            return new Point2D(
                AxisX.FromAbsoluteToLocal(pLocal.X),
                AxisY.FromAbsoluteToLocal(pLocal.Y));
        }

        public Point2I FromLocalToScreen(Point2D shedulerPoint)
        {
            // Point2I pLocal = Provider.Projection.FromSchedulerPointToPixel(shedulerPoint, Zoom);

            var pLocal = new Point2I(
                AxisX.FromLocalToAbsolute(shedulerPoint.X),
                AxisY.FromLocalToAbsolute(shedulerPoint.Y));


            pLocal.Offset(RenderOffsetAbsolute);

            return new Point2I(pLocal.X, pLocal.Y);
        }

        public Point2I FromLocalToAbsolute(Point2D shedulerPoint)
        {
            // Point2I pLocal = Provider.Projection.FromSchedulerPointToPixel(shedulerPoint, Zoom);

            var pLocal = new Point2I(
                AxisX.FromLocalToAbsolute(shedulerPoint.X),
                AxisY.FromLocalToAbsolute(shedulerPoint.Y));

            return new Point2I(pLocal.X, pLocal.Y);
        }

        #endregion

        //public System.Windows.Media.Transform ToViewportInPixels
        //{
        //    get
        //    {
        //        System.Windows.Media.TransformGroup group = new System.Windows.Media.TransformGroup();

        //        int y = RenderOffsetAbsolute.Y;
        //        double sclY = 1.0;
        //        if (AxisY.IsInversed == true)
        //        {
        //            y = WindowAreaZoom.Height + y;
        //            sclY = -1.0;
        //        }

        //        group.Children.Add(new System.Windows.Media.ScaleTransform(1.0, sclY));
        //        group.Children.Add(new System.Windows.Media.TranslateTransform(RenderOffsetAbsolute.X, y));

        //        return group;
        //    }
        //}



        // gets max zoom level to fit rectangle
        public int GetMaxZoomToFitRect(SCViewport rect)
        {
            int zoom = minZoom;

            if (rect.Height == 0.0 || rect.Width == 0.0)
            {
                zoom = maxZoom / 2;
            }
            else
            {
                for (int i = (int)zoom; i <= maxZoom; i++)
                {
                    WindowAreaZoom = CreateWindowAreaZoom(i, scaleX, scaleY);

                    var p0 = new Point2I(
                        AxisX.FromLocalToAbsolute(rect.Left),
                        AxisY.FromLocalToAbsolute(rect.Bottom)
                        );
                    var p1 = new Point2I(
                        AxisX.FromLocalToAbsolute(rect.Right),
                        AxisY.FromLocalToAbsolute(rect.Top)
                        );

                    if (((p1.X - p0.X) <= this.Width__/* WindowArea0.Width*/ + 10) && (p1.Y - p0.Y) <= this.Height__ /*WindowArea0.Height*/ + 10)
                    {
                        zoom = i;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return zoom;
        }

        public event SCDragChanged OnDragChanged;
        public event SCZoomChanged OnZoomChanged;
        public event SCSizeChanged OnSizeChanged;
    }
}
