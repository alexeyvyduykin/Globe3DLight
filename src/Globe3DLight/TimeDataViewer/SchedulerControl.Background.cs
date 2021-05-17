using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using TimeDataViewer.Core;

namespace TimeDataViewer
{
    public partial class SchedulerControl
    {
        private enum BackgroundMode { Hour, Day, Week, Month, Year }
        private readonly IBrush _brushFirst = new SolidColorBrush() { Color = Color.Parse("#BDBDBD") /*Colors.Silver*/ };
        private readonly IBrush _brushSecond = new SolidColorBrush() { Color = Color.Parse("#F5F5F5") /*Colors.WhiteSmoke*/ };         
        
        //private VisualBrush _areaBackground;

        //public VisualBrush AreaBackground => _areaBackground;

        //private Grid CreateGrid(int count, double w, double h)
        //{
        //    Grid grid = new Grid();
        //    grid.RowDefinitions.Add(new RowDefinition());
        //    for (int i = 0; i < count; i++)
        //    {
        //        grid.ColumnDefinitions.Add(new ColumnDefinition());
        //    }

        //    for (int i = 0; i < count; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            var rect = new Rectangle() { Fill = _brushFirst, Width = w / count, Height = h };
        //            Grid.SetColumn(rect, i);
        //            grid.Children.Add(rect);
        //        }
        //        else
        //        {
        //            var rect = new Rectangle() { Fill = _brushSecond, Width = w / count, Height = h };
        //            Grid.SetColumn(rect, i);
        //            grid.Children.Add(rect);
        //        }
        //    }

        //    return grid;
        //}

        //VisualBrush GetBrush__(int count, RelativeRect destinationRect)
        //{
        //    VisualBrush brush = new VisualBrush();
        //    brush.TileMode = TileMode.Tile;
        //    brush.SourceRect = new RelativeRect(0, 0, 20, 10, RelativeUnit.Absolute);
        //    brush.DestinationRect = destinationRect;// new RelativeRect(0, 0, 20, 20, RelativeUnit.Absolute);
        //    brush.Visual = new Path()
        //    {
        //        Data = Geometry.Parse("M 0,0 L 0,10 L 10,10 L 10,0 Z"),
        //        Fill = new SolidColorBrush() { Color = Color.Parse("#FFF0F0F0") },
        //    };

        //    return brush;
        //}

        //VisualBrush CreateGridBrush0()
        //{
        //    // Create a DrawingBrush  
        //    VisualBrush blackBrush = new VisualBrush();

        //    // Create a Geometry with white background  
        //    GeometryDrawing backgroundSquare = new GeometryDrawing()
        //    {
        //        Brush = Brushes.DarkGray,
        //        Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1))
        //    };

        //    // Create a GeometryGroup that will be added to Geometry  
        //    // GeometryGroup gGroup = new GeometryGroup();
        //    // gGroup.Children.Add(new RectangleGeometry(new Rect(0, 0, 0.5, 1)));

        //    // Create a GeomertyDrawing  
        //    GeometryDrawing checkers = new GeometryDrawing()
        //    {
        //        Brush = new SolidColorBrush(Colors.GhostWhite),
        //        Geometry = new RectangleGeometry(new Rect(0, 0, 0.5, 1))//gGroup 
        //    };
        //    DrawingGroup checkersDrawingGroup = new DrawingGroup();
        //    checkersDrawingGroup.Children.Add(backgroundSquare);
        //    checkersDrawingGroup.Children.Add(checkers);

        //    // blackBrush.Visual = checkersDrawingGroup;
        //    blackBrush.Visual = new Rectangle() { Width = 800, Height = 600, Fill = Brushes.Silver };

        //    // Set Viewport and TimeMode  
        //    blackBrush.DestinationRect = new RelativeRect(0, 0, 1.0, 1.0, RelativeUnit.Relative);//Viewport = new Rect(0, 0, 1.0, 1.0);
        //    blackBrush.TileMode = TileMode.Tile;

        //    return blackBrush;
        //}

        //private IBrush UpdateBackgroundBrush()
        //{
        //    var w = ViewportAreaScreen.Width;
        //    var len = ViewportAreaData.Width;

        //    // if (w == 0)
        //    //     return;

        //    int count = 0;

        //    if (IsRange(w, 0.0, 3600.0) == true) // Hour
        //    {
        //        AxisX.TimePeriodMode = TimePeriod.Hour;
        //        count = (int)(len / (60 * 24 * 86400.0));
        //    }
        //    else if (IsRange(w, 0.0, 86400.0) == true) // Day
        //    {
        //        AxisX.TimePeriodMode = TimePeriod.Day;
        //        count = (int)(len / (86400.0 / 24));                
        //    }
        //    else if (IsRange(w, 0.0, 7 * 86400.0) == true) // Week
        //    {
        //        AxisX.TimePeriodMode = TimePeriod.Week;
        //        count = (int)(len / 86400.0);
        //    }
        //    else if (IsRange(w, 0.0, 30 * 86400.0) == true) // Month
        //    {
        //        AxisX.TimePeriodMode = TimePeriod.Month;
        //        count = (int)(len / 86400.0);
        //    }
        //    else if (IsRange(w, 0.0, 12 * 30 * 86400.0) == true) // Year
        //    {
        //        throw new Exception();
        //    }
          
        //    var grid = CreateGrid(count, AbsoluteWindow.Width, AbsoluteWindow.Height);

        //    grid.Width = AbsoluteWindow.Width;
        //    grid.Height = AbsoluteWindow.Height;

        //    var areaBackground = new VisualBrush()
        //    {
        //        Visual = grid,                           
        //        DestinationRect = new RelativeRect(
        //             RenderOffsetAbsolute.X, 0, AbsoluteWindow.Width, AbsoluteWindow.Height, RelativeUnit.Relative)
        //    };

        //    return areaBackground;
        //}

        private void DrawBackground(DrawingContext context)
        {
            var w = ClientViewportArea.Width;
            var len = ViewportArea.Width;

            if (w == 0)
            {
                return;
            }

            int count = 0;

            if (IsRange(w, 0.0, 3600.0) == true) // Hour
            {
                AxisX.TimePeriodMode = TimePeriod.Hour;
                count = (int)(len / (86400.0 / (24 * 60)));
            }
            else if (IsRange(w, 0.0, 86400.0) == true) // Day
            {
                AxisX.TimePeriodMode = TimePeriod.Day;
                count = (int)(len / (86400.0 / 24));           
            }
            else if (IsRange(w, 0.0, 7 * 86400.0) == true) // Week
            {
                AxisX.TimePeriodMode = TimePeriod.Week;
                count = (int)(len / 86400.0);
            }
            else if (IsRange(w, 0.0, 30 * 86400.0) == true) // Month
            {
                AxisX.TimePeriodMode = TimePeriod.Month;
                count = (int)(len / 86400.0);
            }
            else if (IsRange(w, 0.0, 12 * 30 * 86400.0) == true) // Year
            {
                throw new Exception();
            }

            var height = _area.Window.Height;
            var width = _area.Window.Width;

            for (int i = 0; i < count; i++)
            {
                var brush = (i % 2 == 0) ? _brushFirst : _brushSecond;
                double dw = (double)width / count;
                context.FillRectangle(brush, new Rect(dw * i + WindowOffset.X, 0, dw, height));
            }
        }

        private bool IsRange(double value, double min, double max) => value >= min && value <= max;
    }
}
