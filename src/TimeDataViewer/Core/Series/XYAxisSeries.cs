using System;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public abstract class XYAxisSeries : ItemsSeries
    {
        protected XYAxisSeries()
        {

        }

        // Gets or sets the maximum x-coordinate of the dataset.
        public double MaxX { get; protected set; }

        // Gets or sets the maximum y-coordinate of the dataset.
        public double MaxY { get; protected set; }

        // Gets or sets the minimum x-coordinate of the dataset.
        public double MinX { get; protected set; }

        // Gets or sets the minimum y-coordinate of the dataset.
        public double MinY { get; protected set; }

        public Axis? XAxis { get; private set; }

        public string? XAxisKey { get; set; }

        public Axis? YAxis { get; private set; }

        public string? YAxisKey { get; set; }

        // Gets the rectangle the series uses on the screen (screen coordinates).
        public OxyRect GetScreenRectangle()
        {
            return GetClippingRect();
        }

        /// <summary>
        /// Transforms from a screen point to a data point by the axes of this series.
        /// </summary>
        /// <param name="p">The screen point.</param>
        /// <returns>A data point.</returns>
        public DataPoint InverseTransform(ScreenPoint p)
        {
            return XAxis.InverseTransform(p.X, p.Y, YAxis);
        }

        /// <summary>
        /// Transforms the specified coordinates to a screen point by the axes of this series.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>A screen point.</returns>
        public ScreenPoint Transform(double x, double y)
        {
            return XAxis.Transform(x, y, YAxis);
        }

        /// <summary>
        /// Transforms the specified data point to a screen point by the axes of this series.
        /// </summary>
        /// <param name="p">The point.</param>
        /// <returns>A screen point.</returns>
        public ScreenPoint Transform(DataPoint p)
        {
            return XAxis.Transform(p.X, p.Y, YAxis);
        }

        /// <summary>
        /// Check if this data series requires X/Y axes. (e.g. Pie series do not require axes)
        /// </summary>
        /// <returns>The are axes required.</returns>
        protected internal override bool AreAxesRequired()
        {
            return true;
        }

        /// <summary>
        /// Ensures that the axes of the series is defined.
        /// </summary>
        protected internal override void EnsureAxes()
        {
            XAxis = XAxisKey != null ?
                         PlotModel.GetAxis(XAxisKey) :
                         PlotModel.DefaultXAxis;

            YAxis = YAxisKey != null ?
                         PlotModel.GetAxis(YAxisKey) :
                         PlotModel.DefaultYAxis;
        }

        /// <summary>
        /// Check if the data series is using the specified axis.
        /// </summary>
        /// <param name="axis">An axis.</param>
        /// <returns>True if the axis is in use.</returns>
        protected internal override bool IsUsing(Axis axis)
        {
            return false;
        }

        protected internal override void SetDefaultValues()
        {
        }

        protected internal override void UpdateAxisMaxMin()
        {
            XAxis.Include(MinX);
            XAxis.Include(MaxX);
            YAxis.Include(MinY);
            YAxis.Include(MaxY);
        }

        internal protected override void UpdateData()
        {
        }

        protected internal override void UpdateMaxMin()
        {
            MinX = MinY = MaxX = MaxY = double.NaN;
        }

        public OxyRect GetClippingRect()
        {
            var minX = Math.Min(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var minY = Math.Min(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);
            var maxX = Math.Max(XAxis.ScreenMin.X, XAxis.ScreenMax.X);
            var maxY = Math.Max(YAxis.ScreenMin.Y, YAxis.ScreenMax.Y);

            return new OxyRect(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
