#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Globe3DLight.Spatial;

namespace Globe3DLight.ViewModels.TimeDataViewer
{
    public class SCNumericalAxis : SCRangeAxisBase
    {
        public SCNumericalAxis() { }

        public override double FromAbsoluteToLocal(int pixel)
        {
            // int widthInPixel = (int)(Bounds.Width * (zoom + 1.0));
            // int heightInPixel = (int)(MaximumY - MinimumY);

            //     pixel = Clip(pixel, MinPixel, MaxPixel);

            double value = (MaxValue - MinValue) * pixel / (MaxPixel - MinPixel);

            if (IsInversed == true)
            {
                value = (MaxValue - MinValue) - value;
            }

            //  double py = MinimumY + ((MaximumY - MinimumY) - y);

            var res = MinValue + value;

            res = Clip(res, MinValue, MaxValue);

            return res;
        }

        public override int FromLocalToAbsolute(double value)
        {
            //  int widthInPixel = (int)(Bounds.Width * (zoom + 1.0));

            //         value = Clip(value, MinValue, MaxValue);
            //    py = Clip(py, MinimumY, MaximumY);

            int pixel = (int)((value - MinValue) * (MaxPixel - MinPixel) / (MaxValue - MinValue));

            //    int x = (int)(px * widthInPixel / (MaximumX - MinimumX));
            //    int y = (int)((/*(MaximumY - MinimumY) -*/ py));

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
                    //       if (IsInversed == false)
                    //       {
                    MinPixel = 0;// window.Left;
                    MaxPixel = window.Width;// window.Right;
                                            //       }
                                            //       else
                                            //       {
                                            //           MinPixel = window.Right;
                                            //           MaxPixel = window.Left;
                                            //       }
                    break;
                case EAxisCoordType.Y:
                    //     if (IsInversed == false)
                    //     {
                    MinPixel = 0;// window.Bottom;
                    MaxPixel = window.Height;// window.Top;
                                             //     }
                                             //     else
                                             //     {
                                             //         MinPixel = window.Top;
                                             //         MaxPixel = window.Bottom;
                                             //     }
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
                    //               if (IsInversed == false)
                    //               {
                    MinValue = viewport.Left;
                    MaxValue = viewport.Right;
                    //               }
                    //               else
                    //               {
                    //                   MinValue = viewport.Right;
                    //                   MaxValue = viewport.Left;
                    //               }
                    break;
                case EAxisCoordType.Y:
                    //              if (IsInversed == false)
                    //              {
                    MinValue = viewport.Bottom;
                    MaxValue = viewport.Top;
                    //              }
                    //              else
                    //              {
                    //                  MinValue = viewport.Top;
                    //                  MaxValue = viewport.Bottom;
                    //              }

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

            base.UpdateAxis();
        }

        public override void UpdateDynamicLabelPosition(Point2D point)
        {
            if (base.CoordType == EAxisCoordType.Y)
            {
                DynamicLabel = new SCAxisLabelPosition()
                {
                    Label = string.Format("{0:F2}", point.Y),
                    Value = point.Y
                };
            }
            else if (base.CoordType == EAxisCoordType.X)
            {
                DynamicLabel = new SCAxisLabelPosition()
                {
                    Label = string.Format("{0:F2}", point.X),
                    Value = point.X
                };
            }

            base.UpdateAxis();
        }

        public override void UpdateFollowLabelPosition(ISCTargetMarker marker)
        {
        }

        SCAxisLabelPosition DynamicLabel;
        List<SCAxisLabelPosition> FollowLabels = new List<SCAxisLabelPosition>();

        public double MinValue { get; protected set; }
        public double MaxValue { get; protected set; }

        public double MinScreenValue { get; protected set; }
        public double MaxScreenValue { get; protected set; }

        public int MinPixel { get; protected set; }
        public int MaxPixel { get; protected set; }

        //   int LengthPixel { get { return MaxPixel - MinPixel; } }

        public override SCAxisInfo AxisInfo
        {
            get
            {
                SCAxisInfo axisInfo = new SCAxisInfo()
                {
                    Labels = new List<SCAxisLabelPosition>(),
                    CoordType = base.CoordType,
                    MinValue = MinScreenValue,
                    MaxValue = MaxScreenValue,
                    FollowLabels = new List<SCAxisLabelPosition>()
                };

                int count = 10;

                double step = (MaxScreenValue - MinScreenValue) / count;// 3600.0;

                if (step == 0.0)
                    return axisInfo;

                if (this.FollowLabels.Count == 0)
                {
                    for (int i = 0; i < count + 1; i++)
                    {
                        axisInfo.Labels.Add(new SCAxisLabelPosition()
                        {
                            Label = string.Format("{0:F2}", MinScreenValue + i * step),
                            Value = MinScreenValue + i * step
                        });
                    }
                }
                else
                {
                    axisInfo.IsFoolowLabelsMode = true;

                    foreach (var item in FollowLabels)
                    {
                        axisInfo.FollowLabels.Add(item);
                    }
                }
                //for (int i = 0; i < count + 1; i++)
                //{
                //    axisInfo.Labels.Add(string.Format("{0:F2}", MinScreenValue + i * step));
                //}

                if (base.IsDynamicLabelEnable == true)
                {
                    axisInfo.IsDynamicLabelEnable = true;
                    axisInfo.DynamicLabel = DynamicLabel;
                }

                return axisInfo;
            }
        }
    }
}
