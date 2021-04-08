#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class SCCategoryAxis : SCAxisBase
    {
        public SCCategoryAxis()
        {
            base.IsDynamicLabelEnable = false;
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

        public override void UpdateViewport(SCViewport viewport)
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

        public override void UpdateScreen(SCViewport screen)
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
            dirty = true;
            base.UpdateAxis();
        }

        public override void UpdateFollowLabelPosition(ISCTargetMarker marker)
        {
            if (TargetMarkers.ContainsKey(marker.Name) == false)
            {
                TargetMarkers.Add(marker.Name, new Point2D());
            }

            TargetMarkers[marker.Name] = marker.LocalPosition;

            dirty = true;

            base.UpdateAxis();
        }

        //  List<SCAxisLabelPosition> FollowLabels = new List<SCAxisLabelPosition>();
        Dictionary<string, Point2D> TargetMarkers = new Dictionary<string, Point2D>();

        public double MinValue { get; protected set; }
        public double MaxValue { get; protected set; }

        public double MinScreenValue { get; protected set; }
        public double MaxScreenValue { get; protected set; }

        public int MinPixel { get; protected set; }
        public int MaxPixel { get; protected set; }


        void UpdateAxisInfo()
        {
            _axisInfo = new SCAxisInfo()
            {
                Labels = null,
                CoordType = base.CoordType,
                MinValue = MinScreenValue,
                MaxValue = MaxScreenValue,
                FollowLabels = new List<SCAxisLabelPosition>(),
                IsDynamicLabelEnable = false,
                IsFoolowLabelsMode = true,
            };

            foreach (var item in TargetMarkers)
            {
                if (base.CoordType == EAxisCoordType.X)
                {
                    _axisInfo.FollowLabels.Add(new SCAxisLabelPosition()
                    {
                        Value = item.Value.X,
                        Label = item.Key,
                    });
                }
                else if (base.CoordType == EAxisCoordType.Y)
                {
                    _axisInfo.FollowLabels.Add(new SCAxisLabelPosition()
                    {
                        Value = item.Value.Y,
                        Label = item.Key,
                    });
                }
            }
        }

        SCAxisInfo _axisInfo;
        bool dirty = true;
        public override SCAxisInfo AxisInfo
        {
            get
            {
                if (dirty == true)
                {
                    UpdateAxisInfo();
                    dirty = false;
                }

                return _axisInfo;
            }
        }
    }
}
