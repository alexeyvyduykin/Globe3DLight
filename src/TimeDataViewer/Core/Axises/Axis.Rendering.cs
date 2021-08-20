using System;
using System.Collections.Generic;
using System.Linq;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public enum HorizontalAlignment
    {
        Left = -1,
        Center = 0,
        Right = 1
    }

    public enum VerticalAlignment
    {
        Top = -1,
        Middle = 0,
        Bottom = 1
    }

    public partial class Axis
    {
        private IList<double> _majorLabelValues;
        private IList<double> _majorTickValues;
        private IList<double> _minorTickValues;

        private List<ScreenPoint> _extraSegments;
        private List<ScreenPoint> _minorSegments;
        private List<ScreenPoint> _minorTickSegments;
        private List<ScreenPoint> _majorSegments;
        private List<ScreenPoint> _majorTickSegments;
        private List<(ScreenPoint, string, HorizontalAlignment, VerticalAlignment)> _labels;

        public List<ScreenPoint> MyMinorSegments => _minorSegments;
        public List<ScreenPoint> MyMajorSegments => _majorSegments;
        public List<ScreenPoint> MyMinorTickSegments => _minorTickSegments;
        public List<ScreenPoint> MyMajorTickSegments => _majorTickSegments;

        public List<(ScreenPoint, string, HorizontalAlignment, VerticalAlignment)> MyLabels => _labels;

        public EventHandler MyRender;

        public void MyOnRender(PlotModel plot)
        {
            GetTickValues(out _majorLabelValues, out _majorTickValues, out _minorTickValues);

            RenderPass(plot, 0);
            RenderPass(plot, 1);

            MyRender?.Invoke(this, EventArgs.Empty);
        }

        private void RenderPass(PlotModel plot, int pass)
        {
            bool drawAxisLine = true;
            double totalShift = AxisDistance;

            // store properties locally for performance
            //double plotAreaLeft = plot.PlotArea.Left;
            //double plotAreaRight = plot.PlotArea.Right;
            //double plotAreaTop = plot.PlotArea.Top;
            //double plotAreaBottom = plot.PlotArea.Bottom;

            // Axis position (x or y screen coordinate)
            double axisPosition = 0;

            switch (Position)
            {
                case AxisPosition.Left:
                    axisPosition = plot.PlotMarginLeft - totalShift;
                    break;
                case AxisPosition.Right:
                    throw new Exception();
                case AxisPosition.Top:
                    axisPosition = plot.PlotMarginTop - totalShift;
                    break;
                case AxisPosition.Bottom:
                    axisPosition = totalShift;
                    break;
            }

            if (pass == 0)
            {
                RenderMinorItems(plot, axisPosition);
            }

            if (pass == 1)
            {
                RenderMajorItems(plot, axisPosition, drawAxisLine);
            }
        }

        // Interpolates linearly between two values.
        protected static double Lerp(double x0, double x1, double f)
        {
            // http://en.wikipedia.org/wiki/Linear_interpolation
            return (x0 * (1 - f)) + (x1 * f);
        }

        // Snaps v to value if it is within the specified distance.
        protected static void SnapTo(double target, ref double v, double eps = 0.5)
        {
            if (v > target - eps && v < target + eps)
            {
                v = target;
            }
        }

        protected virtual void RenderMajorItems(PlotModel plot, double axisPosition, bool drawAxisLine)
        {
            double eps = ActualMinorStep * 1e-3;

            double actualMinimum = ActualMinimum;
            double actualMaximum = ActualMaximum;

            double plotAreaLeft = plot.PlotArea.Left;
            double plotAreaRight = plot.PlotArea.Right;
            double plotAreaTop = plot.PlotArea.Top;
            double plotAreaBottom = plot.PlotArea.Bottom;
            bool isHorizontal = IsHorizontal();
            bool cropGridlines = CropGridlines;

            _majorSegments = new List<ScreenPoint>();
            _majorTickSegments = new List<ScreenPoint>();
            GetTickPositions(MajorTickSize, Position, out double a0, out double a1);

            var perpendicularAxis = IsHorizontal() ? plot.DefaultYAxis : plot.DefaultXAxis;
            var dontRenderZero = false;

            List<Axis> perpAxises = null;
            if (cropGridlines)
            {
                if (isHorizontal)
                {
                    perpAxises = plot.Axises.Where(x => x.IsXyAxis() && x.IsVertical()).ToList();
                }
                else
                {
                    perpAxises = plot.Axises.Where(x => x.IsXyAxis() && x.IsHorizontal()).ToList();
                }
            }

            foreach (double value in _majorTickValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
                {
                    continue;
                }

                if (dontRenderZero && Math.Abs(value) < eps)
                {
                    continue;
                }

                double transformedValue = Transform(value);
                if (isHorizontal)
                {
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                {
                    AddSegments(
                        _majorSegments,
                        perpAxises,
                        isHorizontal,
                        cropGridlines,
                        transformedValue,
                        plotAreaLeft,
                        plotAreaRight,
                        plotAreaTop,
                        plotAreaBottom);
                }

                if (MajorTickSize > 0)
                {
                    if (isHorizontal)
                    {
                        _majorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a0));
                        _majorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a1));
                    }
                    else
                    {
                        _majorTickSegments.Add(new ScreenPoint(axisPosition + a0, transformedValue));
                        _majorTickSegments.Add(new ScreenPoint(axisPosition + a1, transformedValue));
                    }
                }
            }

            _labels = new List<(ScreenPoint, string, HorizontalAlignment, VerticalAlignment)>();

            // Render the axis labels (numbers or category names)
            foreach (double value in _majorLabelValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
                {
                    continue;
                }

                if (dontRenderZero && Math.Abs(value) < eps)
                {
                    continue;
                }

                double transformedValue = Transform(value);
                if (isHorizontal)
                {
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                var pt = new ScreenPoint();
                var ha = HorizontalAlignment.Right;
                var va = VerticalAlignment.Middle;
                switch (Position)
                {
                    case AxisPosition.Left:
                        pt = new ScreenPoint(axisPosition + a1 - AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(-90, out ha, out va);

                        break;
                    case AxisPosition.Right:
                        pt = new ScreenPoint(axisPosition + a1 + AxisTickToLabelDistance, transformedValue);
                        GetRotatedAlignments(90, out ha, out va);

                        break;
                    case AxisPosition.Top:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 - AxisTickToLabelDistance);
                        GetRotatedAlignments(0, out ha, out va);

                        break;
                    case AxisPosition.Bottom:
                        pt = new ScreenPoint(transformedValue, axisPosition + a1 + AxisTickToLabelDistance);
                        GetRotatedAlignments(-180, out ha, out va);

                        break;
                }

                string text = FormatValue(value);

                _labels.Add((pt, text, ha, va));
            }

            // Draw extra grid lines
            if (ExtraGridlines != null)
            {
                _extraSegments = new List<ScreenPoint>();

                foreach (double value in ExtraGridlines)
                {
                    if (!IsWithin(value, actualMinimum, actualMaximum))
                    {
                        continue;
                    }

                    double transformedValue = Transform(value);
                    AddSegments(_extraSegments, perpAxises, isHorizontal, cropGridlines, transformedValue, plotAreaLeft, plotAreaRight, plotAreaTop, plotAreaBottom);
                }
            }
        }

        protected virtual void RenderMinorItems(PlotModel plot, double axisPosition)
        {
            double eps = ActualMinorStep * 1e-3;
            double actualMinimum = ActualMinimum;
            double actualMaximum = ActualMaximum;

            double plotAreaLeft = plot.PlotArea.Left;
            double plotAreaRight = plot.PlotArea.Right;
            double plotAreaTop = plot.PlotArea.Top;
            double plotAreaBottom = plot.PlotArea.Bottom;
            bool cropGridlines = CropGridlines;
            bool isHorizontal = IsHorizontal();

            double a0;
            double a1;
            _minorSegments = new List<ScreenPoint>();
            _minorTickSegments = new List<ScreenPoint>();

            List<Axis> perpAxises = null;
            if (cropGridlines)
            {
                if (isHorizontal)
                {
                    perpAxises = plot.Axises.Where(x => x.IsXyAxis() && x.IsVertical()).ToList();
                }
                else
                {
                    perpAxises = plot.Axises.Where(x => x.IsXyAxis() && x.IsHorizontal()).ToList();
                }
            }

            GetTickPositions(MinorTickSize, Position, out a0, out a1);

            foreach (double value in _minorTickValues)
            {
                if (value < actualMinimum - eps || value > actualMaximum + eps)
                {
                    continue;
                }

                if (_majorTickValues.Contains(value))
                {
                    continue;
                }

                double transformedValue = Transform(value);

                if (isHorizontal)
                {
                    SnapTo(plotAreaLeft, ref transformedValue);
                    SnapTo(plotAreaRight, ref transformedValue);
                }
                else
                {
                    SnapTo(plotAreaTop, ref transformedValue);
                    SnapTo(plotAreaBottom, ref transformedValue);
                }

                // Draw the minor grid line                
                {
                    AddSegments(_minorSegments, perpAxises, isHorizontal, cropGridlines, transformedValue, plotAreaLeft, plotAreaRight, plotAreaTop, plotAreaBottom);
                }

                // Draw the minor tick
                if (MinorTickSize > 0)
                {
                    if (isHorizontal)
                    {
                        _minorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a0));
                        _minorTickSegments.Add(new ScreenPoint(transformedValue, axisPosition + a1));
                    }
                    else
                    {
                        _minorTickSegments.Add(new ScreenPoint(axisPosition + a0, transformedValue));
                        _minorTickSegments.Add(new ScreenPoint(axisPosition + a1, transformedValue));
                    }
                }
            }
        }

        private void AddSegments(
            List<ScreenPoint> segments,
            List<Axis> perpAxises,
            bool isHorizontal,
            bool cropGridlines,
            double transformedValue,
            double plotAreaLeft,
            double plotAreaRight,
            double plotAreaTop,
            double plotAreaBottom)
        {
            if (isHorizontal)
            {
                if (!cropGridlines)
                {
                    segments.Add(new ScreenPoint(transformedValue, plotAreaTop));
                    segments.Add(new ScreenPoint(transformedValue, plotAreaBottom));
                }
                else
                {
                    foreach (var perpAxis in perpAxises)
                    {
                        segments.Add(new ScreenPoint(transformedValue, perpAxis.Transform(perpAxis.ActualMinimum)));
                        segments.Add(new ScreenPoint(transformedValue, perpAxis.Transform(perpAxis.ActualMaximum)));
                    }
                }
            }
            else
            {
                if (!cropGridlines)
                {
                    segments.Add(new ScreenPoint(plotAreaLeft, transformedValue));
                    segments.Add(new ScreenPoint(plotAreaRight, transformedValue));
                }
                else
                {
                    foreach (var perpAxis in perpAxises)
                    {
                        segments.Add(new ScreenPoint(perpAxis.Transform(perpAxis.ActualMinimum), transformedValue));
                        segments.Add(new ScreenPoint(perpAxis.Transform(perpAxis.ActualMaximum), transformedValue));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the alignments given the specified rotation angle.
        /// </summary>
        /// <param name="boxAngle">The angle of a box to rotate (usually it is label angle).</param>
        /// <param name="axisAngle">
        /// The axis angle, the original angle belongs to. The Top axis should have 0, next angles are computed clockwise. 
        /// The angle should be in [-180, 180). (T, R, B, L) is (0, 90, -180, -90). 
        /// </param>
        /// <param name="ha">Horizontal alignment.</param>
        /// <param name="va">Vertical alignment.</param>
        /// <remarks>
        /// This method is supposed to compute the alignment of the labels that are put near axis. 
        /// Because such labels can have different angles, and the axis can have different angles as well,
        /// computing the alignment is not straightforward.
        /// </remarks>       
        private void GetRotatedAlignments(double axisAngle, out HorizontalAlignment ha, out VerticalAlignment va)
        {
            const double AngleTolerance = 10.0;

            // The axis angle if it would have been turned on 180 and leave it in [-180, 180)
            double flippedAxisAngle = ((axisAngle + 360.0) % 360.0) - 180.0;

            // When the box (assuming the axis and box have the same angle) box starts to turn clockwise near the axis
            // It leans on the right until it gets to 180 rotation, when it is started to lean on the left.
            // In real computation we need to compute this in relation with axisAngle
            // So if axisAngle <= boxAngle < (axisAngle + 180), we align Right, else - left.
            // The check looks inverted because flippedAxisAngle has the opposite sign.
            ha = 0.0 >= Math.Min(axisAngle, flippedAxisAngle) && 0.0 < Math.Max(axisAngle, flippedAxisAngle) ? HorizontalAlignment.Left : HorizontalAlignment.Right;

            // If axisAngle was < 0, we need to shift the previous computation on 180.
            if (axisAngle < 0)
            {
                ha = (HorizontalAlignment)((int)ha * -1);
            }

            va = VerticalAlignment.Middle;

            // If the angle almost the same as axisAngle (or axisAngle + 180) - set horizontal alignment to Center
            if (Math.Abs(0.0 - flippedAxisAngle) < AngleTolerance || Math.Abs(0.0 - axisAngle) < AngleTolerance)
            {
                ha = HorizontalAlignment.Center;
            }

            // And vertical alignment according to whether it is near to axisAngle or flippedAxisAngle
            if (Math.Abs(0.0 - axisAngle) < AngleTolerance)
            {
                va = VerticalAlignment.Bottom;
            }

            if (Math.Abs(0.0 - flippedAxisAngle) < AngleTolerance)
            {
                va = VerticalAlignment.Top;
            }
        }

        private bool IsWithin(double d, double min, double max)
        {
            if (d < min)
            {
                return false;
            }

            if (d > max)
            {
                return false;
            }

            return true;
        }

        protected virtual void GetTickPositions(double tickSize, AxisPosition position, out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;
            bool isTopOrLeft = position == AxisPosition.Top || position == AxisPosition.Left;
            double sign = isTopOrLeft ? -1 : 1;

            x1 = tickSize * sign; // Outside
        }
    }
}
