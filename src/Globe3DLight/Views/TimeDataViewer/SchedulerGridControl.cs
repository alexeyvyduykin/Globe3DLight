using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections;
using Avalonia.Controls;
using System.Globalization;
using System.Windows.Input;
using Avalonia.Data;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia.Media;
using System.ComponentModel;
using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Media.Imaging;
using Avalonia.Controls.Shapes;
using Avalonia.Layout;
using Globe3DLight.ViewModels.TimeDataViewer;
using Globe3DLight.Spatial;

namespace Globe3DLight.Views.TimeDataViewer
{
    public class SchedulerGridControl : BaseSchedulerControl
    {
        private enum BackgroundMode { Hour, Day, Week, Month, Year }
        private VisualBrush _gridBrush;
        private readonly Dictionary<BackgroundMode, Grid> _backgrounds;
        private BackgroundMode _currentBackgroundMode;
        private readonly IDictionary<SchedulerString, IList<SchedulerInterval>> _markers;
        private double _minLeft = Double.MaxValue;
        private double _maxRight = Double.MinValue;

        public SchedulerGridControl() : base()
        {
            _backgrounds = new Dictionary<BackgroundMode, Grid>();
            _markers = new Dictionary<SchedulerString, IList<SchedulerInterval>>();
            //    base.Loaded += SCSchedulerControl_Loaded;
            //    base.OnSchedulerZoomChanged += SchedulerBase_OnMapZoomChanged;

            base.AxisX = new TimeAxis() { CoordType = EAxisCoordType.X, TimePeriodMode = TimePeriod.Month/*Week*/ };
            base.AxisY = new CategoryAxis() { CoordType = EAxisCoordType.Y, IsInversed = true };

            InitBackgrounds();
        }

        public override void Render(DrawingContext drawingContext)
        {
            SchedulerBase_OnMapZoomChanged();

            base.Render(drawingContext);
        }

        private VisualBrush GridBrush
        {
            get
            {
                if (_gridBrush == null)
                {
                    _gridBrush = CreateGridBrush0();
                }

                return _gridBrush;
            }
        }

        private VisualBrush CreateBrush____()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            var lin = new LinearGradientBrush()
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Absolute),
                EndPoint = new RelativePoint(base.AbsoluteWindow.Width, base.AbsoluteWindow.Height, RelativeUnit.Absolute),
            };

            lin.GradientStops.Add(new GradientStop() { Color = Colors.Yellow, Offset = 0 });
            lin.GradientStops.Add(new GradientStop() { Color = Colors.Black, Offset = 1 });

            Rectangle rect = new Rectangle()
            {
                Fill = lin
            };


            Grid.SetColumn(rect, 0);
            Grid.SetRow(rect, 0);
            grid.Children.Add(rect);

            grid.Width = base.AbsoluteWindow.Width;
            grid.Height = base.AbsoluteWindow.Height;


            var brush = new VisualBrush()
            {
                Visual = grid,
                DestinationRect = new RelativeRect(
                base.RenderOffsetAbsolute.X, 0,
                base.AbsoluteWindow.Width, base.AbsoluteWindow.Height, RelativeUnit.Absolute)

            };


            return brush;

        }

        private VisualBrush CreateBrush__()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            Image image1 = new Image()
            {
                //    HorizontalAlignment = HorizontalAlignment.Stretch, 
                //   VerticalAlignment = VerticalAlignment.Stretch 
            };

            image1.Source = new Bitmap(@"C:\data\textureTemp\earth.bmp");

            Grid.SetColumn(image1, 0);
            Grid.SetRow(image1, 0);
            grid.Children.Add(image1);

            grid.Width = base.AbsoluteWindow.Width;
            grid.Height = base.AbsoluteWindow.Height;


            var brush = new VisualBrush()
            {
                Visual = grid,
                TileMode = TileMode.Tile
            };


            return brush;

        }

        private void InitBackgrounds()
        {
            _backgrounds.Add(BackgroundMode.Hour, CreateGrid(60));

            _backgrounds.Add(BackgroundMode.Day, CreateGrid(24));

            _backgrounds.Add(BackgroundMode.Week, CreateGrid(7));

            _backgrounds.Add(BackgroundMode.Month, CreateGrid(12));

            _backgrounds.Add(BackgroundMode.Year, CreateGrid(12));
        }

        private Grid CreateGrid(int count)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < count; i++)
            {
                if (i % 2 == 0)
                {
                    var rect = new Rectangle() { Fill = Brushes.Silver };
                    Grid.SetColumn(rect, i);
                    grid.Children.Add(rect);
                }
                else
                {
                    var rect = new Rectangle() { Fill = Brushes.WhiteSmoke };
                    Grid.SetColumn(rect, i);
                    grid.Children.Add(rect);
                }
            }

            return grid;
        }

        private VisualBrush CreateBrush()
        {
            //Grid grid = new Grid();
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.RowDefinitions.Add(new RowDefinition());

            //  Rectangle rect_silver = new Rectangle() { Height = 600.0, Width = 800.0 / 7.0,  Fill = Brushes.Silver };
            //  Rectangle rect_white = new Rectangle() { Height = 600.0, Width = 800.0 / 7.0, Fill = Brushes.White };

            /*
             
     <Border Height="200" Width="800" BorderBrush="Black" BorderThickness="2">
      <Grid>
        <Rectangle Grid.Column="0" Fill="Silver"></Rectangle>
        <Rectangle Grid.Column="1" Fill="WhiteSmoke"></Rectangle>
        <Rectangle Grid.Column="2" Fill="Silver"></Rectangle>
        <Rectangle Grid.Column="3" Fill="WhiteSmoke"></Rectangle>
        <Rectangle Grid.Column="4" Fill="Silver"></Rectangle>
        <Rectangle Grid.Column="5" Fill="WhiteSmoke"></Rectangle>
        <Rectangle Grid.Column="6" Fill="Silver"></Rectangle>     
      </Grid>
    </Border>
             */



            // week

            //for (int i = 0; i < 7; i++)
            //{
            //    if(i % 2 == 0)
            //    {
            //        var rect = new Rectangle() { Fill = Brushes.Silver };
            //        Grid.SetColumn(rect, i);
            //        grid.Children.Add(rect);
            //    }
            //    else
            //    {
            //        var rect = new Rectangle() { Fill = Brushes.WhiteSmoke };
            //        Grid.SetColumn(rect, i);
            //        grid.Children.Add(rect);
            //    }
            //}

            var grid = _backgrounds[_currentBackgroundMode];

            // grid.RenderTransform = new ScaleTransform(800, 600);
            grid.Width = base.AbsoluteWindow.Width;
            grid.Height = base.AbsoluteWindow.Height;
            // grid.Margin = new Thickness(-base.RenderOffsetAbsolute.X, 0, 0, 0);

            var brush = new VisualBrush()
            {
                Visual = grid,
                DestinationRect = new RelativeRect(
                    base.RenderOffsetAbsolute.X, 0,
                base.AbsoluteWindow.Width, base.AbsoluteWindow.Height, RelativeUnit.Absolute)
            };

            return brush;
        }

        private VisualBrush CreateGridBrush0()
        {
            // Create a DrawingBrush  
            VisualBrush blackBrush = new VisualBrush();

            // Create a Geometry with white background  
            GeometryDrawing backgroundSquare = new GeometryDrawing()
            {
                Brush = Brushes.DarkGray,
                Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1))
            };

            // Create a GeometryGroup that will be added to Geometry  
            // GeometryGroup gGroup = new GeometryGroup();
            // gGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 0.5, 1)));

            // Create a GeomertyDrawing  
            GeometryDrawing checkers = new GeometryDrawing()
            {
                Brush = new SolidColorBrush(Colors.GhostWhite),
                Geometry = new RectangleGeometry(new Rect(0, 0, 0.5, 1))//gGroup 
            };
            DrawingGroup checkersDrawingGroup = new DrawingGroup();
            checkersDrawingGroup.Children.Add(backgroundSquare);
            checkersDrawingGroup.Children.Add(checkers);

            // blackBrush.Visual = checkersDrawingGroup;
            blackBrush.Visual = new Rectangle() { Width = 800, Height = 600, Fill = Brushes.Silver };

            // Set Viewport and TimeMode  
            blackBrush.DestinationRect = new RelativeRect(0, 0, 1.0, 1.0, RelativeUnit.Relative);//Viewport = new Rect(0, 0, 1.0, 1.0);
            blackBrush.TileMode = TileMode.Tile;

            return blackBrush;
        }

        private void SchedulerBase_OnMapZoomChanged()
        {
            var w = base.ViewportAreaScreen.Width;
            var w0 = base.ViewportAreaData.Width;

            if (w == 0)
                return;

            if (IsRange(w, 0.0, 3600.0) == true) // Hour
            {
                _currentBackgroundMode = BackgroundMode.Hour;
                var ww = 1.0 / (12.0 * 12.0 * w0 / 86400.0);
                GridBrush.DestinationRect = new RelativeRect(0, 0, ww, 1.0, RelativeUnit.Relative);// Viewport = new Rect(0, 0, ww, 1.0);
                (base.AxisX as TimeAxis).TimePeriodMode = TimePeriod.Hour;
            }
            else if (IsRange(w, 0.0, 86400.0) == true) // Day
            {
                _currentBackgroundMode = BackgroundMode.Day;
                var ww = 1.0 / (12.0 * w0 / 86400.0);
                GridBrush.DestinationRect = new RelativeRect(0, 0, ww, 1.0, RelativeUnit.Relative);//Viewport = new Rectangle(0, 0, ww, 1.0);
                (base.AxisX as TimeAxis).TimePeriodMode = TimePeriod.Day;
            }
            else if (IsRange(w, 0.0, 7 * 86400.0) == true) // Week
            {
                _currentBackgroundMode = BackgroundMode.Week;
                var ww = 1.0 / (w0 / 86400.0);
                GridBrush.DestinationRect = new RelativeRect(0, 0, ww, 1.0, RelativeUnit.Relative);//Viewport = new Rect(0, 0, ww, 1.0);
                (base.AxisX as TimeAxis).TimePeriodMode = TimePeriod.Week;
            }
            else if (IsRange(w, 0.0, 30 * 86400.0) == true) // Month
            {
                _currentBackgroundMode = BackgroundMode.Month;
                var ww = 1.0 / (w0 / 86400.0);
                GridBrush.DestinationRect = new RelativeRect(0, 0, ww, 1.0, RelativeUnit.Relative);//Viewport = new Rect(0, 0, ww, 1.0);
                (base.AxisX as TimeAxis).TimePeriodMode = TimePeriod.Month;
            }
            else if (IsRange(w, 0.0, 12 * 30 * 86400.0) == true) // Year
            {
                _currentBackgroundMode = BackgroundMode.Year;
                throw new Exception();
            }
            base.AreaBackground = CreateBrush();
            //  base.AreaBackground = GridBrush;
        }

        public void AddIntervals(IEnumerable<SchedulerInterval> ivals, SchedulerString str)
        {
            if (str == null)
                return;

            if (_markers.ContainsKey(str) == false)
            {
                _markers.Add(str, new List<SchedulerInterval>());

                if (_core.AxisY is CategoryAxis)
                {
                    (str as SchedulerTargetMarker).OnTargetMarkerPositionChanged += AxisY.UpdateFollowLabelPosition;
                }

                base.AddMarkers(new List<SchedulerString>() { str });
            }

            if (ivals == null)
                return;

            foreach (var item in ivals)
            {
                str.Intervals.Add(item);
                item.String = str;

                _minLeft = Math.Min(item.Left, _minLeft);
                _maxRight = Math.Max(item.Right, _maxRight);

                _markers[str].Add(item);
            }

            AutoSetViewportArea();

            base.AddMarkers(ivals);
        }

        private void AutoSetViewportArea()
        {
            int height0 = 100;

            var count = _markers.Keys.Count;

            double step = (double)height0 / (double)(count + 1);
            int i = 0;
            foreach (var str in _markers.Keys)
            {
                //  str.PositionY = (++i) * step;

                str.SetLocalPosition(_minLeft, (++i) * step);

                foreach (var ival in str.Intervals)
                {
                    // item.PositionY = str.PositionY;

                    ival.SetLocalPosition(ival.LocalPosition.X, str.LocalPosition.Y);
                }
            }

            if (_minLeft != Double.MaxValue && _maxRight != Double.MinValue)
            {
                base._core.SetViewportArea(new RectD(_minLeft, 0, _maxRight - _minLeft, height0));
            }
        }

        private bool IsRange(double value, double min, double max)
        {
            if (value >= min && value <= max)
                return true;

            return false;
        }
    }
}
