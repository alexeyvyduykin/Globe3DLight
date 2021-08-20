namespace TimeDataViewer.Core
{
    public abstract class PlotElement : UIElement
    {
        protected PlotElement() { }

        public PlotModel PlotModel => (PlotModel)Parent;
    }
}
