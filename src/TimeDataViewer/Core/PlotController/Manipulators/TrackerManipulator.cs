using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class TrackerManipulator : MouseManipulator
    {
        private Series? _currentSeries;

        public TrackerManipulator(IPlotView plotView) : base(plotView)
        {
            Snap = true;
            PointsOnly = false;
            LockToInitialSeries = true;
        }

        public bool PointsOnly { get; set; }

        public bool Snap { get; set; }

        public bool LockToInitialSeries { get; set; }

        public override void Completed(OxyMouseEventArgs e)
        {
            base.Completed(e);
            e.Handled = true;

            _currentSeries = null;
            PlotView.HideTracker();
            if (PlotView.ActualModel != null)
            {
                PlotView.ActualModel.RaiseTrackerChanged(null);
            }
        }

        public override void Delta(OxyMouseEventArgs e)
        {
            base.Delta(e);
            e.Handled = true;

            if (_currentSeries == null || !LockToInitialSeries)
            {
                // get the nearest
                _currentSeries = PlotView.ActualModel != null ? PlotView.ActualModel.GetSeriesFromPoint(e.Position, 20) : null;
            }

            if (_currentSeries == null)
            {
                if (!LockToInitialSeries)
                {
                    PlotView.HideTracker();
                }

                return;
            }

            var actualModel = PlotView.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return;
            }

            var result = GetNearestHit(_currentSeries, e.Position, Snap, PointsOnly);
            if (result != null)
            {
                result.PlotModel = PlotView.ActualModel;
                PlotView.ShowTracker(result);
                PlotView.ActualModel.RaiseTrackerChanged(result);
            }
        }

        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);
            _currentSeries = PlotView.ActualModel != null ? PlotView.ActualModel.GetSeriesFromPoint(e.Position) : null;
            Delta(e);
        }

        private static TrackerHitResult? GetNearestHit(Series series, ScreenPoint point, bool snap, bool pointsOnly)
        {
            if (series == null)
            {
                return null;
            }

            // Check data points only
            if (snap || pointsOnly)
            {
                var result = series.GetNearestPoint(point, false);
                if (result != null)
                {
                    if (result.Position.DistanceTo(point) < 20)
                    {
                        return result;
                    }
                }
            }

            // Check between data points (if possible)
            if (!pointsOnly)
            {
                var result = series.GetNearestPoint(point, true);
                return result;
            }

            return null;
        }
    }
}
