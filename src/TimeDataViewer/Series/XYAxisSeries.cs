using Avalonia;

namespace TimeDataViewer
{
    public abstract class XYAxisSeries : ItemsSeries
    {
        public static readonly StyledProperty<string> XAxisKeyProperty = AvaloniaProperty.Register<XYAxisSeries, string>(nameof(XAxisKey), null);

        public static readonly StyledProperty<string> YAxisKeyProperty = AvaloniaProperty.Register<XYAxisSeries, string>(nameof(YAxisKey), null);

        static XYAxisSeries()
        {
            XAxisKeyProperty.Changed.AddClassHandler<XYAxisSeries>(AppearanceChanged);
            YAxisKeyProperty.Changed.AddClassHandler<XYAxisSeries>(AppearanceChanged);
        }

        public string XAxisKey
        {
            get
            {
                return GetValue(XAxisKeyProperty);
            }

            set
            {
                SetValue(XAxisKeyProperty, value);
            }
        }

        public string YAxisKey
        {
            get
            {
                return GetValue(YAxisKeyProperty);
            }

            set
            {
                SetValue(YAxisKeyProperty, value);
            }
        }

        protected override void SynchronizeProperties(Core.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (Core.XYAxisSeries)series;
            s.XAxisKey = XAxisKey;
            s.YAxisKey = YAxisKey;
        }
    }
}
