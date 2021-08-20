using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace TimeDataViewer
{
    public abstract partial class Axis : Control
    {
        public Core.Axis InternalAxis { get; protected set; }
        protected Pen AxislinePen { get; set; }
        protected Pen ExtraPen { get; set; }
        protected Pen ZeroPen { get; set; }
        protected Pen BlackPen { get; set; }
        protected Axis()
        {
            BlackPen = new Pen() { Brush = Brushes.Black, Thickness = 1 };
            ZeroPen = new Pen() { Brush = Brushes.Black, Thickness = 1 };
            ExtraPen = new Pen() { Brush = Brushes.Black, Thickness = 1, DashStyle = DashStyle.Dot };
            AxislinePen = new Pen() { Brush = Brushes.Black, Thickness = 1 };
        }

        public abstract Core.Axis CreateModel();

        protected static void AppearanceChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Axis)d).OnVisualChanged();
        }

        protected static void DataChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ((Axis)d).OnDataChanged();
        }

        protected void OnDataChanged()
        {
            if (Parent is Core.IPlotView pc)
            {
                pc.InvalidatePlot();
            }
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> e)
        {
            base.OnPropertyChanged(e);
            if (e.Property.OwnerType == GetType())
            {
                if (Parent is Core.IPlotView plot)
                {
                    plot.InvalidatePlot();
                }
            }
        }

        protected void OnVisualChanged()
        {
            if (Parent is Core.IPlotView pc)
            {
                pc.InvalidatePlot(false);
            }
        }

        protected virtual void SynchronizeProperties()
        {
            var a = InternalAxis;
            a.AbsoluteMaximum = AbsoluteMaximum;
            a.AbsoluteMinimum = AbsoluteMinimum;
            a.AxisDistance = AxisDistance;
            a.AxisTickToLabelDistance = AxisTickToLabelDistance;
            a.ExtraGridlines = ExtraGridlines;
            a.IntervalLength = IntervalLength;
            a.IsPanEnabled = IsPanEnabled;
            a.IsAxisVisible = IsAxisVisible;
            a.IsZoomEnabled = IsZoomEnabled;
            a.Key = Key;
            a.MajorStep = MajorStep;
            a.MajorTickSize = MajorTickSize;
            a.MinorStep = MinorStep;
            a.MinorTickSize = MinorTickSize;
            a.Minimum = Minimum;
            a.Maximum = Maximum;
            a.MinimumRange = MinimumRange;
            a.MaximumRange = MaximumRange;
            //   a.MinimumPadding = MinimumPadding;
            //   a.MaximumPadding = MaximumPadding;
            a.Position = Position;
            a.StringFormat = StringFormat;
            //a.ToolTip = ToolTip.GetTip(this) != null ? ToolTip.GetTip(this).ToString() : null;
        }

        public virtual void Render(CanvasRenderContext contextAxis, CanvasRenderContext contextPlot)
        {
            if (InternalAxis == null)
            {
                return;
            }

            var labels = InternalAxis.MyLabels;
            var minorSegments = InternalAxis.MyMinorSegments;
            var minorTickSegments = InternalAxis.MyMinorTickSegments;
            var majorSegments = InternalAxis.MyMajorSegments;
            var majorTickSegments = InternalAxis.MyMajorTickSegments;

            if (MinorPen != null)
            {
                contextPlot.DrawLineSegments(minorSegments, MinorPen);
            }

            if (MinorTickPen != null)
            {
                contextAxis.DrawLineSegments(minorTickSegments, MinorTickPen);
            }

            foreach (var (pt, text, ha, va) in labels)
            {
                var label = DefaultLabelTemplate.Build(new ContentControl());

                if (label.Control is TextBlock textBlock)
                {
                    textBlock.Text = text;
                    textBlock.HorizontalAlignment = ha.ToAvalonia();
                    textBlock.VerticalAlignment = va.ToAvalonia();
                    contextAxis.DrawMathText(pt, textBlock);
                }
            }

            if (MajorPen != null)
            {
                contextPlot.DrawLineSegments(majorSegments, MajorPen);
            }

            if (MajorTickPen != null)
            {
                contextAxis.DrawLineSegments(majorTickSegments, MajorTickPen);
            }

            //if(MinMaxBrush != null && InternalAxis.IsHorizontal() == true)
            //{
            //    var rect0 = InternalAxis.LeftRect;
            //    var rect1 = InternalAxis.RightRect;

            //    if (rect0.Width != 0)
            //    {
            //      //  contextPlot.DrawRectangle(rect0, MinMaxBrush, null);
            //      //  contextPlot.DrawLine(rect0.Right, rect0.Top, rect0.Right, rect0.Bottom, BlackPen);
            //    }

            //    if (rect1.Width != 0)
            //    {
            //      //  contextPlot.DrawRectangle(rect1, MinMaxBrush, null);
            //      //  contextPlot.DrawLine(rect1.Left, rect1.Top, rect1.Left, rect1.Bottom, BlackPen);
            //    }
            //}
        }
    }
}
