#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Threading;
using Avalonia.LogicalTree;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.ViewModels;
using TimeDataViewer;
using TimeDataViewer.Spatial;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using TimeDataViewer.Models;
using TimeDataViewer.Core;
using Avalonia.Controls.Generators;
using System.Threading.Tasks;
using TimeDataViewer.Views;
using Avalonia.Collections;

namespace TimeDataViewer
{
    public delegate void SelectionChangeEventHandler(RectD Selection, bool ZoomToFit);

    public partial class SchedulerControl : ItemsControl, IStyleable, ISchedulerControl
    {
        Type IStyleable.StyleKey => typeof(ItemsControl);

        private readonly Area _area;        
        private readonly Canvas _canvas;
        private ObservableCollection<IMarker> _markers;

        private double _zoom;
        private readonly TranslateTransform _schedulerTranslateTransform;
        private ObservableCollection<Series> _series;
        private IList<ISeries> _seriesViewModels;
        private DateTime _epoch;
        private readonly Popup _popup;
        // center 
        private readonly bool _showCenter = true;
        private readonly Pen _centerCrossPen = new(Brushes.Red, 1);
        // mouse center
        private readonly bool _showMouseCenter = true;
        private readonly Pen _mouseCrossPen = new(Brushes.Blue, 1);

        public event EventHandler? OnSizeChanged;
        public event MousePositionChangedEventHandler? OnMousePositionChanged;
        public event EventHandler? OnZoomChanged;
   
        public SchedulerControl()
        {
            CoreFactory factory = new CoreFactory();

            _area = factory.CreateArea();
          
            _area.OnZoomChanged += ZoomChangedEvent;
           
            _series = new ObservableCollection<Series>();
            _schedulerTranslateTransform = new TranslateTransform();

            PointerWheelChanged += SchedulerControl_PointerWheelChanged;
            PointerPressed += SchedulerControl_PointerPressed;
            PointerReleased += SchedulerControl_PointerReleased;
            PointerMoved += SchedulerControl_PointerMoved;

            _canvas = new Canvas()
            {
                RenderTransform = _schedulerTranslateTransform
            };

            _popup = new Popup()
            {
                //AllowsTransparency = true,
                //PlacementTarget = this,
                PlacementMode = PlacementMode.Pointer,
                IsOpen = false,
            };

            TopLevelForToolTips?.Children.Add(_popup);

            var style = new Style(x => x.OfType<ItemsControl>().Child().OfType<ContentPresenter>());
            style.Setters.Add(new Setter(Canvas.LeftProperty, new Binding(nameof(MarkerViewModel.AbsolutePositionX))));
            style.Setters.Add(new Setter(Canvas.TopProperty, new Binding(nameof(MarkerViewModel.AbsolutePositionY))));
            style.Setters.Add(new Setter(Canvas.ZIndexProperty, new Binding(nameof(MarkerViewModel.ZIndex))));
            Styles.Add(style);

            ItemTemplate = new CustomItemTemplate();

            ItemsPanel = new FuncTemplate<IPanel>(() => _canvas);

            ClipToBounds = true;
     //       SnapsToDevicePixels = true;
            
            LayoutUpdated += SchedulerControl_LayoutUpdated;
         
            Series.CollectionChanged += (s, e) => PassingLogicalTree(e);
            Series.CollectionChanged += (s, e) => Series_CollectionChanged(s, e);

            ZoomProperty.Changed.AddClassHandler<SchedulerControl>((d, e) => d.ZoomChanged(e));
            EpochProperty.Changed.AddClassHandler<SchedulerControl>((d, e) => d.EpochChanged(e));
            CurrentTimeProperty.Changed.AddClassHandler<SchedulerControl>((d, e) => d.CurrentTimeChanged(e));

            OnMousePositionChanged += AxisX.UpdateDynamicLabelPosition;

            _markers = new ObservableCollection<IMarker>();

            Items = _markers;
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
            
            _area.OnZoomChanged -= ZoomChangedEvent;
            OnMousePositionChanged -= AxisX.UpdateDynamicLabelPosition;
        }

        private void Series_CollectionChanged(object? s, NotifyCollectionChangedEventArgs e)
        {           
            foreach (var series in Series)
            {
                series.OnInvalidateData += SeriesInvalidateDataEvent;                              
            }
        }
    
        private void UpdateViewport()
        {
            var maxRight = _seriesViewModels.Max(s => s.MaxTime());

            var rightDate = _epoch.AddSeconds(maxRight).Date.AddDays(1);

            var len = (rightDate - _epoch.Date).TotalSeconds;

            AutoSetViewportArea(len);
        }

        private void ZoomChangedEvent(object? sender, EventArgs e)
        {
            OnZoomChanged?.Invoke(this, EventArgs.Empty);
            //Debug.WriteLine($"SchedulerControl -> OnZoomChanged -> Count = {OnZoomChanged?.GetInvocationList().Length}");

            ForceUpdateOverlays();
        }

        public DateTime Epoch0 => Epoch.Date;

        private void PassingLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var item in e.NewItems.OfType<ISetLogicalParent>())
                {
                    item.SetParent(this);
                }
                LogicalChildren.AddRange(e.NewItems.OfType<ILogical>());
                VisualChildren.AddRange(e.NewItems.OfType<IVisual>());
            }

            if (e.OldItems is not null)
            {
                foreach (var item in e.OldItems.OfType<ISetLogicalParent>())
                {
                    item.SetParent(null);
                }
                foreach (var item in e.OldItems)
                {
                    LogicalChildren.Remove((ILogical)item);
                    VisualChildren.Remove((IVisual)item);
                }
            }
        }

        public void ShowTooltip(Control placementTarget, Control tooltip)
        {
            if (TopLevelForToolTips?.Children.Contains(_popup) == false)
            {
                TopLevelForToolTips?.Children.Add(_popup);
            }

            _popup.PlacementTarget = placementTarget;
            _popup.Child = tooltip;
            _popup.IsOpen = true;
        }

        public void HideTooltip()
        {
            _popup.IsOpen = false;
        }
        
        private void AutoSetViewportArea(double len)
        {
            var d0 = (_epoch - _epoch.Date).TotalSeconds;    
            var count = _seriesViewModels.Count;
            double step = 1.0 / (count + 1);

            int i = 0;
            foreach (var item in _seriesViewModels)
            {
                if (item is not null)
                {
                    var series = item;

                    var seriesLocalPostion = new Point2D(0.0, (++i) * step);
                    var seriesAbsolutePostion = _area.FromLocalToAbsolute(seriesLocalPostion);
                    
                    series.LocalPosition = seriesLocalPostion;              
                    series.AbsolutePositionX = seriesAbsolutePostion.X;
                    series.AbsolutePositionY = seriesAbsolutePostion.Y;

                    foreach (var ival in series.Intervals)
                    {
                        var intervalLocalPosition = new Point2D(d0 + ival.Left + (ival.Right - ival.Left) / 2.0, series.LocalPosition.Y);
                        var intervalAbsolutePostion = _area.FromLocalToAbsolute(intervalLocalPosition);

                        ival.LocalPosition = intervalLocalPosition;                    
                        ival.AbsolutePositionX = intervalAbsolutePostion.X;
                        ival.AbsolutePositionY = intervalAbsolutePostion.Y;
                    }
                }
            }

            _area.UpdateViewport(0.0, 0.0, len, 1.0);            
        }

        public RectD ViewportArea => _area.Viewport;

        public RectD ClientViewportArea => _area.ClientViewport;

        public RectI AbsoluteWindow => _area.Window;

        public RectI ScreenWindow => _area.Screen;

        public Point2I WindowOffset => _area.WindowOffset;

        public ITimeAxis AxisX => _area.AxisX;

        public ICategoryAxis AxisY => _area.AxisY;
            
        private Canvas Canvas => _canvas;

        public Panel? TopLevelForToolTips
        {
            get
            {
                IVisual root = this.GetVisualRoot();

                while (root is not null)
                {
                    if (root is Panel panel)
                    {
                        return panel;
                    }

                    if (root.VisualChildren.Count != 0)
                    {
                        root = root.VisualChildren[0];
                    }
                    else
                    {
                        return null;
                    }
                }

                return null;
            }
        }

        private void SchedulerControl_LayoutUpdated(object? sender, EventArgs e)
        {
            _area.UpdateSize((int)Bounds.Width, (int)Bounds.Height);

            OnSizeChanged?.Invoke(this, EventArgs.Empty);
            //Debug.WriteLine($"SchedulerControl -> OnSizeChanged -> Count = {OnSizeChanged?.GetInvocationList().Length}");

            ForceUpdateOverlays();            
        }

        private void ForceUpdateOverlays() => ForceUpdateOverlays(Items);        

        private void ForceUpdateOverlays(IEnumerable items)
        {
            UpdateMarkersOffset();

            foreach (IMarker item in items)
            {
                var p = _area.FromLocalToAbsolute(item.LocalPosition);

                item.AbsolutePositionX = p.X;
                item.AbsolutePositionY = p.Y;
            }

            InvalidateVisual();
        }

        private void ZoomChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is not null && e.NewValue is double value)
            {                           
                var zoom = Math.Clamp(value, MinZoom, MaxZoom);
                
                if (_zoom != zoom)
                {
                    _zoom = zoom;
                    _area.Zoom = (int)Math.Floor(zoom);
                    
                    if (IsInitialized == true)
                    {
                        ForceUpdateOverlays();                   
                    }                   
                }
            }
        }

        private void EpochChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is DateTime)
            {              
                AxisX.Epoch0 = Epoch0;
            }
        }
        
        private void CurrentTimeChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is double)
            {
                var xValue = (Epoch - Epoch0).TotalSeconds + CurrentTime;
               
                _area.DragToTime(xValue);
            }
        }

        public int MaxZoom => _area.MaxZoom;  

        public int MinZoom => _area.MinZoom;   
  
        private void UpdateMarkersOffset()
        {
            if (Canvas != null)
            {
                _schedulerTranslateTransform.X = _area.WindowOffset.X;
                _schedulerTranslateTransform.Y = _area.WindowOffset.Y;
            }
        }

        public Point2D FromScreenToLocal(int x, int y) => _area.FromScreenToLocal(x, y);        

        public Point2I FromLocalToScreen(Point2D point) => _area.FromLocalToScreen(point);        

        public Point2D FromAbsoluteToLocal(int x, int y) => _area.FromAbsoluteToLocal(x, y);        

        public Point2I FromLocalToAbsolute(Point2D point) => _area.FromLocalToAbsolute(point);
        
        public bool IsTestBrush { get; set; } = false;

        public override void Render(DrawingContext context)
        {
            //SeriesValidate();

            //if (_dirtyItems == false)
            {
                DrawBackground(context);
            }
           // else
            {
               // context.FillRectangle(Brushes.LightBlue, new Rect(0, 0, Bounds.Width, Bounds.Height));
            }

            DrawEpoch(context);
              
            if (_showCenter == true)
            {
                context.DrawLine(_centerCrossPen, new Point((Bounds.Width / 2) - 5, Bounds.Height / 2), new Point((Bounds.Width / 2) + 5, Bounds.Height / 2));
                context.DrawLine(_centerCrossPen, new Point(Bounds.Width / 2, (Bounds.Height / 2) - 5), new Point(Bounds.Width / 2, (Bounds.Height / 2) + 5));
            }

            if (_showMouseCenter == true)
            {
                context.DrawLine(_mouseCrossPen,
                    new Point(_area.ZoomScreenPosition.X - 5, _area.ZoomScreenPosition.Y),
                    new Point(_area.ZoomScreenPosition.X + 5, _area.ZoomScreenPosition.Y));
                context.DrawLine(_mouseCrossPen,
                    new Point(_area.ZoomScreenPosition.X, _area.ZoomScreenPosition.Y - 5),
                    new Point(_area.ZoomScreenPosition.X, _area.ZoomScreenPosition.Y + 5));
            }

            using (context.PushPreTransform(_schedulerTranslateTransform.Value))
            {
                base.Render(context);
            }

            DrawCurrentTime(context);
        }

        private void DrawEpoch(DrawingContext context)
        {
            var d0 = (Epoch - Epoch0).TotalSeconds;
            var p = _area.FromLocalToAbsolute(d0, 0.0);    
            Pen pen = new Pen(Brushes.Yellow, 2.0);
            context.DrawLine(pen, new Point(p.X + WindowOffset.X, 0.0), new Point(p.X + WindowOffset.X, _area.Window.Height));
        }

        private void DrawCurrentTime(DrawingContext context)
        {            
            var d0 = (Epoch - Epoch0).TotalSeconds;
            var p = _area.FromLocalToAbsolute(d0 + CurrentTime, 0.0);
            Pen pen = new Pen(Brushes.Red, 2.0);
            context.DrawLine(pen, new Point(p.X + WindowOffset.X, 0.0), new Point(p.X + WindowOffset.X, _area.Window.Height));
        }        
    }

    //internal class Stuff
    //{
    //    [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
    //    [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
    //    public static extern bool SetCursorPos(int X, int Y);
    //}
}
