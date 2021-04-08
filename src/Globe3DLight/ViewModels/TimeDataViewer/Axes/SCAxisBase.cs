#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public struct SCAxisLabelPosition
    {
        public string Label { get; set; }
        public double Value { get; set; }
    }

    public struct SCAxisInfo
    {
        public List<SCAxisLabelPosition> Labels { get; set; }
        public List<SCAxisLabelPosition> FollowLabels { get; set; }

        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }

        public SCAxisLabelPosition DynamicLabel { get; set; }
        public bool IsDynamicLabelEnable { get; set; }
        public bool IsFoolowLabelsMode { get; set; }

        public EAxisCoordType CoordType { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }



    public abstract class SCAxisBase
    {
        public abstract double FromAbsoluteToLocal(int pixel);
        public abstract int FromLocalToAbsolute(double value);

        public abstract SCAxisInfo AxisInfo { get; }

        public abstract void UpdateWindow(RectI window);
        public abstract void UpdateViewport(SCViewport viewport);
        public abstract void UpdateScreen(SCViewport screen);

        public abstract void UpdateFollowLabelPosition(ISCTargetMarker marker);

        public void UpdateAreaSize(int width, int height)
        {
            switch (CoordType)
            {
                case EAxisCoordType.X:
                    _length = width;
                    break;
                case EAxisCoordType.Y:
                    _length = height;
                    break;
                default:
                    _length = 0;
                    break;
            }

            //switch (Type)
            //{
            //    case AxisPosition.Bottom:
            //    case AxisPosition.Top:
            //        _length = width;
            //        break;
            //    case AxisPosition.Left:
            //    case AxisPosition.Right:
            //        _length = height; 
            //        break;
            //    default:
            //        _length = 0; 
            //        break;
            //}

            if (OnAxisChanged != null)
            {
                OnAxisChanged();
            }
        }

        bool _isInversed = false;
        public bool IsInversed
        {
            get
            {
                return _isInversed;
            }
            set
            {
                _isInversed = value;
            }
        }

        public bool IsDynamicLabelEnable { get; set; } = false;

        //public Transform Transform
        //{
        //    get
        //    {
        //        switch (CoordType)
        //        {
        //            case EAxisCoordType.X:
        //                return new MatrixTransform(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
        //            case EAxisCoordType.Y:
        //                return new MatrixTransform(0.0, -1.0, -1.0, 0.0, 0.0, 0.0);
        //            default:
        //                return Transform.Identity;
        //        }


        //        //switch (Type)
        //        //{
        //        //    case AxisPosition.Bottom:
        //        //        return new MatrixTransform(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
        //        //    case AxisPosition.Left:
        //        //        return new MatrixTransform(0.0, -1.0, -1.0, 0.0, 0.0, 0.0);
        //        //    case AxisPosition.Top:
        //        //        return new MatrixTransform(1.0, 0.0, 0.0, -1.0, 0.0, -Length);
        //        //    case AxisPosition.Right:
        //        //        return new MatrixTransform(0.0, -1.0, 1.0, 0.0, Length, 0.0);
        //        //    default:
        //        //        return Transform.Identity;
        //        //}
        //    }
        //}

        public int _length;
        public int Length
        {
            get
            {
                return _length;
            }
        }

        public void UpdateAxis()
        {
            if (OnAxisChanged != null)
            {
                OnAxisChanged();
            }
        }

        public string Header { get; set; } = "Default Header";

        public EAxisCoordType CoordType { get; set; }

        public event AxisChanged OnAxisChanged;
        //     public event SCAxisLengthChanged OnLengthChanged;

        protected static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        protected static int Clip(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }
    }

    //public enum AxisPosition
    //{
    //    Bottom,
    //    Left,
    //    Top,
    //    Right
    //}

    public enum EAxisCoordType
    {
        X,
        Y
    }

    /*

    public abstract class SCAxisBase : System.Windows.Controls.Control//, ICloneable
    {
        public Rect ArrangeRect { get; } // Gets the bounds of the chart axis size.
        public ObservableCollection<ChartAxisLabel> VisibleLabels { get; } // Gets the collection of axis labels in the visible region.
        public DoubleRange VisibleRange { get; } // Gets the visible range of the axis.  


        public string Header { get; set; } // Gets or sets the header for the chart axis.
        public AxisHeaderPosition HeaderPosition { get; set; } // Gets or sets the position for Axis header, when enabling the ShowAxisNextToOrigin property.
        public LabelStyle HeaderStyle { get; set; } // Gets or sets the style for the chart axis header.  
        public DataTemplate HeaderTemplate { get; set; } // Gets or sets the custom template for the chart header.




        public double ActualPlotOffset { get; } // Gets or sets the plot offset value

        public double AxisLineOffset { get; set; } // Gets or sets the offset value for applying padding to the axis line.

        public Style AxisLineStyle { get; set; } // Gets or sets the style for the axis line.

        public string ContentPath { get; set; } // Gets or sets the property path to be bind with axis label content(text).  

        public DataTemplate CrosshairLabelTemplate { get; set; } // Gets or sets the custom template for the Crosshair labels.

        public ObservableCollection<ChartAxisLabel> CustomLabels { get; } // Gets the axis custom labels collection.

        public Nullable<int> DesiredIntervalsCount { get; set; } // Gets or sets the interval for the axis auto range calculation, if Interval is not set explicitly.

        public EdgeLabelsDrawingMode EdgeLabelsDrawingMode { get; set; } // Gets or sets mode which decides the mechanism for extreme(edge) labels.It can be position center, hide, etc.

        public EdgeLabelsVisibilityMode EdgeLabelsVisibilityMode { get; set; } // Gets or sets the edge labels visibility mode for hiding the edge labels on zooming.

        public bool EnableAutoIntervalOnZooming { get; set; } // Gets or sets a value indicating whether to enable the auto interval calculation while zooming.



        protected bool isInversed; // Gets or sets a value indicating whether the axis should be reversed.When reversed, the axis will render points from right to left if horizontal, top to bottom when vertical and clockwise if radial.

        //public bool IsLogarithmic { get; } // Gets or sets IsLogarithmic property

        public int LabelExtent { get; set; } // Gets or sets the extension width for the axis label.

        public string LabelFormat { get; set; } // Gets or sets the label formatting for the axis labels.

        public double LabelRotationAngle { get; set; } // Gets or sets the rotation angle to the axis label content.

        public AxisLabelsIntersectAction LabelsIntersectAction { get; set; } // Gets or sets a value which decides the mechanism to avoid the axis labels overlapping.The overlapping labels can be hided, rotated or placed on next row.  

        public AxisElementPosition LabelsPosition { get; set; } // Gets or sets the position for the axis labels.Either inside or outside of the plot area.  

        public object LabelsSource { get; set; } // Gets or sets the custom labels collection to be displayed in axis.

        public Label LabelStyle { get; set; } // Gets or sets the style for the axis labels.

        public DataTemplate LabelTemplate { get; set; } // Gets or sets the custom template for the axis labels.

        public Style MajorGridLineStyle { get; set; } // Gets or sets the style for the major grid lines.

        public Style MajorTickLineStyle { get; set; } // Gets or sets the style for the major tick line style.

        public int MaximumLabels { get; set; } // Gets or sets the maximum no of label to be displayed per 100 pixels.

        public Style MinorGridLineStyle { get; set; } // Gets or sets the style for the minor grid lines.

        public Style MinorTickLineStyle { get; set; } // Gets or sets the style for the minor tick line style.

        public bool OpposedPosition { get; set; } // Gets or sets a value indicating whether to enable the axis to position opposite to its actual position.That is, to the other side of plot area.

        public int Origin { get; set; } // Gets or sets the origin value where its associated axis should place.

        public Style OriginLineStyle { get; set; } // Gets or sets the style for origin line when enable the ShowOrigin property.

        public double PlotOffset { get; set; } // Gets or sets the offset value for applying the padding to the plot area.

        public string PositionPath { get; set; } // Gets or sets the property path to be bind with axis label position.

        public DataTemplate PostfixLabelTemplate { get; set; } // Gets or sets the custom template for the axis label postfix.  

        public DataTemplate PrefixLabelTemplate { get; set; } // Gets or sets the custom template for the axis label postfix.  

        public ObservableCollection<ChartAxisRangeStyle> RangeStyles { get; set; } // Gets or sets a collection of the ChartAxisRangeStyle to customize the axis gridlines.  

        public bool ShowAxisNextToOrigin { get; set; } // Gets or sets a value indicating whether axis can be positioned across the plot area.

        public bool ShowGridLines { get; set; } // Gets or sets a value indicating whether the axis gird lines can be display or not.  

        public bool ShowOrigin { get; set; } // Gets or sets a value indicating whether to show the origin line or not.

        public bool ShowTrackBallInfo { get; set; } // Gets or sets a value indicating whether to show track ball label for this axis.

        public DataTemplate ThumbLabelTemplate { get; set; } // Gets or sets the custom template for the scroll bar thumb.

        public Visibility ThumbLabelVisibility { get; set; } // Gets or sets visibility of label.  

        public int TickLineSize { get; set; } // Gets or sets the size for the axis tick lines.  

        public AxisElementPosition TickLinesPosition { get; set; } // Gets or sets a value indcating whether the tick line position, either inside or outside.  

        public DataTemplate TrackBallLabelTemplate { get; set; } // Gets or sets the custom template for the trackball tooltip label.


    }

    */
}
