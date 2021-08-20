namespace TimeDataViewer.Core
{
    public interface IViewCommand<in T> : IViewCommand where T : OxyInputEventArgs
    {
        // Executes the command on the specified plot.
        void Execute(IView view, IController controller, T args);
    }
}
