using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public class SelectManipulator : MouseManipulator
    {
        public SelectManipulator(IPlotView plotView) : base(plotView)
        {

        }

        public override void Started(OxyMouseEventArgs e)
        {
            base.Started(e);

            e.Handled = true;

            var actualModel = PlotView.ActualModel;
            if (actualModel == null)
            {
                return;
            }

            foreach (var item in PlotView.ActualModel.Series)
            {
                if (item is TimelineSeries series)
                {
                    series.ResetSelecIndex();
                }
            }

            if (!actualModel.PlotArea.Contains(e.Position.X, e.Position.Y))
            {
                return;
            }

            var currentSeries = PlotView.ActualModel.GetSeriesFromPoint(e.Position);


            var result = GetNearestHit(currentSeries, e.Position);
            if (result != null)
            {
                if (currentSeries is TimelineSeries series)
                {
                    series.SelectIndex((int)result.Index);

                    if (PlotView is Timeline timeline)
                    {
                        timeline.SliderTo(((TimelineItem)result.Item).Begin);
                    }

                    PlotView.InvalidatePlot(false);
                }
            }
        }

        private static TrackerHitResult? GetNearestHit(Series series, ScreenPoint point)
        {
            if (series == null)
            {
                return null;
            }

            var result = series.GetNearestPoint(point, false);
            if (result != null)
            {
                if (result.Position.DistanceTo(point) < 20)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
