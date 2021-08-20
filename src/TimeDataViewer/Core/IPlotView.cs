namespace TimeDataViewer.Core
{
    public interface IPlotView : IView
    {
        new PlotModel ActualModel { get; }

        void HideTracker();

        // Invalidates the plot (not blocking the UI thread)
        void InvalidatePlot(bool updateData = true);

        void ShowTracker(TrackerHitResult trackerHitResult);

        // Stores text on the clipboard.
        void SetClipboardText(string text);
    }
}
