#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public enum TimePeriod
    {
        Hour,
        Day,
        Week,
        Month,
        Year,
    }

    public class TimeAxis : BaseRangeAxis
    {
        private AxisLabelPosition _dynamicLabel;
        private readonly Dictionary<TimePeriod, string> _labelFormatPool = new()
        {
            { TimePeriod.Hour, @"{0:HH:mm}" },
            { TimePeriod.Day, @"{0:HH:mm}" },
            { TimePeriod.Week, @"{0:dd/MMM}" },
            { TimePeriod.Month, @"{0:dd}" },
            { TimePeriod.Year, @"{0:dd/MMM}" },
        };
        private readonly Dictionary<TimePeriod, double> _labelDeltaPool = new()
        {
            { TimePeriod.Hour, 60.0 * 5 },
            { TimePeriod.Day, 3600.0 * 2 },
            { TimePeriod.Week, 86400.0 },
            { TimePeriod.Month, 86400.0 },
            { TimePeriod.Year, 86400.0 * 12 },
        };
        
        public TimeAxis() { }

        public DateTime Epoch0 { get; set; } = new DateTime(2000, 1, 1);

        public override double FromAbsoluteToLocal(int pixel)
        {
            double value = (MaxValue - MinValue) * pixel / (MaxPixel - MinPixel);

            if (IsInversed == true)
            {
                value = (MaxValue - MinValue) - value;
            }

            var res = MinValue + value;

            res = Clip(res, MinValue, MaxValue);

            return res;
        }

        public override int FromLocalToAbsolute(double value)
        {
            int pixel = (int)((value - MinValue) * (MaxPixel - MinPixel) / (MaxValue - MinValue));

            if (IsInversed == true)
            {
                pixel = (MaxPixel - MinPixel) - pixel;
            }

            var res = /*MinPixel +*/ pixel;

            res = Clip(res, MinPixel, MaxPixel);

            return res;
        }

        public override void UpdateViewport(RectD viewport)
        {
            switch (base.CoordType)
            {
                case EAxisCoordType.X:
                    MinValue = viewport.Left;
                    MaxValue = viewport.Right;
                    break;
                case EAxisCoordType.Y:
                    MinValue = viewport.Bottom;
                    MaxValue = viewport.Top;
                    break;
                default:
                    break;
            }

            //  CreateLabelPool();

            base.UpdateAxis();
        }

        public override void UpdateScreen(RectD screen)
        {
            switch (base.CoordType)
            {
                case EAxisCoordType.X:
                    MinScreenValue = screen.Left;
                    MaxScreenValue = screen.Right;
                    break;
                case EAxisCoordType.Y:
                    MinScreenValue = screen.Bottom;
                    MaxScreenValue = screen.Top;
                    break;
                default:
                    break;
            }

            base.UpdateAxis();
        }

        public override void UpdateWindow(RectI window)
        {
            switch (base.CoordType)
            {
                case EAxisCoordType.X:
                    MinPixel = 0;// window.Left;
                    MaxPixel = window.Width;// window.Right;
                    break;
                case EAxisCoordType.Y:
                    MinPixel = 0;// window.Bottom;
                    MaxPixel = window.Height;// window.Top;
                    break;
                default:
                    break;
            }

            base.UpdateAxis();
        }

        public double MinValue { get; protected set; }

        public double MaxValue { get; protected set; }

        public double MinScreenValue { get; protected set; }

        public double MaxScreenValue { get; protected set; }

        public int MinPixel { get; protected set; }

        public int MaxPixel { get; protected set; }

        public TimePeriod TimePeriodMode { get; set; }
               
        private List<AxisLabelPosition> CreateLabels()
        {
            var labs = new List<AxisLabelPosition>();

            if ((MaxScreenValue - MinScreenValue) == 0.0)
                return labs;

            if (_labelDeltaPool.ContainsKey(TimePeriodMode) == false)
                return labs;

            double delta = _labelDeltaPool[TimePeriodMode];

            int fl = (int)Math.Floor(MinScreenValue / delta);

            double value = fl * delta;

            if (value < MinScreenValue)
                value += delta;

            while (value <= MaxScreenValue)
            {
                labs.Add(new AxisLabelPosition()
                {
                    Label = string.Format(_labelFormatPool[TimePeriodMode], Epoch0.AddSeconds(value)),
                    Value = value
                });
                value += delta;
            }

            return labs;
        }

        private string CreateMinMaxLabel(double value)
        {
            if ((MaxScreenValue - MinScreenValue) == 0.0)
                return string.Empty;

            return Epoch0.AddSeconds(value).ToString(@"dd/MMM/yyyy");
        }

        //List<SCAxisLabelPosition> CreateLabels()
        //{
        //    var labs = new List<SCAxisLabelPosition>();

        //    if ((MaxScreenValue - MinScreenValue) == 0.0)
        //        return labs;

        //    switch (TimePeriodMode)
        //    {
        //        case TimePeriod.Hour: // per 5 minute
        //            {
        //                DateTime dt0 = Epoch0.AddSeconds(MinScreenValue);

        //                var minutes = dt0.Minute;// + 1;

        //                while (minutes % 5 != 0)
        //                {
        //                    minutes = minutes + 1;
        //                }

        //                var hours = (dt0 - Epoch0).Hours;

        //                double value0 = hours * 3600.0 + (minutes) * 60.0;

        //                while (value0 <= MaxScreenValue)
        //                {
        //                    labs.Add(new SCAxisLabelPosition()
        //                    {
        //                        Label = string.Format("{0:HH:mm}", Epoch0.AddSeconds(value0)),
        //                        Value = value0
        //                    });

        //                    value0 += 5 * 60.0;
        //                }
        //            }
        //            break;
        //        case TimePeriod.Day: // per 2 hours
        //            {
        //                DateTime dt0 = Epoch0.AddSeconds(MinScreenValue);

        //                int hours = dt0.Hour;

        //                while (hours % 2 != 0)
        //                {
        //                    hours = hours + 1;
        //                }

        //                double value0 = hours * 3600.0;

        //                while (value0 <= MaxScreenValue)
        //                {
        //                    labs.Add(new SCAxisLabelPosition()
        //                    {
        //                        Value = value0,
        //                        Label = string.Format("{0:HH:mm}", Epoch0.AddSeconds(value0))
        //                    });

        //                    value0 += 2 * 3600.0;
        //                }
        //            }
        //            break;
        //        case TimePeriod.Week: // per 1 days
        //            {
        //                DateTime dt0 = Epoch0.AddSeconds(MinScreenValue);
        //                int days = dt0.Day - 1;

        //                   if((dt0 - Epoch0).TotalSeconds != 0)
        //                   {
        //                       days = days + 1;
        //                   }

        //                double value0 = days * 86400.0;

        //                while (value0 <= MaxScreenValue)
        //                {
        //                    labs.Add(new SCAxisLabelPosition()
        //                    {
        //                        Value = value0,
        //                        Label = string.Format("{0:dd/MMM/yyyy}", Epoch0.AddSeconds(value0))
        //                    });

        //                    value0 += 86400.0;
        //                }
        //            }
        //            break;
        //        case TimePeriod.Month:
        //            break;
        //        case TimePeriod.Year:
        //            break;
        //        default:
        //            break;
        //    }

        //    return labs;
        //}

        public override void UpdateDynamicLabelPosition(Point2D point)
        {
            if (base.CoordType == EAxisCoordType.Y)
            {
                _dynamicLabel = new AxisLabelPosition()
                {
                    Label = string.Format("{0:HH:mm:ss}", Epoch0.AddSeconds(point.Y)),
                    Value = point.Y
                };
            }
            else if (base.CoordType == EAxisCoordType.X)
            {
                _dynamicLabel = new AxisLabelPosition()
                {
                    Label = string.Format("{0:HH:mm:ss}", Epoch0.AddSeconds(point.X)),
                    Value = point.X
                };
            }

            base.UpdateAxis();
        }

        public override void UpdateFollowLabelPosition(ITargetMarker marker) { }
  
        public override AxisInfo AxisInfo
        {
            get
            {
                var axisInfo = new AxisInfo()
                {
                    Labels = CreateLabels(),
                    CoordType = base.CoordType,
                    MinValue = MinScreenValue,
                    MaxValue = MaxScreenValue,
                    MinLabel = CreateMinMaxLabel(MinScreenValue),
                    MaxLabel = CreateMinMaxLabel(MaxScreenValue),
                };

                if (base.IsDynamicLabelEnable == true)
                {
                    axisInfo.IsDynamicLabelEnable = true;
                    axisInfo.DynamicLabel = _dynamicLabel;// new SCAxisLabelPosition() { Label = "!dfgfd!", Value = 43200.0 };
                }

                return axisInfo;
            }
        }
    }
}
