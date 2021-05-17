#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using TimeDataViewer.Spatial;
using System.Globalization;
using TimeDataViewer.ViewModels;

namespace TimeDataViewer.Core
{
    public enum TimePeriod
    {
        Hour,
        Day,
        Week,
        Month,
        Year,
    }

    public class TimeAxis : BaseAxis, ITimeAxis
    {
        private AxisLabelPosition? _dynamicLabel;
        private DateTime _epoch0 = DateTime.MinValue;
        
        public TimeAxis() 
        {
            
        }

        public IDictionary<TimePeriod, string>? LabelFormatPool { get; init; }

        public IDictionary<TimePeriod, double>? LabelDeltaPool { get; init; }

        public DateTime Epoch0 
        { 
            get => _epoch0; 
            set 
            {
                _epoch0 = value;
                Invalidate();
            }
        }

        public TimePeriod TimePeriodMode { get; set; }

        public override double FromAbsoluteToLocal(int pixel)
        {
            double value = (MaxValue - MinValue) * pixel / (MaxPixel - MinPixel);

            if (HasInversion == true)
            {
                value = (MaxValue - MinValue) - value;
            }
        
            return Math.Clamp(MinValue + value, MinValue, MaxValue);
        }

        public override int FromLocalToAbsolute(double value)
        {
            int pixel = (int)((value - MinValue) * (MaxPixel - MinPixel) / (MaxValue - MinValue));

            if (HasInversion == true)
            {
                pixel = (MaxPixel - MinPixel) - pixel;
            }
        
            return Math.Clamp(/*MinPixel +*/ pixel, MinPixel, MaxPixel);
        }

        public override void UpdateViewport(RectD viewport)
        {
            switch (Type)
            {
                case AxisType.X:
                    MinValue = viewport.Left;
                    MaxValue = viewport.Right;
                    break;
                case AxisType.Y:
                    MinValue = viewport.Bottom;
                    MaxValue = viewport.Top;
                    break;
                default:
                    break;
            }

            Invalidate();
        }

        public override void UpdateClientViewport(RectD clientViewport)
        {
            switch (Type)
            {
                case AxisType.X:
                    MinClientValue = clientViewport.Left;
                    MaxClientValue = clientViewport.Right;
                    break;
                case AxisType.Y:
                    MinClientValue = clientViewport.Bottom;
                    MaxClientValue = clientViewport.Top;
                    break;
                default:
                    break;
            }

            Invalidate();
        }

        public override void UpdateWindow(RectI window)
        {
            switch (Type)
            {
                case AxisType.X:
                    MinPixel = 0;
                    MaxPixel = window.Width;
                    break;
                case AxisType.Y:
                    MinPixel = 0;
                    MaxPixel = window.Height;
                    break;
                default:
                    break;
            }

            Invalidate();
        }
   
        private IList<AxisLabelPosition> CreateLabels()
        {
            var labs = new List<AxisLabelPosition>();

            if ((MaxClientValue - MinClientValue) == 0.0)
            {
                return labs;
            }

            if (LabelDeltaPool == null || LabelDeltaPool.ContainsKey(TimePeriodMode) == false || 
                LabelFormatPool == null || LabelFormatPool.ContainsKey(TimePeriodMode) == false)
            {
                return labs;
            }

            double delta = LabelDeltaPool[TimePeriodMode];

            int fl = (int)Math.Floor(MinClientValue / delta);

            double value = fl * delta;

            if (value < MinClientValue)
            {
                value += delta;
            }
            
            while (value <= MaxClientValue)
            {
                labs.Add(new AxisLabelPosition()
                {
                    Label = string.Format(CultureInfo.InvariantCulture, LabelFormatPool[TimePeriodMode], Epoch0.AddSeconds(value)),
                    Value = value
                });

                value += delta;
            }

            return labs;
        }

        private string CreateMinMaxLabel(double value)
        {
            if ((MaxClientValue - MinClientValue) == 0.0)
            {
                return string.Empty;
            }

            return Epoch0.AddSeconds(value).ToString(@"dd/MMM/yyyy", CultureInfo.InvariantCulture);
        }

        public void UpdateDynamicLabelPosition(Point2D point)
        {
            if (Type == AxisType.Y)
            {
                _dynamicLabel = new AxisLabelPosition()
                {
                    Label = string.Format("{0:HH:mm:ss}", Epoch0.AddSeconds(point.Y)),
                    Value = point.Y
                };
            }
            else if (Type == AxisType.X)
            {
                _dynamicLabel = new AxisLabelPosition()
                {
                    Label = string.Format("{0:HH:mm:ss}", Epoch0.AddSeconds(point.X)),
                    Value = point.X
                };
            }

            Invalidate();
        }

       // public override void UpdateFollowLabelPosition(MarkerViewModel marker) { }
  
        public override AxisInfo AxisInfo
        {
            get
            {
                var axisInfo = new AxisInfo()
                {
                    Labels = CreateLabels(),
                    Type = Type,
                    MinValue = MinClientValue,
                    MaxValue = MaxClientValue,
                    MinLabel = CreateMinMaxLabel(MinClientValue),
                    MaxLabel = CreateMinMaxLabel(MaxClientValue),               
                    DynamicLabel = (IsDynamicLabelEnable == true) ? _dynamicLabel : null,
                };

                return axisInfo;
            }
        }
    }
}
