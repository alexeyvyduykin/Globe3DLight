using System;
using System.Linq;
using TimeDataViewer.Spatial;

namespace TimeDataViewer.Core
{
    public partial class PlotModel
    {
        // Renders the plot with the specified rendering context.
        void IPlotModel.Render(double width, double height)
        {
            RenderOverride(width, height);
        }

        // Renders the plot with the specified rendering context.
        protected virtual void RenderOverride(double width, double height)
        {
            lock (SyncRoot)
            {
                try
                {
                    Width = width;
                    Height = height;

                    PlotArea = new OxyRect(0, 0, Width, Height);

                    UpdateAxisTransforms();
                    UpdateIntervals();

                    foreach (var a in Axises)
                    {
                        a.ResetCurrentValues();
                    }

                    RenderSeries();
                    RenderAxises();
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
        }

        private void RenderAxises()
        {
            foreach (var item in Axises)
            {
                item.MyOnRender(this);
            }
        }

        private void RenderSeries()
        {
            foreach (var s in Series.Where(s => s.IsVisible))
            {
                s.Render();
                s.MyOnRender();
            }
        }
    }
}
