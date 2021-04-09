#nullable disable
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class Core
    {
        private int _width;
        private int _height;
        private BaseAxis _axisX;
        private BaseAxis _axisY;
        private RectI _windowAreaZoom;
        private RectD _viewportAreaScreen;
        private RectD _viewportAreaData = new RectD();
        private Point2I _renderOffset;
        public Point2I MouseDown;
        public Point2I MouseCurrent;
   //     private Point2D _mouseCurrentAbsolute;
        private int _zoom;
        private Point2I _dragPoint;
        public bool IsDragging = false;
        public bool CanDragMap = true;
        private int _maxZoom = 100;
        private int _minZoom = 0;
        //    public MouseWheelZoomType MouseWheelZoomType = MouseWheelZoomType.ViewCenter;
        public bool MouseWheelZoomEnabled = true;
        private double _scaleX = 1.0; // 30 %        
        private double _scaleY = 0.0;

        public event SCDragChanged OnDragChanged;
        public event SCZoomChanged OnZoomChanged;
        public event SCSizeChanged OnSizeChanged;

        public Core() { }

        public int TrueHeight => _height;
    
        public BaseAxis AxisX
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

        public BaseAxis AxisY
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
     
        public RectD ViewportAreaScreen
        {
            get
            {
                return _viewportAreaScreen;
            }
            private set
            {
               // if (_viewportAreaScreen != value)
               // {
                    _viewportAreaScreen = value;


                    if (AxisX is TimeAxis)
                    {
                        var axis = AxisX as TimeAxis;
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
               // }
            }
        }
   
        public RectD ViewportAreaData
        {
            get
            {
                //if (_viewportAreaData.IsEmpty == true)
                //{
                //    //      _viewportAreaData = new SCViewport(0.0, 0.0, 1.0, 1.0);
                //}

                return _viewportAreaData;
            }
            private set
            {
                _viewportAreaData = value;

                if (AxisX is TimeAxis)
                {
                    var axis = AxisX as TimeAxis;
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
   
        public void SetViewportArea(RectD viewport)
        {
            ViewportAreaData = viewport;
            ViewportAreaScreen = viewport;
        }

        // size
        public void UpdateSize(int width, int height)
        {
            var temp_pos = FromScreenToLocal(_width / 2, _height / 2);

            //========================================
            _width = width;
            _height = height;

            if (OnSizeChanged != null)
            {
                OnSizeChanged(width, height);
            }

            Debug.WriteLine("OnSizeChanged : Core, w: " + width + ", h: " + height);
            //========================================

            WindowAreaZoom = CreateWindowAreaZoom(_zoom, _scaleX, _scaleY);

            RenderOffsetAbsolute = GetRenderOffset(temp_pos);

            ViewportAreaScreen = CreateViewportAreaScreen();

            ZoomScreenPosition = FromLocalToScreen(temp_pos);


            //    WindowAreaZoom = CreateWindowAreaZoom(_zoom, scaleX, scaleY);
            //    ViewportAreaScreen = CreateViewportAreaScreen();
        }

        public bool IsStarted => IsViewportInit == true;

        public bool IsWindowArea(Point2D point)
        {
            Point2I sc0 = RenderOffsetAbsolute;

            int x0 = Math.Max(sc0.X, 0);
            int y0 = Math.Max(sc0.Y, 0);
            int x1 = Math.Min(WindowAreaZoom.Width + sc0.X, _width);
            int y1 = Math.Min(WindowAreaZoom.Height + sc0.Y, _height);
            var rect = new RectD(x0, y0, Math.Abs(x1 - x0), Math.Abs(y1 - y0));

            return rect.Contains(point);
        }

        public RectD RenderVisibleWindow
        {
            get
            {
                Point2I sc0 = RenderOffsetAbsolute;

                var result = RectI.Intersect(
                    new RectI(0, 0, _width, _height),
                    new RectI(sc0.X, sc0.Y, WindowAreaZoom.Width, WindowAreaZoom.Height));

                return new RectD(result.X, result.Y, result.Width, result.Height);

                //int x0 = Math.Max(sc0.X, 0);
                //int y0 = Math.Max(sc0.Y, 0);
                //int x1 = Math.Min(ZoomingWindowArea.Width + sc0.X, Width__);
                //int y1 = Math.Min(ZoomingWindowArea.Height + sc0.Y, Height__);

                //return new Rect(x0, y0, x1 - x0, y1 - y0);
            }
        }
      
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

        private Point2I RenderOffsetValidate(Point2I offset)
        {
            var x = offset.X;
            var y = offset.Y;

            x = Math.Min(x, 0);
            x = Math.Max(x + WindowAreaZoom.Width, _width) - WindowAreaZoom.Width;

            y = Math.Min(y, 0);
            y = Math.Max(y + WindowAreaZoom.Height, _height) - WindowAreaZoom.Height;

            return new Point2I(x, y);
        }

        public Point2I ZoomScreenPosition { get; set; }
        
        public Point2D ZoomPositionLocal
        {
            get
            {
                return FromScreenToLocal(ZoomScreenPosition/*ZoomPositionAbsolute*/.X, ZoomScreenPosition/*ZoomPositionAbsolute*/.Y);
            }
        }

        public int MaxZoom 
        {
            get => _maxZoom; 
            set => _maxZoom = value; 
        }

        public int MinZoom 
        {
            get => _minZoom; 
            set => _minZoom = value;
        }

        bool IsViewportInit { get; set; } = false;

        public RectI RenderWindowArea
        {
            get
            {
                return new RectI(RenderOffsetAbsolute.X, RenderOffsetAbsolute.Y, WindowAreaZoom.Width, WindowAreaZoom.Height);
            }
        }

        private bool Zooming(int zm)
        {
            if (IsStarted == true)
            {
                var posLoc = ZoomPositionLocal;

                WindowAreaZoom = CreateWindowAreaZoom(zm, _scaleX, _scaleY);

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

            var offsetX = _width / 2 - xAbs;
            var offsetY = _height / 2 - yAbs;

            // var offsetX = wz / 2.0 - this.Width__ / 2.0;
            // var offsetY = hz / 2.0 - this.Height__ / 2.0;

            return new Point2I(offsetX, offsetY);
        }

        private RectI CreateWindowAreaZoom(int zm, double sclX, double sclY)
        {
            var w0 = _width;
            var h0 = _height;

            double stepx = w0 * sclX;
            double stepy = h0 * sclY;

            int w = w0 + (int)(zm * stepx);
            int h = h0 + (int)(zm * stepy);

            return new RectI(0, 0, w, h);
        }

        private RectD CreateViewportAreaScreen__2()
        {
            var wnd1 = RenderVisibleWindow;
            //  var wnd0 = WindowArea0;
            var VpData = ViewportAreaData;

            var left = wnd1.Left * VpData.Width / _width;// wnd0.Width;
            var right = wnd1.Right * VpData.Width / _height;// wnd0.Width;

            return new RectD(left, ViewportAreaData.Y, right - left, ViewportAreaData.Height);
        }

        private RectD CreateViewportAreaScreen__()
        {
            RectI Abs = WindowAreaZoom;
            RectD Loc = ViewportAreaData;

            int x00 = -RenderOffsetAbsolute.X;
            int y00 = -RenderOffsetAbsolute.Y;
            int h = _height;
            // int x01 = x00;
            int y01 = y00 + h;
            int w = _width;
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

            return new RectD(left, bottom, right - left, top - bottom);
        }

        private RectD CreateViewportAreaScreen()
        {
            RectI Abs = WindowAreaZoom;
            RectD Loc = ViewportAreaData;

            int x00 = -RenderOffsetAbsolute.X;
            int y00 = -RenderOffsetAbsolute.Y;

            double bottom = y00 * Loc.Height / Abs.Height + Loc.Y;
            double left = x00 * Loc.Width / Abs.Width + Loc.X;

            double w = _width * Loc.Width / Abs.Width;
            double h = _height * Loc.Height / Abs.Height;

            if (w < 0 || h < 0)
                throw new Exception();

            return new RectD(left, bottom, w, h);
        }
   
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
        
        public void BeginDrag(Point2I pt)
        {
            _dragPoint.X = pt.X - RenderOffsetAbsolute.X;
            _dragPoint.Y = pt.Y - RenderOffsetAbsolute.Y;
            IsDragging = true;
        }

        public void EndDrag()
        {
            IsDragging = false;
            MouseDown = Point2I.Empty;
        }

        public void Drag(Point2I pt)
        {
            //  _renderOffset.X = pt.X - dragPoint.X;
            //  _renderOffset.Y = pt.Y - dragPoint.Y;

            RenderOffsetAbsolute = new Point2I(pt.X - _dragPoint.X, pt.Y - _dragPoint.Y);

            if (IsDragging == true)
            {
                ViewportAreaScreen = CreateViewportAreaScreen();

                if (OnDragChanged != null)
                {
                    OnDragChanged();
                }
            }
        }

        public RectD RenderSize => new RectD(RenderOffsetAbsolute.X, RenderOffsetAbsolute.Y, WindowAreaZoom.Width, WindowAreaZoom.Height);

        public RectI Screen => new RectI(0, 0, _width, _height);

        protected static double Clipp(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clipp(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        public Point2D FromScreenToLocal(int x, int y)
        {
            var pLocal = new Point2I(x, y);

            pLocal.OffsetNegative(RenderOffsetAbsolute);

            return new Point2D(AxisX.FromAbsoluteToLocal(pLocal.X), AxisY.FromAbsoluteToLocal(pLocal.Y));

            //   return Provider.Projection.FromPixelToSchedulerPoint(pLocal, Zoom);
        }

        public Point2D FromAbsoluteToLocal(int x, int y)
        {
            var pLocal = new Point2I(x, y);

            return new Point2D(AxisX.FromAbsoluteToLocal(pLocal.X), AxisY.FromAbsoluteToLocal(pLocal.Y));
        }

        public Point2I FromLocalToScreen(Point2D shedulerPoint)
        {
            // Point2I pLocal = Provider.Projection.FromSchedulerPointToPixel(shedulerPoint, Zoom);

            var pLocal = new Point2I(AxisX.FromLocalToAbsolute(shedulerPoint.X), AxisY.FromLocalToAbsolute(shedulerPoint.Y));

            pLocal.Offset(RenderOffsetAbsolute);

            return new Point2I(pLocal.X, pLocal.Y);
        }

        public Point2I FromLocalToAbsolute(Point2D shedulerPoint)
        {
            // Point2I pLocal = Provider.Projection.FromSchedulerPointToPixel(shedulerPoint, Zoom);

            var pLocal = new Point2I(AxisX.FromLocalToAbsolute(shedulerPoint.X), AxisY.FromLocalToAbsolute(shedulerPoint.Y));

            return new Point2I(pLocal.X, pLocal.Y);
        }

        // gets max zoom level to fit rectangle        
        public int GetMaxZoomToFitRect(RectD rect)
        {
            int zoom = _minZoom;

            if (rect.Height == 0.0 || rect.Width == 0.0)
            {
                zoom = _maxZoom / 2;
            }
            else
            {
                for (int i = (int)zoom; i <= _maxZoom; i++)
                {
                    WindowAreaZoom = CreateWindowAreaZoom(i, _scaleX, _scaleY);

                    var p0 = new Point2I(
                        AxisX.FromLocalToAbsolute(rect.Left),
                        AxisY.FromLocalToAbsolute(rect.Bottom)
                        );
                    var p1 = new Point2I(
                        AxisX.FromLocalToAbsolute(rect.Right),
                        AxisY.FromLocalToAbsolute(rect.Top)
                        );

                    if (((p1.X - p0.X) <= _width + 10) && (p1.Y - p0.Y) <= _height + 10)
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
    }
}
