using Avalonia;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;

namespace TimeDataViewer
{
    public partial class Axis
    {
        static Axis()
        {
            AbsoluteMaximumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AbsoluteMinimumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisTickToLabelDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisTitleDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            AxisDistanceProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            ExtraGridlinesProperty.Changed.AddClassHandler<Axis>(DataChanged);
            FontProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            FontSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            IntervalLengthProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            IsAxisVisibleProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            KeyProperty.Changed.AddClassHandler<Axis>(DataChanged);
            MajorStepProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorTickSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MaximumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MaximumRangeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinimumProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinimumRangeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorStepProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorTickSizeProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            PositionAtZeroCrossingProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            PositionProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            StringFormatProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            TicklineColorProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            UnitProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorPenProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorPenProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MinorTickPenProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
            MajorTickPenProperty.Changed.AddClassHandler<Axis>(AppearanceChanged);
        }

        public static readonly StyledProperty<ControlTemplate> DefaultLabelTemplateProperty =
            AvaloniaProperty.Register<TimelineBase, ControlTemplate>(nameof(DefaultLabelTemplate));

        public ControlTemplate DefaultLabelTemplate
        {
            get
            {
                return GetValue(DefaultLabelTemplateProperty);
            }

            set
            {
                SetValue(DefaultLabelTemplateProperty, value);
            }
        }


        public static readonly StyledProperty<Pen> MinorPenProperty =
            AvaloniaProperty.Register<TimelineBase, Pen>(nameof(MinorPen), new Pen());

        public Pen MinorPen
        {
            get
            {
                return GetValue(MinorPenProperty);
            }

            set
            {
                SetValue(MinorPenProperty, value);
            }
        }

        public static readonly StyledProperty<Pen> MajorPenProperty =
            AvaloniaProperty.Register<TimelineBase, Pen>(nameof(MajorPen), new Pen());

        public Pen MajorPen
        {
            get
            {
                return GetValue(MajorPenProperty);
            }

            set
            {
                SetValue(MajorPenProperty, value);
            }
        }

        public static readonly StyledProperty<Pen> MinorTickPenProperty =
            AvaloniaProperty.Register<TimelineBase, Pen>(nameof(MinorTickPen), new Pen());

        public Pen MinorTickPen
        {
            get
            {
                return GetValue(MinorTickPenProperty);
            }

            set
            {
                SetValue(MinorTickPenProperty, value);
            }
        }

        public static readonly StyledProperty<Pen> MajorTickPenProperty =
            AvaloniaProperty.Register<TimelineBase, Pen>(nameof(MajorTickPen), new Pen());

        public Pen MajorTickPen
        {
            get
            {
                return GetValue(MajorTickPenProperty);
            }

            set
            {
                SetValue(MajorTickPenProperty, value);
            }
        }


        public static readonly StyledProperty<double> AbsoluteMaximumProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(AbsoluteMaximum), double.MaxValue);

        public double AbsoluteMaximum
        {
            get
            {
                return GetValue(AbsoluteMaximumProperty);
            }

            set
            {
                SetValue(AbsoluteMaximumProperty, value);
            }
        }

        public static readonly StyledProperty<double> AbsoluteMinimumProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(AbsoluteMinimum), double.MinValue);

        public double AbsoluteMinimum
        {
            get
            {
                return GetValue(AbsoluteMinimumProperty);
            }

            set
            {
                SetValue(AbsoluteMinimumProperty, value);
            }
        }

        public static readonly StyledProperty<double> AxisTickToLabelDistanceProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(AxisTickToLabelDistance), 4.0);

        public double AxisTickToLabelDistance
        {
            get
            {
                return GetValue(AxisTickToLabelDistanceProperty);
            }

            set
            {
                SetValue(AxisTickToLabelDistanceProperty, value);
            }
        }

        public static readonly StyledProperty<double> AxisTitleDistanceProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(AxisTitleDistance), 4.0);

        public double AxisTitleDistance
        {
            get
            {
                return GetValue(AxisTitleDistanceProperty);
            }

            set
            {
                SetValue(AxisTitleDistanceProperty, value);
            }
        }

        public static readonly StyledProperty<double> AxisDistanceProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(AxisDistance), 0.0);

        public double AxisDistance
        {
            get
            {
                return GetValue(AxisDistanceProperty);
            }

            set
            {
                SetValue(AxisDistanceProperty, value);
            }
        }


        public static readonly StyledProperty<double[]> ExtraGridlinesProperty =
            AvaloniaProperty.Register<Axis, double[]>(nameof(ExtraGridlines), null);

        public double[] ExtraGridlines
        {
            get
            {
                return GetValue(ExtraGridlinesProperty);
            }

            set
            {
                SetValue(ExtraGridlinesProperty, value);
            }
        }

        public static readonly StyledProperty<string> FontProperty =
            AvaloniaProperty.Register<Axis, string>(nameof(Font), null);

        public string Font
        {
            get
            {
                return GetValue(FontProperty);
            }

            set
            {
                SetValue(FontProperty, value);
            }
        }

        public static readonly StyledProperty<double> FontSizeProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(FontSize), double.NaN);

        public double FontSize
        {
            get
            {
                return GetValue(FontSizeProperty);
            }

            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        public static readonly StyledProperty<double> IntervalLengthProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(IntervalLength), 60.0);

        public double IntervalLength
        {
            get
            {
                return GetValue(IntervalLengthProperty);
            }

            set
            {
                SetValue(IntervalLengthProperty, value);
            }
        }

        public static readonly StyledProperty<bool> IsAxisVisibleProperty =
            AvaloniaProperty.Register<Axis, bool>(nameof(IsAxisVisible), true);

        public bool IsAxisVisible
        {
            get
            {
                return GetValue(IsAxisVisibleProperty);
            }

            set
            {
                SetValue(IsAxisVisibleProperty, value);
            }
        }

        public static readonly StyledProperty<bool> IsPanEnabledProperty =
            AvaloniaProperty.Register<Axis, bool>(nameof(IsPanEnabled), true);

        public bool IsPanEnabled
        {
            get
            {
                return GetValue(IsPanEnabledProperty);
            }

            set
            {
                SetValue(IsPanEnabledProperty, value);
            }
        }

        public static readonly StyledProperty<bool> IsZoomEnabledProperty =
            AvaloniaProperty.Register<Axis, bool>(nameof(IsZoomEnabled), true);

        public bool IsZoomEnabled
        {
            get
            {
                return GetValue(IsZoomEnabledProperty);
            }

            set
            {
                SetValue(IsZoomEnabledProperty, value);
            }
        }

        public static readonly StyledProperty<string> KeyProperty =
            AvaloniaProperty.Register<Axis, string>(nameof(Key), null);

        public string Key
        {
            get
            {
                return GetValue(KeyProperty);
            }

            set
            {
                SetValue(KeyProperty, value);
            }
        }

        public static readonly StyledProperty<double> MajorStepProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MajorStep), double.NaN);

        public double MajorStep
        {
            get
            {
                return GetValue(MajorStepProperty);
            }

            set
            {
                SetValue(MajorStepProperty, value);
            }
        }

        public static readonly StyledProperty<double> MajorTickSizeProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MajorTickSize), 7.0);

        public double MajorTickSize
        {
            get
            {
                return GetValue(MajorTickSizeProperty);
            }

            set
            {
                SetValue(MajorTickSizeProperty, value);
            }
        }

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(Maximum), double.NaN);

        public double Maximum
        {
            get
            {
                return GetValue(MaximumProperty);
            }

            set
            {
                SetValue(MaximumProperty, value);
            }
        }

        public static readonly StyledProperty<double> MaximumRangeProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MaximumRange), double.PositiveInfinity);

        public double MaximumRange
        {
            get
            {
                return GetValue(MaximumRangeProperty);
            }

            set
            {
                SetValue(MaximumRangeProperty, value);
            }
        }

        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(Minimum), double.NaN);

        public double Minimum
        {
            get
            {
                return GetValue(MinimumProperty);
            }

            set
            {
                SetValue(MinimumProperty, value);
            }
        }

        public static readonly StyledProperty<double> MinimumRangeProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MinimumRange), 0.0);

        public double MinimumRange
        {
            get
            {
                return GetValue(MinimumRangeProperty);
            }

            set
            {
                SetValue(MinimumRangeProperty, value);
            }
        }


        public static readonly StyledProperty<double> MinorStepProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MinorStep), double.NaN);

        public double MinorStep
        {
            get
            {
                return GetValue(MinorStepProperty);
            }

            set
            {
                SetValue(MinorStepProperty, value);
            }
        }

        public static readonly StyledProperty<double> MinorTickSizeProperty =
            AvaloniaProperty.Register<Axis, double>(nameof(MinorTickSize), 4.0);

        public double MinorTickSize
        {
            get
            {
                return GetValue(MinorTickSizeProperty);
            }

            set
            {
                SetValue(MinorTickSizeProperty, value);
            }
        }

        public static readonly StyledProperty<bool> PositionAtZeroCrossingProperty =
            AvaloniaProperty.Register<Axis, bool>(nameof(PositionAtZeroCrossing), false);

        public bool PositionAtZeroCrossing
        {
            get
            {
                return GetValue(PositionAtZeroCrossingProperty);
            }

            set
            {
                SetValue(PositionAtZeroCrossingProperty, value);
            }
        }

        public static readonly StyledProperty<Core.AxisPosition> PositionProperty =
            AvaloniaProperty.Register<Axis, Core.AxisPosition>(nameof(Position), Core.AxisPosition.Left);

        public Core.AxisPosition Position
        {
            get
            {
                return GetValue(PositionProperty);
            }

            set
            {
                SetValue(PositionProperty, value);
            }
        }

        public static readonly StyledProperty<string> StringFormatProperty =
            AvaloniaProperty.Register<Axis, string>(nameof(StringFormat), null);

        public string StringFormat
        {
            get
            {
                return GetValue(StringFormatProperty);
            }

            set
            {
                SetValue(StringFormatProperty, value);
            }
        }


        public static readonly StyledProperty<Color> TicklineColorProperty =
            AvaloniaProperty.Register<Axis, Color>(nameof(TicklineColor), Colors.Black);

        public Color TicklineColor
        {
            get
            {
                return GetValue(TicklineColorProperty);
            }

            set
            {
                SetValue(TicklineColorProperty, value);
            }
        }

        public static readonly StyledProperty<string> UnitProperty =
            AvaloniaProperty.Register<Axis, string>(nameof(Unit), null);

        public string Unit
        {
            get
            {
                return GetValue(UnitProperty);
            }

            set
            {
                SetValue(UnitProperty, value);
            }
        }
    }
}
