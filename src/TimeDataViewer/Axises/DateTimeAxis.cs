using System;
using System.Globalization;
using Avalonia;

namespace TimeDataViewer
{
    public class DateTimeAxis : Axis
    {
        public static readonly StyledProperty<CalendarWeekRule> CalendarWeekRuleProperty = AvaloniaProperty.Register<DateTimeAxis, CalendarWeekRule>(nameof(CalendarWeekRule), CalendarWeekRule.FirstFourDayWeek);

        public static readonly StyledProperty<DateTime> FirstDateTimeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTime>(nameof(FirstDateTime), DateTime.MinValue);

        public static readonly StyledProperty<DayOfWeek> FirstDayOfWeekProperty = AvaloniaProperty.Register<DateTimeAxis, DayOfWeek>(nameof(FirstDayOfWeek), DayOfWeek.Monday);

        public static readonly StyledProperty<Core.DateTimeIntervalType> IntervalTypeProperty = AvaloniaProperty.Register<DateTimeAxis, Core.DateTimeIntervalType>(nameof(IntervalType), Core.DateTimeIntervalType.Auto);

        public static readonly StyledProperty<DateTime> LastDateTimeProperty = AvaloniaProperty.Register<DateTimeAxis, DateTime>(nameof(LastDateTime), DateTime.MaxValue);

        public static readonly StyledProperty<Core.DateTimeIntervalType> MinorIntervalTypeProperty = AvaloniaProperty.Register<DateTimeAxis, Core.DateTimeIntervalType>(nameof(MinorIntervalType), Core.DateTimeIntervalType.Auto);

        static DateTimeAxis()
        {
            PositionProperty.OverrideDefaultValue<DateTimeAxis>(Core.AxisPosition.Bottom);
            PositionProperty.Changed.AddClassHandler<DateTimeAxis>(AppearanceChanged);
            CalendarWeekRuleProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
            FirstDayOfWeekProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
            MinorIntervalTypeProperty.Changed.AddClassHandler<DateTimeAxis>(DataChanged);
        }

        public DateTimeAxis()
        {
            InternalAxis = new Core.DateTimeAxis();
        }

        public CalendarWeekRule CalendarWeekRule
        {
            get
            {
                return GetValue(CalendarWeekRuleProperty);
            }

            set
            {
                SetValue(CalendarWeekRuleProperty, value);
            }
        }

        public DateTime FirstDateTime
        {
            get
            {
                return GetValue(FirstDateTimeProperty);
            }

            set
            {
                SetValue(FirstDateTimeProperty, value);
            }
        }

        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return GetValue(FirstDayOfWeekProperty);
            }

            set
            {
                SetValue(FirstDayOfWeekProperty, value);
            }
        }

        public Core.DateTimeIntervalType IntervalType
        {
            get
            {
                return GetValue(IntervalTypeProperty);
            }

            set
            {
                SetValue(IntervalTypeProperty, value);
            }
        }

        public DateTime LastDateTime
        {
            get
            {
                return GetValue(LastDateTimeProperty);
            }

            set
            {
                SetValue(LastDateTimeProperty, value);
            }
        }

        public Core.DateTimeIntervalType MinorIntervalType
        {
            get
            {
                return GetValue(MinorIntervalTypeProperty);
            }

            set
            {
                SetValue(MinorIntervalTypeProperty, value);
            }
        }

        public override Core.Axis CreateModel()
        {
            SynchronizeProperties();
            return InternalAxis;
        }

        protected override void SynchronizeProperties()
        {
            base.SynchronizeProperties();
            var a = (Core.DateTimeAxis)InternalAxis;

            a.IntervalType = IntervalType;
            a.MinorIntervalType = MinorIntervalType;
            a.FirstDayOfWeek = FirstDayOfWeek;
            a.CalendarWeekRule = CalendarWeekRule;

            if (FirstDateTime > DateTime.MinValue)
            {
                a.Minimum = Core.DateTimeAxis.ToDouble(FirstDateTime);
            }

            if (LastDateTime < DateTime.MaxValue)
            {
                a.Maximum = Core.DateTimeAxis.ToDouble(LastDateTime);
            }
        }
    }
}
