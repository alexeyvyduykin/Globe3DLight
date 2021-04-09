#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class CategoryAxis : BaseAxis
    {
        private AxisInfo _axisInfo;
        private bool _dirty = true;
        //  List<SCAxisLabelPosition> FollowLabels = new List<SCAxisLabelPosition>();
        private readonly Dictionary<string, Point2D> _targetMarkers;

        public CategoryAxis()
        {
            _targetMarkers = new Dictionary<string, Point2D>();
            IsDynamicLabelEnable = false;
        }

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

            //      base.AreaMap = window;
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
            _dirty = true;
            base.UpdateAxis();
        }

        public override void UpdateFollowLabelPosition(ITargetMarker marker)
        {
            if (_targetMarkers.ContainsKey(marker.Name) == false)
            {
                _targetMarkers.Add(marker.Name, new Point2D());
            }

            _targetMarkers[marker.Name] = marker.LocalPosition;

            _dirty = true;

            base.UpdateAxis();
        }

        public double MinValue { get; protected set; }

        public double MaxValue { get; protected set; }

        public double MinScreenValue { get; protected set; }

        public double MaxScreenValue { get; protected set; }

        public int MinPixel { get; protected set; }

        public int MaxPixel { get; protected set; }

        private void UpdateAxisInfo()
        {
            _axisInfo = new AxisInfo()
            {
                Labels = null,
                CoordType = base.CoordType,
                MinValue = MinScreenValue,
                MaxValue = MaxScreenValue,
                FollowLabels = new List<AxisLabelPosition>(),
                IsDynamicLabelEnable = false,
                IsFollowLabelsMode = true,
            };

            foreach (var item in _targetMarkers)
            {
                if (base.CoordType == EAxisCoordType.X)
                {
                    _axisInfo.FollowLabels.Add(new AxisLabelPosition()
                    {
                        Value = item.Value.X,
                        Label = item.Key,
                    });
                }
                else if (base.CoordType == EAxisCoordType.Y)
                {
                    _axisInfo.FollowLabels.Add(new AxisLabelPosition()
                    {
                        Value = item.Value.Y,
                        Label = item.Key,
                    });
                }
            }
        }

        public override AxisInfo AxisInfo
        {
            get
            {
                if (_dirty == true)
                {
                    UpdateAxisInfo();
                    _dirty = false;
                }

                return _axisInfo;
            }
        }
    }
}
