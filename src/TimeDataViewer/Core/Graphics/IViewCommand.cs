namespace TimeDataViewer.Core
{
    public interface IViewCommand
    {
        // Executes the command on the specified plot.
        void Execute(IView view, IController controller, OxyInputEventArgs args);
    }
}
