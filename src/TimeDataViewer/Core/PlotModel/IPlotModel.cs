namespace TimeDataViewer.Core
{
    public interface IPlotModel
    {
        // Updates the model.
        void Update(bool updateData);

        // Renders the plot with the specified rendering context.
        void Render(double width, double height);

        // Attaches this model to the specified plot view.
        void AttachPlotView(IPlotView plotView);
    }
}
