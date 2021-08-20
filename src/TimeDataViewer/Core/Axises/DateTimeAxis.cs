#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public enum DateTimeIntervalType
    {
        Auto = 0,
        Manual = 1,
        Milliseconds = 2,
        Seconds = 3,
        Minutes = 4,
        Hours = 5,
        Days = 6,
        Weeks = 7,
        Months = 8,
        Years = 9,
    }

    public class DateTimeAxis : LinearAxis
    {
        private static readonly DateTime TimeOrigin = new DateTime(1899, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        private static readonly double MaxDayValue = (DateTime.MaxValue - TimeOrigin).TotalDays;
        private static readonly double MinDayValue = (DateTime.MinValue - TimeOrigin).TotalDays;
        private DateTimeIntervalType _actualIntervalType;
        private DateTimeIntervalType _actualMinorIntervalType;

        public DateTimeAxis()
        {
            Position = AxisPosition.Bottom;
            IntervalType = DateTimeIntervalType.Auto;
            FirstDayOfWeek = DayOfWeek.Monday;
            CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek;
        }

        public CalendarWeekRule CalendarWeekRule { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public DateTimeIntervalType IntervalType { get; set; }

        public DateTimeIntervalType MinorIntervalType { get; set; }

        /// <summary>
        /// Gets or sets the time zone (used when formatting date/time values).
        /// </summary>
        /// <value>The time zone info.</value>
        /// <remarks>No date/time conversion will be performed if this property is <c>null</c>.</remarks>
        public TimeZoneInfo TimeZone { get; set; }

        public static DataPoint CreateDataPoint(DateTime x, double y)
        {
            return new DataPoint(ToDouble(x), y);
        }

        public static DataPoint CreateDataPoint(DateTime x, DateTime y)
        {
            return new DataPoint(ToDouble(x), ToDouble(y));
        }

        public static DataPoint CreateDataPoint(double x, DateTime y)
        {
            return new DataPoint(x, ToDouble(y));
        }

        /// <summary>
        /// Converts a numeric representation of the date (number of days after the time origin) to a DateTime structure.
        /// </summary>
        /// <param name="value">The number of days after the time origin.</param>
        /// <returns>A <see cref="DateTime" /> structure. Ticks = 0 if the value is invalid.</returns>
        public static DateTime ToDateTime(double value)
        {
            if (double.IsNaN(value) || value < MinDayValue || value > MaxDayValue)
            {
                return new DateTime();
            }

            return TimeOrigin.AddDays(value - 1);
        }

        /// <summary>
        /// Converts a DateTime to days after the time origin.
        /// </summary>
        /// <param name="value">The date/time structure.</param>
        /// <returns>The number of days after the time origin.</returns>
        public static double ToDouble(DateTime value)
        {
            var span = value - TimeOrigin;
            return span.TotalDays + 1;
        }

        public override void GetTickValues(out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            minorTickValues = CreateDateTimeTickValues(ActualMinimum, ActualMaximum, ActualMinorStep, _actualMinorIntervalType);
            majorTickValues = CreateDateTimeTickValues(ActualMinimum, ActualMaximum, ActualMajorStep, _actualIntervalType);
            majorLabelValues = majorTickValues;
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if necessary.
        /// e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The value.</returns>
        public override object GetValue(double x)
        {
            var time = ToDateTime(x);

            if (TimeZone != null)
            {
                time = TimeZoneInfo.ConvertTime(time, TimeZone);
            }

            return time;
        }

        internal override void UpdateIntervals(OxyRect plotArea)
        {
            base.UpdateIntervals(plotArea);
            switch (_actualIntervalType)
            {
                case DateTimeIntervalType.Years:
                    ActualMinorStep = 31;
                    _actualMinorIntervalType = DateTimeIntervalType.Years;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "yyyy";
                    }

                    break;
                case DateTimeIntervalType.Months:
                    _actualMinorIntervalType = DateTimeIntervalType.Months;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Weeks:
                    _actualMinorIntervalType = DateTimeIntervalType.Days;
                    ActualMajorStep = 7;
                    ActualMinorStep = 1;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "yyyy/ww";
                    }

                    break;
                case DateTimeIntervalType.Days:
                    ActualMinorStep = ActualMajorStep;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "yyyy-MM-dd";
                    }

                    break;
                case DateTimeIntervalType.Hours:
                    ActualMinorStep = ActualMajorStep;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Minutes:
                    ActualMinorStep = ActualMajorStep;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "HH:mm";
                    }

                    break;
                case DateTimeIntervalType.Seconds:
                    ActualMinorStep = ActualMajorStep;
                    if (StringFormat == null)
                    {
                        ActualStringFormat = "HH:mm:ss";
                    }

                    break;



                case DateTimeIntervalType.Milliseconds:
                    ActualMinorStep = ActualMajorStep;
                    if (ActualStringFormat == null)
                    {
                        ActualStringFormat = "HH:mm:ss.fff";
                    }

                    break;

                case DateTimeIntervalType.Manual:
                    break;
                case DateTimeIntervalType.Auto:
                    break;
            }
        }

        protected override string GetDefaultStringFormat()
        {
            return null;
        }

        protected override string FormatValueOverride(double x)
        {
            // convert the double value to a DateTime
            var time = ToDateTime(x);

            // If a time zone is specified, convert the time
            if (TimeZone != null)
            {
                time = TimeZoneInfo.ConvertTime(time, TimeZone);
            }

            string fmt = ActualStringFormat;
            if (fmt == null)
            {
                return time.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
            }

            int week = GetWeek(time);
            fmt = fmt.Replace("ww", week.ToString("00"));
            fmt = fmt.Replace("w", week.ToString(CultureInfo.InvariantCulture));
            fmt = string.Concat("{0:", fmt, "}");
            return string.Format(System.Globalization.CultureInfo.CurrentCulture/*ActualCulture*/, fmt, time);
        }

        protected override double CalculateActualInterval(double availableSize, double maxIntervalSize)
        {
            const double Year = 365.25;
            const double Month = 30.5;
            const double Week = 7;
            const double Day = 1.0;
            const double Hour = Day / 24;
            const double Minute = Hour / 60;
            const double Second = Minute / 60;
            const double MilliSecond = Second / 1000;

            double range = Math.Abs(ActualMinimum - ActualMaximum);

            var goodIntervals = new[]
                                    {   MilliSecond, 2 * MilliSecond, 10 * MilliSecond, 100 * MilliSecond,
                                        Second, 2 * Second, 5 * Second, 10 * Second, 30 * Second, Minute, 2 * Minute,
                                        5 * Minute, 10 * Minute, 30 * Minute, Hour, 4 * Hour, 8 * Hour, 12 * Hour, Day,
                                        2 * Day, 5 * Day, Week, 2 * Week, Month, 2 * Month, 3 * Month, 4 * Month,
                                        6 * Month, Year
                                    };

            double interval = goodIntervals[0];

            int maxNumberOfIntervals = Math.Max((int)(availableSize / maxIntervalSize), 2);

            while (true)
            {
                if (range / interval < maxNumberOfIntervals)
                {
                    break;
                }

                double nextInterval = goodIntervals.FirstOrDefault(i => i > interval);
                if (Math.Abs(nextInterval) <= double.Epsilon)
                {
                    nextInterval = interval * 2;
                }

                interval = nextInterval;
            }

            _actualIntervalType = IntervalType;
            _actualMinorIntervalType = MinorIntervalType;

            if (IntervalType == DateTimeIntervalType.Auto)
            {
                _actualIntervalType = DateTimeIntervalType.Milliseconds;

                if (interval >= 1.0 / 24 / 60 / 60)
                {
                    _actualIntervalType = DateTimeIntervalType.Seconds;
                }

                if (interval >= 1.0 / 24 / 60)
                {
                    _actualIntervalType = DateTimeIntervalType.Minutes;
                }

                if (interval >= 1.0 / 24)
                {
                    _actualIntervalType = DateTimeIntervalType.Hours;
                }

                if (interval >= 1)
                {
                    _actualIntervalType = DateTimeIntervalType.Days;
                }

                if (interval >= 30)
                {
                    _actualIntervalType = DateTimeIntervalType.Months;
                }

                if (range >= 365.25)
                {
                    _actualIntervalType = DateTimeIntervalType.Years;
                }
            }

            if (_actualIntervalType == DateTimeIntervalType.Months)
            {
                double monthsRange = range / 30.5;
                interval = CalculateActualInterval(availableSize, maxIntervalSize, monthsRange);
            }

            if (_actualIntervalType == DateTimeIntervalType.Years)
            {
                double yearsRange = range / 365.25;
                interval = CalculateActualInterval(availableSize, maxIntervalSize, yearsRange);
            }

            if (_actualMinorIntervalType == DateTimeIntervalType.Auto)
            {
                _actualMinorIntervalType = _actualIntervalType switch
                {
                    DateTimeIntervalType.Years => DateTimeIntervalType.Months,
                    DateTimeIntervalType.Months => DateTimeIntervalType.Days,
                    DateTimeIntervalType.Weeks => DateTimeIntervalType.Days,
                    DateTimeIntervalType.Days => DateTimeIntervalType.Hours,
                    DateTimeIntervalType.Hours => DateTimeIntervalType.Minutes,
                    _ => DateTimeIntervalType.Days,
                };
            }

            return interval;
        }

        private IList<double> CreateDateTickValues(double min, double max, double step, DateTimeIntervalType intervalType)
        {
            var values = new List/*Collection*/<double>();
            var start = ToDateTime(min);
            if (start.Ticks == 0)
            {
                // Invalid start time
                return values;
            }

            switch (intervalType)
            {
                case DateTimeIntervalType.Weeks:

                    // make sure the first tick is at the 1st day of a week
                    start = start.AddDays(-(int)start.DayOfWeek + (int)FirstDayOfWeek);
                    break;
                case DateTimeIntervalType.Months:

                    // make sure the first tick is at the 1st of a month
                    start = new DateTime(start.Year, start.Month, 1);
                    break;
                case DateTimeIntervalType.Years:

                    // make sure the first tick is at Jan 1st
                    start = new DateTime(start.Year, 1, 1);
                    break;
            }

            // Adds a tick to the end time to make sure the end DateTime is included.
            var end = ToDateTime(max).AddTicks(1);
            if (end.Ticks == 0)
            {
                // Invalid end time
                return values;
            }

            var current = start;
            double eps = step * 1e-3;
            var minDateTime = ToDateTime(min - eps);
            var maxDateTime = ToDateTime(max + eps);

            if (minDateTime.Ticks == 0 || maxDateTime.Ticks == 0)
            {
                // Invalid min/max time
                return values;
            }

            while (current < end)
            {
                if (current > minDateTime && current < maxDateTime)
                {
                    values.Add(ToDouble(current));
                }

                try
                {
                    switch (intervalType)
                    {
                        case DateTimeIntervalType.Months:
                            current = current.AddMonths((int)Math.Ceiling(step));
                            break;
                        case DateTimeIntervalType.Years:
                            current = current.AddYears((int)Math.Ceiling(step));
                            break;
                        default:
                            current = current.AddDays(step);
                            break;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    // AddMonths/AddYears/AddDays can throw an exception
                    // We could test this by comparing to MaxDayValue/MinDayValue, but it is easier to catch the exception...
                    break;
                }
            }

            return values;
        }

        private IList<double> CreateDateTimeTickValues(double min, double max, double interval, DateTimeIntervalType intervalType)
        {
            // If the step size is more than 7 days (e.g. months or years) we use a specialized tick generation method that adds tick values with uneven spacing...
            if (intervalType > DateTimeIntervalType.Days)
            {
                return CreateDateTickValues(min, max, interval, intervalType);
            }

            // For shorter step sizes we use the method from Axis
            return CreateTickValues(min, max, interval);
        }

        private int GetWeek(DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule, FirstDayOfWeek);
        }
    }

    //public class DateTimeAxis : Axis
    //{
    //    private AxisLabelPosition? _dynamicLabel;
    //    private DateTime _epoch0 = DateTime.MinValue;

    //    public DateTimeAxis() 
    //    {
    //        Header = "X";
    //        Position = AxisPosition.Bottom;            
    //        IsDynamicLabelEnable = true;
    //        TimePeriodMode = TimePeriod.Month;
    //        LabelFormatPool = new Dictionary<TimePeriod, string>()
    //            {
    //                { TimePeriod.Hour, @"{0:HH:mm}" },
    //                { TimePeriod.Day, @"{0:HH:mm}" },
    //                { TimePeriod.Week, @"{0:dd/MMM}" },
    //                { TimePeriod.Month, @"{0:dd}" },
    //                { TimePeriod.Year, @"{0:dd/MMM}" },
    //            };
    //        LabelDeltaPool = new Dictionary<TimePeriod, double>()
    //            {
    //                { TimePeriod.Hour, 60.0 * 5 },
    //                { TimePeriod.Day, 3600.0 * 2 },
    //                { TimePeriod.Week, 86400.0 },
    //                { TimePeriod.Month, 86400.0 },
    //                { TimePeriod.Year, 86400.0 * 12 },
    //            };
    //    }

    //    public IDictionary<TimePeriod, string>? LabelFormatPool { get; init; }

    //    public IDictionary<TimePeriod, double>? LabelDeltaPool { get; init; }

    //    public DateTime Epoch0 
    //    { 
    //        get => _epoch0; 
    //        set 
    //        {
    //            _epoch0 = value;
    //            Invalidate();
    //        }
    //    }

    //    public TimePeriod TimePeriodMode { get; set; }

    //    private IList<AxisLabelPosition> CreateLabels()
    //    {
    //        var labs = new List<AxisLabelPosition>();

    //        if ((MaxClientValue - MinClientValue) == 0.0)
    //        {
    //            return labs;
    //        }

    //        if (LabelDeltaPool == null || LabelDeltaPool.ContainsKey(TimePeriodMode) == false || 
    //            LabelFormatPool == null || LabelFormatPool.ContainsKey(TimePeriodMode) == false)
    //        {
    //            return labs;
    //        }

    //        double delta = LabelDeltaPool[TimePeriodMode];

    //        int fl = (int)Math.Floor(MinClientValue / delta);

    //        double value = fl * delta;

    //        if (value < MinClientValue)
    //        {
    //            value += delta;
    //        }

    //        while (value <= MaxClientValue)
    //        {
    //            labs.Add(new AxisLabelPosition()
    //            {
    //                Label = string.Format(CultureInfo.InvariantCulture, LabelFormatPool[TimePeriodMode], Epoch0.AddSeconds(value)),
    //                Value = value
    //            });

    //            value += delta;
    //        }

    //        return labs;
    //    }

    //    private string CreateMinMaxLabel(double value)
    //    {
    //        if ((MaxClientValue - MinClientValue) == 0.0)
    //        {
    //            return string.Empty;
    //        }

    //        return Epoch0.ToString();//AddSeconds(value).ToString(@"dd/MMM/yyyy", CultureInfo.InvariantCulture);
    //    }

    //    public void UpdateDynamicLabelPosition(Point2D point)
    //    {
    //        if (IsVertical() == true)
    //        {
    //            _dynamicLabel = new AxisLabelPosition()
    //            {
    //                Label = string.Format("{0:HH:mm:ss}", Epoch0.AddSeconds(point.Y)),
    //                Value = point.Y
    //            };
    //        }
    //        else
    //        {
    //            _dynamicLabel = new AxisLabelPosition()
    //            {
    //                Label = string.Format("{0:HH:mm:ss}", Epoch0),//.AddSeconds(point.X)),
    //                Value = point.X
    //            };
    //        }

    //        Invalidate();
    //    }

    //   // public override void UpdateFollowLabelPosition(MarkerViewModel marker) { }

    //    public override AxisInfo AxisInfo
    //    {
    //        get
    //        {
    //            var axisInfo = new AxisInfo()
    //            {
    //                Labels = CreateLabels(),
    //                Position = Position,
    //                MinValue = MinClientValue,
    //                MaxValue = MaxClientValue,
    //                MinLabel = CreateMinMaxLabel(MinClientValue),
    //                MaxLabel = CreateMinMaxLabel(MaxClientValue),               
    //                DynamicLabel = (IsDynamicLabelEnable == true) ? _dynamicLabel : null,
    //            };

    //            return axisInfo;
    //        }
    //    }
    //}
}
