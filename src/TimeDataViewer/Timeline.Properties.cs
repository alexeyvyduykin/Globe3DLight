using System.Collections.ObjectModel;
using System.Globalization;
using Avalonia;
using Avalonia.Metadata;

namespace TimeDataViewer
{
    public partial class Timeline
    {
        private readonly ObservableCollection<Axis> _axises;
        private readonly ObservableCollection<Series> _series;

        static Timeline()
        {
            PaddingProperty.OverrideDefaultValue<Timeline>(new Thickness(8));
            PaddingProperty.Changed.AddClassHandler<Timeline>(AppearanceChanged);
            CultureProperty.Changed.AddClassHandler<Timeline>(AppearanceChanged);
            DefaultFontProperty.Changed.AddClassHandler<Timeline>(AppearanceChanged);
            DefaultFontSizeProperty.Changed.AddClassHandler<Timeline>(AppearanceChanged);
            PlotMarginsProperty.Changed.AddClassHandler<Timeline>(AppearanceChanged);
            InvalidateFlagProperty.Changed.AddClassHandler<Timeline>((s, e) => s.InvalidateFlagChanged());

            SliderProperty.Changed.AddClassHandler<Timeline>(SliderChanged);
        }

        public Collection<Axis> Axises => _axises;

        [Content]
        public Collection<Series> Series => _series;

        public static readonly StyledProperty<Slider> SliderProperty =
            AvaloniaProperty.Register<Timeline, Slider>(nameof(Slider));

        public Slider Slider
        {
            get
            {
                return GetValue(SliderProperty);
            }

            set
            {
                SetValue(SliderProperty, value);
            }
        }

        public static readonly StyledProperty<CultureInfo> CultureProperty =
            AvaloniaProperty.Register<Timeline, CultureInfo>(nameof(Culture));

        public CultureInfo Culture
        {
            get
            {
                return GetValue(CultureProperty);
            }

            set
            {
                SetValue(CultureProperty, value);
            }
        }

        public static readonly StyledProperty<string> DefaultFontProperty =
            AvaloniaProperty.Register<Timeline, string>(nameof(DefaultFont), "Segoe UI");

        public string DefaultFont
        {
            get
            {
                return GetValue(DefaultFontProperty);
            }

            set
            {
                SetValue(DefaultFontProperty, value);
            }
        }

        public static readonly StyledProperty<double> DefaultFontSizeProperty =
            AvaloniaProperty.Register<Timeline, double>(nameof(DefaultFontSize), 12d);

        public double DefaultFontSize
        {
            get
            {
                return GetValue(DefaultFontSizeProperty);
            }

            set
            {
                SetValue(DefaultFontSizeProperty, value);
            }
        }

        public static readonly StyledProperty<Thickness> PlotMarginsProperty =
            AvaloniaProperty.Register<Timeline, Thickness>(nameof(PlotMargins), new Thickness());

        public Thickness PlotMargins
        {
            get
            {
                return GetValue(PlotMarginsProperty);
            }

            set
            {
                SetValue(PlotMarginsProperty, value);
            }
        }

        public static readonly StyledProperty<int> InvalidateFlagProperty =
            AvaloniaProperty.Register<Timeline, int>(nameof(InvalidateFlag), 0);

        // Gets or sets the refresh flag (an integer value). When the flag is changed, the Plot will be refreshed.
        public int InvalidateFlag
        {
            get
            {
                return GetValue(InvalidateFlagProperty);
            }

            set
            {
                SetValue(InvalidateFlagProperty, value);
            }
        }

        // Invalidates the Plot control/view when the <see cref="InvalidateFlag" /> property is changed.
        private void InvalidateFlagChanged()
        {
            InvalidatePlot();
        }
    }
}
