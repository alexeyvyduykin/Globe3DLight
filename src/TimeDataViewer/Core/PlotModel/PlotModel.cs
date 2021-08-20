#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public partial class PlotModel : Model, IPlotModel
    {
        // The plot view that renders this plot.    
        private WeakReference _plotViewReference;
        // Flags if the data has been updated.  
        private bool _isDataUpdated;

        public PlotModel()
        {
            Axises = new ElementCollection<Axis>(this);
            Series = new ElementCollection<Series>(this);
        }

        public event EventHandler<TrackerEventArgs> TrackerChanged;

        public IPlotView PlotView => (_plotViewReference != null) ? (IPlotView)_plotViewReference.Target : null;

        public ElementCollection<Axis> Axises { get; private set; }

        public ElementCollection<Series> Series { get; private set; }

        // Gets the total width of the plot (in device units).       
        public double Width { get; private set; }

        // Gets the total height of the plot (in device units).      
        public double Height { get; private set; }

        // Gets the plot area. This area is used to draw the series (not including axes or legends).
        public OxyRect PlotArea { get; private set; }

        public double PlotMarginLeft { get; set; }

        public double PlotMarginTop { get; set; }

        public double PlotMarginRight { get; set; }

        public double PlotMarginBottom { get; set; }

        public Axis DefaultXAxis { get; private set; }

        public Axis DefaultYAxis { get; private set; }

        void IPlotModel.AttachPlotView(IPlotView plotView)
        {
            var currentPlotView = PlotView;
            if (!object.ReferenceEquals(currentPlotView, null) &&
                !object.ReferenceEquals(plotView, null) &&
                !object.ReferenceEquals(currentPlotView, plotView))
            {
                throw new InvalidOperationException("This PlotModel is already in use by some other PlotView control.");
            }

            _plotViewReference = (plotView == null) ? null : new WeakReference(plotView);
        }

        public void InvalidatePlot(bool updateData)
        {
            var plotView = PlotView;
            if (plotView == null)
            {
                return;
            }

            plotView.InvalidatePlot(updateData);
        }

        // Gets the first axes that covers the area of the specified point.
        public void GetAxesFromPoint(out Axis xaxis, out Axis yaxis)
        {
            xaxis = yaxis = null;

            foreach (var axis in Axises)
            {
                if (!axis.IsAxisVisible)
                {
                    continue;
                }

                if (axis.IsHorizontal())
                {
                    xaxis = axis;
                }
                else if (axis.IsVertical())
                {
                    yaxis = axis;
                }
            }
        }

        public Series GetSeriesFromPoint(ScreenPoint point, double limit = 100)
        {
            double mindist = double.MaxValue;
            Series nearestSeries = null;
            foreach (var series in Series.Reverse().Where(s => s.IsVisible))
            {
                var thr = series.GetNearestPoint(point, true) ?? series.GetNearestPoint(point, false);

                if (thr == null)
                {
                    continue;
                }

                // find distance to this point on the screen
                double dist = point.DistanceTo(thr.Position);
                if (dist < mindist)
                {
                    nearestSeries = series;
                    mindist = dist;
                }
            }

            if (mindist < limit)
            {
                return nearestSeries;
            }

            return null;
        }


        /// <summary>
        /// Updates all axes and series.
        /// 0. Updates the owner PlotModel of all plot items (axes, series and annotations)
        /// 1. Updates the data of each Series (only if updateData==<c>true</c>).
        /// 2. Ensure that all series have axes assigned.
        /// 3. Updates the max and min of the axes.
        /// </summary>
        /// <param name="updateData">if set to <c>true</c> , all data collections will be updated.</param>
        void IPlotModel.Update(bool updateData)
        {
            lock (SyncRoot)
            {
                try
                {
                    // Updates the default axes
                    EnsureDefaultAxes();

                    var visibleSeries = Series.Where(s => s.IsVisible).ToArray();

                    // Update data of the series
                    if (updateData || _isDataUpdated == false)
                    {
                        foreach (var s in visibleSeries)
                        {
                            s.UpdateData();
                        }

                        _isDataUpdated = true;
                    }


                    // Updates axes with information from the series
                    // This is used by the category axis that need to know the number of series using the axis.
                    foreach (var a in Axises)
                    {
                        a.UpdateFromSeries(visibleSeries);
                        a.ResetCurrentValues();
                    }


                    // Update valid data of the series
                    // This must be done after the axes are updated from series!
                    if (updateData)
                    {
                        foreach (var s in visibleSeries)
                        {
                            s.UpdateValidData();
                        }
                    }

                    // Update the max and min of the axes
                    UpdateMaxMin(updateData);

                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// Gets the axis for the specified key.
        /// </summary>
        /// <param name="key">The axis key.</param>
        /// <returns>The axis that corresponds with the key.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot find axis with the specified key.</exception>
        public Axis GetAxis(string key)
        {
            if (key == null)
            {
                throw new ArgumentException("Axis key cannot be null.");
            }

            var axis = Axises.FirstOrDefault(a => a.Key == key);
            if (axis == null)
            {
                throw new InvalidOperationException($"Cannot find axis with Key = \"{key}\"");
            }
            return axis;
        }

        /// <summary>
        /// Resets all axes in the model.
        /// </summary>
        public void ResetAllAxes()
        {
            foreach (var a in Axises)
            {
                a.Reset();
            }
        }

        public void PanAllAxes(double dx, double dy)
        {
            foreach (var a in Axises)
            {
                a.Pan(a.IsHorizontal() ? dx : dy);
            }
        }

        public void ZoomAllAxes(double factor)
        {
            foreach (var a in Axises)
            {
                a.ZoomAtCenter(factor);
            }
        }

        public void RaiseTrackerChanged(TrackerHitResult result)
        {
            var handler = TrackerChanged;
            if (handler != null)
            {
                var args = new TrackerEventArgs { HitResult = result };
                handler(this, args);
            }
        }

        protected internal virtual void OnTrackerChanged(TrackerHitResult result)
        {
            RaiseTrackerChanged(result);
        }

        /// <summary>
        /// Gets all elements of the model, top-level elements first.
        /// </summary>
        /// <returns>
        /// An enumerator of the elements.
        /// </returns>
        public override IEnumerable<UIElement> GetElements()
        {
            // foreach (var axis in Axises.Reverse().Where(a => a.IsAxisVisible && a.Layer == AxisLayer.AboveSeries))
            // {
            //     yield return axis;
            // }

            foreach (var s in Series.Reverse().Where(s => s.IsVisible))
            {
                yield return s;
            }

            foreach (var axis in Axises.Reverse().Where(a => a.IsAxisVisible))
            {
                yield return axis;
            }
        }

        private void UpdateAxisTransforms()
        {
            // Update the axis transforms
            foreach (var a in Axises)
            {
                a.UpdateTransform(PlotArea);
            }
        }

        private void UpdateIntervals()
        {
            // Update the intervals for all axes
            foreach (var a in Axises)
            {
                a.UpdateIntervals(PlotArea);
            }
        }

        /// <summary>
        /// Finds and sets the default horizontal and vertical axes (the first horizontal/vertical axes in the Axes collection).
        /// </summary>
        private void EnsureDefaultAxes()
        {
            DefaultXAxis = Axises.FirstOrDefault(a => a.IsHorizontal() && a.IsXyAxis());
            DefaultYAxis = Axises.FirstOrDefault(a => a.IsVertical() && a.IsXyAxis());

            if (DefaultYAxis == null)
            {
                DefaultYAxis = new CategoryAxis { Position = AxisPosition.Left };
            }

            var areAxesRequired = Series.Any(s => s.IsVisible && s.AreAxesRequired());

            if (areAxesRequired)
            {
                if (!Axises.Contains(DefaultXAxis))
                {
                    if (DefaultXAxis != null)
                    {
                        Axises.Add(DefaultXAxis);
                    }
                }

                if (!Axises.Contains(DefaultYAxis))
                {
                    if (DefaultYAxis != null)
                    {
                        Axises.Add(DefaultYAxis);
                    }
                }
            }

            // Update the axes of series without axes defined
            foreach (var s in Series)
            {
                if (s.IsVisible && s.AreAxesRequired())
                {
                    s.EnsureAxes();
                }
            }
        }

        private void UpdateMaxMin(bool isDataUpdated)
        {
            if (isDataUpdated)
            {
                foreach (var a in Axises)
                {
                    a.ResetDataMaxMin();
                }

                // data has been updated, so we need to calculate the max/min of the series again
                foreach (var s in Series.Where(s => s.IsVisible))
                {
                    s.UpdateMaxMin();
                }
            }

            foreach (var s in Series.Where(s => s.IsVisible))
            {
                s.UpdateAxisMaxMin();
            }

            foreach (var a in Axises)
            {
                a.UpdateActualMaxMin();
            }
        }
    }
}
